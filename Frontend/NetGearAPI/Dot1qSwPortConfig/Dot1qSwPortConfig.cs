using System.Text.Json.Serialization;

namespace Frontend.NetgearAPI
{
    public class Dot1qSwPortConfig
    {
        [JsonPropertyName("interface")]
        public int Interface { get; set; }
        [JsonPropertyName("accessVlan")]
        public int AccessVlan { get; set; }
        [JsonPropertyName("allowedVlanList")]
        public string[] AllowedVlanList { get; set; } = new string[0];
        [JsonPropertyName("dynamicallyAddedVlanList")]
        public string DynamicallyAddedVlanList { get; set; } = "";
        [JsonPropertyName("forbiddenVlanList")]
        public string[] ForbiddenVlanList { get; set; } = new string[0];
        [JsonPropertyName("configMode")]
        public string ConfigMode { get; set; } = "";
        [JsonPropertyName("nativeVlan")]
        public int NativeVlan { get; set; } = 0;
        [JsonPropertyName("taggedVlanList")]
        public string[] TaggedVlanList { get; set; } = new string[0];
        [JsonPropertyName("untaggedVlanList")]
        public string[] UnTaggedVlanList { get; set; } = new string[0];
    }
}
