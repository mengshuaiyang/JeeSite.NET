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
}