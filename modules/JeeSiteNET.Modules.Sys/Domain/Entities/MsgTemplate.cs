using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 消息模板实体，承载短信/邮件/站内信/推送通知的模板内容。
/// 发送消息时根据模板键（TplKey）动态填充变量后下发。
/// </summary>
public class MsgTemplate : DataEntity
{
    /// <summary>模板 ID。</summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>所属模块编码。</summary>
    public string? ModuleCode { get; set; }
    /// <summary>模板键（模板唯一标识符，如 user.forgotPwd）。</summary>
    public string TplKey { get; set; } = string.Empty;
    /// <summary>模板名称（显示用）。</summary>
    public string TplName { get; set; } = string.Empty;
    /// <summary>模板类型：sms（短信）、email（邮件）、inner（站内信）、push（推送）。</summary>
    public string TplType { get; set; } = string.Empty;
    /// <summary>模板内容（支持 ${variable} 占位符）。</summary>
    public string TplContent { get; set; } = string.Empty;
}
