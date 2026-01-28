using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class Dot1qSwPortConfigResponse
    {
        [JsonPropertyName("dot1q_sw_port_config")]
        public Dot1qSwPortConfig? Dot1qSwPortConfig { get; set; }

        [JsonPropertyName("resp")]
        public Response? Res { get; set; }
    }
}
