using Microsoft.EntityFrameworkCore;
using SwitchesDll;

namespace API.Context
{
    public class ContextSwitch : DbContext
    {
        public ContextSwitch()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public DbSet<SwitchDB> SwitchDB { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=sql_server,1433;Database=switches_database;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True;");
        }
    }
}

