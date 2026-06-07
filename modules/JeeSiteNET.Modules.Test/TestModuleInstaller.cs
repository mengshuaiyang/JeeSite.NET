using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.Test.Application.Services;
using JeeSiteNET.Modules.Test.Domain.Interfaces;
using JeeSiteNET.Modules.Test.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Test;

[ModuleDescription(Code = "Test", Name = "测试模块", Version = "1.0.0")]
public class TestModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITestDataRepository, TestDataRepository>();
        services.AddScoped<ITestTreeRepository, TestTreeRepository>();
        services.AddScoped<TestService>();
    }
}
