using System;

public class LeaveRequest
{
    public int LeaveRequestId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool PaidLeave { get; set; }
    public bool UnpaidLeave { get; set; }
    public bool OtherLeave { get; set; }
}
