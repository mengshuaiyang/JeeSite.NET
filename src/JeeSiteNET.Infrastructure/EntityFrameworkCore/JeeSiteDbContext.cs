using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

// ================================================================
// JeeSite.NET 主数据库上下文
//
// 作用位置：Program.cs 第 124-153 行
// 关联链路：Program.cs → ModuleLoader 加载的实体程序集集合
//           → SysModuleInstaller 中注册的各个仓储（Repository）
//           → AuthService / UserService 等应用服务
//
// 每个业务模块在自己的程序集中定义实体类和 IEntityTypeConfiguration，
// 在 Program.cs 中注册程序集后，EF Core 会自动发现所有实体配置。
// 不需要每个模块各自调用 AddDbContext——所有模块共享同一个 JeeSiteDbContext。
//
// 关键特性：
//   - 支持多数据库（SqlServer/Sqlite/PostgreSQL/达梦/人大金仓）
//   - 自动审计（AuditInterceptor 记录谁在何时改了哪些字段）
//   - 自动树形维护（TreeEntityInterceptor 计算 ParentCodes/TreeSort/TreeLevel）
//   - 软删除（SoftDeleteInterceptor 将 DELETE 转为 UPDATE IsDeleted=1）
// ================================================================

/// <summary>JeeSite.NET 主数据库上下文</summary>
public class JeeSiteDbContext : DbContext
{
    /// <summary>各业务模块的实体配置程序集列表</summary>
    private readonly IEnumerable<Assembly> _configurationAssemblies;

    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="options">EF Core 选项（含数据库类型和连接字符串）</param>
    /// <param name="configurationAssemblies">存放 IEntityTypeConfiguration 的程序集集合，在 Program.cs 中注册</param>
    public JeeSiteDbContext(DbContextOptions<JeeSiteDbContext> options, IEnumerable<Assembly> configurationAssemblies)
        : base(options)
    {
        _configurationAssemblies = configurationAssemblies;
    }

    /// <summary>
    /// 模型创建时，扫描各模块程序集中的 IEntityTypeConfiguration 配置。
    /// 这意味着新增一个实体只需要写 Entity + Configuration 两个类，
    /// EF Core 自动应用，无需手动调用 modelBuilder.Entity<T>()。
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 遍历每个模块程序集，自动应用其中的 EntityTypeConfiguration
        foreach (var assembly in _configurationAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
