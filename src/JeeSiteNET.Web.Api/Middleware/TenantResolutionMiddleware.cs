using JeeSiteNET.Core;

namespace JeeSiteNET.Web.Api.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

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
