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

// 应用程序启动入口：配置服务、中间件管道、SignalR 路由、Swagger UI 并运行 Web 主机。
var builder = WebApplication.CreateBuilder(args);

// —— 控制器与 JSON 序列化：注册全局异常过滤器，使用小驼峰命名，忽略 null 值，保留枚举字符串
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddJeeSiteSwagger();

// —— JWT 认证：从配置读取 Secret，未配置时使用内置默认值（仅用于开发环境）
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
        // —— 令牌吊销检查：登录后每次请求验证 FusionCache 中是否有黑名单
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
// —— 授权：默认要求已认证用户
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// —— CORS：允许前端本地开发端口访问（Vite/CRA/Webpack 开发服务器）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173", "http://localhost:3000")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

// —— 速率限制：固定窗口策略，避免接口被恶意滥用
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

// —— 基础身份与上下文服务：当前用户、SignalR、任务调度、租户信息
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// —— 注册模块 Assembly 供 EF Core 自动发现实体配置
builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(JeeSiteNET.Modules.Sys.Domain.Entities.User).Assembly,
     typeof(JeeSiteNET.Modules.Tasks.Domain.Entities.SysJob).Assembly,
     typeof(JeeSiteNET.Modules.Cms.Domain.Entities.Site).Assembly,
     typeof(JeeSiteNET.Modules.Bpm.Domain.Entities.ApprovalRecord).Assembly,
     typeof(JeeSiteNET.Modules.CodeGen.Domain.Entities.GenTable).Assembly]);

// —— EF Core：审计/树形/软删除拦截器 + 数据库提供工厂 + 多写多读
builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddSingleton<TreeEntityInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddScoped<IDbConnectionStringResolver, DbConnectionStringResolver>();

builder.Services.AddDbContext<JeeSiteDbContext>((sp, options) =>
{
    // —— 从配置解析数据库类型（SqlServer/Sqlite/PostgreSQL/达梦/人大金仓）
    var dbProvider = DatabaseProviderFactory.Parse(
        builder.Configuration.GetValue<string>("DatabaseProvider"));
    var resolver = sp.GetRequiredService<IDbConnectionStringResolver>();
    var connStr = resolver.GetConnectionString(DbOperation.Write);

    if (dbProvider == DatabaseProviderType.Sqlite)
    {
        // —— Sqlite 使用 BaseDirectory 下默认文件路径，便于开发环境快速启动
        var dbPath = builder.Configuration.GetValue<string>("SqliteDbPath")
            ?? Path.Combine(AppContext.BaseDirectory, "JeeSiteNET.db");
        options.UseSqlite($"DataSource={dbPath}");
    }
    else
    {
        // —— 其他数据库使用 provider 工厂按程序集加载（支持 SqlServer/PG/Dm/KingbaseES）
        options.UseProvider(dbProvider, connStr,
            typeof(JeeSiteDbContext).Assembly.FullName);
    }

    // —— 全局拦截器注入：审计、树节点、软删除
    options.AddInterceptors(
        sp.GetRequiredService<AuditInterceptor>(),
        sp.GetRequiredService<TreeEntityInterceptor>(),
        sp.GetRequiredService<SoftDeleteInterceptor>());
});
// —— 将 DbContext 基类暴露，便于泛型仓储解析
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<JeeSiteDbContext>());

// —— FusionCache 二级缓存：Memory 一级 + Redis 二级
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

// —— 模块加载器：按约定扫描模块 Assembly，注册控制器/服务/仓储等
var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

// —— 用 SignalR 实现替换核心通知服务接口的默认实现
builder.Services.AddScoped<JeeSiteNET.Core.INotificationService, NotificationService>();

// —— 构建 Web 应用
var app = builder.Build();

// —— 数据初始化：确保数据库已创建，再初始化系统角色/菜单/用户等种子数据
await JeeSiteNET.Modules.Sys.Infrastructure.SeedData.InitializeAsync(app.Services);

// —— 定时任务初始化：注册/启动默认调度任务
using (var scope = app.Services.CreateScope())
{
    var schedulerService = scope.ServiceProvider.GetRequiredService<SchedulerService>();
    await schedulerService.InitDefaultJobsAsync();
}

// —— 开发/生产通用：Swagger 仅在开发或 Docker 环境启用
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseJeeSiteSwagger();
}

// —— 中间件管道顺序：Cors → RateLimiting → SecurityHeaders → Authentication → Authorization → RequestLog → TenantResolution → Controllers → SignalR
app.UseCors("AllowFrontend");
app.UseRateLimiter();
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.SecurityHeadersMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.RequestLogMiddleware>();
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.TenantResolutionMiddleware>();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.Run();

/// <summary>
/// partial Program 类，用于支持集成测试时对 Program 类型的访问。
/// </summary>
public partial class Program { }
