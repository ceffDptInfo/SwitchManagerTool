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
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=switches_database; Trusted_Connection=True;");
        }
    }
}

