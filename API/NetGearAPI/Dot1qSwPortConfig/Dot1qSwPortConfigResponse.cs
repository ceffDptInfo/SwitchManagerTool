using SW.Request.Common;
using System.Text.Json.Serialization;

namespace APISignalR.NetGearAPI.Dot1qSwPortConfig
{
    public class Dot1qSwPortConfigResponse
    {
        [JsonPropertyName("dot1q_sw_port_config")]
        public Dot1qSwPortConfig? Dot1qSwPortConfig { get; set; }

        [JsonPropertyName("resp")]
        public Response? Res { get; set; }
    }
}
