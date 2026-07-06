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

        // 声明并初始化变量：safeRedirectUri
        // 开放重定向防护：redirectUri 必须为相对路径（同源），或主机在白名单 OAuth2:AllowedRedirectHosts 内；
        // 否则回退到默认 frontendUrl，避免被用作跳板将用户导向恶意站点。
        var safeRedirectUri = IsRedirectUriAllowed(redirectUri) ? redirectUri : frontendUrl;

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
        HttpContext.Items[cacheKey] = safeRedirectUri;

        // return 返回结果
        return Redirect(authUrl);
    }

    /// <summary>
    /// 校验重定向地址是否安全：允许相对路径（以 / 开头且非 // 协议相对）或白名单主机（OAuth2:AllowedRedirectHosts，逗号分隔）。
    /// </summary>
    /// <param name="redirectUri">待校验的重定向地址。</param>
    /// <returns>安全返回 true；为空或不安全返回 false。</returns>
    private bool IsRedirectUriAllowed(string? redirectUri)
    {
        if (string.IsNullOrWhiteSpace(redirectUri)) return false;

        // 相对路径：同源，安全（排除协议相对 //evil.com 与反斜杠 \evil.com 形式）。
        if (redirectUri.StartsWith("/", StringComparison.Ordinal)
            && !redirectUri.StartsWith("//", StringComparison.Ordinal)
            && !redirectUri.StartsWith("/\\", StringComparison.Ordinal))
        {
            return true;
        }

        // 绝对地址：仅允许白名单主机（配置项留空则不允许任何外部主机）。
        var allowedHosts = _configuration.GetValue<string>("OAuth2:AllowedRedirectHosts");
        if (string.IsNullOrWhiteSpace(allowedHosts)) return false;

        if (Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) && uri.IsWellFormedOriginalString())
        {
            foreach (var host in allowedHosts.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (string.Equals(uri.Host, host, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        return false;
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
