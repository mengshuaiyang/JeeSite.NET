    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
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

[ApiController]
[Route("api/v1/sys/auth")]
[AllowAnonymous]
// 定义class CasAuthController
// 定义类：CasAuthController

public class CasAuthController : ControllerBase
{
    // 字段 _casAuthService
    // 字段：_casAuthService

    private readonly CasAuthService _casAuthService;
    // 字段 _configuration
    // 字段：_configuration

    private readonly IConfiguration _configuration;

    // 方法 CasAuthController
    // 构造函数：CasAuthController

    public CasAuthController(CasAuthService casAuthService, IConfiguration configuration)
    {
        _casAuthService = casAuthService;
        _configuration = configuration;
    }

    [HttpPost("cas-login")]
    // 方法 CasLogin
    // 方法：CasLogin

    public async Task<ApiResult<LoginResultDto>> CasLogin([FromBody] CasLoginDto dto)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(dto.Ticket))
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "ticket 不能为空");
        // if 条件判断
        if (string.IsNullOrEmpty(dto.Service))
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "service 不能为空");

        // return 返回结果
        return await _casAuthService.CasLoginAsync(dto.Ticket, dto.Service);
    }

    [HttpGet("cas-login")]
    // 方法 CasLoginRedirect
    // 方法：CasLoginRedirect

    public async Task<IActionResult> CasLoginRedirect([FromQuery] string ticket, [FromQuery] string? service)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(ticket))
        {
            // 声明并初始化变量：casServerUrl
            var casServerUrl = _configuration.GetSection("Cas")["ServerUrl"] ?? "";
            // 声明并初始化变量：frontendUrl
            var frontendUrl = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
            // if 条件判断
            if (!string.IsNullOrEmpty(casServerUrl) && !string.IsNullOrEmpty(frontendUrl))
            {
                // 声明并初始化变量：redirectUrl
                var redirectUrl = $"{casServerUrl.TrimEnd('/')}/login?service={Uri.EscapeDataString(frontendUrl.TrimEnd('/') + "/api/v1/sys/auth/cas-login")}";
                // return 返回结果
                return Redirect(redirectUrl);
            }
            // return 返回结果
            return BadRequest(ApiResult<LoginResultDto>.Fail(400, "CAS 未配置"));
        }

        // 调用 IsNullOrEmpty
        var callbackUrl = string.IsNullOrEmpty(service)
            ? $"{Request.Scheme}://{Request.Host}{Request.Path}"
            : service;

        var result = await _casAuthService.CasLoginAsync(ticket, callbackUrl);
        // if 条件判断
        if (result.Code != 200)
        {
            // 声明并初始化变量：frontendUrl
            var frontendUrl = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
            // if 条件判断
            if (!string.IsNullOrEmpty(frontendUrl))
            {
                // 声明并初始化变量：errorUrl
                var errorUrl = $"{frontendUrl.TrimEnd('/')}/#/login?cas_error={Uri.EscapeDataString(result.Message ?? "CAS 登录失败")}";
                // return 返回结果
                return Redirect(errorUrl);
            }
            // return 返回结果
            return Unauthorized(result);
        }

        // 声明并初始化变量：frontendRedirect
        var frontendRedirect = _configuration.GetSection("Cas")["FrontendRedirectUrl"] ?? "/#/cas-callback";
        // 声明并初始化变量：redirectTo
        var redirectTo = _configuration.GetSection("Cas")["ClientUrl"] ?? "";
        // 声明并初始化变量：token
        var token = result.Data?.Token ?? "";
        // 调用 ToString
        var expires = result.Data?.Expires.ToString("o") ?? "";

        // 声明并初始化变量：finalUrl
        var finalUrl = $"{redirectTo.TrimEnd('/')}{frontendRedirect}#token={Uri.EscapeDataString(token)}&expires={Uri.EscapeDataString(expires)}";
        // return 返回结果
        return Redirect(finalUrl);
    }
}
