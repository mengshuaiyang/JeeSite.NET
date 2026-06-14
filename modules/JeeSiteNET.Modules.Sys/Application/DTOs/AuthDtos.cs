namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 用户注册请求 DTO。
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// 注册登录账号。
    /// </summary>
    public string LoginCode { get; set; } = string.Empty;

    /// <summary>
    /// 登录密码。
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 用户显示名称。
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 邮箱地址；用于注册确认与密码重置通知。
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 手机号；用于短信验证码登录。
    /// </summary>
    public string? Phone { get; set; }
}

/// <summary>
/// 忘记密码请求 DTO。
/// </summary>
public class ForgotPasswordDto
{
    /// <summary>
    /// 用户登录账号。
    /// </summary>
    public string LoginCode { get; set; } = string.Empty;

    /// <summary>
    /// 注册时填写的邮箱；用于匹配账户并发送重置链接。
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// 重置密码请求 DTO。
/// </summary>
public class ResetPasswordDto
{
    /// <summary>
    /// 重置令牌（由忘记密码流程下发）。
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 新密码明文（由服务端加密存储）。
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
