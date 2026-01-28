using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class RtLimitBCast
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }
}
