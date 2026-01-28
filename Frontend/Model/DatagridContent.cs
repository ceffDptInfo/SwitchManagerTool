using Frontend.Helpers;
using Frontend.NetgearAPI;
using SwitchesDll;

namespace Frontend.Model
{
    public class DatagridContent
    {
        public DatagridContent(bool connectionState, Windows windows, int? vlan = null)
        {
            this.ConnectionState = connectionState;

            this.ChassisId = windows.MacAdress;

            if (windows.Name != null && windows.Name != "")
            {
                this.ChassisId = windows.Name;
            }

            if (vlan != null)
            {
                this.AccessVlan = (int)vlan;
            }

            this.Id = windows.PortNumber;
        }

        public DatagridContent(bool connectionState, Switch switchEq, int? vlan = null)
        {
            this.ConnectionState = connectionState;

            if (switchEq.Name != null || switchEq.Name != "")
            {
                this.ChassisId = switchEq.Name!;
            }
            else
            {
                this.ChassisId = switchEq.MacAdress;
            }

            if (vlan != null)
            {
                this.AccessVlan = (int)vlan;
            }

            this.IpV4 = switchEq.Ip;
            this.Id = switchEq.PortNumber;
        }

        public DatagridContent(bool connectionState, LldpRemoteDevice lldp, int? vlan = null, string? ownerIp = null)
        {
            this.ConnectionState = connectionState;

            this.Id = lldp.Id;
            this.OwnerIp = ownerIp ?? "";
            this.IfIndex = lldp.IfIndex;
            this.RemoteId = lldp.RemoteId;
            this.ChassisId = lldp.ChassisId;
            this.ChassisIdSubtype = lldp.ChassisIdSubtype;
            this.RemotePortId = lldp.RemotePortId;
            this.RemotePortIdSubtype = lldp.RemotePortIdSubtype;
            this.RemotePortDesc = lldp.RemotePortDesc;
            this.RemoteSysDesc = lldp.RemoteSysDesc;
            this.RemoteSysName = lldp.RemoteSysName;
            this.SysCapabilitiesSupported = lldp.SysCapabilitiesSupported;
            this.sysCapabilitiesEnabled = lldp.SysCapabilitiesEnabled;
            this.RemoteTTL = lldp.RemoteTTL;

            if (lldp.MgmtAddresses is not null)
            {
                foreach (MgmtAdress mgmt in lldp.MgmtAddresses)
                {
                    if (mgmt.Type == "IPv4")
                    {
                        this.IpV4 = mgmt.Address;
                    }
                }
            }

            if (vlan != null)
            {
                this.AccessVlan = (int)vlan;
            }

            SetEquipementType();
        }

        public DatagridContent(
        bool connectionState,
        bool adminMode,
        int interfaceId,
        int accessVlan,
        string configMode,
        int id,
        string ownerIp,
        int ifIndex,
        int remoteId,
        string chassisId,
        int chassisIdSubtype,
        string remotePortId,
        int remotePortIdSubtype,
        string remotePortDesc,
        string remoteSysName,
        string remoteSysDesc,
        string[] sysCapabilitiesSupported,
        string[] sysCapabilitiesEnabled,
        int remoteTTL,
        string ipV4,
        EquipmentTypeEnum equipmentType
        )
        {
            this.ConnectionState = connectionState;
            this.AdminMode = adminMode;
            this.Interface = interfaceId;
            this.AccessVlan = accessVlan;
            this.ConfigMode = configMode;
            this.Id = id;
            this.OwnerIp = ownerIp;
            this.IfIndex = ifIndex;
            this.RemoteId = remoteId;
            this.ChassisId = chassisId;
            this.ChassisIdSubtype = chassisIdSubtype;
            this.RemotePortId = remotePortId;
            this.RemotePortIdSubtype = remotePortIdSubtype;
            this.RemotePortDesc = remotePortDesc;
            this.RemoteSysName = remoteSysName;
            this.RemoteSysDesc = remoteSysDesc;
            this.SysCapabilitiesSupported = sysCapabilitiesSupported;
            this.sysCapabilitiesEnabled = sysCapabilitiesEnabled;
            this.RemoteTTL = remoteTTL;
            this.IpV4 = ipV4;
            this.EquipmentType = equipmentType;
        }

        public DatagridContent() { }

        public enum EquipmentTypeEnum
        {
            WIN_10,
            WIN_11,
            SWITCH,
            ROUTER,
            OTHER,
            None,
        }

        public EquipmentTypeEnum EquipmentType { get; set; } = EquipmentTypeEnum.OTHER;

        public bool ConnectionState { get; set; } = false;
        public bool AdminMode { get; set; } = false;

        public int Interface { get; set; } = 0;
        public int AccessVlan { get; set; } = 0;
        public string ConfigMode { get; set; } = "Access";


        public int Id { get; set; } = 0;
        public string OwnerIp { get; set; } = "";
        public int IfIndex { get; set; } = 0;
        public int RemoteId { get; set; } = 0;
        public string ChassisId { get; set; } = "";
        public int ChassisIdSubtype { get; set; } = 0;
        public string RemotePortId { get; set; } = "";
        public int RemotePortIdSubtype { get; set; } = 0;
        public string RemotePortDesc { get; set; } = "";
        public string RemoteSysName { get; set; } = "";
        public string RemoteSysDesc { get; set; } = "";
        public string[] SysCapabilitiesSupported { get; set; } = new string[0];
        public string[] sysCapabilitiesEnabled { get; set; } = new string[0];
        public int RemoteTTL { get; set; } = 0;


        public string IpV4 { get; set; } = "";

        public DatagridContent CopyWith(
        bool? ConnectionState = null,
        bool? AdminMode = null,
        int? Interface = null,
        int? AccessVlan = null,
        string? ConfigMode = null,
        int? Id = null,
        string? OwnerIp = null,
        int? IfIndex = null,
        int? RemoteId = null,
        string? ChassisId = null,
        int? ChassisIdSubtype = null,
        string? RemotePortId = null,
        int? RemotePortIdSubtype = null,
        string? RemotePortDesc = null,
        string? RemoteSysName = null,
        string? RemoteSysDesc = null,
        string[]? SysCapabilitiesSupported = null,
        string[]? SysCapabilitiesEnabled = null,
        int? RemoteTTL = null,
        string? IpV4 = null,
        EquipmentTypeEnum? EquipmentType = null
            )
        {
            return new DatagridContent(
                ConnectionState ?? this.ConnectionState,
                AdminMode ?? this.AdminMode,
                Interface ?? this.Interface,
                AccessVlan ?? this.AccessVlan,
                ConfigMode ?? this.ConfigMode,
                Id ?? this.Id,
                OwnerIp ?? this.OwnerIp,
                IfIndex ?? this.IfIndex,
                RemoteId ?? this.RemoteId,
                ChassisId ?? this.ChassisId,
                ChassisIdSubtype ?? this.ChassisIdSubtype,
                RemotePortId ?? this.RemotePortId,
                RemotePortIdSubtype ?? this.RemotePortIdSubtype,
                RemotePortDesc ?? this.RemotePortDesc,
                RemoteSysName ?? this.RemoteSysName,
                RemoteSysDesc ?? this.RemoteSysDesc,
                SysCapabilitiesSupported ?? this.SysCapabilitiesSupported,
                SysCapabilitiesEnabled ?? this.sysCapabilitiesEnabled,
                RemoteTTL ?? this.RemoteTTL,
                IpV4 ?? this.IpV4,
                EquipmentType ?? this.EquipmentType
                );
        }

        public void SetEquipementType(bool isNone = false)
        {
            if (isNone)
            {
                this.EquipmentType = EquipmentTypeEnum.None;
                return;
            }
            if ((this.sysCapabilitiesEnabled.FirstOrDefault() ?? "").Contains("bridge"))
            {
                this.EquipmentType = EquipmentTypeEnum.SWITCH;
            }
            else if ((this.sysCapabilitiesEnabled.FirstOrDefault() ?? "").Contains("router"))
            {
                this.EquipmentType = EquipmentTypeEnum.ROUTER;
            }
            else if (!Helper.IsValidMACAddress(this.ChassisId))
            {
                this.EquipmentType = EquipmentTypeEnum.WIN_11;
            }
            else if (Helper.IsValidMACAddress(this.ChassisId) && (!string.IsNullOrEmpty(this.IpV4) && !this.IpV4.Contains("No IpV4")))
            {
                this.EquipmentType = EquipmentTypeEnum.WIN_10;
            }
            else
            {
                this.EquipmentType = EquipmentTypeEnum.OTHER;
            }
        }

        public static EquipmentTypeEnum GetTypeFromString(string str, bool isNone = false)
        {
            if (isNone)
            {
                return EquipmentTypeEnum.None;
            }

            str = str.ToUpper();
            if (str == "SWITCH")
            {
                return EquipmentTypeEnum.SWITCH;
            }
            else if (str == "ROUTER")
            {
                return EquipmentTypeEnum.ROUTER;
            }
            else if (str == "WIN_11")
            {
                return EquipmentTypeEnum.WIN_11;
            }
            else if (str == "WIN_10")
            {
                return EquipmentTypeEnum.WIN_10;
            }
            else
            {
                return EquipmentTypeEnum.OTHER;
            }
        }
    }
}
