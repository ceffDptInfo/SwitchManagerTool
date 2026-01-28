using SW.Request.Common;
using System.Text.Json.Serialization;

namespace SW.Request.Login
{
    public class LoginResponse
    {
        [JsonPropertyName("resp")]
        public Response? Resp { get; set; }
        [JsonPropertyName("login")]
        public Login? Login { get; set; }
    }
}
