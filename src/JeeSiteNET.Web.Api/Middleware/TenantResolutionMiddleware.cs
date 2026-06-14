using JeeSiteNET.Core;

namespace JeeSiteNET.Web.Api.Middleware;

/// <summary>
/// 多租户解析中间件。
/// 按请求头 X-Tenant-Code / 域名子段 / 用户 Claim 顺序解析租户编码，
/// 并写入 <see cref="ITenantContext"/>。
/// </summary>
public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 初始化 <see cref="TenantResolutionMiddleware"/> 的新实例。
    /// </summary>
    /// <param name="next">管道中的下一个委托。</param>
    public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

    /// <summary>
    /// 解析租户编码并设置租户上下文，然后继续执行管道。
    /// </summary>
    /// <param name="context">当前 HTTP 上下文。</param>
    /// <param name="tenantContext">租户上下文服务。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        var code = context.Request.Headers["X-Tenant-Code"].FirstOrDefault();

        if (string.IsNullOrEmpty(code))
        {
            var host = context.Request.Host.Host;
            var dotIndex = host.IndexOf('.');
            if (dotIndex > 0 && host.Length > 3)
                code = host[..dotIndex];
        }

        if (string.IsNullOrEmpty(code))
            code = context.User?.FindFirst("TenantCode")?.Value;

        if (code is not null && tenantContext is TenantContext tc)
        {
            tc.TenantCode = code;
            tc.TenantName = code;
        }

        await _next(context);
    }
}
