using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 消息推送记录实体，记录向用户发送的通知（短信/邮件/站内信/推送）及发送状态。
/// </summary>
public class MsgPush : DataEntity
{
    /// <summary>推送记录 ID。</summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>消息类型：sms（短信）、email（邮件）、inner（站内信）、push（App 推送）。</summary>
    public string MsgType { get; set; } = string.Empty;
    /// <summary>消息标题。</summary>
    public string MsgTitle { get; set; } = string.Empty;
    /// <summary>消息正文内容。</summary>
    public string MsgContent { get; set; } = string.Empty;
    /// <summary>关联业务对象主键。</summary>
    public string? BizKey { get; set; }
    /// <summary>关联业务类型编码。</summary>
    public string? BizType { get; set; }
    /// <summary>接收方标识（手机号/邮箱/用户编码等）。</summary>
    public string ReceiveCode { get; set; } = string.Empty;
    /// <summary>接收人用户编码。</summary>
    public string ReceiveUserCode { get; set; } = string.Empty;
    /// <summary>接收人姓名。</summary>
    public string ReceiveUserName { get; set; } = string.Empty;
    /// <summary>发送人用户编码。</summary>
    public string SendUserCode { get; set; } = string.Empty;
    /// <summary>发送人姓名。</summary>
    public string SendUserName { get; set; } = string.Empty;
    /// <summary>发送时间。</summary>
    public DateTime SendDate { get; set; }
    /// <summary>是否为合并推送：1=是，0=否。</summary>
    public string? IsMergePush { get; set; }
    /// <summary>计划推送时间（定时推送）。</summary>
    public DateTime? PlanPushDate { get; set; }
    /// <summary>推送次数。</summary>
    public int? PushNumber { get; set; }
    /// <summary>推送返回编码（运营商/三方 SDK 返回码）。</summary>
    public string? PushReturnCode { get; set; }
    /// <summary>推送返回消息 ID（运营商/三方 SDK 返回）。</summary>
    public string? PushReturnMsgId { get; set; }
    /// <summary>推送返回详细内容/错误信息。</summary>
    public string? PushReturnContent { get; set; }
    /// <summary>推送状态：pending（待发送）、success（成功）、fail（失败）。</summary>
    public string? PushStatus { get; set; }
    /// <summary>实际推送完成时间。</summary>
    public DateTime? PushDate { get; set; }
    /// <summary>读取状态：read（已读）、unread（未读）。</summary>
    public string? ReadStatus { get; set; }
    /// <summary>读取时间。</summary>
    public DateTime? ReadDate { get; set; }
}
