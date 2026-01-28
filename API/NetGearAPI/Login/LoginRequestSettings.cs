using System.Text.Json.Serialization;

namespace SW.Request.Login
{
    public class LoginRequestSettings
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }

        public LoginRequestSettings(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
