using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

public class JeeSiteDesignTimeFactory : IDesignTimeDbContextFactory<JeeSiteDbContext>
{
    public JeeSiteDbContext CreateDbContext(string[] args)
    {
        var connStr = args.Length > 0
            ? args[0]
            : "Server=localhost;Database=JeeSiteNET;Trusted_Connection=true;TrustServerCertificate=true";

        var providerType = args.Length > 1
            ? DatabaseProviderFactory.Parse(args[1])
            : DatabaseProviderType.SqlServer;

        var options = new DbContextOptionsBuilder<JeeSiteDbContext>();
        if (providerType == DatabaseProviderType.Sqlite)
        {
            options.UseSqlite(connStr);
        }
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

        return new JeeSiteDbContext(options.Options, assemblies);
    }
}
