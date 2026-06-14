using System.Security.Claims;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

/// <summary>
/// 当前用户信息实现。
/// 通过 HTTP 上下文 Claims 读取用户编码/名称/类型/组织，
/// 用户角色与权限通过 FusionCache 二级缓存加速，避免重复查询。
/// </summary>
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IFusionCache _cache;
    private List<string>? _cachedRoles;
    private List<string>? _cachedPermissions;

    /// <summary>
    /// 初始化 <see cref="CurrentUser"/> 的新实例。
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器。</param>
    /// <param name="userRoleRepository">用户-角色仓储。</param>
    /// <param name="menuRepository">菜单/权限仓储。</param>
    /// <param name="cache">FusionCache 缓存服务。</param>
    public CurrentUser(IHttpContextAccessor httpContextAccessor, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IFusionCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _cache = cache;
    }

    /// <summary>
    /// 当前用户编码（取自 ClaimTypes.NameIdentifier）。
    /// </summary>
    public string UserCode => GetClaim(ClaimTypes.NameIdentifier);

    /// <summary>
    /// 当前用户姓名（取自 ClaimTypes.GivenName）。
    /// </summary>
    public string UserName => GetClaim(ClaimTypes.GivenName);

    /// <summary>
    /// 当前用户类型（员工/管理员等）；若未声明则返回 null。
    /// </summary>
    public string? UserType => GetClaim("UserType");

    /// <summary>
    /// 当前用户所属机构编码；若未声明则返回 null。
    /// </summary>
    public string? OrgCode => GetClaim("OrgCode");

    /// <summary>
    /// 当前用户所属机构名称；本实现不填充（保留扩展点）。
    /// </summary>
    public string? OrgName => null;

    /// <summary>
    /// 是否已认证。根据 HTTP 上下文 User.Identity 判断；无上下文视为未认证。
    /// </summary>
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// 是否为超级管理员（admin）。
    /// </summary>
    public bool IsSuperAdmin => GetClaim(ClaimTypes.Name) == "admin";

    /// <summary>
    /// 当前用户拥有的角色编码集合。
    /// 超级管理员返回空集合（表示不受限制）；其余用户通过缓存+仓储读取。
    /// </summary>
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

    /// <summary>
    /// 当前用户拥有的权限标识集合。
    /// 超级管理员返回 ["*"]；其余用户根据角色缓存后聚合。
    /// </summary>
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

    /// <summary>
    /// 从当前 HTTP 上下文的 ClaimsPrincipal 按类型读取单个声明值；不存在返回空字符串。
    /// </summary>
    /// <param name="type">声明类型。</param>
    /// <returns>声明的值或空字符串。</returns>
    private string GetClaim(string type)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value ?? string.Empty;
    }
}
