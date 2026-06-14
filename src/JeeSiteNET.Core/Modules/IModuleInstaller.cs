using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Modules;

/// <summary>
/// 模块安装器接口：各业务模块实现此接口以注册自身的服务、仓储、控制器等依赖，
/// 由 ModuleLoader 在应用启动时统一调用
/// </summary>
public interface IModuleInstaller
{
    /// <summary>
    /// 注册模块所需的服务与配置
    /// </summary>
    /// <param name="services">DI 服务集合（用于 AddScoped/AddSingleton 等）</param>
    /// <param name="configuration">应用配置（读取模块参数）</param>
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
