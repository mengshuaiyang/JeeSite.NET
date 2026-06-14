    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Core.Search 命名空间
// 引入命名空间：JeeSiteNET.Core.Search
using JeeSiteNET.Core.Search;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.Storage 命名空间
// 引入命名空间：JeeSiteNET.Core.Storage
using JeeSiteNET.Core.Storage;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Infrastructure.Storage 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.Storage
using JeeSiteNET.Infrastructure.Storage;
    // 引入 JeeSiteNET.Infrastructure.FileStorage 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.FileStorage
using JeeSiteNET.Infrastructure.FileStorage;
    // 引入 JeeSiteNET.Infrastructure.Search 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.Search
using JeeSiteNET.Infrastructure.Search;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services.OAuth2 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services.OAuth2
using JeeSiteNET.Modules.Sys.Application.Services.OAuth2;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Sys.Infrastructure 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Infrastructure
using JeeSiteNET.Modules.Sys.Infrastructure;
    // 引入 JeeSiteNET.Modules.Sys.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Infrastructure.Repositories
using JeeSiteNET.Modules.Sys.Infrastructure.Repositories;
    // 引入 Microsoft.AspNetCore.Hosting 命名空间
// 引入命名空间：Microsoft.AspNetCore.Hosting
using Microsoft.AspNetCore.Hosting;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;
    // 引入 Microsoft.Extensions.Logging 命名空间
// 引入命名空间：Microsoft.Extensions.Logging
using Microsoft.Extensions.Logging;

// 定义 JeeSiteNET.Modules.Sys 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys
namespace JeeSiteNET.Modules.Sys;

[ModuleDescription(Code = "Sys", Name = "系统管理模块", Version = "1.0.0")]
// 定义class SysModuleInstaller
// 定义类：SysModuleInstaller
public class SysModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
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
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IEmpUserRepository, EmpUserRepository>();
        services.AddScoped<IRoleDataScopeRepository, RoleDataScopeRepository>();
        services.AddScoped<IUserDataScopeRepository, UserDataScopeRepository>();
        services.AddScoped<IRoleFieldScopeRepository, RoleFieldScopeRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<RoleMenuService>();
        services.AddScoped<RoleService>();
        services.AddScoped<MenuService>();
        services.AddScoped<OrganizationService>();
        services.AddScoped<AuthService>();
        services.AddScoped<CasAuthService>();
        services.AddScoped<OAuth2Service>();
        services.AddScoped<IOAuth2Provider, GitHubOAuth2Provider>();
        services.AddScoped<IOAuth2Provider, WeChatOAuth2Provider>();
        services.AddScoped<IOAuth2Provider, DingTalkOAuth2Provider>();
        services.AddScoped<ISmsSender, SmsService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<DictTypeService>();
        services.AddScoped<DictDataService>();
        services.AddScoped<PostService>();
        services.AddScoped<ConfigService>();
        services.AddScoped<ModuleService>();
        services.AddScoped<LogService>();
        services.AddScoped<RoleDataScopeService>();
        services.AddScoped<RoleFieldScopeService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<CompanyService>();
        services.AddScoped<AreaService>();
        services.AddScoped<IFileEntityRepository, FileEntityRepository>();
        services.AddScoped<IFileUploadRepository, FileUploadRepository>();
        services.AddScoped<FileService>();
        services.AddSingleton<IFileStorageProvider>(sp =>
        {
            // 声明并初始化变量：config
            var config = sp.GetRequiredService<IConfiguration>();
            // null 合并操作 ??（若为 null 则使用右侧值）
            var options = config.GetSection("Storage").Get<StorageOptions>() ?? new StorageOptions();

            // return 返回结果
            return options.Provider.ToLowerInvariant() switch
            {
                // null 合并操作 ??（若为 null 则使用右侧值）
                "s3" or "minio" => new S3StorageProvider(options.S3 ?? new S3Options(),
                    // 从 DI 容器获取服务
                    sp.GetRequiredService<ILogger<S3StorageProvider>>()),
                // null 合并操作 ??（若为 null 则使用右侧值）
                "aliyun" or "oss" => new AliyunOssStorageProvider(options.Aliyun ?? new AliyunOssOptions(),
                    // 从 DI 容器获取服务
                    sp.GetRequiredService<ILogger<AliyunOssStorageProvider>>()),
                _ => CreateLocalProvider(sp),
            };
        });
        services.AddScoped<IMsgInnerRepository, MsgInnerRepository>();
        services.AddScoped<IMsgPushRepository, MsgPushRepository>();
        services.AddScoped<IMsgPushedRepository, MsgPushedRepository>();
        services.AddScoped<IMsgTemplateRepository, MsgTemplateRepository>();
        services.AddScoped<MsgService>();
        services.AddScoped<IDataScopeRuleProvider, SysDataScopeRuleProvider>();
        services.AddScoped<IDataScopeService, DataScopeService>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<TenantService>();
        services.AddScoped<ILangRepository, LangRepository>();
        services.AddScoped<LangService>();
        services.AddScoped<IBizCategoryRepository, BizCategoryRepository>();
        services.AddScoped<BizCategoryService>();
        services.AddScoped<MonitorService>();
        services.AddSingleton<ExcelService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<AuditService>();
        services.AddScoped<PreviewService>();
        services.AddScoped<ChunkUploadService>();

        // 声明并初始化变量：esUrl
        var esUrl = configuration.GetValue<string>("Elasticsearch:Url");
        // if 条件判断
        if (!string.IsNullOrEmpty(esUrl))
        {
            // 中间件/服务：AddSingleton
            services.AddSingleton(new Elastic.Clients.Elasticsearch.ElasticsearchClient(
                new Uri(esUrl)));
            services.AddSingleton<ISearchService, ElasticsearchService>();
            services.AddScoped<SearchService>();
        }

        services.AddScoped<INotificationService>(_ => new NullNotificationService());
    }

    // 方法 CreateLocalProvider
    // 方法：CreateLocalProvider
    private static IFileStorageProvider CreateLocalProvider(IServiceProvider sp)
    {
        // 声明并初始化变量：env
        var env = sp.GetRequiredService<IWebHostEnvironment>();
        // return 返回结果
        return new LocalFileStorageProvider(
            Path.Combine(env.ContentRootPath, "uploads"),
            "/uploads");
    }
}
