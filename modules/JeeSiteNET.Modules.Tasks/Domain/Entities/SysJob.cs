    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
namespace JeeSiteNET.Modules.Tasks.Domain.Entities;

// 定义class SysJob
// 定义类：SysJob
public class SysJob : DataEntity
{
    // 属性 JobId
    // 属性：JobId
    public string JobId { get; set; } = string.Empty;
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
    // 属性：RunStatus
    public string? RunStatus { get; set; } = "0";
}
