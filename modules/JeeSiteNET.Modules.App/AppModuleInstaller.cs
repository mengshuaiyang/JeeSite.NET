using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.App.Application.Services;
using JeeSiteNET.Modules.App.Domain.Interfaces;
using JeeSiteNET.Modules.App.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.App;

[ModuleDescription(Code = "App", Name = "移动APP模块", Version = "1.0.0")]
public class AppModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppCommentRepository, AppCommentRepository>();
        services.AddScoped<IAppUpgradeRepository, AppUpgradeRepository>();
        services.AddScoped<AppService>();
    }
}
