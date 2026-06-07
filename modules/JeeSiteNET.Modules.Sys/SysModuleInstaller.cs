using JeeSiteNET.Core.Modules;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Modules.Sys.Infrastructure;
using JeeSiteNET.Modules.Sys.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Sys;

[ModuleDescription(Code = "Sys", Name = "系统管理模块", Version = "1.0.0")]
public class SysModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IDictTypeRepository, DictTypeRepository>();
        services.AddScoped<IDictDataRepository, DictDataRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IConfigRepository, ConfigRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
        services.AddScoped<IRoleDataScopeRepository, RoleDataScopeRepository>();
        services.AddScoped<IUserDataScopeRepository, UserDataScopeRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<RoleMenuService>();
        services.AddScoped<RoleService>();
        services.AddScoped<MenuService>();
        services.AddScoped<OrganizationService>();
        services.AddScoped<AuthService>();
        services.AddScoped<DictTypeService>();
        services.AddScoped<DictDataService>();
        services.AddScoped<PostService>();
        services.AddScoped<ConfigService>();
        services.AddScoped<ModuleService>();
        services.AddScoped<LogService>();
        services.AddScoped<RoleDataScopeService>();
        services.AddScoped<IDataScopeRuleProvider, SysDataScopeRuleProvider>();
        services.AddScoped<IDataScopeService, DataScopeService>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<TenantService>();
    }
}
