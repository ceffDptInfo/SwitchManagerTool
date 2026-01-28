using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class LldpRemoteDevice
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("ifIndex")]
        public int IfIndex { get; set; }
        [JsonPropertyName("remoteId")]
        public int RemoteId { get; set; }
        [JsonPropertyName("chassisId")]
        public string ChassisId { get; set; } = "";
        [JsonPropertyName("chassisIdSubtype")]
        public int ChassisIdSubtype { get; set; }
        [JsonPropertyName("remotePortId")]
        public string RemotePortId { get; set; } = "";
        [JsonPropertyName("remotePortIdSubtype")]
        public int RemotePortIdSubtype { get; set; }
        [JsonPropertyName("remotePortDesc")]
        public string RemotePortDesc { get; set; } = "";
        [JsonPropertyName("remoteSysName")]
        public string RemoteSysName { get; set; } = "";
        [JsonPropertyName("remoteSysDesc")]
        public string RemoteSysDesc { get; set; } = "";
        [JsonPropertyName("sysCapabilitiesSupported")]
        public string[] SysCapabilitiesSupported { get; set; } = new string[0];
        [JsonPropertyName("sysCapabilitiesEnabled")]
        public string[] SysCapabilitiesEnabled { get; set; } = new string[0];
        [JsonPropertyName("remoteTTL")]
        public int RemoteTTL { get; set; }
        [JsonPropertyName("mgmtAddresses")]
        public MgmtAdress[] MgmtAddresses { get; set; } = new MgmtAdress[0];
    }
}
