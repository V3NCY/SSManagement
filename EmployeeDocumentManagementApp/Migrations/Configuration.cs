using System.Data.Entity.Migrations;

public class Configuration : DbMigrationsConfiguration<AppDbContext>
{
    public Configuration()
    {
        AutomaticMigrationsEnabled = true;
        AutomaticMigrationDataLossAllowed= true;
    }

    protected override void Seed(AppDbContext context)
    {
    }
}