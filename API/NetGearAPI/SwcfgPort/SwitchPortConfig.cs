using System.Text.Json.Serialization;

namespace SW.ApiObjects.SwcfgPort
{
    public class SwitchPortConfig
    {
        [JsonPropertyName("ID")]
        public int Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        [JsonPropertyName("portType")]
        public int PortType { get; set; }
        [JsonPropertyName("adminMode")]
        public bool AdminMode { get; set; }
        [JsonPropertyName("portSpeed")]
        public int PortSpeed { get; set; }
        [JsonPropertyName("duplexMode")]
        public int DuplexMode { get; set; }
        [JsonPropertyName("linkStatus")]
        public int LinkStatus { get; set; }
        [JsonPropertyName("linkTrap")]
        public bool LinkTrap { get; set; }
        [JsonPropertyName("maxFrameSize")]
        public int MaxFrameSize { get; set; }
        [JsonPropertyName("txRate")]
        public int TxRate { get; set; }
        [JsonPropertyName("rtlimitUcast")]
        public RtLimitUCast? RtLimitUCast { get; set; }
        [JsonPropertyName("rtlimitMcast")]
        public RtLimitMCast? RtLimitMCast { get; set; }
        [JsonPropertyName("rtlimitBcast")]
        public RtLimitBCast? RtLimitBCast { get; set; }
        [JsonPropertyName("portVlanId")]
        public int PortVlanId { get; set; }
        [JsonPropertyName("defVlanPrio")]
        public int DefVlanPrio { get; set; }
        [JsonPropertyName("isPoE")]
        public bool IsPoE { get; set; }
        [JsonPropertyName("scheduleName")]
        public string ScheduleName { get; set; } = "";
    }
}
