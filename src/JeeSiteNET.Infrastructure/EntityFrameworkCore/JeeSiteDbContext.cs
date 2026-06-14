using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

/// <summary>
/// JeeSite.NET 主数据库上下文：通过扫描配置程序集自动装载实体映射（IEntityTypeConfiguration）
/// </summary>
public class JeeSiteDbContext : DbContext
{
    /// <summary>
    /// 实体映射配置所在程序集集合
    /// </summary>
    private readonly IEnumerable<Assembly> _configurationAssemblies;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">EF Core 配置选项（含数据库提供程序）</param>
    /// <param name="configurationAssemblies">存放 IEntityTypeConfiguration 的程序集集合</param>
    public JeeSiteDbContext(DbContextOptions<JeeSiteDbContext> options, IEnumerable<Assembly> configurationAssemblies)
        : base(options)
    {
        _configurationAssemblies = configurationAssemblies;
    }

    /// <summary>
    /// 模型创建时从指定程序集中应用所有 IEntityTypeConfiguration 实体映射配置
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var assembly in _configurationAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
