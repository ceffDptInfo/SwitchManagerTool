using Microsoft.AspNetCore.Mvc;
using SwitchesDll;

namespace API.Interfaces
{
    public interface IOfflineEquipment
    {
        public Task<Dictionary<int, List<object>>>? GetOfflineEquipmentFromSwitch(int switchDBId);
        public Task<bool> PostEquipment([FromQuery] int portNumb,
            [FromBody] EquipmentRootObject equipmentRootObject, [FromQuery] int equipmentType);

        public Task<bool> DeleteOfflineEquipment(int switchId, int portNumb, int equipmentType);
    }
}
