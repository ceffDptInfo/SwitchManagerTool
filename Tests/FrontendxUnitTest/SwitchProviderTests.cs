using Frontend.Providers;
using Moq;
using Moq.Protected;
using SwitchesDll;
using System.Net;
using System.Text.Json;

namespace FrontendxUnitTest
{

    public class SwitchProviderTests
    {

        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly SwitchProvider _provider;

        public SwitchProviderTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            _provider = new SwitchProvider(_httpClient);
        }

        [Fact]
        public async Task FetchSwitches_Success_ShouldUpdateState()
        {
            var fakeSwitches = new List<SwitchDB>
            {
                new SwitchDB {Id = 1, Name = "Switch1", Ip = "192.168.1.1" },
                new SwitchDB {Id = 2, Name = "Switch2", Ip = "192.168.1.2"}
            };

            var jsonResponse = JsonSerializer.Serialize(fakeSwitches);
            SetupMockApiResponse(HttpStatusCode.OK, jsonResponse);

            var result = await _provider.FetchSwitches();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, _provider.state.Switches.Count);
        }

        [Fact]
        public async Task FetchSwitches_ShouldSetErrorState_WhenApiReturnsAnError()
        {
            SetupMockApiResponse(HttpStatusCode.InternalServerError, "Server Error");

            var result = await _provider.FetchSwitches();

            Assert.Null(result);
            Assert.Equal(SwitchProviderDataStates.Error, _provider.state.DataState);
            Assert.False(string.IsNullOrEmpty(_provider.state.ErrorMsg));
        }

        [Fact]
        public async Task AddSwitch_ShouldSetErrorState_WhenSwitchValidationFails()
        {
            var invalidSwitch = new SwitchDB();

            await _provider.AddSwitch(invalidSwitch);

            Assert.Equal(SwitchProviderDataStates.Error, _provider.state.DataState);
            Assert.False(string.IsNullOrEmpty(_provider.state.ErrorMsg));
        }

        [Fact]
        public async Task AddSwitch_ShouldAddTheSwitchToTheProvider_WhenIsSuccess()
        {
            var newSwitch = new SwitchDB()
            {
                Ip = "1.1.1.1",
                MacAdress = "AA-BB-CC-DD",
                Name = "Switch",
                Username = "user",
                Password = "password"
            };

            SetupMockApiResponse(HttpStatusCode.OK, "{}");

            await _provider.AddSwitch(newSwitch);

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
                );

            Assert.NotEqual(SwitchProviderDataStates.Error, _provider.state.DataState);
        }

        [Fact]
        public async void ContainsSwitch_ShouldReturnTrue_WhenIpExists()
        {
            var fakeSwitches = new List<SwitchDB>
            {
                new SwitchDB {Ip = "10.0.0.1"}
            };

            SetupMockApiResponse(HttpStatusCode.OK, JsonSerializer.Serialize(fakeSwitches));

            await _provider.FetchSwitches();

            bool exists = _provider.ContainsSwitch("10.0.0.1");
            bool doesNotExist = _provider.ContainsSwitch("99.99.99.99");
            Assert.True(exists);
            Assert.False(doesNotExist);
        }

        [Fact]
        public async Task SearchForSwitchFromIp_ShouldReturnSwitch_WhenSwitchIsFound()
        {
            string ip = "192.168.1.10";
            string macAddr = "AA-BB-CC-DD-EE-FF";
            string user = "student";
            string password = "password";
            string name = "SW-BD12-01";

            var successResponse = new
            {
                deviceInfo = new { ipAddr = ip, macAddr = macAddr },
                deviceName = new { name = name },
            };

            SetupMockApiResponse(HttpStatusCode.OK, JsonSerializer.Serialize(successResponse));


            var result = await _provider.SearchForSwitchFromIP(ip, user, password);

            Assert.NotNull(result);
            Assert.Equal(ip, result.Ip);
            Assert.Equal(name, result.Name);
            Assert.Equal(macAddr, result.MacAdress);
            Assert.Equal(user, result.Username);
            Assert.Equal(password, result.Password);

        }

        [Fact]
        public async Task DeleteSwitch()
        {
            var fakeSwitch = new SwitchDB
            {
                Ip = "1.1.1.1",
                MacAdress = "AA-BB-CC-DD",
                Name = "Switch",
                Username = "user",
                Password = "password"
            };

            SetupMockApiResponse(HttpStatusCode.OK, "{}");

            await _provider.DeleteSwitch(fakeSwitch);

            Assert.Equal(SwitchProviderDataStates.Empty, _provider.state.DataState);
        }


        private void SetupMockApiResponse(HttpStatusCode statusCode, string content)
        {
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }

    }
}
