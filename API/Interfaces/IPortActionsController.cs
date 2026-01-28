using APISignalR.NetGearAPI.Dot1qSwPortConfig;
using SW.ApiObjects.LldpRemoteDevices;
using SW.ApiObjects.SwcfgPort;

namespace APISignalR.Interfaces
{
    public interface IPortActionsController
    {

        public Task<SwitchPortConfigResponse?> GetSwitchPortConfig(int portnumber, string ip, string username, string password);

        public Task<LldpRemoteDevicesResponse?> GetLldpRemoteDevices(string ip, string username, string password);

        public Task<SwitchPortConfigResponse?> GetPortAdminMode(int portnumber, bool status, string ip, string username, string password);

        public Task<Dot1qSwPortConfigResponse?> GetDot1qSwPortConfig(int iInterface, string ip, string username, string password);

        public Task<Dot1qSwPortConfigResponse?> PostDot1qSwPortConfig(int iInterface, int vlanNumber, string ip, string username, string password);
    }
}
