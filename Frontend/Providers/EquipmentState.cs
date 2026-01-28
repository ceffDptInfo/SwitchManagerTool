using Frontend.Model;

namespace Frontend.Providers
{
    public enum EquipmemtProviderDataStates
    {
        Loading,
        Loaded,
        Empty,
        Error,
    }
    public class EquipmentState
    {
        public List<DatagridContent> Equipments;

        public EquipmemtProviderDataStates DataState;
        public string ErrorMsg;
        public bool IsLoading { get { return DataState == EquipmemtProviderDataStates.Loading; } }
        public bool IsError { get { return DataState == EquipmemtProviderDataStates.Error; } }


        public EquipmentState(List<DatagridContent> equipments, EquipmemtProviderDataStates dataState, string errorMsg)
        {
            this.Equipments = equipments;
            this.DataState = dataState;
            this.ErrorMsg = errorMsg;
        }

        public EquipmentState()
        {
            Equipments = new();
            DataState = EquipmemtProviderDataStates.Empty;
            ErrorMsg = "";
        }

        public EquipmentState CopyWith(
            List<DatagridContent>? Equipments = null,
            EquipmemtProviderDataStates? DataState = null,
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
