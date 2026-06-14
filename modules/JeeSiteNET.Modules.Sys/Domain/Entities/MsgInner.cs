using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 站内信消息实体，代表系统内部消息/公告/通知。按接收人类型区分用户/角色/部门/全员发送。
/// </summary>
public class MsgInner : DataEntity
{
    /// <summary>站内信 ID。</summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>消息标题。</summary>
    public string MsgTitle { get; set; } = string.Empty;
    /// <summary>消息等级：1=普通、2=重要、3=紧急，默认 1。</summary>
    public string ContentLevel { get; set; } = "1";
    /// <summary>内容类型：text / html / markdown。</summary>
    public string? ContentType { get; set; }
    /// <summary>消息正文内容。</summary>
    public string MsgContent { get; set; } = string.Empty;
    /// <summary>接收人类型：user（按用户）、role（按角色）、office（按部门）、all（全员）。</summary>
    public string ReceiveType { get; set; } = "user";
    /// <summary>接收人编码列表（逗号分隔）。</summary>
    public string? ReceiveCodes { get; set; }
    /// <summary>接收人姓名列表（逗号分隔，冗余便于展示）。</summary>
    public string? ReceiveNames { get; set; }
    /// <summary>发送人用户编码。</summary>
    public string? SendUserCode { get; set; }
    /// <summary>发送人姓名。</summary>
    public string? SendUserName { get; set; }
    /// <summary>发送时间。</summary>
    public DateTime? SendDate { get; set; }
    /// <summary>是否带附件：1=是，0=否。</summary>
    public string? IsAttac { get; set; }
    /// <summary>通知方式（如站内信+短信+邮件，逗号分隔）。</summary>
    public string? NotifyTypes { get; set; }
}
