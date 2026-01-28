using SwitchesDll;

namespace Frontend.Providers
{
    public class SwitchProvider
    {
        static public SwitchProvider Instance = new SwitchProvider();

        public SwitchProvider(HttpClient? httpClient = null)
        {
            _state = new SwitchState();
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7140/api/");
        }

        public event EventHandler<EventArgs>? UpdatedContent;
        public SwitchState state { get => _state; }

        protected SwitchState _state;
        protected HttpClient _httpClient;

        public async Task<List<SwitchDB>?> FetchSwitches()
        {
            try
            {
                string url = "SwitchesDB/GetSwitches";
                List<SwitchDB>? switches = await _httpClient.GetFromJsonAsync<List<SwitchDB>>(url);

                _state = _state.CopyWith(Switches: switches);
                return switches;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Error, ErrorMsg: ex.Message);
                return null;
            }
            finally
            {
                UpdatedContent?.Invoke(_state, EventArgs.Empty);
            }
        }

        public async Task<SwitchDB?> SearchForSwitchFromIP(string ipOrHostName, string username, string password)
        {
            try
            {
                string url = $"PortActions/GetDeviceInfo?ipOrHostname={ipOrHostName}&username={username}&password={password}";
                Dictionary<string, Dictionary<string, object>> deviceInfo = await _httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, object>>>(url) ?? new();

                SwitchDB sw = new SwitchDB() { Ip = deviceInfo["deviceInfo"]["ipAddr"].ToString() ?? "No IP!", MacAdress = deviceInfo["deviceInfo"]["macAddr"].ToString() ?? "No MAC!", Name = deviceInfo["deviceName"]["name"].ToString() ?? "No name!", Password = password, Username = username };
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Loaded, ErrorMsg: "");

                return sw;
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Error, ErrorMsg: ex.Message);
                return null;
            }
            finally
            {
                UpdatedContent?.Invoke(null, EventArgs.Empty);
            }
        }

        public async Task AddSwitch(SwitchDB sw)
        {
            try
            {
                string url = "SwitchesDB/PostSwitch";

                if (!_ValidateSwitchForm(sw))
                {
                    throw new Exception("Please Complete Switch Information!");
                }

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, sw);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"HTTP ERROR: {response.StatusCode} : {response.Content}");
                }

                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Error, ErrorMsg: ex.Message);
            }
            finally
            {
                UpdatedContent?.Invoke(state, EventArgs.Empty);
            }
        }

        public async void UpdateSwitch(int id)
        {
            try
            {
                string url = $"SwitchesDB/PutSwitch?id={id}";

                SwitchDB? sw = _state.Switches.Where(s => s.Id == id).FirstOrDefault();
                if (sw == null)
                {
                    throw new Exception("No switch found to update!");
                }

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync<SwitchDB>(url, sw);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                _state = _state.CopyWith(DataState: _GetDataState(), ErrorMsg: "");
            }
            catch (Exception e)
            {
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Error, ErrorMsg: e.Message);
            }
            finally
            {
                UpdatedContent?.Invoke(null, EventArgs.Empty);
            }
        }

        public async Task DeleteSwitch(SwitchDB sw)
        {
            try
            {
                string url = $"SwitchesDB/DeleteSwitch?id={sw.Id}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    _state = _state.CopyWith(Switches: _state.Switches.Where(s => s.Id != sw.Id).ToList());
                }
            }
            catch (Exception ex)
            {
                _state = _state.CopyWith(DataState: SwitchProviderDataStates.Error, ErrorMsg: ex.Message);
            }
            finally
            {
                UpdatedContent?.Invoke(state, EventArgs.Empty);
            }

        }

        public void SelectSwitches(List<SwitchDB> selectedSwitches)
        {
            _state = _state.CopyWith(SelectedSwitches: selectedSwitches);
            UpdatedContent?.Invoke(_state, EventArgs.Empty);
        }

        public bool ContainsSwitch(string swIp)
        {
            return _state.Switches.Any(sw => sw.Ip == swIp);
        }

        protected SwitchProviderDataStates _GetDataState()
        {
            return _state.Switches.Count == 0 ? SwitchProviderDataStates.Empty : SwitchProviderDataStates.Loaded;
        }

        protected bool _ValidateSwitchForm(SwitchDB sw)
        {
            bool isValide = true;

            if (string.IsNullOrEmpty(sw.Ip))
            {
                isValide = false;
            }
            if (string.IsNullOrEmpty(sw.MacAdress))
            {
                isValide = false;
            }
            if (string.IsNullOrEmpty(sw.Name))
            {
                isValide = false;
            }
            if (string.IsNullOrEmpty(sw.Username))
            {
                isValide = false;
            }
            if (string.IsNullOrEmpty(sw.Password))
            {
                isValide = false;
            }

            return isValide;

        }
    }
}
