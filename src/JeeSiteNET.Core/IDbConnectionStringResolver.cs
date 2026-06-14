namespace JeeSiteNET.Core;

/// <summary>
/// 数据库操作类型枚举：用于读写分离场景下指定使用的数据源
/// </summary>
public enum DbOperation
{
    /// <summary>
    /// 读操作：路由到只读/查询数据源（可能是从库）
    /// </summary>
    Read,

    /// <summary>
    /// 写操作：路由到主库数据源
    /// </summary>
    Write
}

/// <summary>
/// 数据库连接字符串解析器接口：支持多租户 + 读写分离场景下动态选择连接字符串
/// </summary>
public interface IDbConnectionStringResolver
{
    /// <summary>
    /// 根据租户编码与操作类型获取对应的连接字符串
    /// </summary>
    /// <param name="tenantCode">租户编码（null 表示使用默认租户）</param>
    /// <param name="operation">操作类型（Read/Write）</param>
    /// <returns>数据库连接字符串</returns>
    string GetConnectionString(string? tenantCode = null, DbOperation operation = DbOperation.Write);

    /// <summary>
    /// 按操作类型获取默认连接字符串（不区分租户）
    /// </summary>
    /// <param name="operation">操作类型（Read/Write）</param>
    /// <returns>数据库连接字符串</returns>
    string GetConnectionString(DbOperation operation);
}
