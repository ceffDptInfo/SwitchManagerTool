using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class RtLimitMCast
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }
}
