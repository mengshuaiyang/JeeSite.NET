using JeeSiteNET.Core.Security;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

/// <summary>
/// 数据范围规则提供者。
/// 按以下优先级返回用户适用的数据范围规则：
/// 1. 用户级数据范围（UserDataScope，最高优先级）
/// 2. 菜单级数据范围（MenuDataScope，按目标菜单覆盖）
/// 3. 角色级数据范围（Role.DataScope，最低优先级）
/// </summary>
public class SysDataScopeRuleProvider : IDataScopeRuleProvider
{
    private readonly JeeSiteDbContext _db;

    /// <summary>
    /// 初始化 <see cref="SysDataScopeRuleProvider"/> 的新实例。
    /// </summary>
    /// <param name="db">数据库上下文。</param>
    public SysDataScopeRuleProvider(JeeSiteDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// 获取当前用户在指定目标菜单下的数据范围规则。
    /// 超级管理员或无角色用户返回空集合（表示不受限制）。
    /// </summary>
    /// <param name="user">当前用户上下文。</param>
    /// <param name="targetType">目标类型（通常为菜单编码）。</param>
    /// <returns>数据范围规则列表。</returns>
    public List<DataScopeRule> GetRules(ICurrentUser user, string targetType)
    {
        if (user.IsSuperAdmin || user.RoleCodes.Count == 0)
            return [];

        // 1. 用户级数据范围（最高优先级）
        var userScopes = _db.Set<UserDataScope>().AsNoTracking()
            .Where(u => user.UserCode == u.UserCode)
            .ToList();

        if (userScopes.Count > 0)
        {
            return userScopes
                .Select(MapToRuleFromUser)
                .Where(r => r != null)
                .Cast<DataScopeRule>()
                .ToList();
        }

        // 2. 菜单级数据范围（按菜单覆盖）
        var menuScopes = _db.Set<MenuDataScope>().AsNoTracking()
            .Where(m => user.RoleCodes.Contains(m.RoleCode) && m.MenuCode == targetType)
            .ToList();

        if (menuScopes.Count > 0)
        {
            return menuScopes
                .Select(MapToRuleFromMenu)
                .ToList();
        }

        // 3. 角色级数据范围（回退到 Role.DataScope）
        var roles = _db.Set<Role>().AsNoTracking()
            .Where(r => user.RoleCodes.Contains(r.RoleCode))
            .ToList();

        var rules = new List<DataScopeRule>();
        foreach (var role in roles)
        {
            if (string.IsNullOrEmpty(role.DataScope))
            {
                rules.Add(new DataScopeRule { ScopeType = DataScopeType.All });
                continue;
            }

            if (Enum.TryParse<DataScopeType>(role.DataScope, true, out var scopeType))
                rules.Add(new DataScopeRule { ScopeType = scopeType });
            else
                rules.Add(new DataScopeRule { ScopeType = DataScopeType.All });
        }

        return rules;
    }

    /// <summary>
    /// 将 UserDataScope 实体映射为 <see cref="DataScopeRule"/>。
    /// </summary>
    /// <param name="u">用户级数据范围实体。</param>
    /// <returns>数据范围规则；解析失败时默认为仅自己。</returns>
    private static DataScopeRule? MapToRuleFromUser(UserDataScope u)
    {
        if (!Enum.TryParse<DataScopeType>(u.CtrlType, true, out var scopeType))
            return new DataScopeRule { ScopeType = DataScopeType.Self };
        return new DataScopeRule { ScopeType = scopeType, ScopeCustomSql = u.CtrlData };
    }

    /// <summary>
    /// 将 MenuDataScope 实体映射为 <see cref="DataScopeRule"/>。
    /// </summary>
    /// <param name="m">菜单级数据范围实体。</param>
    /// <returns>数据范围规则；解析失败时默认为全部范围。</returns>
    private static DataScopeRule MapToRuleFromMenu(MenuDataScope m)
    {
        if (!Enum.TryParse<DataScopeType>(m.RuleType, true, out var scopeType))
            return new DataScopeRule { ScopeType = DataScopeType.All };
        return new DataScopeRule { ScopeType = scopeType, ScopeCustomSql = m.RuleConfig };
    }
}
