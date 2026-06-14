    // 引入 System.Text 命名空间
// 引入命名空间：System.Text
using System.Text;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class SmsService
// 定义类：SmsService
public class SmsService : ISmsSender
{
    // 字段 _configuration
    // 字段：_configuration
    private readonly IConfiguration _configuration;

    // 方法 SmsService
    // 构造函数：SmsService
    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 方法 SendAsync
    // 方法：SendAsync
    public async Task<bool> SendAsync(string phoneNumber, string message)
    {
        // 声明并初始化变量：section
        var section = _configuration.GetSection("Sms");
        // 声明并初始化变量：url
        var url = section["Url"] ?? "";
        // 声明并初始化变量：data
        var data = section["Data"] ?? "";
        // 声明并初始化变量：prefix
        var prefix = section["Prefix"] ?? "";
        // 声明并初始化变量：suffix
        var suffix = section["Suffix"] ?? "";

        // if 条件判断
        if (string.IsNullOrEmpty(url))
            // return 返回结果
            return false;

        // 声明并初始化变量：content
        var content = $"{prefix}{message}{suffix}";

        // if 条件判断
        if (!string.IsNullOrEmpty(data))
        {
            // 声明并初始化变量：payload
            var payload = $"{data}&mobile={Uri.EscapeDataString(phoneNumber)}&content={Uri.EscapeDataString(content)}";
            // await 异步等待
            await HttpClientUtil.PostAsync(url, payload, "application/x-www-form-urlencoded");
        }
        // else 否则分支
        else
        {
            var payload = new { mobile = phoneNumber, content };
            // 声明并初始化变量：json
            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            // await 异步等待
            await HttpClientUtil.PostAsync(url, json);
        }

        // return 返回结果
        return true;
    }
}
