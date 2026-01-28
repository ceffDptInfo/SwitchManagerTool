using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Frontend.NetgearAPI
{
    public class RtLimitBCast
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
    }
}
