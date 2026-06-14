namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 登录请求 DTO。
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 用户登录账号。
    /// </summary>
    public string LoginCode { get; set; } = string.Empty;

    /// <summary>
    /// 登录密码（前端加密或明文，实际由认证服务处理）。
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 图片验证码文本。
    /// </summary>
    public string? ValidCode { get; set; }

    /// <summary>
    /// 验证码键（用于从服务端匹配对应的验证码）。
    /// </summary>
    public string? ValidCodeKey { get; set; }
}

/// <summary>
/// 短信发送 DTO。
/// </summary>
public class SmsDto
{
    /// <summary>
    /// 接收短信的手机号码。
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 短信内容（已拼接模板后的值）。
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// 邮件发送 DTO。
/// </summary>
public class EmailDto
{
    /// <summary>
    /// 收件人地址。
    /// </summary>
    public string ToAddress { get; set; } = string.Empty;

    /// <summary>
    /// 邮件主题。
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// 邮件正文（支持 HTML/纯文本，由调用方决定）。
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 抄送地址；多个地址以逗号分隔。
    /// </summary>
    public string? Cc { get; set; }

    /// <summary>
    /// 密送地址；多个地址以逗号分隔。
    /// </summary>
    public string? Bcc { get; set; }
}

/// <summary>
/// CAS 单点登录票据验证 DTO。
/// </summary>
public class CasLoginDto
{
    /// <summary>
    /// CAS 服务器签发的 Service Ticket。
    /// </summary>
    public string Ticket { get; set; } = string.Empty;

    /// <summary>
    /// 业务系统回调 Service 地址。
    /// </summary>
    public string Service { get; set; } = string.Empty;
}

/// <summary>
/// 登录响应 DTO。
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// JWT 访问令牌。
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 令牌过期时间（UTC + 偏移量，由服务端提供）。
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// 当前登录用户的基本资料。
    /// </summary>
    public UserDto User { get; set; } = null!;

    /// <summary>
    /// 是否通过验证码登录；用于审计或风控判定。
    /// </summary>
    public bool IsValidCodeLogin { get; set; }
}
