using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Tasks.Domain.Entities;

public class SysJob : DataEntity
{
    public string JobId { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string? JobGroup { get; set; } = "DEFAULT";
    public string CronExpression { get; set; } = string.Empty;
    public string? AssemblyName { get; set; }
    public string? ClassName { get; set; }
    public string? Description { get; set; }
    public string? RunStatus { get; set; } = "0";
}
