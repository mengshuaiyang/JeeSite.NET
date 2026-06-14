using System.Net;
using System.Net.Mail;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// SMTP 邮件发送工具类
/// </summary>
public static class EmailUtil
{
    /// <summary>
    /// 同步发送邮件
    /// </summary>
    /// <param name="smtpHost">SMTP 服务器地址</param>
    /// <param name="smtpPort">SMTP 服务器端口</param>
    /// <param name="fromAddress">发件人邮箱地址</param>
    /// <param name="fromPassword">发件人邮箱密码或授权码</param>
    /// <param name="toAddress">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件正文（支持 HTML）</param>
    /// <param name="useSsl">是否使用 SSL 加密，默认为 true</param>
    /// <returns>发送成功返回 true；否则抛出异常</returns>
    public static bool Send(string smtpHost, int smtpPort, string fromAddress, string fromPassword, string toAddress, string subject, string body, bool useSsl = true)
    {
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            EnableSsl = useSsl
        };
        using var msg = new MailMessage(fromAddress, toAddress, subject, body) { IsBodyHtml = true };
        client.Send(msg);
        return true;
    }

    /// <summary>
    /// 异步发送邮件
    /// </summary>
    /// <param name="smtpHost">SMTP 服务器地址</param>
    /// <param name="smtpPort">SMTP 服务器端口</param>
    /// <param name="fromAddress">发件人邮箱地址</param>
    /// <param name="fromPassword">发件人邮箱密码或授权码</param>
    /// <param name="toAddress">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件正文（支持 HTML）</param>
    /// <param name="useSsl">是否使用 SSL 加密，默认为 true</param>
    /// <returns>发送成功返回 true；否则抛出异常</returns>
    public static async Task<bool> SendAsync(string smtpHost, int smtpPort, string fromAddress, string fromPassword, string toAddress, string subject, string body, bool useSsl = true)
    {
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            EnableSsl = useSsl
        };
        using var msg = new MailMessage(fromAddress, toAddress, subject, body) { IsBodyHtml = true };
        await client.SendMailAsync(msg);
        return true;
    }
}
