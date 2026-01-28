using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class LoginRequest
    {
        [JsonPropertyName("login")]
        public LoginRequestSettings Login { get; set; }

        public LoginRequest(string username, string password)
        {
            this.Login = new LoginRequestSettings(username, password);
        }
    }
}
