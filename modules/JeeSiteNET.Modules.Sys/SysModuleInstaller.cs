using JeeSiteNET.Core;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Core.Search;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Storage;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Infrastructure.Storage;
using JeeSiteNET.Infrastructure.FileStorage;
using JeeSiteNET.Infrastructure.Search;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Application.Services.OAuth2;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Modules.Sys.Infrastructure;
using JeeSiteNET.Modules.Sys.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var config = sp.GetRequiredService<IConfiguration>();
            var options = config.GetSection("Storage").Get<StorageOptions>() ?? new StorageOptions();

            return options.Provider.ToLowerInvariant() switch
            {
                "s3" or "minio" => new S3StorageProvider(options.S3 ?? new S3Options(),
                    sp.GetRequiredService<ILogger<S3StorageProvider>>()),
                "aliyun" or "oss" => new AliyunOssStorageProvider(options.Aliyun ?? new AliyunOssOptions(),
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

        var esUrl = configuration.GetValue<string>("Elasticsearch:Url");
        if (!string.IsNullOrEmpty(esUrl))
        {
            services.AddSingleton(new Elastic.Clients.Elasticsearch.ElasticsearchClient(
                new Uri(esUrl)));
            services.AddSingleton<ISearchService, ElasticsearchService>();
            services.AddScoped<SearchService>();
        }

        services.AddScoped<INotificationService>(_ => new NullNotificationService());
    }

    private static IFileStorageProvider CreateLocalProvider(IServiceProvider sp)
    {
        var env = sp.GetRequiredService<IWebHostEnvironment>();
        return new LocalFileStorageProvider(
            Path.Combine(env.ContentRootPath, "uploads"),
            "/uploads");
    }
}
