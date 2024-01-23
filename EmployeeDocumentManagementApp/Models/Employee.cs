using System.Windows.Media;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;

[Serializable]
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int EGN { get; set; }
    public string EmployeeName { get; set; }
    public int RemainingLeaveDays { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public bool IsArchived { get; set; }
    public byte[] RowVersion { get; set; }
}
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }


    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().Property(e => e.RowVersion).IsConcurrencyToken();
    }
}