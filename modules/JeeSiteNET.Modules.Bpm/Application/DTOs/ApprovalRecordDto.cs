    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
namespace JeeSiteNET.Modules.Bpm.Application.DTOs;

// 定义class ApprovalRecordDto
// 定义类：ApprovalRecordDto
public class ApprovalRecordDto
{
    // 属性 RecordId
    // 属性：RecordId
    public string RecordId { get; set; } = string.Empty;
    // 属性 WorkflowInstanceId
    // 属性：WorkflowInstanceId
    public string WorkflowInstanceId { get; set; } = string.Empty;
    // 属性 BusinessKey
    // 属性：BusinessKey
    public string BusinessKey { get; set; } = string.Empty;
    // 属性 BusinessType
    // 属性：BusinessType
    public string BusinessType { get; set; } = string.Empty;
    // 属性：ActivityName
    public string? ActivityName { get; set; }
    // 属性：AssigneeName
    public string? AssigneeName { get; set; }
    // 属性：Result
    public string? Result { get; set; }
    // 属性：Comment
    public string? Comment { get; set; }
    // 属性：CompletedDate
    public DateTime? CompletedDate { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static ApprovalRecordDto FromEntity(ApprovalRecord e) => new()
    {
        RecordId = e.RecordId, WorkflowInstanceId = e.WorkflowInstanceId,
        BusinessKey = e.BusinessKey, BusinessType = e.BusinessType,
        ActivityName = e.ActivityName, AssigneeName = e.AssigneeName,
        Result = e.Result, Comment = e.Comment,
        CompletedDate = e.CompletedDate, CreateDate = e.CreateDate
    };
}

// 定义class ApprovalSubmitDto
// 定义类：ApprovalSubmitDto
public class ApprovalSubmitDto
{
    // 属性 BusinessKey
    // 属性：BusinessKey
    public string BusinessKey { get; set; } = string.Empty;
    // 属性 BusinessType
    // 属性：BusinessType
    public string BusinessType { get; set; } = string.Empty;
    // 属性：FormData
    public string? FormData { get; set; }
    // 属性：Comment
    public string? Comment { get; set; }
}

// 定义class ApprovalActionDto
// 定义类：ApprovalActionDto
public class ApprovalActionDto
{
    // 属性 RecordId
    // 属性：RecordId
    public string RecordId { get; set; } = string.Empty;
    // 属性 Result
    // 属性：Result
    public string Result { get; set; } = "approved";
    // 属性：Comment
    public string? Comment { get; set; }
}
