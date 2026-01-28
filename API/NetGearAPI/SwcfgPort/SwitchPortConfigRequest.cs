using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class SwitchPortConfigRequest
    {
        [JsonPropertyName("switchPortConfig")]
        public SwitchPortConfig SwitchPortConfig { get; set; }

        public SwitchPortConfigRequest(SwitchPortConfig switchPortConfig)
        {
            this.SwitchPortConfig = switchPortConfig;
        }
    }
}
