using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

public class WorkflowForm : DataEntity
{
    public string FormId { get; set; } = string.Empty;
    public string WorkflowInstanceId { get; set; } = string.Empty;
    public string BusinessKey { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string? FormData { get; set; }
    public string? CurrentActivityId { get; set; }
    public string? CurrentAssignee { get; set; }
    public string? FormStatus { get; set; } = "pending";
}
