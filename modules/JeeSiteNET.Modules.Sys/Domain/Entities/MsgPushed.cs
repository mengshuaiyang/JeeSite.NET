    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class MsgPushed
// 定义类：MsgPushed
public class MsgPushed : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 MsgType
    // 属性：MsgType
    public string MsgType { get; set; } = string.Empty;
    // 属性 MsgTitle
    // 属性：MsgTitle
    public string MsgTitle { get; set; } = string.Empty;
    // 属性 MsgContent
    // 属性：MsgContent
    public string MsgContent { get; set; } = string.Empty;
    // 属性：BizKey
    public string? BizKey { get; set; }
    // 属性：BizType
    public string? BizType { get; set; }
    // 属性 ReceiveCode
    // 属性：ReceiveCode
    public string ReceiveCode { get; set; } = string.Empty;
    // 属性 ReceiveUserCode
    // 属性：ReceiveUserCode
    public string ReceiveUserCode { get; set; } = string.Empty;
    // 属性 ReceiveUserName
    // 属性：ReceiveUserName
    public string ReceiveUserName { get; set; } = string.Empty;
    // 属性 SendUserCode
    // 属性：SendUserCode
    public string SendUserCode { get; set; } = string.Empty;
    // 属性 SendUserName
    // 属性：SendUserName
    public string SendUserName { get; set; } = string.Empty;
    // 属性 SendDate
    // 属性：SendDate
    public DateTime SendDate { get; set; }
    // 属性：IsMergePush
    public string? IsMergePush { get; set; }
    // 属性：PlanPushDate
    public DateTime? PlanPushDate { get; set; }
    // 属性：PushNumber
    public int? PushNumber { get; set; }
    // 属性：PushReturnCode
    public string? PushReturnCode { get; set; }
    // 属性：PushReturnMsgId
    public string? PushReturnMsgId { get; set; }
    // 属性：PushReturnContent
    public string? PushReturnContent { get; set; }
    // 属性：PushStatus
    public string? PushStatus { get; set; }
    // 属性：PushDate
    public DateTime? PushDate { get; set; }
    // 属性：ReadStatus
    public string? ReadStatus { get; set; }
    // 属性：ReadDate
    public DateTime? ReadDate { get; set; }
}
