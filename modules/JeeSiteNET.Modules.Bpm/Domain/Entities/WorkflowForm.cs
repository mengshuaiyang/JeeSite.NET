    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义class WorkflowForm
// 定义类：WorkflowForm
public class WorkflowForm : DataEntity
{
    // 属性 FormId
    // 属性：FormId
    public string FormId { get; set; } = string.Empty;
    // 属性 WorkflowInstanceId
    // 属性：WorkflowInstanceId
    public string WorkflowInstanceId { get; set; } = string.Empty;
    // 属性 BusinessKey
    // 属性：BusinessKey
    public string BusinessKey { get; set; } = string.Empty;
    // 属性 BusinessType
    // 属性：BusinessType
    public string BusinessType { get; set; } = string.Empty;
    // 属性：FormData
    public string? FormData { get; set; }
    // 属性：CurrentActivityId
    public string? CurrentActivityId { get; set; }
    // 属性：CurrentAssignee
    public string? CurrentAssignee { get; set; }
    // 属性：FormStatus
    public string? FormStatus { get; set; } = "pending";
}
