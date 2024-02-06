using System;
using System.Data.Entity;
using System.Diagnostics;
public class AppDbContext : DbContext
{
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

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().Property(e => e.RowVersion)
            .IsConcurrencyToken()
            .HasColumnType("timestamp")
            .IsRowVersion();
    }
}
