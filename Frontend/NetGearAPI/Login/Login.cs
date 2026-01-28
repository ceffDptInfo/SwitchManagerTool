using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class Login
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
        [JsonPropertyName("expires")]
        public int Expires { get; set; }
    }
}
