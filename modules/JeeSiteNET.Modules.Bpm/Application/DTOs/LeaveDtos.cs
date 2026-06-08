using JeeSiteNET.Modules.Bpm.Domain.Entities;

namespace JeeSiteNET.Modules.Bpm.Application.DTOs;

public class SubmitLeaveDto
{
    public string Applicant { get; set; } = string.Empty;
    public string LeaveType { get; set; } = "annual";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
    public string ManagerApprover { get; set; } = string.Empty;
    public string? ManagerName { get; set; }
    public string HrApprover { get; set; } = string.Empty;
    public string? HrName { get; set; }
}

public class ApproveLeaveDto
{
    public string LeaveRequestId { get; set; } = string.Empty;
    public string Step { get; set; } = "manager";
    public string Result { get; set; } = "approved";
    public string? Comment { get; set; }
    public string Operator { get; set; } = string.Empty;
    public string? RecordId { get; set; }
    public string? NextAssigneeName { get; set; }
}

public class LeaveRequestDto
{
    public string LeaveRequestId { get; set; } = string.Empty;
    public string Applicant { get; set; } = string.Empty;
    public string LeaveType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double DurationDays { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ManagerApprover { get; set; }
    public string? HrApprover { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public DateTime CreateDate { get; set; }
    public List<ApprovalRecordDto>? History { get; set; }

    public static LeaveRequestDto FromEntity(LeaveRequest e) => new()
    {
        LeaveRequestId = e.LeaveRequestId, Applicant = e.Applicant,
        LeaveType = e.LeaveType, StartDate = e.StartDate, EndDate = e.EndDate,
        DurationDays = e.DurationDays, Reason = e.Reason, Status = e.Status,
        ManagerApprover = e.ManagerApprover, HrApprover = e.HrApprover,
        SubmitDate = e.SubmitDate, CompleteDate = e.CompleteDate, CreateDate = e.CreateDate
    };
}

public class LeaveListRequest
{
    public string? Applicant { get; set; }
    public string? Approver { get; set; }
}
