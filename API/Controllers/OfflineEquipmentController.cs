using API.Context;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwitchesDll;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OfflineEquipmentController : ControllerBase, IOfflineEquipment
    {
        private readonly API.Context.ContextSavedState _dbcontextSaved;
        private readonly API.Context.ContextSwitch _dbcontextSwitch;

        public OfflineEquipmentController(API.Context.ContextSavedState _dbcontextSaved, ContextSwitch _dbcontextSwitch)
        {
            this._dbcontextSaved = _dbcontextSaved;
            this._dbcontextSwitch = _dbcontextSwitch;
        }

        [HttpGet("GetOfflineEquipmentFromSwitch")]
        public async Task<Dictionary<int, List<object>>>? GetOfflineEquipmentFromSwitch(int switchDBId)
        {
            Dictionary<int, List<object>> dict = new Dictionary<int, List<object>>();
            List<object>? windows = new List<object>();
            List<object>? switchs = new List<object>();
            windows.AddRange((await _dbcontextSaved.Windows.ToListAsync()).Where(x => x.SwitchDBId == switchDBId));
            switchs.AddRange((await _dbcontextSaved.Switches.ToListAsync()).Where(x => x.SwitchDBId == switchDBId));
            dict.Add(1, windows);
            dict.Add(2, switchs);

            return dict;
        }

        [HttpPost("PostEquipment")]
        public async Task<bool> PostEquipment([FromQuery] int portNumb,
            [FromBody] EquipmentRootObject equipmentRootObject, [FromQuery] int equipmentType)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                if (equipmentType == 0 || equipmentType == 1)
                {
                    string jsonEquipment = JsonSerializer.Serialize(equipmentRootObject.Equipment);
                    string jsonSwitchDb = JsonSerializer.Serialize(equipmentRootObject.UpperSwitchDB);

                    Windows? windows = JsonSerializer.Deserialize<Windows>(jsonEquipment, options);
                    SwitchDB? switchDB = JsonSerializer.Deserialize<SwitchDB>(jsonSwitchDb, options);

                    if (windows == null || switchDB == null)
                    {
                        return false;
                    }

                    windows.PortNumber = portNumb;

                    SwitchDB? switchFromDb = (await _dbcontextSwitch.SwitchDB.ToListAsync()).Where(x => x.MacAdress == switchDB.MacAdress)
                        .FirstOrDefault();

                    if (switchFromDb == null)
                    {
                        return false;
                    }
                    windows.SwitchDBId = switchFromDb.Id;

                    await _dbcontextSaved.AddAsync(windows);
                    _dbcontextSaved.SaveChanges();
                }
                else if (equipmentType == 1)
                {
                    string json = JsonSerializer.Serialize(equipmentRootObject.Equipment);
                    Switch? switchFromJson = JsonSerializer.Deserialize<Switch>(json, options);
                    if (switchFromJson == null)
                    {
                        return false;
                    }
                    switchFromJson.PortNumber = portNumb;

                    await _dbcontextSaved.AddAsync(switchFromJson);
                    _dbcontextSaved.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        [HttpDelete("DeleteOfflineEquipment")]
        public async Task<bool> DeleteOfflineEquipment(int switchId, int portNumb, int equipmentType)
        {
            try
            {
                if (equipmentType == 0 || equipmentType == 1)
                {
                    List<Windows>? windows = (await _dbcontextSaved.Windows.ToListAsync()).Where(x => x.PortNumber == portNumb && x.SwitchDBId == switchId).ToList();
                    if (windows != null)
                    {
                        _dbcontextSaved.Windows.RemoveRange(windows);
                        await _dbcontextSaved.SaveChangesAsync();
                    }
                }
                else if (equipmentType == 1)
                {
                    Switch? switchEq = (await _dbcontextSaved.Switches.ToListAsync()).Where(x => x.PortNumber == portNumb && x.SwitchDBId == switchId).FirstOrDefault();
                    if (switchEq != null)
                    {
                        _dbcontextSaved.Switches.Remove(switchEq);
                        await _dbcontextSaved.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
