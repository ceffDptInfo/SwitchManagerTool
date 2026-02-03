using Frontend.Model;

namespace Frontend.Providers
{
    public class EquipmentState
    {
        public List<DatagridContent> Equipments;

        public ProviderDataStates DataState;
        public string ErrorMsg;
        public bool IsLoading { get { return DataState == ProviderDataStates.Loading; } }
        public bool IsError { get { return DataState == ProviderDataStates.Error; } }


        public EquipmentState(List<DatagridContent> equipments, ProviderDataStates dataState, string errorMsg)
        {
            this.Equipments = equipments;
            this.DataState = dataState;
            this.ErrorMsg = errorMsg;
        }

        public EquipmentState()
        {
            Equipments = new();
            DataState = ProviderDataStates.Empty;
            ErrorMsg = "";
        }

        public EquipmentState CopyWith(
            List<DatagridContent>? Equipments = null,
            ProviderDataStates? DataState = null,
            string? ErrorMsg = null
            )
        {
            return new EquipmentState(
                Equipments ?? this.Equipments,
                DataState ?? this.DataState,
                ErrorMsg ?? this.ErrorMsg
                );
        }
    }
}
