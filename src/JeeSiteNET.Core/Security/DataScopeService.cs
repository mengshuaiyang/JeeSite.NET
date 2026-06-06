using System.Linq.Expressions;
using System.Reflection;

namespace JeeSiteNET.Core.Security;

public class DataScopeService : IDataScopeService
{
    private readonly ICurrentUser _currentUser;
    private readonly IDataScopeRuleProvider _ruleProvider;

    public DataScopeService(ICurrentUser currentUser, IDataScopeRuleProvider ruleProvider)
    {
        _currentUser = currentUser;
        _ruleProvider = ruleProvider;
    }

    public IQueryable<T> ApplyDataScope<T>(IQueryable<T> query, string targetType) where T : class
    {
        if (_currentUser.IsSuperAdmin)
            return query;

        var rules = _ruleProvider.GetRules(_currentUser, targetType);
        if (rules.Count == 0)
            return query;

        // Check if entity has OrgCode property
        var orgCodeProp = typeof(T).GetProperty("OrgCode", BindingFlags.Public | BindingFlags.Instance);
        if (orgCodeProp == null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "e");
        Expression? filter = null;

        foreach (var rule in rules)
        {
            Expression? ruleExpr = rule.ScopeType switch
            {
                DataScopeType.All => Expression.Constant(true),
                DataScopeType.Company => BuildOrgFilter<T>(parameter, orgCodeProp, _currentUser.OrgCode),
                DataScopeType.CompanyAndChild => BuildOrgLikeFilter<T>(parameter, _currentUser.OrgCode),
                DataScopeType.Office => BuildOrgFilter<T>(parameter, orgCodeProp, _currentUser.OrgCode),
                DataScopeType.OfficeAndChild => BuildOrgLikeFilter<T>(parameter, _currentUser.OrgCode),
                DataScopeType.Self => BuildUserFilter<T>(parameter, _currentUser.UserCode),
                DataScopeType.Custom when !string.IsNullOrEmpty(rule.ScopeCustomSql) => null,
                _ => null
            };

            if (ruleExpr != null)
                filter = filter == null ? ruleExpr : Expression.OrElse(filter, ruleExpr);
        }

        if (filter == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(filter, parameter);
        return query.Where(lambda);
    }

    private static Expression BuildOrgFilter<T>(ParameterExpression parameter, PropertyInfo orgCodeProp, string? orgCode)
    {
        if (string.IsNullOrEmpty(orgCode))
            return Expression.Constant(true);
        var propAccess = Expression.Property(parameter, orgCodeProp);
        var orgValue = Expression.Constant(orgCode);
        return Expression.Equal(propAccess, orgValue);
    }

    private static Expression BuildOrgLikeFilter<T>(ParameterExpression parameter, string? orgCode)
    {
        if (string.IsNullOrEmpty(orgCode))
            return Expression.Constant(true);

        // Build: e.OrgCode.StartsWith(orgCode) || e.ParentCodes.Contains(orgCode)
        var orgCodeProp = Expression.Property(parameter, "OrgCode");
        var startsWithMethod = typeof(string).GetMethod("StartsWith", [typeof(string)])!;
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;

        var orgValue = Expression.Constant(orgCode);
        var startsWithCall = Expression.Call(orgCodeProp, startsWithMethod, orgValue);

        // Check ParentCodes for ancestor containment
        var parentCodesProp = Expression.Property(parameter, "ParentCodes");
        var containsCall = Expression.Call(parentCodesProp, containsMethod, orgValue);

        return Expression.OrElse(startsWithCall, containsCall);
    }

    private static Expression BuildUserFilter<T>(ParameterExpression parameter, string userCode)
    {
        var createByProp = typeof(T).GetProperty("CreateBy", BindingFlags.Public | BindingFlags.Instance);
        if (createByProp == null)
            return Expression.Constant(true);

        var propAccess = Expression.Property(parameter, createByProp);
        var userValue = Expression.Constant(userCode);
        return Expression.Equal(propAccess, userValue);
    }
}


