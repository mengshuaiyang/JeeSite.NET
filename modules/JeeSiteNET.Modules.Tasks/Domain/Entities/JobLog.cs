    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
namespace JeeSiteNET.Modules.Tasks.Domain.Entities;

// 定义class JobLog
// 定义类：JobLog
public class JobLog : BaseEntity
{
    // 属性 LogId
    // 属性：LogId
    public string LogId { get; set; } = string.Empty;
    // 属性 JobId
    // 属性：JobId
    public string JobId { get; set; } = string.Empty;
    // 属性：JobName
    public string? JobName { get; set; }
    // 属性：JobGroup
    public string? JobGroup { get; set; }
    // 属性：Result
    public string? Result { get; set; }
    // 属性：ErrorMessage
    public string? ErrorMessage { get; set; }
    // 属性：Duration
    public long? Duration { get; set; }
    // 属性：RunDate
    public DateTime? RunDate { get; set; }
}
