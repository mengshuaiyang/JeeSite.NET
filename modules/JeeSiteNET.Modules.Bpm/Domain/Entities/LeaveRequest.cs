// 定义 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义class LeaveRequest
// 定义类：LeaveRequest
public class LeaveRequest
{
    // 属性 LeaveRequestId
    // 属性：LeaveRequestId
    public string LeaveRequestId { get; set; } = string.Empty;
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
    // 属性 DurationDays
    // 属性：DurationDays
    public double DurationDays { get; set; }
    // 属性：Reason
    public string? Reason { get; set; }
    // 属性 Status
    // 属性：Status
    public string Status { get; set; } = "draft";
    // 属性：ManagerApprover
    public string? ManagerApprover { get; set; }
    // 属性：HrApprover
    public string? HrApprover { get; set; }
    // 属性：SubmitDate
    public DateTime? SubmitDate { get; set; }
    // 属性：CompleteDate
    public DateTime? CompleteDate { get; set; }
    // 属性 CreateBy
    // 属性：CreateBy
    public string CreateBy { get; set; } = string.Empty;
    // 属性 CreateDate
    // 属性：CreateDate
    public DateTime CreateDate { get; set; }
    // 属性 UpdateBy
    // 属性：UpdateBy
    public string UpdateBy { get; set; } = string.Empty;
    // 属性 UpdateDate
    // 属性：UpdateDate
    public DateTime UpdateDate { get; set; }
}
