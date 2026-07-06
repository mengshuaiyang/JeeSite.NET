using System.Text;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Core.Security;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>短信发送服务：通过 HTTP API 接入第三方短信平台（可扩展为阿里云 SMS 等 SDK）。</summary>
public class SmsService : ISmsSender
{
    private readonly IConfiguration _configuration;

    /// <summary>构造函数，注入配置服务。</summary>
    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>向指定手机号发送短信内容。</summary>
    public async Task<bool> SendAsync(string phoneNumber, string message)
    {
        var section = _configuration.GetSection("Sms");
        var url = section["Url"] ?? "";
        // 凭证优先取自环境变量 JEE_SITE_SMS_DATA，回退到配置（配置中应为占位空串，避免明文弱凭证入库）。
        var data = Environment.GetEnvironmentVariable("JEE_SITE_SMS_DATA") ?? section["Data"] ?? "";
        var prefix = section["Prefix"] ?? "";
        var suffix = section["Suffix"] ?? "";

        if (string.IsNullOrEmpty(url)) return false;

        var content = $"{prefix}{message}{suffix}";

        if (!string.IsNullOrEmpty(data))
        {
            var payload = $"{data}&mobile={Uri.EscapeDataString(phoneNumber)}&content={Uri.EscapeDataString(content)}";
            await HttpClientUtil.PostAsync(url, payload, "application/x-www-form-urlencoded");
        }
        else
        {
            var payload = new { mobile = phoneNumber, content };
            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            await HttpClientUtil.PostAsync(url, json);
        }

        return true;
    }
}
