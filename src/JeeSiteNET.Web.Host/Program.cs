using System.Reflection;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Infrastructure;
using JeeSiteNET.Web.Host.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<BlazorAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/login";
        options.LogoutPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(User).Assembly]);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=JeeSiteNET;Trusted_Connection=true;TrustServerCertificate=true";

builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddSingleton<TreeEntityInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();

builder.Services.AddDbContext<JeeSiteDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, sql =>
        sql.MigrationsAssembly(typeof(JeeSiteDbContext).Assembly.FullName));
    options.AddInterceptors(
        sp.GetRequiredService<AuditInterceptor>(),
        sp.GetRequiredService<TreeEntityInterceptor>(),
        sp.GetRequiredService<SoftDeleteInterceptor>());
});

var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

var app = builder.Build();

await SeedData.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<JeeSiteNET.Web.Host.App>()
    .AddInteractiveServerRenderMode();

app.Run();
