using JeeSiteNET.Core.Security;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

public class SysDataScopeRuleProvider : IDataScopeRuleProvider
{
    private readonly JeeSiteDbContext _db;

    public SysDataScopeRuleProvider(JeeSiteDbContext db)
    {
        _db = db;
    }

    public List<DataScopeRule> GetRules(ICurrentUser user, string targetType)
    {
        if (user.IsSuperAdmin || user.RoleCodes.Count == 0)
            return [];

        // 1. User-level data scope (highest priority)
        var userScopes = _db.Set<UserDataScope>()
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

        // 2. Menu-level data scope (per menu override)
        var menuScopes = _db.Set<MenuDataScope>()
            .Where(m => user.RoleCodes.Contains(m.RoleCode) && m.MenuCode == targetType)
            .ToList();

        if (menuScopes.Count > 0)
        {
            return menuScopes
                .Select(MapToRuleFromMenu)
                .ToList();
        }

        // 3. Role-level data scope (fallback to Role.DataScope field)
        var roles = _db.Set<Role>()
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

    private static DataScopeRule? MapToRuleFromUser(UserDataScope u)
    {
        if (!Enum.TryParse<DataScopeType>(u.CtrlType, true, out var scopeType))
            return new DataScopeRule { ScopeType = DataScopeType.Self };
        return new DataScopeRule { ScopeType = scopeType, ScopeCustomSql = u.CtrlData };
    }

    private static DataScopeRule MapToRuleFromMenu(MenuDataScope m)
    {
        if (!Enum.TryParse<DataScopeType>(m.RuleType, true, out var scopeType))
            return new DataScopeRule { ScopeType = DataScopeType.All };
        return new DataScopeRule { ScopeType = scopeType, ScopeCustomSql = m.RuleConfig };
    }
}
