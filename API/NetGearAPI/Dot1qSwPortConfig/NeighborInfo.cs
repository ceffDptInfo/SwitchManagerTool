using System.Text.Json.Serialization;

namespace API.NetGearAPI.Dot1qSwPortConfig
{
    public class NeighborInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("capabilities")]
        public string Capabilities { get; set; } = string.Empty;
        [JsonPropertyName("chassisId")]
        public string ChassisId { get; set; } = string.Empty;
        [JsonPropertyName("chassisIdSubtype")]
        public int ChassisIdSubtype { get; set; }
        [JsonPropertyName("portId")]
        public string PortId { get; set; } = string.Empty;
        [JsonPropertyName("portIdSubtype")]
        public int PortIdSubtype { get; set; }
        [JsonPropertyName("portDescription")]
        public string PortDescription { get; set; } = string.Empty;
        [JsonPropertyName("mgmtIpAddress")]
        public string MgmtIpAddress { get; set; } = string.Empty;
    }
}
