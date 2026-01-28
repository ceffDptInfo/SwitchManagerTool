using System.Text.Json.Serialization;

namespace SW.Request.Login
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
