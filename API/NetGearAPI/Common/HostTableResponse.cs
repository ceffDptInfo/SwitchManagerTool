using SW.Request.Common;
using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class HostTableResponse
    {
        [JsonPropertyName("hostTable")]
        public List<HostTable> HostTable { get; set; } = new();

        [JsonPropertyName("resp")]
        public Response Response { get; set; } = new Response();
    }
}
