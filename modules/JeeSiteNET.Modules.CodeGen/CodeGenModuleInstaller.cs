    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.CodeGen.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Application.Services
using JeeSiteNET.Modules.CodeGen.Application.Services;
    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Interfaces
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection
using JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;
    // 引入 JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories
using JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;

// 定义 JeeSiteNET.Modules.CodeGen 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen
namespace JeeSiteNET.Modules.CodeGen;

[ModuleDescription(Code = "CodeGen", Name = "代码生成器模块", Version = "1.0.0")]
// 定义class CodeGenModuleInstaller
// 定义类：CodeGenModuleInstaller
public class CodeGenModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbIntrospectionProvider, DbIntrospectionProvider>();
        services.AddScoped<IGenTableRepository, GenTableRepository>();
        services.AddScoped<IGenTableColumnRepository, GenTableColumnRepository>();
        services.AddScoped<GenTableService>();
        services.AddScoped<CodeGenService>();
    }
}
