    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;

// 定义 JeeSiteNET.Modules.Tasks.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Application.DTOs
namespace JeeSiteNET.Modules.Tasks.Application.DTOs;

// 定义class SysJobDto
// 定义类：SysJobDto
public class SysJobDto
{
    // 属性 JobId
    // 属性：JobId
    public string JobId { get; set; } = string.Empty;
    // 属性 JobName
    // 属性：JobName
    public string JobName { get; set; } = string.Empty;
    // 属性：JobGroup
    public string? JobGroup { get; set; }
    // 属性 CronExpression
    // 属性：CronExpression
    public string CronExpression { get; set; } = string.Empty;
    // 属性：AssemblyName
    public string? AssemblyName { get; set; }
    // 属性：ClassName
    public string? ClassName { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：RunStatus
    public string? RunStatus { get; set; }
    // 属性：Status
    public string? Status { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static SysJobDto FromEntity(SysJob e) => new()
    {
        JobId = e.JobId, JobName = e.JobName, JobGroup = e.JobGroup,
        CronExpression = e.CronExpression, AssemblyName = e.AssemblyName,
        ClassName = e.ClassName, Description = e.Description,
        RunStatus = e.RunStatus, Status = e.Status
    };
}

// 定义class SysJobSaveDto
// 定义类：SysJobSaveDto
public class SysJobSaveDto
{
    // 属性：JobId
    public string? JobId { get; set; }
    // 属性 JobName
    // 属性：JobName
    public string JobName { get; set; } = string.Empty;
    // 属性：JobGroup
    public string? JobGroup { get; set; } = "DEFAULT";
    // 属性 CronExpression
    // 属性：CronExpression
    public string CronExpression { get; set; } = string.Empty;
    // 属性：AssemblyName
    public string? AssemblyName { get; set; }
    // 属性：ClassName
    public string? ClassName { get; set; }
    // 属性：Description
    public string? Description { get; set; }
}

// 定义class JobLogDto
// 定义类：JobLogDto
public class JobLogDto
{
    // 属性 LogId
    // 属性：LogId
    public string LogId { get; set; } = string.Empty;
    // 属性：JobName
    public string? JobName { get; set; }
    // 属性：Result
    public string? Result { get; set; }
    // 属性：ErrorMessage
    public string? ErrorMessage { get; set; }
    // 属性：Duration
    public long? Duration { get; set; }
    // 属性：RunDate
    public DateTime? RunDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static JobLogDto FromEntity(JobLog e) => new()
    {
        LogId = e.LogId, JobName = e.JobName, Result = e.Result,
        ErrorMessage = e.ErrorMessage, Duration = e.Duration, RunDate = e.RunDate
    };
}
