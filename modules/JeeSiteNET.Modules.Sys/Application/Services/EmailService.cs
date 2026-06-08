using System.Net;
using System.Net.Mail;
using JeeSiteNET.Core.Security;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class EmailService : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendAsync(string toAddress, string subject, string body, string? cc = null, string? bcc = null)
    {
        var section = _configuration.GetSection("Email");
        var fromAddress = section["FromAddress"] ?? "";
        var fromPassword = section["FromPassword"] ?? "";
        var fromHostName = section["FromHostName"] ?? "";
        var smtpPort = int.Parse(section["SmtpPort"] ?? "25");
        var sslOnConnect = section["SslOnConnect"] == "true";
        var sslSmtpPort = section["SslSmtpPort"] ?? "465";

        if (string.IsNullOrEmpty(fromAddress) || string.IsNullOrEmpty(fromHostName))
            return false;

        using var client = new SmtpClient(fromHostName, sslOnConnect ? int.Parse(sslSmtpPort) : smtpPort)
        {
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            EnableSsl = sslOnConnect,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        using var msg = new MailMessage
        {
            From = new MailAddress(fromAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        foreach (var addr in toAddress.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            msg.To.Add(addr);

        if (!string.IsNullOrEmpty(cc))
        {
            foreach (var addr in cc.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                msg.CC.Add(addr);
        }

        if (!string.IsNullOrEmpty(bcc))
        {
            foreach (var addr in bcc.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                msg.Bcc.Add(addr);
        }

        try
        {
            await client.SendMailAsync(msg);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
