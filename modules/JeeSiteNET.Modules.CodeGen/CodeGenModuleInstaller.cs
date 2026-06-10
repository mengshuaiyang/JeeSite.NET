using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.CodeGen.Application.Services;
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
using JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;
using JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.CodeGen;

[ModuleDescription(Code = "CodeGen", Name = "代码生成器模块", Version = "1.0.0")]
public class CodeGenModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbIntrospectionProvider, DbIntrospectionProvider>();
        services.AddScoped<IGenTableRepository, GenTableRepository>();
        services.AddScoped<IGenTableColumnRepository, GenTableColumnRepository>();
        services.AddScoped<GenTableService>();
        services.AddScoped<CodeGenService>();
    }
}
