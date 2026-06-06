using System.Security.Claims;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private List<string>? _cachedRoles;
    private List<string>? _cachedPermissions;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
    }

    public string UserCode => GetClaim(ClaimTypes.NameIdentifier);
    public string UserName => GetClaim(ClaimTypes.GivenName);
    public string? UserType => GetClaim("UserType");
    public string? OrgCode => GetClaim("OrgCode");
    public string? OrgName => null;
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public bool IsSuperAdmin => UserCode == "admin";

    public List<string> RoleCodes
    {
        get
        {
            if (IsSuperAdmin) return [];
            _cachedRoles ??= _userRoleRepository.GetRoleCodesByUserAsync(UserCode).GetAwaiter().GetResult();
            return _cachedRoles;
        }
    }

    public List<string> Permissions
    {
        get
        {
            if (IsSuperAdmin) return ["*"];
            _cachedPermissions ??= _menuRepository.GetPermissionsByRoleCodesAsync(RoleCodes).GetAwaiter().GetResult();
            return _cachedPermissions;
        }
    }

    private string GetClaim(string type)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value ?? string.Empty;
    }
}
