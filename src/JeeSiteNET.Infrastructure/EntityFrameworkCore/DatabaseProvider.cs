using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

public enum DatabaseProviderType
{
    SqlServer,
    Sqlite,
    PostgreSQL,
    Dm,
    KingbaseES,
}

public static class DatabaseProviderFactory
{
    private static readonly Dictionary<DatabaseProviderType, (string Package, string Method)> ProviderMap = new()
    {
        [DatabaseProviderType.PostgreSQL] = ("Npgsql.EntityFrameworkCore.PostgreSQL", "UseNpgsql"),
        [DatabaseProviderType.Dm] = ("Dm.EntityFrameworkCore", "UseDm"),
        [DatabaseProviderType.KingbaseES] = ("Kdbndp.EntityFrameworkCore.KingbaseES", "UseKingbaseES"),
    };

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
