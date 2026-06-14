    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.App.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Application.Services
using JeeSiteNET.Modules.App.Application.Services;
    // 引入 JeeSiteNET.Modules.App.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Interfaces
using JeeSiteNET.Modules.App.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.App.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Infrastructure.Repositories
using JeeSiteNET.Modules.App.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;

// 定义 JeeSiteNET.Modules.App 命名空间
// 定义命名空间：JeeSiteNET.Modules.App
namespace JeeSiteNET.Modules.App;

[ModuleDescription(Code = "App", Name = "移动APP模块", Version = "1.0.0")]
// 定义class AppModuleInstaller
// 定义类：AppModuleInstaller
public class AppModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppCommentRepository, AppCommentRepository>();
        services.AddScoped<IAppUpgradeRepository, AppUpgradeRepository>();
        services.AddScoped<AppService>();
    }
}
