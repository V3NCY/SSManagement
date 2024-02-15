using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Serializable]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    [Required(ErrorMessage = "EGN is required")]
    public int EGN { get; set; } 

    [Required(ErrorMessage = "Employee Name is required.")]
    public string EmployeeName => $"{FirstName} {LastName}";

    [Required(ErrorMessage = "Remaining Leave Days is required")]
    public int RemainingLeaveDays { get; set; }

    [Required(ErrorMessage = "Job Title is required")]
    public string JobTitle { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public string Department { get; set; }

    public bool IsArchived { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
    //[Timestamp]
    //public byte[] NewRowVersion { get; set; }
    public bool PaidLeave { get; set; }
    public bool UnpaidLeave { get; set; }
    public bool OtherLeave { get; set; }
}
