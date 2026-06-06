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
}
