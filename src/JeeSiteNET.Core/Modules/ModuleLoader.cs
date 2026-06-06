using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Core.Modules;

public class ModuleLoader
{
    private readonly List<IModuleDescriptor> _modules = [];

    public IReadOnlyList<IModuleDescriptor> Modules => _modules.AsReadOnly();

    public void LoadModules(IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var installerTypes = assembly.GetTypes()
                .Where(t => typeof(IModuleInstaller).IsAssignableFrom(t)
                            && t is { IsClass: true, IsAbstract: false });

            foreach (var type in installerTypes)
            {
                var installer = (IModuleInstaller)Activator.CreateInstance(type)!;
                installer.ConfigureServices(services, configuration);

                var descriptor = type.GetCustomAttributes(typeof(ModuleDescriptionAttribute), false)
                    .OfType<ModuleDescriptionAttribute>()
                    .FirstOrDefault();

                if (descriptor != null)
                {
                    _modules.Add(descriptor);
                }
            }
        }
    }
}
