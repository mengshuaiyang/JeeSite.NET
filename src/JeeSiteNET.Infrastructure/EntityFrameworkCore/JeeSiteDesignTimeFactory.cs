    // 引入 System.Reflection 命名空间
// 引入命名空间：System.Reflection
using System.Reflection;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Design 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Design
using Microsoft.EntityFrameworkCore.Design;

// 定义 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 定义命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

// 定义class JeeSiteDesignTimeFactory
// 定义类：JeeSiteDesignTimeFactory
public class JeeSiteDesignTimeFactory : IDesignTimeDbContextFactory<JeeSiteDbContext>
{
    // 方法 CreateDbContext
    // 方法：CreateDbContext
    public JeeSiteDbContext CreateDbContext(string[] args)
    {
        // 声明并初始化变量：connStr
        var connStr = args.Length > 0
            // 三元条件表达式
            ? args[0]
            : "Server=localhost;Database=JeeSiteNET;Trusted_Connection=true;TrustServerCertificate=true";

        // 声明并初始化变量：providerType
        var providerType = args.Length > 1
            // 调用 Parse
            ? DatabaseProviderFactory.Parse(args[1])
            : DatabaseProviderType.SqlServer;

        // 创建 DbContextOptionsBuilder实例并赋给 options
        var options = new DbContextOptionsBuilder<JeeSiteDbContext>();
        // if 条件判断
        if (providerType == DatabaseProviderType.Sqlite)
        {
            options.UseSqlite(connStr);
        }
        // else 否则分支
        else
        {
            options.UseProvider(providerType, connStr,
                typeof(JeeSiteDbContext).Assembly.FullName);
        }

        Assembly[] assemblies =
        [
            Assembly.Load("JeeSiteNET.Modules.Sys"),
            Assembly.Load("JeeSiteNET.Modules.Tasks"),
            Assembly.Load("JeeSiteNET.Modules.Cms"),
            Assembly.Load("JeeSiteNET.Modules.Bpm"),
            Assembly.Load("JeeSiteNET.Modules.CodeGen"),
        ];

        // return 返回结果
        return new JeeSiteDbContext(options.Options, assemblies);
    }
}
