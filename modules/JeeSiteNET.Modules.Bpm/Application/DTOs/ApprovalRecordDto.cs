using JeeSiteNET.Modules.Bpm.Domain.Entities;

namespace JeeSiteNET.Modules.Bpm.Application.DTOs;

public class ApprovalRecordDto
{
    public string RecordId { get; set; } = string.Empty;
    public string WorkflowInstanceId { get; set; } = string.Empty;
    public string BusinessKey { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string? ActivityName { get; set; }
    public string? AssigneeName { get; set; }
    public string? Result { get; set; }
    public string? Comment { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CreateDate { get; set; }

    public static ApprovalRecordDto FromEntity(ApprovalRecord e) => new()
    {
        RecordId = e.RecordId, WorkflowInstanceId = e.WorkflowInstanceId,
        BusinessKey = e.BusinessKey, BusinessType = e.BusinessType,
        ActivityName = e.ActivityName, AssigneeName = e.AssigneeName,
        Result = e.Result, Comment = e.Comment,
        CompletedDate = e.CompletedDate, CreateDate = e.CreateDate
    };
}

public class ApprovalSubmitDto
{
    public string BusinessKey { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string? FormData { get; set; }
    public string? Comment { get; set; }
}

public class ApprovalActionDto
{
    public string RecordId { get; set; } = string.Empty;
    public string Result { get; set; } = "approved";
    public string? Comment { get; set; }
}
