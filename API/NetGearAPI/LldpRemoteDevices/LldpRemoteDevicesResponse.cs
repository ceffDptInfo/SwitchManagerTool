using SW.Request.Common;
using System.Text.Json.Serialization;

namespace SW.ApiObjects.LldpRemoteDevices
{
    public class LldpRemoteDevicesResponse
    {
        [JsonPropertyName("lldp_remote_devices")]
        public LldpRemoteDevice[] LldpRemoteDevices { get; set; } = new LldpRemoteDevice[0];
        [JsonPropertyName("resp")]
        public Response? Resp { get; set; }
    }
}
