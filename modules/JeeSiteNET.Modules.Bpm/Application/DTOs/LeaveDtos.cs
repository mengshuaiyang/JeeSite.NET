    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
namespace JeeSiteNET.Modules.Bpm.Application.DTOs;

// 定义class SubmitLeaveDto
// 定义类：SubmitLeaveDto
public class SubmitLeaveDto
{
    // 属性 Applicant
    // 属性：Applicant
    public string Applicant { get; set; } = string.Empty;
    // 属性 LeaveType
    // 属性：LeaveType
    public string LeaveType { get; set; } = "annual";
    // 属性 StartDate
    // 属性：StartDate
    public DateTime StartDate { get; set; }
    // 属性 EndDate
    // 属性：EndDate
    public DateTime EndDate { get; set; }
    // 属性：Reason
    public string? Reason { get; set; }
    // 属性 ManagerApprover
    // 属性：ManagerApprover
    public string ManagerApprover { get; set; } = string.Empty;
    // 属性：ManagerName
    public string? ManagerName { get; set; }
    // 属性 HrApprover
    // 属性：HrApprover
    public string HrApprover { get; set; } = string.Empty;
    // 属性：HrName
    public string? HrName { get; set; }
}

// 定义class ApproveLeaveDto
// 定义类：ApproveLeaveDto
public class ApproveLeaveDto
{
    // 属性 LeaveRequestId
    // 属性：LeaveRequestId
    public string LeaveRequestId { get; set; } = string.Empty;
    // 属性 Step
    // 属性：Step
    public string Step { get; set; } = "manager";
    // 属性 Result
    // 属性：Result
    public string Result { get; set; } = "approved";
    // 属性：Comment
    public string? Comment { get; set; }
    // 属性 Operator
    // 属性：Operator
    public string Operator { get; set; } = string.Empty;
    // 属性：RecordId
    public string? RecordId { get; set; }
    // 属性：NextAssigneeName
    public string? NextAssigneeName { get; set; }
}

// 定义class LeaveRequestDto
// 定义类：LeaveRequestDto
public class LeaveRequestDto
{
    // 属性 LeaveRequestId
    // 属性：LeaveRequestId
    public string LeaveRequestId { get; set; } = string.Empty;
    // 属性 Applicant
    // 属性：Applicant
    public string Applicant { get; set; } = string.Empty;
    // 属性 LeaveType
    // 属性：LeaveType
    public string LeaveType { get; set; } = string.Empty;
    // 属性 StartDate
    // 属性：StartDate
    public DateTime StartDate { get; set; }
    // 属性 EndDate
    // 属性：EndDate
    public DateTime EndDate { get; set; }
    // 属性 DurationDays
    // 属性：DurationDays
    public double DurationDays { get; set; }
    // 属性：Reason
    public string? Reason { get; set; }
    // 属性 Status
    // 属性：Status
    public string Status { get; set; } = string.Empty;
    // 属性：ManagerApprover
    public string? ManagerApprover { get; set; }
    // 属性：HrApprover
    public string? HrApprover { get; set; }
    // 属性：SubmitDate
    public DateTime? SubmitDate { get; set; }
    // 属性：CompleteDate
    public DateTime? CompleteDate { get; set; }
    // 属性 CreateDate
    // 属性：CreateDate
    public DateTime CreateDate { get; set; }
    // 属性：History
    public List<ApprovalRecordDto>? History { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static LeaveRequestDto FromEntity(LeaveRequest e) => new()
    {
        LeaveRequestId = e.LeaveRequestId, Applicant = e.Applicant,
        LeaveType = e.LeaveType, StartDate = e.StartDate, EndDate = e.EndDate,
        DurationDays = e.DurationDays, Reason = e.Reason, Status = e.Status,
        ManagerApprover = e.ManagerApprover, HrApprover = e.HrApprover,
        SubmitDate = e.SubmitDate, CompleteDate = e.CompleteDate, CreateDate = e.CreateDate
    };
}

// 定义class LeaveListRequest
// 定义类：LeaveListRequest
public class LeaveListRequest
{
    // 属性：Applicant
    public string? Applicant { get; set; }
    // 属性：Approver
    public string? Approver { get; set; }
}
