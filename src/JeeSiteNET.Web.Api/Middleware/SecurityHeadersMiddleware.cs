namespace JeeSiteNET.Web.Api.Middleware;

/// <summary>
/// 安全响应头中间件。
/// 统一输出 CSP / X-Frame-Options / X-XSS-Protection / HSTS 等常见安全头。
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 初始化 <see cref="SecurityHeadersMiddleware"/> 的新实例。
    /// </summary>
    /// <param name="next">管道中的下一个委托。</param>
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 在响应开始写入前附加安全响应头，再继续执行管道。
    /// </summary>
    /// <param name="context">当前 HTTP 上下文。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(state =>
        {
            var ctx = (HttpContext)state!;
            var headers = ctx.Response.Headers;

            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "SAMEORIGIN";
            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), interest-cohort=()";

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
