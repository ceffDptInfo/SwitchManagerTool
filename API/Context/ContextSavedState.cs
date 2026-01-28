using Microsoft.EntityFrameworkCore;
using SwitchesDll;
using System.Reflection.Metadata;

namespace API.Context
{
    public class ContextSavedState : DbContext
    {
        public ContextSavedState() 
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

        public DbSet<Switch> Switches { get; set; }
        public DbSet<Windows> Windows { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=switch_port_managment; Trusted_Connection=True;");
        }
    }
}
