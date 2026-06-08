using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/auth")]
[AllowAnonymous]
public class CasAuthController : ControllerBase
{
    private readonly CasAuthService _casAuthService;
    private readonly IConfiguration _configuration;

    public CasAuthController(CasAuthService casAuthService, IConfiguration configuration)
    {
        _casAuthService = casAuthService;
        _configuration = configuration;
    }

    [HttpPost("cas-login")]
    public async Task<ApiResult<LoginResultDto>> CasLogin([FromBody] CasLoginDto dto)
    {
        if (string.IsNullOrEmpty(dto.Ticket))
            return ApiResult<LoginResultDto>.Fail(400, "ticket 不能为空");
        if (string.IsNullOrEmpty(dto.Service))
            return ApiResult<LoginResultDto>.Fail(400, "service 不能为空");

        return await _casAuthService.CasLoginAsync(dto.Ticket, dto.Service);
    }

    [HttpGet("cas-login")]
    public async Task<IActionResult> CasLoginRedirect([FromQuery] string ticket, [FromQuery] string? service)
    {
        if (string.IsNullOrEmpty(ticket))
        {
            var casServerUrl = _configuration.GetSection("Cas")["ServerUrl"] ?? "";
            var frontendUrl = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
            if (!string.IsNullOrEmpty(casServerUrl) && !string.IsNullOrEmpty(frontendUrl))
            {
                var redirectUrl = $"{casServerUrl.TrimEnd('/')}/login?service={Uri.EscapeDataString(frontendUrl.TrimEnd('/') + "/api/v1/sys/auth/cas-login")}";
                return Redirect(redirectUrl);
            }
            return BadRequest(ApiResult<LoginResultDto>.Fail(400, "CAS 未配置"));
        }

        var callbackUrl = string.IsNullOrEmpty(service)
            ? $"{Request.Scheme}://{Request.Host}{Request.Path}"
            : service;

        var result = await _casAuthService.CasLoginAsync(ticket, callbackUrl);
        if (result.Code != 200)
        {
            var frontendUrl = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
            if (!string.IsNullOrEmpty(frontendUrl))
            {
                var errorUrl = $"{frontendUrl.TrimEnd('/')}/#/login?cas_error={Uri.EscapeDataString(result.Message ?? "CAS 登录失败")}";
                return Redirect(errorUrl);
            }
            return Unauthorized(result);
        }

        var frontendRedirect = _configuration.GetSection("Cas")["FrontendRedirectUrl"] ?? "/#/cas-callback";
        var redirectTo = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
        var token = result.Data?.Token ?? "";
        var expires = result.Data?.Expires.ToString("o") ?? "";

        var finalUrl = $"{redirectTo.TrimEnd('/')}{frontendRedirect}#token={Uri.EscapeDataString(token)}&expires={Uri.EscapeDataString(expires)}";
        return Redirect(finalUrl);
    }
}


