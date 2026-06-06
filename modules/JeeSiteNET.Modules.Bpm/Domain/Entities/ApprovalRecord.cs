using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

public class ApprovalRecord : DataEntity
{
    public string RecordId { get; set; } = string.Empty;
    public string WorkflowInstanceId { get; set; } = string.Empty;
    public string BusinessKey { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public string? Assignee { get; set; }
    public string? AssigneeName { get; set; }
    public string? Result { get; set; }
    public string? Comment { get; set; }
    public DateTime? CompletedDate { get; set; }
}
