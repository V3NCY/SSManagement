using System;
using System.Data.Entity.Migrations;

public partial class RemoveRowVersionColumn : DbMigration
{
    public override void Up()
    {
        AddColumn("dbo.EmployeeManagement", "NewRowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
    }



    public override void Down()
    {
        AlterColumn("dbo.EmployeeManagement", "RowVersion", c => c.Binary(nullable: false));
    }
}
