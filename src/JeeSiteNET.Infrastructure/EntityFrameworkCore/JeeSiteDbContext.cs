using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

public class JeeSiteDbContext : DbContext
{
    private readonly IEnumerable<Assembly> _configurationAssemblies;

    public JeeSiteDbContext(DbContextOptions<JeeSiteDbContext> options, IEnumerable<Assembly> configurationAssemblies)
        : base(options)
    {
        _configurationAssemblies = configurationAssemblies;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var assembly in _configurationAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
