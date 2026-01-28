using API.Context;
using API.Helpers;
using API.NetGearAPI.Common;
using APISignalR.NetGearAPI.Dot1qSwPortConfig;
using Microsoft.EntityFrameworkCore;
using SW.ApiObjects.LldpRemoteDevices;
using SW.Request.Login;
using SwitchesDll;
using System.Text.Json;

namespace API.Singletons
{
    public class Dot1qSwPortConfigSingleton
    {
        //TUTO
        private readonly IServiceProvider _scopeFactory;
        private readonly BearerTokenSingleton _bearerTokenSingleton;
        public Dot1qSwPortConfigSingleton(IServiceProvider scopeFactory, BearerTokenSingleton bearerTokenSingleton)
        {
            _scopeFactory = scopeFactory;
            _bearerTokenSingleton = bearerTokenSingleton;

            handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
        }

        protected HttpClientHandler handler = new HttpClientHandler();
        protected HttpClient httpClient { get; set; } = new HttpClient();
        protected Thread pollingThread { get; set; } = new Thread(() => { });



        public int iX { get; set; } = 0;
        public Dictionary<string, Dictionary<int, LiteVlanConfig>> Dot1qDict { get; set; } = new Dictionary<string, Dictionary<int, LiteVlanConfig>>();

        public Dictionary<int, LiteVlanConfig> GetStoredDot1qSwPortConfig(string switchIp)
        {
            return Dot1qDict.TryGetValue(switchIp, out var portAndVlan) ? portAndVlan : new Dictionary<int, LiteVlanConfig>();
        }

        public bool AddInStoredDot1qSwPortConfig(string ip, int portNumb, LiteVlanConfig vlan)
        {
            if (!Dot1qDict.ContainsKey(ip))
            {
                Dot1qDict.Add(ip, new Dictionary<int, LiteVlanConfig>()
                {
                    { portNumb , vlan },
                });
                return true;
            }
            else
            {
                if (!Dot1qDict[ip].ContainsKey(portNumb))
                {
                    Dot1qDict[ip].Add(portNumb, vlan);
                    return true;
                }
                else
                {
                    if (Dot1qDict[ip][portNumb].Vlan != vlan.Vlan)
                    {
                        Dot1qDict[ip][portNumb].Vlan = vlan.Vlan;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ChangeVlan(string ip, int portNumb, int newVlan)
        {
            if (Dot1qDict.ContainsKey(ip) && Dot1qDict[ip].ContainsKey(portNumb))
            {
                Dot1qDict[ip][portNumb].Vlan = newVlan;
            }
        }

        public void StartPollingThread()
        {
            pollingThread = new Thread(async () => await PollingLoop());
            pollingThread.IsBackground = true;
            pollingThread.Start();
        }

        protected async Task PollingLoop()
        {
            IServiceScope scope = _scopeFactory.CreateScope();
            ContextSwitch? dbContext = scope.ServiceProvider.GetRequiredService<ContextSwitch>();

            List<SwitchDB> changedSwitchDBList = new List<SwitchDB>();
            //bool hasChanged = false;

            while (true)
            {
                try
                {
                    List<SwitchDB> switchDBs = await dbContext.SwitchDB.ToListAsync();
                    changedSwitchDBList.Clear();

                    foreach (SwitchDB sw in switchDBs)
                    {
                        HttpClient httpClientSwitch = new HttpClient(handler)
                        {
                            BaseAddress = new Uri($"https://{sw.Ip}:8443/api/v1/"),
                        };
                        //BearerTokenGestionAsync(httpClientSwitch, sw.Ip, sw.Username, sw.Password);

                        //LldpRemoteDevicesResponse? lldpRemoteDevicesResponse = await GetLldpRemoteDevices(httpClientSwitch, sw.Ip, sw.Username, sw.Password);

                        //if (lldpRemoteDevicesResponse == null || lldpRemoteDevicesResponse.LldpRemoteDevices == null)
                        //{
                        //    continue;
                        //}

                        //foreach (LldpRemoteDevice lldp in lldpRemoteDevicesResponse.LldpRemoteDevices)
                        int i = 0;
                        while (true)
                        {
                            i++;
                            HttpClient httpClientLddp = new HttpClient(handler)
                            {
                                BaseAddress = new Uri($"https://{sw.Ip}:8443/api/v1/"),
                            };
                            BearerTokenGestionAsync(httpClientLddp, sw.Ip, sw.Username, sw.Password);

                            Dot1qSwPortConfigResponse? dot1QResp = await GetDot1qSwPortConfig(httpClientLddp, i, sw.Ip, sw.Username, sw.Password);

                            if (dot1QResp == null)
                            {
                                break;
                            }

                            try
                            {
                                if (dot1QResp != null && dot1QResp.Dot1qSwPortConfig != null)
                                {
                                    bool b = this.AddInStoredDot1qSwPortConfig(sw.Ip, i, new LiteVlanConfig(i, dot1QResp.Dot1qSwPortConfig.AccessVlan, dot1QResp.Dot1qSwPortConfig.ConfigMode, false));
                                    if (b is true && !changedSwitchDBList.Contains(sw))
                                    {
                                        changedSwitchDBList.Add(sw);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                break;
                            }
                        }
                    }

                    if (changedSwitchDBList.Count > 0)
                    {
                        try
                        {
                            HttpClient clientHttpClient = new HttpClient(handler)
                            {

                                BaseAddress = new Uri("https://localhost:7168/api/"),
                                Timeout = TimeSpan.FromSeconds(1.2f),
                            };

                            await clientHttpClient.GetAsync($"DataChange/GetVlanUpdated");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }

                    Console.WriteLine("POLLING FINISH");
                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public async Task<LldpRemoteDevicesResponse?> GetLldpRemoteDevices(HttpClient httpClient, string ip, string username, string password)
        {
            try
            {
                //await BearerTokenGestionAsync(this.httpClient, ip, username, password);
                HttpResponseMessage resultLldp = await httpClient.GetAsync($"lldp_remote_devices");
                if (!resultLldp.IsSuccessStatusCode)
                {
                    return null;
                }
                return await resultLldp.Content.ReadFromJsonAsync<LldpRemoteDevicesResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void BearerTokenGestionAsync(HttpClient httpClient, string ip, string username, string password)
        {
            SetBasicToHttp(httpClient, username, password);
        }


        private void SetBasicToHttp(HttpClient httpClient, string username, string password)
        {
            username = Helper.Decrypt(username, Helper.PublicKey);
            password = Helper.Decrypt(password, Helper.PublicKey);
            string authString = $"{username}:{password}";
            var base64EncodedAuthString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authString));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthString);
        }

        public async Task<LoginResponse?> GetBearerToken(HttpClient httpClient, string ip, string username, string password)
        {
            try
            {
                HttpResponseMessage resultLogin = await httpClient.PostAsJsonAsync("login", new LoginRequest(username, password));
                LoginResponse? loginResponse = null;
                loginResponse = await resultLogin.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse is null)
                {
                    return null;
                }

                return loginResponse;
            }
            catch
            {
                return null;
            }
        }


        public async Task<Dot1qSwPortConfigResponse?> GetDot1qSwPortConfig(HttpClient httpClient, int iInterface, string ip, string username, string password)
        {
            try
            {
                var resp = await httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, dynamic>>>($"dot1q_sw_port_config?interface={iInterface}");

                if (resp == null || !resp!.ContainsKey("dot1q_sw_port_config"))
                {
                    return null;
                }
                if (!resp["dot1q_sw_port_config"].ContainsKey("forbiddenVlanList"))
                {
                    resp["dot1q_sw_port_config"].Add("forbiddenVlanList", new string[0]);
                }
                if (!resp["dot1q_sw_port_config"].ContainsKey("taggedVlanList"))
                {
                    resp["dot1q_sw_port_config"].Add("taggedVlanList", new string[0]);
                }

                dynamic forbiddenVlanList = resp?["dot1q_sw_port_config"]["forbiddenVlanList"] ?? new string[0];
                dynamic taggedVlanList = resp?["dot1q_sw_port_config"]["taggedVlanList"] ?? new string[0];

                return new Dot1qSwPortConfigResponse()
                {
                    Dot1qSwPortConfig = new Dot1qSwPortConfig()
                    {

                        Interface = Helper.SafeGet<int>(resp?["dot1q_sw_port_config"]["interface"], 0),
                        AccessVlan = Helper.SafeGet<int>(resp?["dot1q_sw_port_config"]["accessVlan"], 0),
                        AllowedVlanList = Helper.SafeGet<string[]>(resp?["dot1q_sw_port_config"]["allowedVlanList"], Array.Empty<string>()),
                        DynamicallyAddedVlanList = Helper.SafeGet<string>(resp?["dot1q_sw_port_config"]["dynamicallyAddedVlanList"], ""),
                        ForbiddenVlanList = (forbiddenVlanList is JsonValueKind.Array) ? Helper.SafeGet<string[]>(forbiddenVlanList, Array.Empty<string>()) : new string[0],
                        ConfigMode = Helper.SafeGet<string>(resp?["dot1q_sw_port_config"]["configMode"], ""),
                        NativeVlan = Helper.SafeGet<int>(resp?["dot1q_sw_port_config"]["nativeVlan"], 0),
                        TaggedVlanList = (taggedVlanList is System.Text.Json.JsonValueKind.Array) ? Helper.SafeGet<string[]>(taggedVlanList, Array.Empty<string>()) : new string[0],
                        UnTaggedVlanList = Helper.SafeGet<string[]>(resp?["dot1q_sw_port_config"]["untaggedVlanList"], Array.Empty<string>()),
                    },

                    Res = new SW.Request.Common.Response()
                    {
                        RespCode = Helper.SafeGet<int>(resp?["resp"]["respCode"], 0),
                        RespMsg = Helper.SafeGet<string>(resp?["resp"]["respMsg"], ""),
                        Status = Helper.SafeGet<string>(resp?["resp"]["status"], ""),
                    }
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
