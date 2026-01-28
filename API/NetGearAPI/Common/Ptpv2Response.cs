using SW.Request.Common;
using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class Ptpv2Response
    {
        [JsonPropertyName("ptpv2")]
        public Ptpv2[] Ptpv2 { get; set; } = new Ptpv2[0];
        [JsonPropertyName("resp")]
        public Response Resp { get; set; } = new Response();
    }
}
