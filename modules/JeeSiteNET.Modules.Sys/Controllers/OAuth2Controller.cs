using JeeSiteNET.Modules.Sys.Application.Services.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Controllers;

[Route("api/v1/sys/auth/oauth2")]
[ApiController]
[AllowAnonymous]
public class OAuth2Controller : ControllerBase
{
    private readonly OAuth2Service _oauth2Service;
    private readonly IConfiguration _configuration;

    public OAuth2Controller(OAuth2Service oauth2Service, IConfiguration configuration)
    {
        _oauth2Service = oauth2Service;
        _configuration = configuration;
    }

    [HttpGet("{provider}")]
    public IActionResult RedirectToProvider(string provider, [FromQuery] string? redirectUri)
    {
        var oauthProvider = _oauth2Service.GetProvider(provider);
        if (oauthProvider == null)
            return BadRequest(new { code = 400, message = $"不支持的 OAuth2 提供商: {provider}" });

        var frontendUrl = _configuration.GetValue<string>("OAuth2:FrontendRedirectUrl") ?? "/#/login";
        var state = Guid.NewGuid().ToString("N");

        var callbackUrl = Url.Action(nameof(Callback), "OAuth2", new { provider, state }, Request.Scheme)
            ?? throw new InvalidOperationException("无法生成回调 URL");

        var authUrl = oauthProvider.GetAuthorizationUrl(callbackUrl, state);

        var cacheKey = $"OAuth2State:{state}";
        HttpContext.Items[cacheKey] = redirectUri ?? frontendUrl;

        return Redirect(authUrl);
    }

    [HttpGet("{provider}/callback")]
    public async Task<IActionResult> Callback(string provider, [FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new { code = 400, message = "缺少授权码" });

        var frontendUrl = _configuration.GetValue<string>("OAuth2:FrontendRedirectUrl") ?? "/#/login";
        var redirectUri = Url.Action(nameof(Callback), "OAuth2", new { provider, state }, Request.Scheme)
            ?? throw new InvalidOperationException("无法生成回调 URL");

        var result = await _oauth2Service.LoginAsync(provider, code, redirectUri);

        if (result.Code != 200)
        {
            var errorUrl = $"{frontendUrl}?error={Uri.EscapeDataString(result.Message ?? "登录失败")}";
            return Redirect(errorUrl);
        }

        var tokenUrl = $"{frontendUrl}?token={result.Data?.Token}";
        return Redirect(tokenUrl);
    }
}
