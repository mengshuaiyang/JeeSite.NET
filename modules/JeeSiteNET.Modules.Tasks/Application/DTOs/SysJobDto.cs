using JeeSiteNET.Modules.Tasks.Domain.Entities;

namespace JeeSiteNET.Modules.Tasks.Application.DTOs;

public class SysJobDto
{
    public string JobId { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string? JobGroup { get; set; }
    public string CronExpression { get; set; } = string.Empty;
    public string? AssemblyName { get; set; }
    public string? ClassName { get; set; }
    public string? Description { get; set; }
    public string? RunStatus { get; set; }
    public string? Status { get; set; }

    public static SysJobDto FromEntity(SysJob e) => new()
    {
        JobId = e.JobId, JobName = e.JobName, JobGroup = e.JobGroup,
        CronExpression = e.CronExpression, AssemblyName = e.AssemblyName,
        ClassName = e.ClassName, Description = e.Description,
        RunStatus = e.RunStatus, Status = e.Status
    };
}

public class SysJobSaveDto
{
    public string? JobId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string? JobGroup { get; set; } = "DEFAULT";
    public string CronExpression { get; set; } = string.Empty;
    public string? AssemblyName { get; set; }
    public string? ClassName { get; set; }
    public string? Description { get; set; }
}

public class JobLogDto
{
    public string LogId { get; set; } = string.Empty;
    public string? JobName { get; set; }
    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public long? Duration { get; set; }
    public DateTime? RunDate { get; set; }

    public static JobLogDto FromEntity(JobLog e) => new()
    {
        LogId = e.LogId, JobName = e.JobName, Result = e.Result,
        ErrorMessage = e.ErrorMessage, Duration = e.Duration, RunDate = e.RunDate
    };
}
