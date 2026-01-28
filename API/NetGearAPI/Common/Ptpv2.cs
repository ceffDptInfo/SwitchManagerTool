using SwitchesDll;
using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class Ptpv2
    {
        [JsonPropertyName("portid")]
        public int PortId { get; set; }
        [JsonPropertyName("adminMode")]
        public string AdminMode { get; set; } = "";
        [JsonPropertyName("operaMode")]
        public string OperaMode { get; set; } = "";
        EquipmentRootObject EquipmentRootObject
        {
            get; set;
        } = new EquipmentRootObject();
    }
}
