using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyNorka_Terekhov;

public class ApplicationContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkShift> WorkShifts { get; set; }
    public ApplicationContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        // optionsBuilder.UseSqlite("Data Source=Employees.db");
        optionsBuilder.UseMySql(
            "server=localhost; user = root; password = password; database= Employees;"
            ,
            new MySqlServerVersion(new Version(8, 0, 34))
        );
    }
}