using System.Text.Json.Serialization;

namespace SW.ApiObjects.LldpRemoteDevices
{
    public class MgmtAdress
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
        [JsonPropertyName("address")]
        public string Address { get; set; } = "";
    }
}
