using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 数据库提供程序类型枚举
/// </summary>
public enum DatabaseProviderType
{
    /// <summary>
    /// Microsoft SQL Server
    /// </summary>
    SqlServer,
    /// <summary>
    /// SQLite（轻量本地文件数据库）
    /// </summary>
    Sqlite,
    /// <summary>
    /// PostgreSQL / GaussDB（高斯）
    /// </summary>
    PostgreSQL,
    /// <summary>
    /// 达梦数据库 DM8
    /// </summary>
    Dm,
    /// <summary>
    /// 人大金仓 KingbaseES
    /// </summary>
    KingbaseES,
}

/// <summary>
/// 数据库提供程序工厂：按配置字符串解析目标类型并动态注入 EF Core UseXxx 扩展方法。
/// 内置 SqlServer / Sqlite 直接引用；PostgreSQL / DM / KingbaseES 通过反射加载，支持按需 NuGet 安装。
/// </summary>
public static class DatabaseProviderFactory
{
    /// <summary>
    /// 外部提供程序（非内置）的 NuGet 包名与 UseXxx 扩展方法映射
    /// </summary>
    private static readonly Dictionary<DatabaseProviderType, (string Package, string Method)> ProviderMap = new()
    {
        [DatabaseProviderType.PostgreSQL] = ("Npgsql.EntityFrameworkCore.PostgreSQL", "UseNpgsql"),
        [DatabaseProviderType.Dm] = ("Dm.EntityFrameworkCore", "UseDm"),
        [DatabaseProviderType.KingbaseES] = ("Kdbndp.EntityFrameworkCore.KingbaseES", "UseKingbaseES"),
    };

    /// <summary>
    /// 将字符串（大小写兼容的别名）解析为 DatabaseProviderType；无法识别时默认 SqlServer
    /// </summary>
    /// <param name="provider">提供者字符串，如 "PostgreSQL"、"gaussdb"、"dm8" 等</param>
    /// <returns>解析后的枚举值</returns>
    public static DatabaseProviderType Parse(string? provider)
    {
        return provider?.ToLowerInvariant() switch
        {
            "sqlserver" => DatabaseProviderType.SqlServer,
            "sqlite" => DatabaseProviderType.Sqlite,
            "postgresql" or "npgsql" or "gaussdb" => DatabaseProviderType.PostgreSQL,
            "dm" or "dm8" or "dameng" => DatabaseProviderType.Dm,
            "kingbase" or "kingbasees" or "kdbndp" => DatabaseProviderType.KingbaseES,
            _ => DatabaseProviderType.SqlServer,
        };
    }

    /// <summary>
    /// 将所选提供程序应用到 DbContextOptionsBuilder（配置数据库连接）
    /// </summary>
    /// <param name="options">DbContext 选项构建器</param>
    /// <param name="provider">数据库提供者类型</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="migrationsAssembly">迁移所在程序集（可为 null）</param>
    public static void UseProvider(this DbContextOptionsBuilder options, DatabaseProviderType provider, string connectionString, string? migrationsAssembly)
    {
        switch (provider)
        {
            case DatabaseProviderType.SqlServer:
                options.UseSqlServer(connectionString, sql =>
                {
                    if (migrationsAssembly != null)
                        sql.MigrationsAssembly(migrationsAssembly);
                    sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
                break;

            case DatabaseProviderType.Sqlite:
                options.UseSqlite(connectionString);
                break;

            default:
                UseExternalProvider(options, provider, connectionString);
                break;
        }
    }

    /// <summary>
    /// 通过反射动态加载外部 Provider 程序集并调用 UseXxx 扩展方法
    /// </summary>
    /// <param name="options">DbContext 选项构建器</param>
    /// <param name="provider">目标提供程序类型</param>
    /// <param name="connectionString">连接字符串</param>
    private static void UseExternalProvider(DbContextOptionsBuilder options, DatabaseProviderType provider, string connectionString)
    {
        var (package, methodName) = ProviderMap[provider];

        Assembly asm;
        try
        {
            asm = Assembly.Load(package);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"缺少 {provider} 的 EF Core 提供程序包。请安装 NuGet 包: {package}", ex);
        }

        var extensionType = asm.GetTypes()
            .FirstOrDefault(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Any(m => m.Name == methodName && !m.IsGenericMethod));

        if (extensionType == null)
            throw new InvalidOperationException($"在程序集 '{package}' 中未找到扩展方法 '{methodName}'。");

        var method = extensionType.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m => m.Name == methodName && !m.IsGenericMethod &&
                m.GetParameters().Length == 2 &&
                m.GetParameters()[0].ParameterType == typeof(DbContextOptionsBuilder) &&
                m.GetParameters()[1].ParameterType == typeof(string));

        if (method != null)
        {
            method.Invoke(null, [options, connectionString]);
            return;
        }

        method = extensionType.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m => m.Name == methodName && !m.IsGenericMethod &&
                m.GetParameters().Length == 3 &&
                m.GetParameters()[0].ParameterType == typeof(DbContextOptionsBuilder) &&
                m.GetParameters()[1].ParameterType == typeof(string));

        if (method != null)
        {
            method.Invoke(null, [options, connectionString, null]);
            return;
        }

        throw new InvalidOperationException(
            $"无法匹配 {methodName} 扩展方法签名 (程序集: {package})。");
    }
}
