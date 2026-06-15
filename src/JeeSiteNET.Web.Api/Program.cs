// ================================================================
// JeeSite.NET Web API —— 程序入口
//
// 这是一个 ASP.NET Core 10 Web 应用程序。启动流程如下：
//   1. 创建 WebApplicationBuilder（加载 appsettings.json、环境变量）
//   2. 注册框架级服务（JWT、EF Core、FusionCache、SignalR、CORS 等）
//   3. 加载业务模块（每个模块的 ModuleInstaller 注册自己的服务）
//   4. 构建应用管道（Build）
//   5. 初始化种子数据 + 定时任务
//   6. 配置中间件管线（认证 → 授权 → 日志 → 控制器）
//
// 关键的代码跳转链路（按启动顺序读）：
//   Program.cs
//   → ModuleLoader.cs          （如何发现并加载模块）
//   → IModuleInstaller.cs      （模块安装器接口定义）
//   → SysModuleInstaller.cs    （系统管理模块注册了哪些服务）
//   → AuthController.cs        （第一个被调用的 API 控制器）
//   → AuthService.cs           （登录、注册、Token 颁发的业务逻辑）
//   → JeeSiteDbContext.cs      （数据库上下文，所有实体从这里操作）
//   → EncryptUtil.cs / RsaUtil.cs  / Sm2Util.cs（加密工具链）
// ================================================================

using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Infrastructure;
using JeeSiteNET.Modules.Tasks.Application.Services;
using JeeSiteNET.Web.Api.Filters;
using JeeSiteNET.Web.Api.Hubs;
using JeeSiteNET.Web.Api.Services;
using JeeSiteNET.Web.Api.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using ZiggyCreatures.Caching.Fusion;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// ============ 第 1 步：创建应用构建器 ============
// WebApplication.CreateBuilder 做了三件事：
//   ① 加载配置（appsettings.json、环境变量、命令行参数）
//   ② 初始化默认的依赖注入（DI）容器
//   ③ 注册内置服务（日志、配置、环境等）
// 后续往 builder.Services 添加自己的服务。
var builder = WebApplication.CreateBuilder(args);

// ============ 第 2 步：注册框架级服务 ============

// --- 2a. MVC 控制器 + JSON 选项 ---
// 所有 API 控制器都注册到 DI，全局异常过滤器确保异常返回统一格式：
//   { code: 500, message: "错误描述", data: null }
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilter>(); // 全局异常处理
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 注册 Swagger（访问 /swagger 查看 API 文档）
builder.Services.AddJeeSiteSwagger();

// --- 2b. JWT 认证配置 ---
// 用户登录成功 → 服务端签发 JWT Token（含用户编码、角色等声明）
// 前端后续请求携带 Authorization: Bearer <token>
// JWT 中间件自动验证签名、有效期、是否被拉黑（TokenBlacklist 缓存键）
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["Secret"] ?? "JeeSiteNET_Default_SuperSecret_Key_2024!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"] ?? "JeeSiteNET",
            ValidAudience = jwtSection["Audience"] ?? "JeeSiteNET.Client",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
        // Token 验证通过后，检查该 Token 是否在缓存黑名单中
        // 用户退出 / 踢下线 / 修改密码时会将当前 Token 加入黑名单
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx =>
            {
                var cache = ctx.HttpContext.RequestServices.GetRequiredService<IFusionCache>();
                var tokenStr = ctx.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var blacklisted = await cache.GetOrDefaultAsync<string>($"TokenBlacklist:{tokenStr}");
                if (!string.IsNullOrEmpty(blacklisted))
                    ctx.Fail("Token has been revoked");
            }
        };
    });

// 全局授权策略：所有请求默认需要登录，除 [AllowAnonymous] 标记的方法外
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// --- 2c. CORS：允许 Vue 前端跨域访问 ---
// 开发环境前端运行在 http://localhost:5173（Vite 默认端口）
// 生产环境可能运行在 nginx 反向代理后，端口由 nginx 决定
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173", "http://localhost:3000")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

// --- 2d. 速率限制：防止接口被刷 ---
// 固定窗口策略，每分钟最多 100 次请求，超过则返回 HTTP 429
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddFixedWindowLimiter("Global", config =>
    {
        config.PermitLimit = 100;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 10;
    });
});

// --- 2e. 基础设施服务 ---
// AddHttpContextAccessor()：在非 Controller 类中通过 IHttpContextAccessor 获取当前请求上下文
// ICurrentUser：从 JWT 声明中解析当前用户信息（UserCode、LoginCode 等）
// AddHttpClient()：注册 HttpClient 工厂，供 AiChatService（调用 DeepSeek API）
//                  和 OAuth2Provider（调用 GitHub/微信 OAuth 接口）使用
// SignalR：实时推送通知（有新消息、任务完成等场景）
// ITenantContext：多租户上下文，不同租户的数据隔离
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// --- 2f. 注册模块程序集，供 EF Core 自动发现 IEntityTypeConfiguration ---
// 每个模块的实体都有自己的 EntityTypeConfiguration，EF Core 通过此列表
// 扫描所有模块的实体映射配置，无需在每个模块手动 AddDbContext
builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(JeeSiteNET.Modules.Sys.Domain.Entities.User).Assembly,
     typeof(JeeSiteNET.Modules.Tasks.Domain.Entities.SysJob).Assembly,
     typeof(JeeSiteNET.Modules.Cms.Domain.Entities.Site).Assembly,
     typeof(JeeSiteNET.Modules.Bpm.Domain.Entities.ApprovalRecord).Assembly,
     typeof(JeeSiteNET.Modules.CodeGen.Domain.Entities.GenTable).Assembly]);

// --- 2g. EF Core 数据库配置 ---
// 三个全局 EF Core 拦截器：
//   AuditInterceptor      → 自动记录谁在何时增/删/改了哪些数据
//   TreeEntityInterceptor  → 自动维护树形结构（ParentCodes、TreeSort、TreeLevel）
//   SoftDeleteInterceptor  → 删除时设置 IsDeleted=1，而非物理删除
builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddSingleton<TreeEntityInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddScoped<IDbConnectionStringResolver, DbConnectionStringResolver>();

// JeeSiteDbContext 是整个应用的主数据库上下文
// 通过 DatabaseProviderFactory 支持多种数据库：
//   SqlServer / Sqlite / PostgreSQL / 达梦 DM8 / 人大金仓 KingbaseES
// 数据库类型由配置文件中的 DatabaseProvider 字段决定
builder.Services.AddDbContext<JeeSiteDbContext>((sp, options) =>
{
    var dbProvider = DatabaseProviderFactory.Parse(
        builder.Configuration.GetValue<string>("DatabaseProvider"));
    var resolver = sp.GetRequiredService<IDbConnectionStringResolver>();
    var connStr = resolver.GetConnectionString(DbOperation.Write);

    if (dbProvider == DatabaseProviderType.Sqlite)
    {
        var dbPath = builder.Configuration.GetValue<string>("SqliteDbPath")
            ?? Path.Combine(AppContext.BaseDirectory, "JeeSiteNET.db");
        options.UseSqlite($"DataSource={dbPath}");
    }
    else
    {
        options.UseProvider(dbProvider, connStr,
            typeof(JeeSiteDbContext).Assembly.FullName);
    }

    options.AddInterceptors(
        sp.GetRequiredService<AuditInterceptor>(),
        sp.GetRequiredService<TreeEntityInterceptor>(),
        sp.GetRequiredService<SoftDeleteInterceptor>());
});
// 对外暴露 DbContext 基类，供 CodeGen 模块等需要通过 DbContext 注入的地方使用
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<JeeSiteDbContext>());

// --- 2h. FusionCache 二级缓存（L1 内存 + L2 Redis）---
// 读取数据时：先从内存查（极快）→ 没命中则查 Redis → 再没命中查数据库并回填缓存
// 写入/更新数据时：同时更新内存和 Redis，保证一致性
var redisConnection = builder.Configuration.GetSection("Redis")["Connection"] ?? "localhost:6379";
var redisInstance = builder.Configuration.GetSection("Redis")["InstanceName"] ?? "JeeSiteNET";
builder.Services.AddFusionCache()
    .WithDefaultEntryOptions(new FusionCacheEntryOptions
    {
        Duration = TimeSpan.FromMinutes(30),
        Priority = CacheItemPriority.Normal
    })
    .WithSerializer(new FusionCacheSystemTextJsonSerializer())
    .WithDistributedCache(new RedisCache(new RedisCacheOptions
    {
        Configuration = redisConnection,
        InstanceName = redisInstance
    }));

// ============ 第 3 步：加载业务模块 ============
// ModuleLoader 扫描所有实现了 IModuleInstaller 接口的类，
// 依次调用它们的 ConfigureServices 方法以注册仓储、服务等。
// 每个模块独立部署在自己的程序集中，互不依赖。
// 具体看：ModuleLoader.cs → IModuleInstaller.cs → SysModuleInstaller.cs
var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

// 用 SignalR 通知服务覆盖默认的空通知服务（INotificationService）
// SignalR 通过 WebSocket 实时推送消息给前端
builder.Services.AddScoped<JeeSiteNET.Core.INotificationService, NotificationService>();

// ============ 第 4 步：构建应用 ============
// Build() 之后不能再修改 DI 注册，但可以使用 app.Services 获取服务
var app = builder.Build();

// ============ 第 5 步：初始化数据 ============

// --- 5a. 种子数据 ---
// 首次启动时自动创建数据库表（EF Core EnsureCreated）
// 并插入基础数据：管理员用户（admin/admin123）、系统角色、系统菜单、
// 字典数据（数据状态、用户类型等）、系统配置等
await JeeSiteNET.Modules.Sys.Infrastructure.SeedData.InitializeAsync(app.Services);

// --- 5b. 定时任务 ---
// 从数据库 Sys_Job 表中加载已启用的任务，注册到 Quartz.NET 调度器
// 定时任务包括：清理过期日志、定时推送消息等
using (var scope = app.Services.CreateScope())
{
    var schedulerService = scope.ServiceProvider.GetRequiredService<SchedulerService>();
    await schedulerService.InitDefaultJobsAsync();
}

// ============ 第 6 步：中间件管道 ============
// 请求依次通过以下中间件（顺序很重要）：
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    // ① Swagger 文档页面（仅开发/Docker 环境）
    app.UseJeeSiteSwagger();
}

app.UseCors("AllowFrontend");                    // ② 跨域处理
app.UseRateLimiter();                             // ③ 速率限制
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.SecurityHeadersMiddleware>(); // ④ 安全头（XSS/CSRF 防护）
app.UseAuthentication();                          // ⑤ JWT 认证：解析 Token → 填充 HttpContext.User
app.UseAuthorization();                           // ⑥ 授权：检查 [Authorize] / [AllowAnonymous]
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.RequestLogMiddleware>();       // ⑦ 记录每次请求的 URL/耗时/用户
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.TenantResolutionMiddleware>(); // ⑧ 从请求头解析租户编码
app.MapControllers();                               // ⑨ 路由到具体的 Controller Action
app.MapHub<NotificationHub>("/hubs/notifications"); // ⑩ SignalR WebSocket 端点（实时推送）
app.Run();

// 此 partial class 用于集成测试（测试项目通过 Program 类型引用 Web API）
public partial class Program { }
