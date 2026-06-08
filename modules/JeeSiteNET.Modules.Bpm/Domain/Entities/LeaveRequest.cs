namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

public class LeaveRequest
{
    public string LeaveRequestId { get; set; } = string.Empty;
    public string Applicant { get; set; } = string.Empty;
    public string LeaveType { get; set; } = "annual";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double DurationDays { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "draft";
    public string? ManagerApprover { get; set; }
    public string? HrApprover { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
    public DateTime UpdateDate { get; set; }
}
