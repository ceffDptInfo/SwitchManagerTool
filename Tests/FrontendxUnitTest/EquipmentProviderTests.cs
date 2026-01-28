using Frontend.Model;
using Frontend.NetgearAPI;
using Frontend.NetGearAPI.Common;
using Frontend.Providers;
using Moq;
using Moq.Protected;
using SwitchesDll;
using System.Net;
using System.Text.Json;

namespace FrontendxUnitTest
{
    public class EquipmentProviderTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly EquipmentProvider _provider;
        private readonly SwitchProvider _fakeSwitchProvider;

        public EquipmentProviderTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://test-api.com/")
            };

            _fakeSwitchProvider = new SwitchProvider(null);
            _provider = new EquipmentProvider(httpClient, _fakeSwitchProvider);
        }

        [Fact]
        public async Task FetchLldpRemoteDevices_ShouldBeSuccessfull_WhenCalled()
        {
            var selectedSwitch = new SwitchDB
            {
                Id = 1,
                Ip = "192.168.1.1",
                Username = "student",
                Password = "1234"
            };

            _fakeSwitchProvider.SelectSwitches(new List<SwitchDB> { selectedSwitch });

            var lldpResponse = new LldpRemoteDevicesResponse
            {
                LldpRemoteDevices = new LldpRemoteDevice[]
                {
                    new LldpRemoteDevice
                    {
                        Id = 101,
                        IfIndex = 1,
                        RemoteId = 1,
                        ChassisId = "AA-BB-CC-DD-EE-FF",
                        ChassisIdSubtype = 1,
                        RemotePortId = "",
                        RemotePortIdSubtype = 1,
                        RemotePortDesc = "",
                        RemoteSysName = "PC-BD12-16",
                        RemoteSysDesc = "",
                        SysCapabilitiesSupported = new string[0],
                        SysCapabilitiesEnabled = new string[0],
                        RemoteTTL = 0,
                        MgmtAddresses = new MgmtAdress[] { new MgmtAdress{ Type = "IPv4", Address = "10.0.0.5"} }

                    }
                }
            };

            var vlanResponse = new Dictionary<int, LiteVlanConfig>
            {
                {101, new LiteVlanConfig(){PortId=101, Vlan = 20, VlanMode = "access", AdminMode=true} }
            };

            var offlineResponse = new Dictionary<int, List<object>>();

            SetupMockResponse("GetLldpRemoteDevices", JsonSerializer.Serialize(lldpResponse));
            SetupMockResponse("GetDot1qSwPortConfigCache", JsonSerializer.Serialize(vlanResponse));
            SetupMockResponse("GetOfflineEquipmentFromSwitch", JsonSerializer.Serialize(offlineResponse));

            await _provider.FetchLldpRemoteDevices();

            Assert.Equal(EquipmemtProviderDataStates.Loaded, _provider.state.DataState);
            Assert.Single(_provider.state.Equipments);

            var equipment = _provider.state.Equipments.First();
            Assert.True(equipment != null);
            Assert.Equal(101, equipment.Id);
            Assert.Equal("PC-BD12-16", equipment.RemoteSysName);
            Assert.Equal(20, equipment.AccessVlan);
            Assert.Equal("192.168.1.1", equipment.OwnerIp);

        }

        [Fact]
        public async Task SwitchPortModeTo_Success_ShouldCallApi()
        {
            var switchIp = "192.168.1.1";

            _fakeSwitchProvider.SelectSwitches(new List<SwitchDB> { new SwitchDB { Ip = switchIp, Username = "student", Password = "password" } });

            var fakeEquipment = new List<DatagridContent> { new DatagridContent { Id = 50, OwnerIp = switchIp } };

            SetPrivateState(_provider, fakeEquipment);

            SetupMockResponse("GetPortAdminMode", "", HttpStatusCode.OK);

            bool success = await _provider.SwitchPortModeTo(true, 50);

            Assert.True(success);

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri!.ToString().Contains("portnumber=50") &&
                req.RequestUri.ToString().Contains("status=True")),
                ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async Task SwitchPortModeTo_NoSwitchSelected_ReturnsFalse()
        {
            SetPrivateState(_provider, new List<DatagridContent> { new DatagridContent { Id = 1 } });

            bool result = await _provider.SwitchPortModeTo(true, 1);

            Assert.False(result);
            Assert.Equal(EquipmemtProviderDataStates.Error, _provider.state.DataState);
        }

        [Fact]
        public async Task ChangePortVlan_ShouldReturnTrue_WhenAPIResponseIsSuccessfull()
        {
            int id = 1;
            int newVlan = 2;
            _fakeSwitchProvider.SelectSwitches(new List<SwitchDB> { new SwitchDB { Id = id, Ip = "192.168.1.1", } });
            SetPrivateState(_provider, new List<DatagridContent> { new DatagridContent { AccessVlan = 1, OwnerIp = "192.168.1.1" } });
            SetupMockResponse("PostDot1qSwPortConfig", "{}", HttpStatusCode.OK);

            bool result = await _provider.ChangePortVlan(id, newVlan);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOfflineEquipment_ShouldReturnTrue_WhenAPIResponseIsSuccessfull()
        {
            int id = 1;

            _fakeSwitchProvider.SelectSwitches(new List<SwitchDB> { new SwitchDB { Id = id, Ip = "192.168.1.1", } });
            SetPrivateState(_provider, new List<DatagridContent> { new DatagridContent { AccessVlan = 1, OwnerIp = "192.168.1.1" } });
            SetupMockResponse("DeleteOfflineEquipment", "true", HttpStatusCode.OK);

            bool result = await _provider.DeleteOfflineEquipment(id, DatagridContent.EquipmentTypeEnum.WIN_11);

            Assert.True(result);
        }

        [Fact]
        public async Task SaveSwitchPortAndChild_ShouldReturnTrue_WhenAPIResponseIsSuccessfull()
        {
            int id = 1;

            _fakeSwitchProvider.SelectSwitches(new List<SwitchDB> { new SwitchDB { Id = id, Ip = "192.168.1.1", } });
            SetPrivateState(_provider, new List<DatagridContent> { new DatagridContent { AccessVlan = 1, OwnerIp = "192.168.1.1" } });
            SetupMockResponse("PostEquipment", "{}", HttpStatusCode.OK);

            bool result = await _provider.SaveSwitchPortAndChild(id, DatagridContent.EquipmentTypeEnum.WIN_11, new EquipmentRootObject());

            Assert.True(result);
        }



        private void SetupMockResponse(string urlPart, string jsonResponse, HttpStatusCode status = HttpStatusCode.OK)
        {
            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => (req.RequestUri!.ToString()).Contains(urlPart)),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage { StatusCode = status, Content = new StringContent(jsonResponse) });
        }

        private void SetPrivateState(EquipmentProvider provider, List<DatagridContent> equipments)
        {
            var newState = new EquipmentState(equipments, EquipmemtProviderDataStates.Loaded, "");

            var field = typeof(EquipmentProvider).GetField("_state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(provider, newState);
            }
        }

    }
}
