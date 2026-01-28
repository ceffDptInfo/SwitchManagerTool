using System.Text.Json.Serialization;

namespace APISignalR.NetGearAPI.Dot1qSwPortConfig
{
    public class Dot1qSwPortConfigRequest
    {
        [JsonPropertyName("dot1q_sw_port_config")]
        public Dot1qSwPortConfigPost Dot1qSwPortConfigPost { get; set; }

        public Dot1qSwPortConfigRequest(Dot1qSwPortConfig dot1qSwPortConfig)
        {
            this.Dot1qSwPortConfigPost = new Dot1qSwPortConfigPost()
            {
                AccessVlan = dot1qSwPortConfig.AccessVlan,
                AllowedVlanList = dot1qSwPortConfig.AllowedVlanList,
                ConfigMode = dot1qSwPortConfig.ConfigMode,
                //NativeVlan = dot1qSwPortConfig.NativeVlan.ToString(),
                NativeVlan = dot1qSwPortConfig.NativeVlan
            };
        }
    }
}
