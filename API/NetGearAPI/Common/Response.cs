using System.Text.Json.Serialization;

namespace SW.Request.Common
{
    public class Response
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("respCode")]
        public int RespCode { get; set; }
        [JsonPropertyName("respMsg")]
        public string RespMsg { get; set; } = "";
    }
}
