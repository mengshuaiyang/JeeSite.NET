using System.Security.Claims;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IFusionCache _cache;
    private List<string>? _cachedRoles;
    private List<string>? _cachedPermissions;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IFusionCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _cache = cache;
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
            _cachedRoles ??= _cache.GetOrSet(CacheKeys.RoleCodesByUser(UserCode), (ct) =>
            {
                return _userRoleRepository.GetRoleCodesByUserAsync(UserCode).GetAwaiter().GetResult();
            }, TimeSpan.FromMinutes(10));
            return _cachedRoles;
        }
    }

    public List<string> Permissions
    {
        get
        {
            if (IsSuperAdmin) return ["*"];
            _cachedPermissions ??= _cache.GetOrSet(CacheKeys.PermissionsByRoles(string.Join(",", RoleCodes)), (ct) =>
            {
                return _menuRepository.GetPermissionsByRoleCodesAsync(RoleCodes).GetAwaiter().GetResult();
            }, TimeSpan.FromMinutes(10));
            return _cachedPermissions;
        }
    }

    private string GetClaim(string type)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value ?? string.Empty;
    }
}
