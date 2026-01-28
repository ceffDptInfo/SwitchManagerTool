using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class RtLimitUCast
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }
}
