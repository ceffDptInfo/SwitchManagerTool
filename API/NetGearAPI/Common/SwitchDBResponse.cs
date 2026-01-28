using SW.Request.Common;
using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class SwitchDBResponse
    {
        [JsonPropertyName("device_info")]
        public SwitchInfo Device { get; set; } = new();
        [JsonPropertyName("resp")]
        public Response Response { get; set; } = new Response();
    }
}
