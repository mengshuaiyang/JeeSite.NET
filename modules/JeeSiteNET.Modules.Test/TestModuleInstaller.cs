    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.Test.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Application.Services
using JeeSiteNET.Modules.Test.Application.Services;
    // 引入 JeeSiteNET.Modules.Test.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Interfaces
using JeeSiteNET.Modules.Test.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Test.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Infrastructure.Repositories
using JeeSiteNET.Modules.Test.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;

// 定义 JeeSiteNET.Modules.Test 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test
namespace JeeSiteNET.Modules.Test;

[ModuleDescription(Code = "Test", Name = "测试模块", Version = "1.0.0")]
// 定义class TestModuleInstaller
// 定义类：TestModuleInstaller
public class TestModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITestDataRepository, TestDataRepository>();
        services.AddScoped<ITestTreeRepository, TestTreeRepository>();
        services.AddScoped<TestService>();
    }
}
