using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class Response
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("respCode")]
        public int RespCode { get; set; } = 0;
        [JsonPropertyName("respMsg")]
        public string RespMsg { get; set; } = "";
    }
}
