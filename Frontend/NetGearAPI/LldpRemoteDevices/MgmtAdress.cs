using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class MgmtAdress
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
        [JsonPropertyName("address")]
        public string Address { get; set; } = "";
    }
}
