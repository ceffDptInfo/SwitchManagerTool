using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
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
