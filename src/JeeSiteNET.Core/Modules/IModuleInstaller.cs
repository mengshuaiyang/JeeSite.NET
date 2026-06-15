using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Modules;

// ================================================================
// 模块安装器接口 —— 模块化架构的核心约定
//
// JeeSite.NET 采用模块化设计，每个业务模块（Sys、Cms、CodeGen、Bpm 等）
// 都是一个独立的类库项目，拥有自己的 Domain、Application、Infrastructure
// 和 Controllers 文件夹。
//
// 每个模块必须在入口处放置一个实现了 IModuleInstaller 的类，
// ModuleLoader 在应用启动时自动扫描并执行所有模块的 ConfigureServices，
// 从而完成服务注册。详情见 ModuleLoader.cs → <模块名>ModuleInstaller.cs
//
// 阅读顺序：
//   Program.cs → ModuleLoader.cs → IModuleInstaller.cs
//   → SysModuleInstaller.cs → AuthController.cs → AuthService.cs
// ================================================================

public interface IModuleInstaller
{
    /// <summary>
    /// 模块服务注册入口。
    /// 每个模块在此方法中注册自己的：
    ///   - 仓储（IRepository → RepositoryImpl）
    ///   - 应用服务（Service）
    ///   - 第三方服务（如 OSS/Search/Notification）
    ///   - 拦截器、工具类等
    /// </summary>
    /// <param name="services">DI 容器，调用 AddScoped / AddSingleton / AddTransient 等</param>
    /// <param name="configuration">appsettings.json 等配置来源</param>
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
