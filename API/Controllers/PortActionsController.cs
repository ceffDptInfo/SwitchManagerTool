using API.Helpers;
using API.NetGearAPI.Common;
using API.NetGearAPI.Dot1qSwPortConfig;
using API.Singletons;
using APISignalR.Interfaces;
using APISignalR.NetGearAPI.Dot1qSwPortConfig;
using Microsoft.AspNetCore.Mvc;
using SW.ApiObjects.LldpRemoteDevices;
using SW.ApiObjects.SwcfgPort;
using SW.Request.Common;
using System.Net;

namespace APISignalR.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PortActionsController : ControllerBase, IPortActionsController
    {
        private readonly HttpClientHandler handler = new HttpClientHandler();
        private readonly HttpClient httpClient;

        //DI
        private readonly Dot1qSwPortConfigSingleton _dot1QSwPortConfigSingleton;
        public PortActionsController(API.Context.ContextSavedState _dbcontext, BearerTokenSingleton _bearerTokenSingleton,
            Dot1qSwPortConfigSingleton dot1QSwPortConfigSingleton)
        {
            //Avoid TLS error
            handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
            this.httpClient = new HttpClient(handler);
            this._dot1QSwPortConfigSingleton = dot1QSwPortConfigSingleton;
        }



        private string GetUrl(string ip, string endpoint)
        {
            return $"https://{ip}:8443/api/v1/{endpoint}";
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void BearerTokenGestionAsync(string ip, string username, string password)
        {
            username = username.Replace(' ', '+');
            password = password.Replace(' ', '+');

            SetBasicToHttp(username, password);

        }



        [ApiExplorerSettings(IgnoreApi = true)]
        protected void SetBasicToHttp(string username, string password)
        {
            username = Helper.Decrypt(username, Helper.PublicKey);
            password = Helper.Decrypt(password, Helper.PublicKey);
            string authString = $"{username}:{password}";
            var base64EncodedAuthString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authString));
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthString);
        }




        // Get the config of a port
        [HttpGet("GetSwitchPortConfig")]
        public async Task<SwitchPortConfigResponse?> GetSwitchPortConfig(int portnumber, string ip, string username, string password)
        {
            try
            {
                string url = GetUrl(ip, $"swcfg_port?portid={portnumber}");
                BearerTokenGestionAsync(ip, username, password);
                return await httpClient.GetFromJsonAsync<SwitchPortConfigResponse>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }


        // Get all lldp devices on the switch
        [HttpGet("GetLldpRemoteDevices")]
        public async Task<LldpRemoteDevicesResponse?> GetLldpRemoteDevices(string ip, string username, string password)
        {
            try
            {

                BearerTokenGestionAsync(ip, username, password);
                string url = GetUrl(ip, "lldp_remote_devices");
                HttpResponseMessage resultLldp = await httpClient.GetAsync(url);

                var result = await resultLldp.Content.ReadFromJsonAsync<LldpRemoteDevicesResponse>();



                foreach (var item in result!.LldpRemoteDevices)
                {
                    IPHostEntry? response;
                    try
                    {
                        response = await Dns.GetHostEntryAsync(item.ChassisId);
                    }
                    catch
                    {
                        response = null;
                    }
                    string addr = response != null ? response.AddressList[0].ToString() : "No IpV4";
                    if (item.MgmtAddresses.Length == 0)
                    {
                        item.MgmtAddresses = [new MgmtAdress() { Type = "IPv4", Address = addr }];
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new LldpRemoteDevicesResponse();
            }
        }

        //Not used, is too slow, 52 ports take 1 - 2 minutes
        [HttpGet("GetSwitchPortStats")]
        public async Task<SwitchPortStats[]> GetSwitchPortStats(string ip, string username, string password)
        {


            try
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                BearerTokenGestionAsync(ip, username, password);
                string url = GetUrl(ip, "sw_portstats?portid=ALL");
                var response = await httpClient.GetFromJsonAsync<SwitchPortStatsResponse>(url);
                var portStats = response?.SwitchPortStats;

                return portStats ?? new SwitchPortStats[0];

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new SwitchPortStats[0];
            }
        }

        // Set the value of AdminMode on the port to TRUE or FALSE. If FALSE no connexion on the port.
        [HttpGet("GetPortAdminMode")]
        public async Task<SwitchPortConfigResponse?> GetPortAdminMode(int portnumber, bool status, string ip, string username, string password)
        {
            try
            {
                SwitchPortConfigResponse? portConfigResp = await GetSwitchPortConfig(portnumber, ip, username, password);
                if (portConfigResp == null || portConfigResp.SwitchPortConfig == null)
                {
                    return null;
                }

                portConfigResp.SwitchPortConfig.AdminMode = status;
                SwitchPortConfigRequest switchPortConfigRequest = new SwitchPortConfigRequest(portConfigResp.SwitchPortConfig);

                BearerTokenGestionAsync(ip, username, password);
                string url = GetUrl(ip, $"swcfg_port?portid={portnumber}");
                HttpResponseMessage resultPortConfig = await httpClient.PostAsJsonAsync(url, switchPortConfigRequest);

                try
                {
                    HttpClient clientHttpClient = new HttpClient(handler)
                    {
                        BaseAddress = new Uri("https://localhost:7168/api/"),
                        Timeout = TimeSpan.FromSeconds(1.2f),
                    };
                    string datachange_url = GetUrl(ip, "DataChange/GetLldpPortUpdated");
                    await clientHttpClient.GetAsync(datachange_url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return await resultPortConfig.Content.ReadFromJsonAsync<SwitchPortConfigResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        // Get the information relative to Vlan on the switch
        [HttpGet("GetDot1qSwPortConfig")]
        public async Task<Dot1qSwPortConfigResponse?> GetDot1qSwPortConfig(int iInterface, string ip, string username, string password)
        {
            try
            {
                // ChangeUrlIfNotSame(ip);
                BearerTokenGestionAsync(ip, username, password);
                string url = GetUrl(ip, $"dot1q_sw_port_config?interface={iInterface}");
                var resp = await httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, dynamic>>>(url) ?? new();

                var dot1SwPortConfig = new Dot1qSwPortConfigResponse
                {
                    Dot1qSwPortConfig = new Dot1qSwPortConfig
                    {
                        Interface = Helper.SafeGet<int>(resp["dot1q_sw_port_config"]["interface"]),
                        AccessVlan = Helper.SafeGet<int>(resp["dot1q_sw_port_config"]["accessVlan"]),
                        AllowedVlanList = Helper.SafeGet<string[]>(resp["dot1q_sw_port_config"]["allowedVlanList"]),
                        DynamicallyAddedVlanList = Helper.SafeGet<string>(resp["dot1q_sw_port_config"]["dynamicallyAddedVlanList"]),
                        ForbiddenVlanList = Helper.SafeGet<string[]>(resp["dot1q_sw_port_config"]["forbiddenVlanList"]),
                        ConfigMode = Helper.SafeGet<string>(resp["dot1q_sw_port_config"]["configMode"]),
                        NativeVlan = Helper.SafeGet<int>(resp["dot1q_sw_port_config"]["nativeVlan"]),
                        TaggedVlanList = Helper.SafeGet<string[]>(resp["dot1q_sw_port_config"]["taggedVlanList"]),
                        UnTaggedVlanList = Helper.SafeGet<string[]>(resp["dot1q_sw_port_config"]["untaggedVlanList"]),
                    },
                    Res = new Response
                    {
                        Status = Helper.SafeGet<string>(resp["resp"]["status"]),
                        RespCode = Helper.SafeGet<int>(resp["resp"]["respCode"]),
                        RespMsg = Helper.SafeGet<string>(resp["resp"]["respMsg"])
                    }
                };

                return dot1SwPortConfig;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        [HttpGet("GetDot1qSwPortConfigCache")]
        public Dictionary<int, LiteVlanConfig>? GetDot1qSwPortConfigCache(string ip)
        {
            return _dot1QSwPortConfigSingleton.GetStoredDot1qSwPortConfig(ip);
        }


        [HttpGet("GetDeviceInfo")]
        public async Task<Dictionary<string, Dictionary<string, dynamic>>> GetDeviceInfo(string ipOrHostname, string username, string password)
        {
            try
            {
                var ip_response = await Dns.GetHostAddressesAsync(ipOrHostname);

                if (ip_response == null)
                {
                    return new();
                }

                string ip = ip_response[0].ToString();


                BearerTokenGestionAsync(ip, Helper.Encrypt(username, Helper.PublicKey), Helper.Encrypt(password, Helper.PublicKey));
                string url = GetUrl(ip, "device_info");
                var response = await httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, dynamic>>>(url);

                BearerTokenGestionAsync(ip, Helper.Encrypt(username, Helper.PublicKey), Helper.Encrypt(password, Helper.PublicKey));
                url = GetUrl(ip, "device_name");
                var name_response = await httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, dynamic>>>(url);


                if (response == null || name_response == null)
                {
                    return new();
                }
                response["deviceInfo"].Add("ipAddr", ip);
                response.Add("deviceName", name_response["deviceName"]);

                return response;

            }
            catch
            {
                return new();
            }
        }

        // Update the Vlan number
        [HttpPost("PostDot1qSwPortConfig")]
        public async Task<Dot1qSwPortConfigResponse?> PostDot1qSwPortConfig(int iInterface, int vlanNumber, string ip, string username, string password)
        {
            try
            {
                Dot1qSwPortConfigResponse? dot1qSwPortConfigResp = await GetDot1qSwPortConfig(iInterface, ip, username, password);
                if (dot1qSwPortConfigResp == null || dot1qSwPortConfigResp.Dot1qSwPortConfig == null)
                {
                    return null;
                }

                //From TPI of Paul Pereitha
                dot1qSwPortConfigResp.Dot1qSwPortConfig.AccessVlan = vlanNumber;
                //From TPI of Paul Pereitha
                //for not sending the big list of allowed Vlan
                dot1qSwPortConfigResp.Dot1qSwPortConfig.AllowedVlanList = new[] { "all" };
                //From TPI of Paul Pereitha
                dot1qSwPortConfigResp.Dot1qSwPortConfig.ConfigMode = "access";
                //From TPI of Paul Pereitha
                dot1qSwPortConfigResp.Dot1qSwPortConfig.NativeVlan = 1;

                _dot1QSwPortConfigSingleton.ChangeVlan(ip, iInterface, vlanNumber);
                string url = GetUrl(ip, $"dot1q_sw_port_config?interface={iInterface}");
                Dot1qSwPortConfigRequest dot1qSwPortConfigRequest = new Dot1qSwPortConfigRequest(dot1qSwPortConfigResp.Dot1qSwPortConfig);
                HttpResponseMessage resultdot1qSwPortConfig = await httpClient.PostAsJsonAsync(url, dot1qSwPortConfigRequest);

                try
                {
                    HttpClient clientHttpClient = new HttpClient(handler)
                    {
                        BaseAddress = new Uri("https://localhost:7168/api/"),
                        Timeout = TimeSpan.FromSeconds(1.2f),
                    };

                    await clientHttpClient.GetAsync($"DataChange/GetVlanUpdated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return await resultdot1qSwPortConfig.Content.ReadFromJsonAsync<Dot1qSwPortConfigResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private async Task TriggerFrontendUpdate()
        {
            await new HttpClient().GetAsync($"https://localhost:7168/api/DataChange/GetVlanUpdated");
        }

        private bool IsValidIpAddr(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            if (IPAddress.TryParse(input, out _))
            {
                return true;
            }

            return false;
        }
    }
}
