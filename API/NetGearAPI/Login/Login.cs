using System.Text.Json.Serialization;

namespace SW.Request.Login
{
    public class Login
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
        [JsonPropertyName("expire")]
        public int Expire { get; set; }
    }
}
