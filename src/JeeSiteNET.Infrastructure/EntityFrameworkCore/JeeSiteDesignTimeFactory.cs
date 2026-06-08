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

        var options = new DbContextOptionsBuilder<JeeSiteDbContext>();
        options.UseSqlServer(connStr, sql =>
            sql.MigrationsAssembly(typeof(JeeSiteDbContext).Assembly.FullName)
               .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

        return new JeeSiteDbContext(options.Options, []);
    }
}
