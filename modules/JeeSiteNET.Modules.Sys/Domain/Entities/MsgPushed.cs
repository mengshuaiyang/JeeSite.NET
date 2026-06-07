using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class MsgPushed : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string MsgType { get; set; } = string.Empty;
    public string MsgTitle { get; set; } = string.Empty;
    public string MsgContent { get; set; } = string.Empty;
    public string? BizKey { get; set; }
    public string? BizType { get; set; }
    public string ReceiveCode { get; set; } = string.Empty;
    public string ReceiveUserCode { get; set; } = string.Empty;
    public string ReceiveUserName { get; set; } = string.Empty;
    public string SendUserCode { get; set; } = string.Empty;
    public string SendUserName { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public string? IsMergePush { get; set; }
    public DateTime? PlanPushDate { get; set; }
    public int? PushNumber { get; set; }
    public string? PushReturnCode { get; set; }
    public string? PushReturnMsgId { get; set; }
    public string? PushReturnContent { get; set; }
    public string? PushStatus { get; set; }
    public DateTime? PushDate { get; set; }
    public string? ReadStatus { get; set; }
    public DateTime? ReadDate { get; set; }
}
