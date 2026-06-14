    // 引入 System.Reflection 命名空间
// 引入命名空间：System.Reflection
using System.Reflection;
    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors
using JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Infrastructure 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Infrastructure
using JeeSiteNET.Modules.Sys.Infrastructure;
    // 引入 JeeSiteNET.Web.Host.Services 命名空间
// 引入命名空间：JeeSiteNET.Web.Host.Services
using JeeSiteNET.Web.Host.Services;
    // 引入 Microsoft.AspNetCore.Authentication.Cookies 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authentication.Cookies
using Microsoft.AspNetCore.Authentication.Cookies;
    // 引入 Microsoft.AspNetCore.Components.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Components.Authorization
using Microsoft.AspNetCore.Components.Authorization;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 声明并初始化变量：builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 处理当前 HTTP 上下文
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<BlazorAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

// 中间件/服务：AddAuthentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // 设置配置选项
        options.Cookie.HttpOnly = true;
        // 设置配置选项
        options.Cookie.SameSite = SameSiteMode.Strict;
        // 设置配置选项
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // 设置配置选项
        options.LoginPath = "/login";
        // 设置配置选项
        options.LogoutPath = "/login";
        // 设置配置选项
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        // 设置配置选项
        options.SlidingExpiration = true;
    });
// 中间件/服务：AddAuthorization
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(User).Assembly]);

// 声明并初始化变量：connectionString
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    // null 合并操作 ??（若为 null 则使用右侧值）
    ?? "Server=localhost;Database=JeeSiteNET;Trusted_Connection=true;TrustServerCertificate=true";

builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddSingleton<TreeEntityInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();

builder.Services.AddDbContext<JeeSiteDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, sql =>
        sql.MigrationsAssembly(typeof(JeeSiteDbContext).Assembly.FullName));
    options.AddInterceptors(
        // 从 DI 容器获取服务
        sp.GetRequiredService<AuditInterceptor>(),
        // 从 DI 容器获取服务
        sp.GetRequiredService<TreeEntityInterceptor>(),
        // 从 DI 容器获取服务
        sp.GetRequiredService<SoftDeleteInterceptor>());
});

// 创建 ModuleLoader实例并赋给 moduleLoader
var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

// 调用 Build
var app = builder.Build();

// await 异步等待
await SeedData.InitializeAsync(app.Services);

// if 条件判断
if (app.Environment.IsDevelopment())
{
    // 中间件/服务：UseDeveloperExceptionPage
    app.UseDeveloperExceptionPage();
}

// 中间件/服务：UseStaticFiles
app.UseStaticFiles();
// 中间件/服务：UseAuthentication
app.UseAuthentication();
// 中间件/服务：UseAuthorization
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<JeeSiteNET.Web.Host.App>()
    .AddInteractiveServerRenderMode();

app.Run();
