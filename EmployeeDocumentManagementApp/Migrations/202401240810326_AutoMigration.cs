using System;
using System.Data.Entity.Migrations;

public partial class AutoMigration : DbMigration
{
    public override void Up()
    {
        CreateTable(
            "dbo.Employees",
            c => new
            {
                EmployeeId = c.Int(nullable: false, identity: true),
                FirstName = c.String(nullable: false),
                LastName = c.String(nullable: false),
                EGN = c.Int(nullable: false),
                EmployeeName = c.String(nullable: false),
                RemainingLeaveDays = c.Int(nullable: false),
                JobTitle = c.String(nullable: false),
                Department = c.String(nullable: false),
                IsArchived = c.Boolean(nullable: false),
                RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
            })
            .PrimaryKey(t => t.EmployeeId);

    }

    public override void Down()
    {
        DropColumn("dbo.Employees", "RowVersion");
    }
}