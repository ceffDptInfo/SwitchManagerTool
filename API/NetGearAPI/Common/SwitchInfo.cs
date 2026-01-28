using System.Text.Json.Serialization;

namespace API.NetGearAPI.Common
{
    public class SwitchInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; } = "";

        [JsonPropertyName("macAddr")]
        public string MacAddr { get; set; } = "";

        [JsonPropertyName("model")]
        public string Model { get; set; } = "";

        [JsonPropertyName("lanIpAddress")]
        public string LanIpAddress { get; set; } = "";

        [JsonPropertyName("swVer")]
        public string SwVer { get; set; } = "";

        [JsonPropertyName("lastReboot")]
        public string LastReboot { get; set; } = "";

        [JsonPropertyName("numOfPorts")]
        public int NumOfPorts { get; set; }

        [JsonPropertyName("numOfActivePorts")]
        public int NumOfActivePorts { get; set; }

        [JsonPropertyName("rstpState")]
        public bool RstpState { get; set; }

        [JsonPropertyName("memoryUsed")]
        public string MemoryUsed { get; set; } = "";

        [JsonPropertyName("memoryUsage")]
        public string MemoryUsage { get; set; } = "";

        [JsonPropertyName("cpuUsage")]
        public string CpuUsage { get; set; } = "";

        [JsonPropertyName("fanState")]
        public string FanState { get; set; } = "";

        [JsonPropertyName("poeState")]
        public bool PoeState { get; set; }

        [JsonPropertyName("upTime")]
        public List<SensorState> temperatureSensors { get; set; } = new();

        [JsonPropertyName("bootVersion")]
        public string BootVersion { get; set; } = "";

        [JsonPropertyName("rxData")]
        public int RxData { get; set; }

        [JsonPropertyName("txData")]
        public int TxData { get; set; }

        [JsonPropertyName("adminPoePower")]
        public int AdminPoePower { get; set; }
    }

    public enum SensorState
    {
        NONE,
        NORMAL,
        WARNING,
        SHUTDOWN,
        NOT_PRESENT,
        NOT_OPERATIONAL,
    }

    public class TemperatureSensors
    {
        [JsonPropertyName("sensorNum")]
        public int SensorNum { get; set; }

        [JsonPropertyName("sensorDesc")]
        public int SensorDesc { get; set; }

        [JsonPropertyName("sensorTemp")]
        public string SensorTemp { get; set; } = "";

        [JsonPropertyName("sensorState")]
        public SensorState SensorState { get; set; } = SensorState.NONE;
    }
}

