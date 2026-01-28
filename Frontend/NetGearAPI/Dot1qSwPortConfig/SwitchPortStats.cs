using System.Text.Json.Serialization;

namespace Frontend.NetGearAPI.Dot1qSwPortConfig
{
    public class SwitchPortStats
    {
        [JsonPropertyName("portId")]
        public int PortId { get; set; }
        [JsonPropertyName("myDesc")]
        public string MyDesc { get; set; } = string.Empty;
        [JsonPropertyName("adminMode")]
        public bool AdminMode { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("mode")]
        public int Mode { get; set; }
        [JsonPropertyName("vlans")]
        public int[] Vlans { get; set; } = new int[0];
        [JsonPropertyName("trafficRx")]
        public int TrafficRx { get; set; }
        [JsonPropertyName("trafficTx")]
        public int TrafficTx { get; set; }
        [JsonPropertyName("rxMbps")]
        public string RxMbps { get; set; } = string.Empty;
        [JsonPropertyName("txMbps")]
        public string TxMpbs { get; set; } = string.Empty;
        [JsonPropertyName("ctcErrorsRx")]
        public int CtcErrorsRx { get; set; }
        [JsonPropertyName("errorsRxTx")]
        public int ErrorsRxTx { get; set; }
        [JsonPropertyName("dropsRxTx")]
        public int DropsRxTx { get; set; }
        [JsonPropertyName("portMacAddress")]
        public string PortMacAddress { get; set; } = string.Empty;
        [JsonPropertyName("speed")]
        public int Speed { get; set; }
        [JsonPropertyName("duplex")]
        public int Duplex { get; set; }
        [JsonPropertyName("frameSize")]
        public int FrameSize { get; set; }
        [JsonPropertyName("stpStatus")]
        public bool StpStatus { get; set; }
        [JsonPropertyName("portState")]
        public int PortState { get; set; }
        [JsonPropertyName("oprState")]
        public int oprState { get; set; }
        [JsonPropertyName("poeState")]
        public int PoeState { get; set; }
        [JsonPropertyName("powerLimitClass")]
        public int PowerLimitClass { get; set; }
        [JsonPropertyName("neighborInfo")]
        public NeighborInfo NeighborInfo { get; set; } = new NeighborInfo();
        [JsonPropertyName("portAuthState")]
        public int PortAuthState { get; set; }
    }
}
