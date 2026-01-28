namespace API.NetGearAPI.Common
{
    public class LiteVlanConfig
    {
        public int PortId { get; set; }
        public int Vlan { get; set; }
        public string VlanMode { get; set; } = "";
        public bool AdminMode { get; set; }

        public LiteVlanConfig(int portId, int vlan, string mode, bool adminMode)
        {
            PortId = portId;
            Vlan = vlan;
            VlanMode = mode;
            AdminMode = adminMode;
        }
        public LiteVlanConfig() { }
    }
}
