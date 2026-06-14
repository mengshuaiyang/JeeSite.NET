using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Modules;

/// <summary>
/// 模块加载器：在应用启动时扫描所有已加载的程序集，
/// 自动发现并调用 IModuleInstaller 实现类，同时收集模块描述信息用于调试或展示
/// </summary>
public class ModuleLoader
{
    /// <summary>
    /// 已发现的模块描述符集合（用于对外暴露模块清单）
    /// </summary>
    private readonly List<IModuleDescriptor> _modules = [];

    /// <summary>
    /// 已发现的模块描述符只读列表
    /// </summary>
    public IReadOnlyList<IModuleDescriptor> Modules => _modules.AsReadOnly();

    /// <summary>
    /// 扫描当前应用域中的所有程序集，查找并执行所有 IModuleInstaller 实现，
    /// 同时收集 ModuleDescriptionAttribute 中的元信息
    /// </summary>
    /// <param name="services">DI 服务集合，传递给各模块的 ConfigureServices</param>
    /// <param name="configuration">应用配置，传递给各模块的 ConfigureServices</param>
    public void LoadModules(IServiceCollection services, IConfiguration configuration)
    {
        // 获取当前应用域中已加载的全部程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            // 在每个程序集中过滤出具体的、可实例化的 IModuleInstaller 实现
            var installerTypes = assembly.GetTypes()
                .Where(t => typeof(IModuleInstaller).IsAssignableFrom(t)
                            && t is { IsClass: true, IsAbstract: false });

            foreach (var type in installerTypes)
            {
                // 通过反射创建安装器实例并执行依赖注入
                var installer = (IModuleInstaller)Activator.CreateInstance(type)!;
                installer.ConfigureServices(services, configuration);

                // 同时读取类上标注的模块描述，登记到模块清单
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
