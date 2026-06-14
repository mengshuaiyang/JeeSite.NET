    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
namespace JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义class ApprovalRecord
// 定义类：ApprovalRecord
public class ApprovalRecord : DataEntity
{
    // 属性 RecordId
    // 属性：RecordId
    public string RecordId { get; set; } = string.Empty;
    // 属性 WorkflowInstanceId
    // 属性：WorkflowInstanceId
    public string WorkflowInstanceId { get; set; } = string.Empty;
    // 属性 BusinessKey
    // 属性：BusinessKey
    public string BusinessKey { get; set; } = string.Empty;
    // 属性 BusinessType
    // 属性：BusinessType
    public string BusinessType { get; set; } = string.Empty;
    // 属性：ActivityId
    public string? ActivityId { get; set; }
    // 属性：ActivityName
    public string? ActivityName { get; set; }
    // 属性：Assignee
    public string? Assignee { get; set; }
    // 属性：AssigneeName
    public string? AssigneeName { get; set; }
    // 属性：Result
    public string? Result { get; set; }
    // 属性：Comment
    public string? Comment { get; set; }
    // 属性：CompletedDate
    public DateTime? CompletedDate { get; set; }
}
