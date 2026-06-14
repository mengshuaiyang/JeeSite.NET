namespace JeeSiteNET.Core.Security;

/// <summary>
/// 短信发送接口：用于登录验证码、通知短信、告警短信等场景
/// </summary>
public interface ISmsSender
{
    /// <summary>
    /// 异步发送短信
    /// </summary>
    /// <param name="phoneNumber">接收方手机号</param>
    /// <param name="message">短信内容</param>
    /// <returns>true 表示发送成功；false 表示发送失败</returns>
    Task<bool> SendAsync(string phoneNumber, string message);
}

/// <summary>
/// 邮件发送接口：用于注册验证、密码找回、系统通知等场景
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// 异步发送邮件（支持抄送与密送）
    /// </summary>
    /// <param name="toAddress">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件正文（可为 HTML）</param>
    /// <param name="cc">抄送地址（可为 null）</param>
    /// <param name="bcc">密送地址（可为 null）</param>
    /// <returns>true 表示发送成功；false 表示发送失败</returns>
    Task<bool> SendAsync(string toAddress, string subject, string body, string? cc = null, string? bcc = null);
}
