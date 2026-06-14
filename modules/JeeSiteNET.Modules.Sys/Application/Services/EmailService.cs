    // 引入 System.Net 命名空间
// 引入命名空间：System.Net
using System.Net;
    // 引入 System.Net.Mail 命名空间
// 引入命名空间：System.Net.Mail
using System.Net.Mail;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class EmailService
// 定义类：EmailService
public class EmailService : IEmailSender
{
    // 字段 _configuration
    // 字段：_configuration
    private readonly IConfiguration _configuration;

    // 方法 EmailService
    // 构造函数：EmailService
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 方法 SendAsync
    // 方法：SendAsync
    public async Task<bool> SendAsync(string toAddress, string subject, string body, string? cc = null, string? bcc = null)
    {
        // 声明并初始化变量：section
        var section = _configuration.GetSection("Email");
        // 声明并初始化变量：fromAddress
        var fromAddress = section["FromAddress"] ?? "";
        // 声明并初始化变量：fromPassword
        var fromPassword = section["FromPassword"] ?? "";
        // 声明并初始化变量：fromHostName
        var fromHostName = section["FromHostName"] ?? "";
        // 调用 Parse
        var smtpPort = int.Parse(section["SmtpPort"] ?? "25");
        // 声明并初始化变量：sslOnConnect
        var sslOnConnect = section["SslOnConnect"] == "true";
        // 声明并初始化变量：sslSmtpPort
        var sslSmtpPort = section["SslSmtpPort"] ?? "465";

        // if 条件判断
        if (string.IsNullOrEmpty(fromAddress) || string.IsNullOrEmpty(fromHostName))
            // return 返回结果
            return false;

    // 引入 var client 命名空间
        // 调用 Parse
        using var client = new SmtpClient(fromHostName, sslOnConnect ? int.Parse(sslSmtpPort) : smtpPort)
        {
            // 创建 NetworkCredential实例并赋给 Credentials
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            EnableSsl = sslOnConnect,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

    // 引入 var msg 命名空间
        using var msg = new MailMessage
        {
            // 创建 MailAddress实例并赋给 From
            From = new MailAddress(fromAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        // foreach 遍历集合
        foreach (var addr in toAddress.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            // 集合操作：添加元素
            msg.To.Add(addr);

        // if 条件判断
        if (!string.IsNullOrEmpty(cc))
        {
            // foreach 遍历集合
            foreach (var addr in cc.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                // 集合操作：添加元素
                msg.CC.Add(addr);
        }

        // if 条件判断
        if (!string.IsNullOrEmpty(bcc))
        {
            // foreach 遍历集合
            foreach (var addr in bcc.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                // 集合操作：添加元素
                msg.Bcc.Add(addr);
        }

        // try 异常捕获开始
        try
        {
            // await 异步等待
            await client.SendMailAsync(msg);
            // return 返回结果
            return true;
        }
        // catch 捕获异常
        catch
        {
            // return 返回结果
            return false;
        }
    }
}
