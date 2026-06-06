using System.Security.Claims;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Http;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserCode => GetClaim(ClaimTypes.NameIdentifier);
    public string UserName => GetClaim(ClaimTypes.GivenName);
    public string? UserType => GetClaim("UserType");
    public string? OrgCode => GetClaim("OrgCode");
    public string? OrgName => null;
    public List<string> RoleCodes => [];
    public List<string> Permissions => [];
    public bool IsSuperAdmin => UserCode == "admin";
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    private string GetClaim(string type)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value ?? string.Empty;
    }
}
