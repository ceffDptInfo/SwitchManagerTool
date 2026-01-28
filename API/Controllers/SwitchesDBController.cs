using API.Context;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwitchesDll;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class SwitchesDBController : ControllerBase
    {
        private readonly ContextSwitch _dbcontext;

        public SwitchesDBController(ContextSwitch _dbcontext)
        {
            this._dbcontext = _dbcontext;
        }

        [HttpGet("GetSwitch")]
        public async Task<SwitchDB?> GetSwitch(int id)
        {
            try
            {
                SwitchDB? switchElement = await _dbcontext.SwitchDB.Include(s => s.Equipments).FirstOrDefaultAsync(s => s.Id == id);

                if (switchElement == null)
                {
                    return null;
                }

                return switchElement;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        [HttpGet("GetSwitches")]
        public async Task<List<SwitchDB>?> GetSwitches()
        {
            try
            {
                return await _dbcontext.SwitchDB.Include(s => s.Equipments).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<SwitchDB>();
            }
        }

        [HttpGet("test")]
        public string Test(int id)
        {
            string ps = GetSwitch(id).Result?.Password ?? "";
            string usr = GetSwitch(id).Result?.Username ?? "";

            return $"{Helper.Decrypt(ps, Helper.PublicKey)} {Helper.Decrypt(usr, Helper.PublicKey)}";
        }

        [HttpPost("PostSwitch")]
        public async Task<bool> PostSwitch(SwitchDB switchElement)
        {
            try
            {
                switchElement.Password = Helper.Encrypt(switchElement.Password, Helper.PublicKey);
                switchElement.Username = Helper.Encrypt(switchElement.Username, Helper.PublicKey);
                await _dbcontext.SwitchDB.AddAsync(switchElement);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        [HttpPost("PostSwitches")]
        public async Task<bool> PostSwitches(List<SwitchDB> switchElements)
        {
            try
            {
                foreach (SwitchDB switchElement in switchElements)
                {
                    switchElement.Password = Helper.Encrypt(switchElement.Password, Helper.PublicKey);
                    switchElement.Username = Helper.Encrypt(switchElement.Username, Helper.PublicKey);
                }
                await _dbcontext.SwitchDB.AddRangeAsync(switchElements);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        [HttpDelete("DeleteSwitch")]
        public async Task<bool> DeleteSwitch(int id)
        {
            try
            {
                SwitchDB? db = await _dbcontext.SwitchDB.FindAsync(id);
                if (db == null)
                {
                    return true;
                }
                _dbcontext.SwitchDB.Remove(db);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        [HttpPut("PutSwitch")]
        public async Task<bool> PutSwitch(int id, SwitchDB switchDB)
        {
            try
            {
                SwitchDB? sw = await _dbcontext.SwitchDB.Include(s => s.Equipments).FirstOrDefaultAsync(s => s.Id == id);
                if (sw == null)
                {
                    return false;
                }
                sw.Ip = switchDB.Ip;
                sw.MacAdress = switchDB.MacAdress;
                sw.Name = switchDB.Name;
                sw.Equipments.Clear();
                foreach (var equipment in switchDB.Equipments)
                {
                    sw.Equipments.Add(equipment);
                }
                await _dbcontext.SaveChangesAsync();
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
