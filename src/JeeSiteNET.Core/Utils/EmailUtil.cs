using System.Net;
using System.Net.Mail;

namespace JeeSiteNET.Core.Utils;

public static class EmailUtil
{
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
