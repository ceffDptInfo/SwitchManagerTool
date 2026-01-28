using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class ContextSwitch : DbContext
    {

        public DbSet<SwitchesDll.SwitchDB> SwitchDB { get; set; }
        public DbSet<SwitchesDll.Equipment> Equipments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=sql_server,1433;Database=switches_database;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True;");
        }
    }
}

