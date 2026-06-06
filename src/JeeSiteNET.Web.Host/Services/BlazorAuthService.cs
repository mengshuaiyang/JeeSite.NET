using System.Security.Claims;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace JeeSiteNET.Web.Host.Services;

public class BlazorAuthService
{
    private readonly AuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlazorAuthService(AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, string? Error)> LoginAsync(string loginCode, string password)
    {
        var result = await _authService.LoginAsync(new() { LoginCode = loginCode, Password = password });
        if (result.Code != 200)
            return (false, result.Message);

        var ctx = _httpContextAccessor.HttpContext;
        if (ctx == null) return (false, "No HttpContext");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Data!.User.UserCode),
            new(ClaimTypes.Name, result.Data.User.LoginCode),
            new(ClaimTypes.GivenName, result.Data.User.UserName),
            new("UserType", result.Data.User.UserType ?? ""),
            new("Avatar", result.Data.User.Avatar ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
        });
        return (true, null);
    }

    public async Task LogoutAsync()
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx != null)
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
