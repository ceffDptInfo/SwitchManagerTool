using SwitchesDll;

namespace Frontend.Providers
{

    public class SwitchState
    {
        public List<SwitchDB> Switches;
        public List<SwitchDB> SelectedSwitches;
        public ProviderDataStates DataState;
        public string ErrorMsg;

        public bool IsLoading { get { return DataState == ProviderDataStates.Loading; } }
        public bool IsError { get { return DataState == ProviderDataStates.Error; } }

        public SwitchState(List<SwitchDB> switches, List<SwitchDB> selectedSwitches, ProviderDataStates dataState, string errorMsg)
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
            DataState = ProviderDataStates.Empty;
            ErrorMsg = "";
        }

        public SwitchState CopyWith(
        List<SwitchDB>? Switches = null,
        List<SwitchDB>? SelectedSwitches = null,
        ProviderDataStates? DataState = null,
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
