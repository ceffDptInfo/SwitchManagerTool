using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class HostTable
    {
        [JsonPropertyName("ipAddr")]
        public string IpAddr { get; set; } = "";
        [JsonPropertyName("macAddr")]
        public string MacAddr { get; set; } = "";
        [JsonPropertyName("vlanId")]
        public int VlanId { get; set; }
        [JsonPropertyName("port")]
        public int Port { get; set; }
    }
}
