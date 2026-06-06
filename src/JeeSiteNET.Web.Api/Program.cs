using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// Register module assemblies for EF Core configuration discovery
builder.Services.AddSingleton<IEnumerable<Assembly>>(
    [typeof(User).Assembly]);

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

// Load modules
var moduleLoader = new ModuleLoader();
moduleLoader.LoadModules(builder.Services, builder.Configuration);

var app = builder.Build();

// Seed data
await JeeSiteNET.Modules.Sys.Infrastructure.SeedData.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
