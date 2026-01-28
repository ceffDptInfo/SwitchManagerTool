using SW.Request.Common;
using System.Text.Json.Serialization;

namespace API.NetGearAPI.Dot1qSwPortConfig
{
    public class SwitchPortStatsResponse
    {
        [JsonPropertyName("switchStatsPort")]
        public SwitchPortStats[] SwitchPortStats { get; set; } = new SwitchPortStats[0];

        [JsonPropertyName("resp")]
        public Response Response { get; set; } = new Response();
    }
}
