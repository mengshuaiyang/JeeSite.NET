using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>用户认证接口控制器，提供登录/注册/忘记密码/重置密码/获取用户信息与菜单路由等接口。</summary>
[ApiController]
[Route("api/v1/sys/auth")]
public class AuthController : ControllerBase

{
    private readonly AuthService _authService;

    public AuthController(AuthService authService) => _authService = authService;

    /// <summary>HTTP POST - 账号密码登录，校验用户凭据并返回 Token 与登录信息。</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ApiResult<LoginResultDto>> Login([FromBody] LoginDto dto)
    {
        return await _authService.LoginAsync(dto);
    }

    /// <summary>HTTP POST - 新用户注册，根据注册信息创建本地账号。</summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ApiResult> Register([FromBody] RegisterDto dto)
    {
        return await _authService.RegisterAsync(dto);
    }

    /// <summary>HTTP POST - 触发忘记密码流程，向用户发送重置指引或验证码。</summary>
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ApiResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        return await _authService.ForgotPasswordAsync(dto);
    }

    /// <summary>HTTP POST - 根据验证码或其他安全验证重置用户密码。</summary>
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

/// <summary>短信/邮件验证码登录请求 DTO。</summary>

public class LoginByCodeRequest

{
    public string Target { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}
