    // 引入 JeeSiteNET.Modules.Sys.Application.Services.OAuth2 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services.OAuth2
using JeeSiteNET.Modules.Sys.Application.Services.OAuth2;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[Route("api/v1/sys/auth/oauth2")]
[ApiController]
[AllowAnonymous]
// 定义class OAuth2Controller
// 定义类：OAuth2Controller

public class OAuth2Controller : ControllerBase
{
    // 字段 _oauth2Service
    // 字段：_oauth2Service

    private readonly OAuth2Service _oauth2Service;
    // 字段 _configuration
    // 字段：_configuration

    private readonly IConfiguration _configuration;

    // 方法 OAuth2Controller
    // 构造函数：OAuth2Controller

    public OAuth2Controller(OAuth2Service oauth2Service, IConfiguration configuration)
    {
        _oauth2Service = oauth2Service;
        _configuration = configuration;
    }

    [HttpGet("{provider}")]
    // 方法 RedirectToProvider
    // 方法：RedirectToProvider

    public IActionResult RedirectToProvider(string provider, [FromQuery] string? redirectUri)
    {
        // 声明并初始化变量：oauthProvider
        var oauthProvider = _oauth2Service.GetProvider(provider);
        // if 条件判断
        if (oauthProvider == null)
            // return 返回结果
            return BadRequest(new { code = 400, message = $"不支持的 OAuth2 提供商: {provider}" });

        // 声明并初始化变量：frontendUrl
        var frontendUrl = _configuration.GetValue<string>("OAuth2:FrontendRedirectUrl") ?? "/#/login";
        // 调用 ToString
        var state = Guid.NewGuid().ToString("N");

        var callbackUrl = Url.Action(nameof(Callback), "OAuth2", new { provider, state }, Request.Scheme)
            // null 合并操作 ??（若为 null 则使用右侧值）
            ?? throw new InvalidOperationException("无法生成回调 URL");

        // 声明并初始化变量：authUrl
        var authUrl = oauthProvider.GetAuthorizationUrl(callbackUrl, state);

        // 声明并初始化变量：cacheKey
        var cacheKey = $"OAuth2State:{state}";
        // null 合并操作 ??（若为 null 则使用右侧值）
        HttpContext.Items[cacheKey] = redirectUri ?? frontendUrl;

        // return 返回结果
        return Redirect(authUrl);
    }

    [HttpGet("{provider}/callback")]
    // 方法 Callback
    // 方法：Callback

    public async Task<IActionResult> Callback(string provider, [FromQuery] string code, [FromQuery] string state)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(code))
            // return 返回结果
            return BadRequest(new { code = 400, message = "缺少授权码" });

        // 声明并初始化变量：frontendUrl
        var frontendUrl = _configuration.GetValue<string>("OAuth2:FrontendRedirectUrl") ?? "/#/login";
        var redirectUri = Url.Action(nameof(Callback), "OAuth2", new { provider, state }, Request.Scheme)
            // null 合并操作 ??（若为 null 则使用右侧值）
            ?? throw new InvalidOperationException("无法生成回调 URL");

        var result = await _oauth2Service.LoginAsync(provider, code, redirectUri);

        // if 条件判断
        if (result.Code != 200)
        {
            // 声明并初始化变量：errorUrl
            var errorUrl = $"{frontendUrl}?error={Uri.EscapeDataString(result.Message ?? "登录失败")}";
            // return 返回结果
            return Redirect(errorUrl);
        }

        // 声明并初始化变量：tokenUrl
        var tokenUrl = $"{frontendUrl}?token={result.Data?.Token}";
        // return 返回结果
        return Redirect(tokenUrl);
    }
}
