namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 站内信消息 DTO。
/// </summary>
public class MsgInnerDto
{
    /// <summary>
    /// 消息标识（主键）。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题。
    /// </summary>
    public string MsgTitle { get; set; } = string.Empty;

    /// <summary>
    /// 内容重要级别（normal/urgent 等）。
    /// </summary>
    public string? ContentLevel { get; set; }

    /// <summary>
    /// 内容类型（text/html/markdown 等）。
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// 消息正文。
    /// </summary>
    public string? MsgContent { get; set; }

    /// <summary>
    /// 接收范围类型（user/role/dept/all 等）。
    /// </summary>
    public string? ReceiveType { get; set; }

    /// <summary>
    /// 发送者用户编码。
    /// </summary>
    public string? SendUserCode { get; set; }

    /// <summary>
    /// 发送者姓名。
    /// </summary>
    public string? SendUserName { get; set; }

    /// <summary>
    /// 发送时间。
    /// </summary>
    public DateTime? SendDate { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 通知方式（逗号分隔，站内信/邮件/短信等）。
    /// </summary>
    public string? NotifyTypes { get; set; }
}

/// <summary>
/// 站内信保存请求 DTO。
/// </summary>
public class MsgInnerSaveDto
{
    /// <summary>
    /// 消息标识；空表示新建。
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 消息标题。
    /// </summary>
    public string MsgTitle { get; set; } = string.Empty;

    /// <summary>
    /// 内容重要级别。
    /// </summary>
    public string? ContentLevel { get; set; }

    /// <summary>
    /// 内容类型。
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// 消息正文。
    /// </summary>
    public string MsgContent { get; set; } = string.Empty;

    /// <summary>
    /// 接收范围类型。
    /// </summary>
    public string? ReceiveType { get; set; }

    /// <summary>
    /// 接收者具体编码（逗号分隔）。
    /// </summary>
    public string? ReceiveCodes { get; set; }

    /// <summary>
    /// 通知方式。
    /// </summary>
    public string? NotifyTypes { get; set; }

    /// <summary>
    /// 是否携带附件（1 是 / 0 否）。
    /// </summary>
    public string? IsAttac { get; set; }
}

/// <summary>
/// 站内信接收记录 DTO。
/// </summary>
public class MsgInnerRecordDto
{
    /// <summary>
    /// 接收记录标识（主键）。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 所属消息标识。
    /// </summary>
    public string MsgInnerId { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户编码。
    /// </summary>
    public string ReceiveUserCode { get; set; } = string.Empty;

    /// <summary>
    /// 接收者姓名。
    /// </summary>
    public string ReceiveUserName { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0 未读 / 1 已读）。
    /// </summary>
    public string ReadStatus { get; set; } = "0";

    /// <summary>
    /// 阅读时间。
    /// </summary>
    public DateTime? ReadDate { get; set; }

    /// <summary>
    /// 是否加星（1 是 / 0 否）。
    /// </summary>
    public string? IsStar { get; set; }
}

/// <summary>
/// 推送消息 DTO。
/// </summary>
public class MsgPushDto
{
    /// <summary>
    /// 推送消息标识（主键）。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（pc/app/sms 等）。
    /// </summary>
    public string? MsgType { get; set; }

    /// <summary>
    /// 推送标题。
    /// </summary>
    public string? MsgTitle { get; set; }

    /// <summary>
    /// 推送内容。
    /// </summary>
    public string? MsgContent { get; set; }

    /// <summary>
    /// 业务键（用于关联业务实体）。
    /// </summary>
    public string? BizKey { get; set; }

    /// <summary>
    /// 业务类型。
    /// </summary>
    public string? BizType { get; set; }

    /// <summary>
    /// 接收者用户编码。
    /// </summary>
    public string? ReceiveUserCode { get; set; }

    /// <summary>
    /// 接收者姓名。
    /// </summary>
    public string? ReceiveUserName { get; set; }

    /// <summary>
    /// 发送者用户编码。
    /// </summary>
    public string? SendUserCode { get; set; }

    /// <summary>
    /// 发送者姓名。
    /// </summary>
    public string? SendUserName { get; set; }

    /// <summary>
    /// 发送时间。
    /// </summary>
    public DateTime? SendDate { get; set; }

    /// <summary>
    /// 推送状态（0 待推送 / 1 成功 / 2 失败）。
    /// </summary>
    public string? PushStatus { get; set; }

    /// <summary>
    /// 已读状态（0 未读 / 1 已读）。
    /// </summary>
    public string? ReadStatus { get; set; }
}

/// <summary>
/// 消息模板 DTO。
/// </summary>
public class MsgTemplateDto
{
    /// <summary>
    /// 模板标识（主键）。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 所属模块编码。
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// 模板键（用于代码引用的唯一标识）。
    /// </summary>
    public string TplKey { get; set; } = string.Empty;

    /// <summary>
    /// 模板名称（显示用）。
    /// </summary>
    public string TplName { get; set; } = string.Empty;

    /// <summary>
    /// 模板类型（inner/push/email/sms 等）。
    /// </summary>
    public string TplType { get; set; } = string.Empty;

    /// <summary>
    /// 模板内容（支持占位符）。
    /// </summary>
    public string TplContent { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 推送消息保存请求 DTO。
/// </summary>
public class MsgPushSaveDto
{
    /// <summary>
    /// 推送消息标识；空表示新建。
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 消息类型。
    /// </summary>
    public string MsgType { get; set; } = "pc";

    /// <summary>
    /// 推送标题。
    /// </summary>
    public string MsgTitle { get; set; } = string.Empty;

    /// <summary>
    /// 推送内容。
    /// </summary>
    public string MsgContent { get; set; } = string.Empty;

    /// <summary>
    /// 业务键。
    /// </summary>
    public string? BizKey { get; set; }

    /// <summary>
    /// 业务类型。
    /// </summary>
    public string? BizType { get; set; }

    /// <summary>
    /// 接收者编码（批量推送的目标集合拼接）。
    /// </summary>
    public string? ReceiveCode { get; set; }

    /// <summary>
    /// 接收者用户编码。
    /// </summary>
    public string? ReceiveUserCode { get; set; }

    /// <summary>
    /// 接收者姓名。
    /// </summary>
    public string? ReceiveUserName { get; set; }

    /// <summary>
    /// 计划推送时间。
    /// </summary>
    public DateTime? PlanPushDate { get; set; }
}

/// <summary>
/// 消息模板保存请求 DTO。
/// </summary>
public class MsgTemplateSaveDto
{
    /// <summary>
    /// 模板标识；空表示新建。
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 所属模块编码。
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// 模板键。
    /// </summary>
    public string TplKey { get; set; } = string.Empty;

    /// <summary>
    /// 模板名称。
    /// </summary>
    public string TplName { get; set; } = string.Empty;

    /// <summary>
    /// 模板类型。
    /// </summary>
    public string TplType { get; set; } = string.Empty;

    /// <summary>
    /// 模板内容。
    /// </summary>
    public string TplContent { get; set; } = string.Empty;
}
