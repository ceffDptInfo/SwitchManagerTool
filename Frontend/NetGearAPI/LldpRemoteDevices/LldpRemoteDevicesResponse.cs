using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class LldpRemoteDevicesResponse
    {
        [JsonPropertyName("lldp_remote_devices")]
        public LldpRemoteDevice[] LldpRemoteDevices { get; set; } = new LldpRemoteDevice[0];
        [JsonPropertyName("resp")]
        public Response? Resp { get; set; }
    }
}
