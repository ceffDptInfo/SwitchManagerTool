using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class LoginResponse
    {
        [JsonPropertyName("resp")]
        public Response? Resp { get; set; }
        [JsonPropertyName("login")]
        public Login? Login { get; set; }
    }
}
