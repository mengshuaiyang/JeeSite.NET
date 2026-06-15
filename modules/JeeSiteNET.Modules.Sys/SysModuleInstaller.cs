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

// ================================================================
// 系统管理模块安装器 —— 注册用户/角色/菜单/机构/字典/权限/文件 等服务
//
// 执行时机：Program.cs 中 ModuleLoader.LoadModules() 调用
// 注册内容：仓储（Repository）和应用服务（Service）
//
// 本模块是整个平台的核心，注册了以下功能领域：
//   ① 用户/角色/菜单（认证授权）
//   ② 机构/公司/岗位/员工（组织架构）
//   ③ 字典/配置/模块（系统基础）
//   ④ 审计/监控/日志（运维审计）
//   ⑤ OAuth2（GitHub/微信/钉钉三方登录）
//   ⑥ 文件存储（本地/S3/MinIO/阿里云 OSS）
//   ⑦ 搜索（Elasticsearch）
//   ⑧ 消息通知（SignalR）
//
// 调用链：
//   Program.cs → ModuleLoader.LoadModules → SysModuleInstaller.ConfigureServices
//   → AuthController/AuthService 登录 → UserController/UserService 用户管理
//   → RoleService 角色管理 → MenuService 菜单管理
// ================================================================

[ModuleDescription(Code = "Sys", Name = "系统管理模块", Version = "1.0.0")]
public class SysModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // ========== ① 仓储层注册 ==========
        // 每个实体对应一个仓储接口和实现，供服务层调用
        // 仓储封装了对 EF Core DbSet 的查询逻辑

        // 用户关联
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

        // ========== ② 应用服务层注册 ==========
        // 每个业务功能对应一个 Service 类，处理具体的业务逻辑
        // 服务层调用仓储层获取数据，调用工具类做校验/转换

        // --- 用户/角色/菜单/权限 ---
        services.AddScoped<UserService>();       // 用户 CRUD + 状态管理
        services.AddScoped<RoleMenuService>();   // 角色菜单分配
        services.AddScoped<RoleService>();       // 角色 CRUD
        services.AddScoped<MenuService>();       // 菜单 CRUD + 树操作
        services.AddScoped<OrganizationService>();// 机构 CRUD + 树操作
        services.AddScoped<AuthService>();        // 登录/注册/Token 签发/菜单路由（认证核心）

        // --- OAuth2 第三方登录 ---
        services.AddScoped<CasAuthService>();    // CAS 单点登录
        services.AddScoped<OAuth2Service>();     // GitHub/微信/钉钉 OAuth2 登录
        services.AddScoped<IOAuth2Provider, GitHubOAuth2Provider>();
        services.AddScoped<IOAuth2Provider, WeChatOAuth2Provider>();
        services.AddScoped<IOAuth2Provider, DingTalkOAuth2Provider>();

        // --- 消息通知 ---
        services.AddScoped<ISmsSender, SmsService>();    // 短信发送（阿里云/腾讯云等）
        services.AddScoped<IEmailSender, EmailService>(); // 邮件发送（SMTP）

        // --- 字典/配置/模块/岗位 ---
        services.AddScoped<DictTypeService>();   // 字典类型管理
        services.AddScoped<DictDataService>();   // 字典数据（树形字典）
        services.AddScoped<PostService>();        // 岗位管理
        services.AddScoped<ConfigService>();      // 系统参数配置
        services.AddScoped<ModuleService>();      // 功能模块管理
        services.AddScoped<LogService>();         // 操作日志查询

        // --- 数据权限/字段权限 ---
        services.AddScoped<RoleDataScopeService>();  // 角色数据权限（按机构/公司/自定义）
        services.AddScoped<RoleFieldScopeService>(); // 角色字段权限（敏感字段可见性控制）

        // --- 组织架构 ---
        services.AddScoped<EmployeeService>();   // 员工管理
        services.AddScoped<CompanyService>();    // 公司管理
        services.AddScoped<AreaService>();       // 区域管理（省/市/区县）

        // --- 文件存储 ---
        services.AddScoped<IFileEntityRepository, FileEntityRepository>();
        services.AddScoped<IFileUploadRepository, FileUploadRepository>();
        services.AddScoped<FileService>();        // 文件上传/下载/预览

        // 文件存储提供者：根据配置选择本地/S3/MinIO/阿里云 OSS
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
                _ => CreateLocalProvider(sp), // 默认本地文件存储
            };
        });

        // --- 消息（站内信/推送）---
        services.AddScoped<IMsgInnerRepository, MsgInnerRepository>();
        services.AddScoped<IMsgPushRepository, MsgPushRepository>();
        services.AddScoped<IMsgPushedRepository, MsgPushedRepository>();
        services.AddScoped<IMsgTemplateRepository, MsgTemplateRepository>();
        services.AddScoped<MsgService>();          // 消息发送/接收/模板

        // --- 数据权限引擎 ---
        services.AddScoped<IDataScopeRuleProvider, SysDataScopeRuleProvider>();
        services.AddScoped<IDataScopeService, DataScopeService>();

        // --- 多租户 ---
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<TenantService>();

        // --- 国际化 ---
        services.AddScoped<ILangRepository, LangRepository>();
        services.AddScoped<LangService>();

        // --- 业务分类 ---
        services.AddScoped<IBizCategoryRepository, BizCategoryRepository>();
        services.AddScoped<BizCategoryService>();

        // --- 监控/审计/仪表盘 ---
        services.AddScoped<MonitorService>();      // 系统监控（服务器/数据库/缓存状态）
        services.AddSingleton<ExcelService>();     // Excel 导入导出（可复用，故单例）
        services.AddScoped<DashboardService>();    // 仪表盘统计数据
        services.AddScoped<AuditService>();        // 审计日志查询
        services.AddScoped<PreviewService>();      // 文件预览（Playwright PDF 转换）
        services.AddScoped<ChunkUploadService>();  // 大文件分块上传

        // ========== ③ 可选的 Elasticsearch 搜索 ==========
        // 如果配置了 Elasticsearch:Url，则注册搜索服务
        var esUrl = configuration.GetValue<string>("Elasticsearch:Url");
        if (!string.IsNullOrEmpty(esUrl))
        {
            services.AddSingleton(new Elastic.Clients.Elasticsearch.ElasticsearchClient(
                new Uri(esUrl)));
            services.AddSingleton<ISearchService, ElasticsearchService>();
            services.AddScoped<SearchService>();    // 全文搜索（文章/文件索引）
        }

        // ========== ④ 占位通知服务 ==========
        // 初始注册空实现，后续在 Program.cs 中被 SignalR 实现覆盖（Override）
        services.AddScoped<INotificationService>(_ => new NullNotificationService());
    }

    /// <summary>创建本地文件存储提供者，文件保存在 ContentRootPath/uploads 目录</summary>
    private static IFileStorageProvider CreateLocalProvider(IServiceProvider sp)
    {
        var env = sp.GetRequiredService<IWebHostEnvironment>();
        return new LocalFileStorageProvider(
            Path.Combine(env.ContentRootPath, "uploads"),
            "/uploads");
    }
}
