using SwitchesDll;

namespace Frontend.Providers
{
    public enum SwitchProviderDataStates
    {
        Loading,
        Loaded,
        Empty,
        Error,
    }
    public class SwitchState
    {
        public List<SwitchDB> Switches;
        public List<SwitchDB> SelectedSwitches;
        public SwitchProviderDataStates DataState;
        public string ErrorMsg;

        public bool IsLoading { get { return DataState == SwitchProviderDataStates.Loading; } }
        public bool IsError { get { return DataState == SwitchProviderDataStates.Error; } }

        public SwitchState(List<SwitchDB> switches, List<SwitchDB> selectedSwitches, SwitchProviderDataStates dataState, string errorMsg)
        {
            Switches = switches;
            SelectedSwitches = selectedSwitches;
            DataState = dataState;
            ErrorMsg = errorMsg;
        }

        public SwitchState()
        {
            Switches = new List<SwitchDB>();
            SelectedSwitches = new List<SwitchDB>();
            DataState = SwitchProviderDataStates.Empty;
            ErrorMsg = "";
        }

        public SwitchState CopyWith(
        List<SwitchDB>? Switches = null,
        List<SwitchDB>? SelectedSwitches = null,
        SwitchProviderDataStates? DataState = null,
        string? ErrorMsg = null
        )
        {
            return new SwitchState(
                Switches ?? this.Switches,
                SelectedSwitches ?? this.SelectedSwitches,
                DataState ?? this.DataState,
                ErrorMsg ?? this.ErrorMsg
                );
        }

    }
}
