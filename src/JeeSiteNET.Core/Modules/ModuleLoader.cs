using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Modules;

// ================================================================
// 模块加载器 —— 自动发现并加载所有业务模块
//
// 工作流程：
//   ① Program.cs 创建 ModuleLoader 实例
//   ② 调用 LoadModules()，扫描当前 AppDomain 中所有已加载的程序集
//   ③ 在每个程序集中查找实现了 IModuleInstaller 接口的类
//   ④ 对找到的每个类，反射创建实例，调用 ConfigureServices
//   ⑤ 同时收集 [ModuleDescription] 属性中的模块元信息
//
// 这样设计的好处：
//   - 新增一个业务模块只需要新建类库 + 写一个 ModuleInstaller
//   - 不需要修改 Program.cs 或任何中央配置文件
//   - 模块之间相互独立，可以单独编译、部署
// ================================================================

public class ModuleLoader
{
    private readonly List<IModuleDescriptor> _modules = [];

    /// <summary>已加载的所有模块描述列表，可供管理界面查看模块清单</summary>
    public IReadOnlyList<IModuleDescriptor> Modules => _modules.AsReadOnly();

    /// <summary>
    /// 扫描所有已加载的程序集，自动发现并执行所有 IModuleInstaller 实现。
    /// 这是模块化架构的"自动注册"核心机制，消除了手动逐个注册模块的繁琐操作。
    /// </summary>
    /// <param name="services">DI 服务集合，传递给各模块的 ConfigureServices</param>
    /// <param name="configuration">应用配置，传递给各模块的 ConfigureServices</param>
    public void LoadModules(IServiceCollection services, IConfiguration configuration)
    {
        // 获取当前 AppDomain 中所有已加载的程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            // 从程序集中筛选出：实现了 IModuleInstaller 接口的、非抽象的具体类
            var installerTypes = assembly.GetTypes()
                .Where(t => typeof(IModuleInstaller).IsAssignableFrom(t)
                            && t is { IsClass: true, IsAbstract: false });

            foreach (var type in installerTypes)
            {
                // 通过 Activator.CreateInstance 反射创建模块安装器实例
                var installer = (IModuleInstaller)Activator.CreateInstance(type)!;
                // 调用模块安装器的 ConfigureServices，让模块注册自己的服务
                installer.ConfigureServices(services, configuration);

                // 如果该类标注了 [ModuleDescription]，收集到模块清单中
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
