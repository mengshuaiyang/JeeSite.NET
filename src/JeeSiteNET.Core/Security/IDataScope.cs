namespace JeeSiteNET.Core.Security;

/// <summary>
/// 数据范围类型枚举：描述用户可见的数据覆盖范围
/// </summary>
public enum DataScopeType
{
    /// <summary>
    /// 全部数据：无过滤
    /// </summary>
    All,

    /// <summary>
    /// 本公司：仅查看当前用户公司下的数据
    /// </summary>
    Company,

    /// <summary>
    /// 本公司及下属：查看当前用户公司及所有下级子公司数据
    /// </summary>
    CompanyAndChild,

    /// <summary>
    /// 本部门：仅查看当前用户所属部门数据
    /// </summary>
    Office,

    /// <summary>
    /// 本部门及下属：查看当前用户部门及所有下级部门数据
    /// </summary>
    OfficeAndChild,

    /// <summary>
    /// 仅本人：仅查看当前用户创建的数据
    /// </summary>
    Self,

    /// <summary>
    /// 自定义 SQL：由业务方自行定义过滤表达式
    /// </summary>
    Custom
}

/// <summary>
/// 数据范围规则接口：描述某一目标类型所对应的可见范围
/// </summary>
public interface IDataScopeRule
{
    /// <summary>
    /// 目标类型（如实体名/表名，用于规则匹配）
    /// </summary>
    string TargetType { get; }

    /// <summary>
    /// 目标值（用于细粒度匹配，如特定菜单编码）
    /// </summary>
    string? TargetValue { get; }

    /// <summary>
    /// 数据范围类型
    /// </summary>
    DataScopeType ScopeType { get; }

    /// <summary>
    /// 自定义 SQL 片段（当 ScopeType = Custom 时使用）
    /// </summary>
    string? ScopeCustomSql { get; }
}

/// <summary>
/// 数据范围服务接口：将数据范围规则应用到查询对象上
/// </summary>
public interface IDataScopeService
{
    /// <summary>
    /// 为查询应用数据范围过滤
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <param name="targetType">目标类型标识</param>
    /// <returns>应用过滤后的查询对象</returns>
    IQueryable<T> ApplyDataScope<T>(IQueryable<T> query, string targetType) where T : class;
}

/// <summary>
/// 数据范围规则提供者接口：按用户与目标类型返回对应的规则列表
/// </summary>
public interface IDataScopeRuleProvider
{
    /// <summary>
    /// 获取当前用户在指定目标类型上的有效规则列表
    /// </summary>
    /// <param name="user">当前用户上下文</param>
    /// <param name="targetType">目标类型标识</param>
    /// <returns>数据范围规则列表</returns>
    List<DataScopeRule> GetRules(ICurrentUser user, string targetType);
}

/// <summary>
/// 数据范围规则实体：承载规则的具体内容
/// </summary>
public class DataScopeRule
{
    /// <summary>
    /// 数据范围类型
    /// </summary>
    public DataScopeType ScopeType { get; set; }

    /// <summary>
    /// 自定义 SQL 片段（当 ScopeType = Custom 时使用）
    /// </summary>
    public string? ScopeCustomSql { get; set; }
}
