using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class SwitchPortConfigResponse
    {
        [JsonPropertyName("switchPortConfig")]
        public SwitchPortConfig? SwitchPortConfig { get; set; }

        [JsonPropertyName("resp")]
        public Response? Res { get; set; }
    }
}
