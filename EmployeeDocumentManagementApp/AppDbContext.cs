using System;
using System.Data.Entity;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public AppDbContext() : base("DBEmployees")
    {
        try
        {
            Database.Log = s => Debug.WriteLine(s);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex}");
        }
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().Ignore(e => e.RowVersion);
    }
}
