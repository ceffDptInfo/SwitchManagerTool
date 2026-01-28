using SW.Request.Common;
using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class SwitchPortConfigResponse
    {
        [JsonPropertyName("switchPortConfig")]
        public SwitchPortConfig? SwitchPortConfig { get; set; }

        [JsonPropertyName("resp")]
        public Response? Res { get; set; }
    }
}
