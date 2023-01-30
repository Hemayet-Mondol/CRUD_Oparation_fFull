using CRUD_Oparation_PC.Models;
using Microsoft.EntityFrameworkCore;
namespace CRUD_Oparation_PC.Data
{
    public class CRUDDbContext: DbContext
    {
        public CRUDDbContext(DbContextOptions<CRUDDbContext> options):base(options)
        {
            
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
