using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

// ================================================================
// 用户认证控制器 —— 用户登录/注册/找回密码 的入口
//
// 位置：应用启动后第一个被调用的 API
// 调用链：用户访问登录页 → POST /api/v1/sys/auth/login
//         → AuthService.LoginAsync → UserRepository.GetByLoginCode
//         → 校验密码 → 签发 JWT Token → 返回前端
//
// 其他端点：
//   POST /register        — 新用户注册
//   POST /forgot-password  — 发送重置密码邮件/短信
//   POST /reset-password   — 根据验证码重置密码
//   GET  /info             — 获取当前用户信息（权限/角色）
//   GET  /menu-route       — 获取动态菜单路由树
// ================================================================

[ApiController]
[Route("api/v1/sys/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    /// <summary>构造函数：通过 DI 注入认证服务</summary>
    public AuthController(AuthService authService) => _authService = authService;

    /// <summary>
    /// HTTP POST - 账号密码登录。
    /// 接收登录名、密码、验证码（可选），返回 JWT Token + 用户信息 + 权限列表。
    /// 验证码校验逻辑见 AuthService.LoginAsync。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ApiResult<LoginResultDto>> Login([FromBody] LoginDto dto)
        => await _authService.LoginAsync(dto);

    /// <summary>
    /// HTTP POST - 新用户注册。
    /// 创建本地账号，注册成功后可直接登录。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ApiResult> Register([FromBody] RegisterDto dto)
        => await _authService.RegisterAsync(dto);

    /// <summary>
    /// HTTP POST - 忘记密码。
    /// 根据登录名/邮箱/手机号发送重置指引（验证码或链接）。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ApiResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        => await _authService.ForgotPasswordAsync(dto);

    /// <summary>
    /// HTTP POST - 重置密码。
    /// 提交验证码和新密码完成密码重置。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ApiResult> ResetPassword([FromBody] ResetPasswordDto dto)
        => await _authService.ResetPasswordAsync(dto);

    /// <summary>
    /// HTTP GET - 获取当前登录用户的认证信息。
    /// 返回：用户基本信息、角色列表、权限列表、系统编码。
    /// 前端据此决定页面布局和功能可见性。
    /// </summary>
    [Authorize]
    [HttpGet("info")]
    public async Task<ApiResult<object>> GetAuthInfo()
        => await _authService.GetAuthInfoAsync();

    /// <summary>
    /// HTTP GET - 获取当前用户的菜单路由树。
    /// 前端根据返回的树结构动态生成侧边导航。
    /// sysCode 参数用于多系统导航（如分开管理后台和前台）。
    /// </summary>
    [Authorize]
    [HttpGet("menu-route")]
    public async Task<ApiResult<List<MenuDto>>> GetMenuRoute([FromQuery] string? sysCode = null)
        => await _authService.GetMenuRouteAsync(sysCode);

    /// <summary>
    /// HTTP POST - 验证码登录。
    /// 通过短信或邮件验证码直接登录，无需密码。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login-by-code")]
    public async Task<ApiResult<LoginResultDto>> LoginByCode([FromBody] LoginByCodeRequest dto)
        => await _authService.LoginByCodeAsync(dto.Target, dto.Code);
}

// ================================================================
// 验证码登录请求体
// Target：手机号或邮箱
// Code：收到的短信或邮件验证码
// ================================================================
public class LoginByCodeRequest
{
    public string Target { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
