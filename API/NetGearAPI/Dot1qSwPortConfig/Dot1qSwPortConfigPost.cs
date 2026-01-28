using System.Text.Json.Serialization;

namespace APISignalR.NetGearAPI.Dot1qSwPortConfig
{
    public class Dot1qSwPortConfigPost
    {
        [JsonPropertyName("accessVlan")]
        public int AccessVlan { get; set; }
        [JsonPropertyName("allowedVlanList")]
        public string[] AllowedVlanList { get; set; } = new string[0];
        [JsonPropertyName("configMode")]
        public string ConfigMode { get; set; } = "";
        [JsonPropertyName("nativeVlan")]
        public int NativeVlan { get; set; }
    }
}
