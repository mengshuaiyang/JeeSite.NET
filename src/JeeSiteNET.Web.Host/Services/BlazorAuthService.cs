    // 引入 System.Security.Claims 命名空间
// 引入命名空间：System.Security.Claims
using System.Security.Claims;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Authentication 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authentication
using Microsoft.AspNetCore.Authentication;
    // 引入 Microsoft.AspNetCore.Authentication.Cookies 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authentication.Cookies
using Microsoft.AspNetCore.Authentication.Cookies;

// 定义 JeeSiteNET.Web.Host.Services 命名空间
// 定义命名空间：JeeSiteNET.Web.Host.Services
namespace JeeSiteNET.Web.Host.Services;

// 定义class BlazorAuthService
// 定义类：BlazorAuthService
public class BlazorAuthService
{
    // 字段 _authService
    // 字段：_authService
    private readonly AuthService _authService;
    // 字段 _httpContextAccessor
    // 字段：_httpContextAccessor
    private readonly IHttpContextAccessor _httpContextAccessor;

    // 方法 BlazorAuthService
    // 构造函数：BlazorAuthService
    public BlazorAuthService(AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    // 方法 LoginAsync
    public async Task<(bool Success, string? Error)> LoginAsync(string loginCode, string password)
    {
        var result = await _authService.LoginAsync(new() { LoginCode = loginCode, Password = password });
        // if 条件判断
        if (result.Code != 200)
            // return 返回结果
            return (false, result.Message);

        // 声明并初始化变量：ctx
        var ctx = _httpContextAccessor.HttpContext;
        // if 条件判断
        if (ctx == null) return (false, "No HttpContext");

        // 创建 List实例并赋给 claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Data!.User.UserCode),
            new(ClaimTypes.Name, result.Data.User.LoginCode),
            new(ClaimTypes.GivenName, result.Data.User.UserName),
            // null 合并操作 ??（若为 null 则使用右侧值）
            new("UserType", result.Data.User.UserType ?? ""),
            // null 合并操作 ??（若为 null 则使用右侧值）
            new("Avatar", result.Data.User.Avatar ?? "")
        };

        // 创建 ClaimsIdentity实例并赋给 identity
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // 创建 ClaimsPrincipal实例并赋给 principal
        var principal = new ClaimsPrincipal(identity);

        // await 异步等待
        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
        });
        // return 返回结果
        return (true, null);
    }

    // 方法 LogoutAsync
    // 方法：LogoutAsync
    public async Task LogoutAsync()
    {
        // 声明并初始化变量：ctx
        var ctx = _httpContextAccessor.HttpContext;
        // if 条件判断
        if (ctx != null)
            // await 异步等待
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
