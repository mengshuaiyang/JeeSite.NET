using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService) => _authService = authService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ApiResult<LoginResultDto>> Login([FromBody] LoginDto dto)
    {
        return await _authService.LoginAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ApiResult> Register([FromBody] RegisterDto dto)
    {
        return await _authService.RegisterAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ApiResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        return await _authService.ForgotPasswordAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ApiResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        return await _authService.ResetPasswordAsync(dto);
    }

    /// <summary>获取当前登录用户的完整认证信息（用户信息+权限+角色+系统编码）</summary>
    [Authorize]
    [HttpGet("info")]
    public async Task<ApiResult<object>> GetAuthInfo()
    {
        return await _authService.GetAuthInfoAsync();
    }

    /// <summary>获取当前用户的菜单路由树（支持前端动态路由）</summary>
    [Authorize]
    [HttpGet("menu-route")]
    public async Task<ApiResult<List<MenuDto>>> GetMenuRoute([FromQuery] string? sysCode = null)
    {
        return await _authService.GetMenuRouteAsync(sysCode);
    }

    /// <summary>短信/邮件验证码登录</summary>
    [AllowAnonymous]
    [HttpPost("login-by-code")]
    public async Task<ApiResult<LoginResultDto>> LoginByCode([FromBody] LoginByCodeRequest dto)
    {
        return await _authService.LoginByCodeAsync(dto.Target, dto.Code);
    }
}

public class LoginByCodeRequest
{
    public string Target { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}