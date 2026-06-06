using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Tasks.Domain.Entities;

public class JobLog : BaseEntity
{
    public string LogId { get; set; } = string.Empty;
    public string JobId { get; set; } = string.Empty;
    public string? JobName { get; set; }
    public string? JobGroup { get; set; }
    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public long? Duration { get; set; }
    public DateTime? RunDate { get; set; }
}
