namespace JeeSiteNET.Web.Api.Middleware;

/// <summary>
/// 添加安全响应头。
/// 注意：严格按顺序处理 X-Frame-Options / CSP / X-XSS-Protection / nosniff 等头。
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 在开始写响应前注册回调，避免 Headers 已经发送
        context.Response.OnStarting(state =>
        {
            var ctx = (HttpContext)state!;
            var headers = ctx.Response.Headers;

            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "SAMEORIGIN";
            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), interest-cohort=()";

            // 内容安全策略：默认只信任自身；图片/媒体允许自身 + https 协议；
            // style 'unsafe-inline' 是 Vue 3 开发环境必须；生产环境建议收紧。
            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' localhost:5173 localhost:4173; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https: http:; " +
                "media-src 'self' https: http:; " +
                "font-src 'self' data:; " +
                "connect-src 'self' ws://localhost:5173 wss://localhost:5173 ws://localhost:4173 wss://localhost:4173 https: http:; " +
                "frame-ancestors 'self'; " +
                "object-src 'none'; " +
                "base-uri 'self'; " +
                "form-action 'self'; " +
                "worker-src 'self' blob:; ";

            if (ctx.Request.IsHttps)
            {
                headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
            }

            return Task.CompletedTask;
        }, context);

        await _next(context);
    }
}
