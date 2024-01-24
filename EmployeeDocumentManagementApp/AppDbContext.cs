using System.Data.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext() : base("YourConnectionStringName")
    {
        Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
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
