using System.Text;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class SmsService : ISmsSender
{
    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendAsync(string phoneNumber, string message)
    {
        var section = _configuration.GetSection("Sms");
        var url = section["Url"] ?? "";
        var data = section["Data"] ?? "";
        var prefix = section["Prefix"] ?? "";
        var suffix = section["Suffix"] ?? "";

        if (string.IsNullOrEmpty(url))
            return false;

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
