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
using JeeSiteNET.Web.Api.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

// JWT Authentication
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
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173", "http://localhost:3000")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Register module assemblies for EF Core configuration discovery
builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(JeeSiteNET.Modules.Sys.Domain.Entities.User).Assembly,
     typeof(JeeSiteNET.Modules.Tasks.Domain.Entities.SysJob).Assembly,
     typeof(JeeSiteNET.Modules.Cms.Domain.Entities.Site).Assembly,
     typeof(JeeSiteNET.Modules.Bpm.Domain.Entities.ApprovalRecord).Assembly,
     typeof(JeeSiteNET.Modules.CodeGen.Domain.Entities.GenTable).Assembly]);

// Database
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

// FusionCache with Redis L2
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

// Load modules
var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

var app = builder.Build();

// Seed data
await JeeSiteNET.Modules.Sys.Infrastructure.SeedData.InitializeAsync(app.Services);

// Init default scheduler jobs
using (var scope = app.Services.CreateScope())
{
    var schedulerService = scope.ServiceProvider.GetRequiredService<SchedulerService>();
    await schedulerService.InitDefaultJobsAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseJeeSiteSwagger();
}

app.UseCors("AllowFrontend");
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.SecurityHeadersMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.RequestLogMiddleware>();
app.UseMiddleware<JeeSiteNET.Web.Api.Middleware.TenantResolutionMiddleware>();
app.MapControllers();
app.Run();
