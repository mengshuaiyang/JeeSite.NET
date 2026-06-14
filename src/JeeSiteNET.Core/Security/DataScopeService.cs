using System.Linq.Expressions;
using System.Reflection;

namespace JeeSiteNET.Core.Security;

/// <summary>
/// 数据范围服务：根据当前登录用户拥有的角色和权限规则，动态为查询附加数据权限过滤条件
/// 支持 "全部数据 / 本公司 / 本公司及下属 / 本部门 / 本部门及下属 / 仅本人 / 自定义 SQL" 等范围
/// </summary>
public class DataScopeService : IDataScopeService
{
    /// <summary>
    /// 当前登录用户上下文（提供角色/组织/权限信息）
    /// </summary>
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// 数据范围规则提供者（根据用户与目标类型返回具体规则列表）
    /// </summary>
    private readonly IDataScopeRuleProvider _ruleProvider;

    /// <summary>
    /// 构造函数：通过 DI 注入当前用户上下文与规则提供者
    /// </summary>
    /// <param name="currentUser">当前用户上下文</param>
    /// <param name="ruleProvider">规则提供者</param>
    public DataScopeService(ICurrentUser currentUser, IDataScopeRuleProvider ruleProvider)
    {
        _currentUser = currentUser;
        _ruleProvider = ruleProvider;
    }

    /// <summary>
    /// 为指定查询应用数据范围过滤：超级管理员跳过过滤；其余用户根据规则构建 Expression 并附加到 query
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <param name="targetType">目标类型标识（如实体名/模块名，用于规则匹配）</param>
    /// <returns>应用过滤后的查询对象</returns>
    public IQueryable<T> ApplyDataScope<T>(IQueryable<T> query, string targetType) where T : class
    {
        // 超级管理员拥有全部数据权限，直接返回原始查询
        if (_currentUser.IsSuperAdmin)
            return query;

        // 获取当前用户在该目标类型上的规则集合；无规则时跳过过滤
        var rules = _ruleProvider.GetRules(_currentUser, targetType);
        if (rules.Count == 0)
            return query;

        // 要求实体具有 OrgCode 字段（按组织隔离的前置条件），否则无法应用组织级过滤
        var orgCodeProp = typeof(T).GetProperty("OrgCode", BindingFlags.Public | BindingFlags.Instance);
        if (orgCodeProp == null)
            return query;

        // 动态构建 Expression：最终拼出 e => rule1 || rule2 || ...
        var parameter = Expression.Parameter(typeof(T), "e");
        Expression? filter = null;

        foreach (var rule in rules)
        {
            // 按规则类型分别构建子表达式；自定义 SQL 暂由外部处理，此处返回 null
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

            // 多条规则采用 OR 合并（用户可能同时拥有多个角色的多个范围）
            if (ruleExpr != null)
                filter = filter == null ? ruleExpr : Expression.OrElse(filter, ruleExpr);
        }

        // 若全部规则都未构建出表达式（如仅含 Custom 且未实现），则不修改原查询
        if (filter == null)
            return query;

        // 将合成的 filter 包装成 lambda 并附加到 query.Where
        var lambda = Expression.Lambda<Func<T, bool>>(filter, parameter);
        return query.Where(lambda);
    }

    /// <summary>
    /// 构建等值组织过滤表达式：e.OrgCode == orgCode
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="parameter">lambda 参数表达式</param>
    /// <param name="orgCodeProp">OrgCode 属性反射信息</param>
    /// <param name="orgCode">用户所属组织编码</param>
    /// <returns>表达式树片段</returns>
    private static Expression BuildOrgFilter<T>(ParameterExpression parameter, PropertyInfo orgCodeProp, string? orgCode)
    {
        // 组织编码为空时视为无限制，返回 true
        if (string.IsNullOrEmpty(orgCode))
            return Expression.Constant(true);
        var propAccess = Expression.Property(parameter, orgCodeProp);
        var orgValue = Expression.Constant(orgCode);
        return Expression.Equal(propAccess, orgValue);
    }

    /// <summary>
    /// 构建包含子组织的过滤表达式：e.OrgCode.StartsWith(orgCode) || e.ParentCodes.Contains(orgCode)
    /// 适用于树形组织，通过 StartsWith 匹配自身与子级，ParentCodes 匹配祖先链中的包含关系
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="parameter">lambda 参数表达式</param>
    /// <param name="orgCode">用户所属组织编码</param>
    /// <returns>表达式树片段</returns>
    private static Expression BuildOrgLikeFilter<T>(ParameterExpression parameter, string? orgCode)
    {
        if (string.IsNullOrEmpty(orgCode))
            return Expression.Constant(true);

        // 使用反射动态获取 string.StartsWith / string.Contains 方法信息
        var orgCodeProp = Expression.Property(parameter, "OrgCode");
        var startsWithMethod = typeof(string).GetMethod("StartsWith", [typeof(string)])!;
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;

        var orgValue = Expression.Constant(orgCode);
        var startsWithCall = Expression.Call(orgCodeProp, startsWithMethod, orgValue);

        // ParentCodes 字段存储上级组织编码路径，用于判断是否为子组织
        var parentCodesProp = Expression.Property(parameter, "ParentCodes");
        var containsCall = Expression.Call(parentCodesProp, containsMethod, orgValue);

        // OR 组合两种匹配方式
        return Expression.OrElse(startsWithCall, containsCall);
    }

    /// <summary>
    /// 构建仅本人数据过滤表达式：e.CreateBy == userCode
    /// 要求实体实现 CreateBy 字段（BaseEntity 已提供）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="parameter">lambda 参数表达式</param>
    /// <param name="userCode">当前用户编码</param>
    /// <returns>表达式树片段</returns>
    private static Expression BuildUserFilter<T>(ParameterExpression parameter, string userCode)
    {
        // 实体不具备 CreateBy 属性时视为无限制
        var createByProp = typeof(T).GetProperty("CreateBy", BindingFlags.Public | BindingFlags.Instance);
        if (createByProp == null)
            return Expression.Constant(true);

        var propAccess = Expression.Property(parameter, createByProp);
        var userValue = Expression.Constant(userCode);
        return Expression.Equal(propAccess, userValue);
    }
}
