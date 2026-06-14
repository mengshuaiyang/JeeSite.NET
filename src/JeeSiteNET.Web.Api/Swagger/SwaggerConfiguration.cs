using JeeSiteNET.Core.Security;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace JeeSiteNET.Web.Api.Swagger;

/// <summary>
/// Swagger 操作过滤器。
/// 为标注了 <see cref="PermissionAttribute"/> 的接口在 Swagger 摘要中补充权限说明。
/// </summary>
public class ApiResponseOperationFilter : IOperationFilter
{
    /// <summary>
    /// 应用权限信息到 Swagger 操作描述。
    /// </summary>
    /// <param name="operation">OpenAPI 操作。</param>
    /// <param name="context">操作上下文（包含方法信息）。</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var attr in context.MethodInfo.GetCustomAttributes())
        {
            if (attr is PermissionAttribute perm)
            {
                var perms = string.Join(", ", perm.Permissions);
                operation.Summary ??= $"[{perms}]";
                operation.Description ??= $"需要权限: {perms}";
            }
        }
    }
}

/// <summary>
/// Swagger/OpenAPI 文档配置扩展方法集合。
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// 添加 JeeSite.NET Swagger 生成器：多模块分组、安全定义、XML 注释与权限过滤器。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <returns>链式调用返回原服务集合。</returns>
    public static IServiceCollection AddJeeSiteSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("sys", new OpenApiInfo { Title = "JeeSite.NET - 系统管理", Version = "v1", Description = "用户/角色/菜单/机构/字典/参数/日志/多租户管理" });
            options.SwaggerDoc("cms", new OpenApiInfo { Title = "JeeSite.NET - 内容管理", Version = "v1", Description = "站点/栏目/文章管理" });
            options.SwaggerDoc("bpm", new OpenApiInfo { Title = "JeeSite.NET - 工作流", Version = "v1", Description = "审批记录/流程表单/工作流引擎" });
            options.SwaggerDoc("codegen", new OpenApiInfo { Title = "JeeSite.NET - 代码生成", Version = "v1", Description = "表结构导入/模板生成/预览/下载" });
            options.SwaggerDoc("tasks", new OpenApiInfo { Title = "JeeSite.NET - 定时任务", Version = "v1", Description = "任务调度/启停/执行日志" });

            options.DocInclusionPredicate((docName, apiDesc) => docName switch
            {
                "sys" => apiDesc.RelativePath?.Contains("/sys/") == true,
                "cms" => apiDesc.RelativePath?.Contains("/cms/") == true,
                "bpm" => apiDesc.RelativePath?.Contains("/bpm/") == true,
                "codegen" => apiDesc.RelativePath?.Contains("/codegen/") == true,
                "tasks" => apiDesc.RelativePath?.Contains("/tasks/") == true,
                _ => false
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "输入 JWT Token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
                options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);

            options.OperationFilter<ApiResponseOperationFilter>();
        });
        return services;
    }

    /// <summary>
    /// 启用 JeeSite.NET Swagger UI，按模块暴露多文档分组，默认展开列表并显示请求耗时。
    /// </summary>
    /// <param name="app">Web 应用程序构建器。</param>
    /// <returns>链式调用返回原应用程序。</returns>
    public static WebApplication UseJeeSiteSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/sys/swagger.json", "系统管理");
            options.SwaggerEndpoint("/swagger/cms/swagger.json", "内容管理");
            options.SwaggerEndpoint("/swagger/bpm/swagger.json", "工作流");
            options.SwaggerEndpoint("/swagger/codegen/swagger.json", "代码生成");
            options.SwaggerEndpoint("/swagger/tasks/swagger.json", "定时任务");

            options.DocExpansion(DocExpansion.List);
            options.DefaultModelsExpandDepth(1);
            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
        });
        return app;
    }
}
