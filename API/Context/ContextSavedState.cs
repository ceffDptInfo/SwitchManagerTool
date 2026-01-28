using Microsoft.EntityFrameworkCore;
using SwitchesDll;

namespace API.Context
{
    public class ContextSavedState : DbContext
    {

        public DbSet<Switch> Switches { get; set; }
        public DbSet<Windows> Windows { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=sql_server,1433;Database=switch_port_managment;User Id=sa;Password=YourStrong!Password123;TrustServerCertificate=True;");
        }
    }
}
