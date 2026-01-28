using Frontend.Helpers;
using Frontend.Model;
using Frontend.NetgearAPI;
using Frontend.NetGearAPI.Common;
using Frontend.Singletons;
using SwitchesDll;
using System.Text.Json;
using static Frontend.Model.DatagridContent;

namespace Frontend.Providers
{
    public class EquipmentProvider
    {
        static public EquipmentProvider Instance = new EquipmentProvider();

        public event EventHandler<EventArgs>? UpdatedContent;
        public EquipmentState state { get => _state; }

        private EquipmentState _state;
        private HttpClient _httpClient;
        private readonly EventsSingleton _eventsSingleton;
        private SwitchProvider _switchProvider;

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        public EquipmentProvider(HttpClient? httpClient = null, SwitchProvider? switchProvider = null)
        {
            _state = new EquipmentState();
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri($"{Helper.BaseAPIServerUrl}api/");
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _eventsSingleton = EventsSingleton.Instance;
            _eventsSingleton.VlanChanged += Update;

            _switchProvider = switchProvider ?? SwitchProvider.Instance;
        }
        public async void Update()
        {
            await FetchLldpRemoteDevices();
        }

        public async Task FetchLldpRemoteDevices()
        {

            _state = _state.CopyWith(Equipments: [], DataState: EquipmemtProviderDataStates.Loading);
            UpdatedContent?.Invoke(_state, EventArgs.Empty);

            foreach (SwitchDB sw in _switchProvider.state.SelectedSwitches)
            {



                if (sw == null)
                {
                    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: "No Switch is selected!");
                    UpdatedContent?.Invoke(_state, EventArgs.Empty);
                    return;
                }


                //device
                string endpoint = $"PortActions/GetLldpRemoteDevices?ip={sw.Ip}&username={sw.Username}&password={sw.Password}";
                LldpRemoteDevicesResponse? devicesResponse = await _httpClient.GetFromJsonAsync<LldpRemoteDevicesResponse?>(endpoint);

                if (devicesResponse == null)
                {
                    return;
                }
                LldpRemoteDevice[] devices = devicesResponse.LldpRemoteDevices;
                List<DatagridContent> contentWithPrevious = _state.Equipments.ToList();
                List<DatagridContent> content = new List<DatagridContent>();
                foreach (LldpRemoteDevice d in devices)
                {
                    var data = new DatagridContent().CopyWith(
                        ConnectionState: true,
                        AdminMode: true,
                        Id: d.Id,
                        OwnerIp: sw.Ip,
                        IfIndex: d.IfIndex,
                        RemoteId: d.RemoteId,
                        ChassisId: d.ChassisId,
                        ChassisIdSubtype: d.ChassisIdSubtype,
                        RemotePortId: d.RemotePortId,
                        RemotePortIdSubtype: d.RemotePortIdSubtype,
                        RemotePortDesc: d.RemotePortDesc,
                        RemoteSysName: d.RemoteSysName,
                        RemoteSysDesc: d.RemoteSysDesc,
                        SysCapabilitiesEnabled: d.SysCapabilitiesEnabled,
                        SysCapabilitiesSupported: d.SysCapabilitiesSupported,
                        RemoteTTL: d.RemoteTTL,
                        IpV4: d.MgmtAddresses.Where(addr => addr.Type == "IPv4").ToList().FirstOrDefault()?.Address ?? ""
                        );
                    data.SetEquipementType();

                    content.Add(data);
                    contentWithPrevious.Add(data.CopyWith());
                }

                //vlan
                endpoint = $"PortActions/GetDot1qSwPortConfigCache?ip={sw.Ip}";
                Dictionary<int, LiteVlanConfig>? vlans = await _httpClient.GetFromJsonAsync<Dictionary<int, LiteVlanConfig>?>(endpoint);

                if (vlans == null)
                {
                    return;
                }

                foreach (LiteVlanConfig vlan in vlans.Values)
                {
                    DatagridContent? equipment = contentWithPrevious.Where(e => e.Id == vlan.PortId && e.OwnerIp == sw.Ip).FirstOrDefault();
                    if (equipment != null)
                    {
                        equipment.AccessVlan = vlan.Vlan;
                        equipment.ConfigMode = vlan.VlanMode;
                        equipment.AdminMode = vlan.AdminMode;
                        var e = content.Where(e => e.Id == vlan.PortId).FirstOrDefault();
                        if (e != null)
                        {
                            e.AccessVlan = vlan.Vlan;
                            e.ConfigMode = vlan.VlanMode;
                            e.AdminMode = vlan.AdminMode;
                        }
                    }
                    else
                    {
                        Equipment? savedEquipment = sw.Equipments.Where(e => e.PortId == vlan.PortId).FirstOrDefault();
                        if (savedEquipment != null)
                        {
                            DatagridContent newEquipment = new()
                            {
                                AccessVlan = vlan.Vlan,
                                ConfigMode = vlan.VlanMode,
                                EquipmentType = DatagridContent.GetTypeFromString(savedEquipment.Type),
                                ConnectionState = false,
                                Id = vlan.PortId,
                                Interface = vlan.PortId,
                                AdminMode = vlan.AdminMode,
                                RemotePortId = savedEquipment.MacAddress,
                                RemoteSysName = savedEquipment.Name,
                                IpV4 = savedEquipment.IpV4
                            };

                            content.Add(newEquipment);
                            contentWithPrevious.Add(newEquipment);
                        }
                        else
                        {
                            DatagridContent newEquipment = new()
                            {
                                AccessVlan = vlan.Vlan,
                                ConfigMode = vlan.VlanMode,
                                EquipmentType = EquipmentTypeEnum.None,
                                ConnectionState = false,
                                Id = vlan.PortId,
                                Interface = vlan.PortId,
                                AdminMode = vlan.AdminMode,
                            };

                            contentWithPrevious.Add(newEquipment);
                        }
                    }
                }

                ////Offline
                //endpoint = $"OfflineEquipment/GetOfflineEquipmentFromSwitch?switchDBId={sw.Id}";
                //Dictionary<int, List<object>>? offlineDevices = await _httpClient.GetFromJsonAsync<Dictionary<int, List<object>>?>(endpoint);

                //_state = _state.CopyWith(Equipments: content);

                //if (offlineDevices == null)
                //{
                //    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Loaded);
                //    return;
                //}


                //foreach ((int key, List<object> value) in offlineDevices)
                //{
                //    if (key == 1)
                //    {
                //        foreach (object device in value)
                //        {
                //            Windows? windows = JsonSerializer.Deserialize<Windows>(JsonSerializer.Serialize(device), options);

                //            if (windows == null)
                //            {
                //                return;
                //            }

                //            var c = contentWithPrevious.Where((c) => c.Id == windows.Id).ToList();
                //            if (c.Count() == 0)
                //            {
                //                continue;
                //            }
                //            c[0].ConnectionState = false;
                //        }
                //    }
                //    else if (key == 2)
                //    {
                //        foreach (object device in value)
                //        {
                //            Switch? switchEq = JsonSerializer.Deserialize<Switch?>(JsonSerializer.Serialize(value), options);
                //            if (switchEq == null)
                //            {
                //                return;
                //            }

                //            var c = contentWithPrevious.Where((c) => c.Id == switchEq.Id).ToList();
                //            c[0].ConnectionState = false;
                //        }
                //    }
                var previous = _state.Equipments.ToList();
                previous.AddRange(contentWithPrevious);
                sw.Equipments = content.Select(e => new Equipment { IpV4 = e.IpV4, MacAddress = e.RemotePortId, Name = e.RemoteSysName, PortId = e.Id, Type = e.EquipmentType.ToString() }).ToList();
                _switchProvider.UpdateSwitch(sw.Id);

                previous.Sort((a, b) => a.Id.CompareTo(b.Id));
                _state = _state.CopyWith(Equipments: previous.Distinct().ToList(), DataState: EquipmemtProviderDataStates.Loading);



                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Loaded);
                UpdatedContent?.Invoke(_state, new EventArgs());
            }

        }



        public async Task<bool> SwitchPortModeTo(bool status, int id)
        {
            try
            {
                SwitchDB? sw = _switchProvider.state.SelectedSwitches.Where(s => _state.Equipments.Any(e => e.OwnerIp == s.Ip)).First();

                if (sw == null)
                {
                    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: "No Switch is selected!");
                    return false;
                }

                string url = $"PortActions/GetPortAdminMode?portnumber={id}&status={status}&ip={sw.Ip}&username={sw.Username}&password={sw.Password}";
                await _httpClient.GetAsync(url);
                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
                return true;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: ex.Message);
                return false;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }

        public async Task<bool> ChangePortVlan(int vlan, int id)
        {
            try
            {
                SwitchDB? sw = _switchProvider.state.SelectedSwitches.Where(s => _state.Equipments.Any(e => e.OwnerIp == s.Ip)).First();

                if (sw == null)
                {
                    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: "No Switch is selected!");
                    return false;
                }

                Dot1qSwPortConfig config = new Dot1qSwPortConfig()
                {
                    AccessVlan = vlan,
                    AllowedVlanList = new[] { "all" },
                    ConfigMode = "access",
                    NativeVlan = 1,
                };

                string url = $"PortActions/PostDot1qSwPortConfig?iInterface={id}&vlanNumber={vlan}&ip={sw.Ip}&username={sw.Username}&password={sw.Password}";
                await _httpClient.PostAsJsonAsync(url, new Dot1qSwPortConfigRequest(config));
                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
                return true;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: ex.Message);
                return false;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }

        public async Task<bool> DeleteOfflineEquipment(int id, EquipmentTypeEnum type)
        {
            try
            {
                SwitchDB? sw = _switchProvider.state.SelectedSwitches.Where(s => _state.Equipments.Any(e => e.OwnerIp == s.Ip)).First();

                if (sw == null)
                {
                    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: "No Switch is selected!");
                    return false;
                }

                string url = $"OfflineEquipment/DeleteOfflineEquipment?switchId={sw.Id}&portNumb={id}&equipmentType={(int)type}";
                await _httpClient.DeleteFromJsonAsync<bool>(url);
                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
                return true;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: ex.Message);
                return false;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }

        public async Task<bool> SaveSwitchPortAndChild(int id, EquipmentTypeEnum type, EquipmentRootObject obj)
        {
            try
            {
                string url = $"OfflineEquipment/PostEquipment?portNumb={id}&equipmentType={(int)type}";
                await _httpClient.PostAsJsonAsync(url, obj);
                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
                return true;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: ex.Message);
                return false;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }

        public async Task<Dot1qSwPortConfigResponse?> GetDot1qSwPortConfig(int id)
        {
            try
            {
                SwitchDB? sw = _switchProvider.state.SelectedSwitches.Where(s => _state.Equipments.Any(e => e.OwnerIp == s.Ip)).First();

                if (sw == null)
                {
                    _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: "No Switch is selected!");
                    return null;
                }

                string url = $"PortActions/GetDot1qSwPortConfig?iInterface={id}&ip={sw.Ip}&username={sw.Username}&password={sw.Password}";
                Dot1qSwPortConfigResponse? response = await _httpClient.GetFromJsonAsync<Dot1qSwPortConfigResponse?>(url);


                return response;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: EquipmemtProviderDataStates.Error, ErrorMsg: ex.Message);
                return null;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }
        private EquipmemtProviderDataStates _GetDataState()
        {
            return _state.Equipments.Count == 0 ? EquipmemtProviderDataStates.Empty : EquipmemtProviderDataStates.Loaded;
        }
    }
}
