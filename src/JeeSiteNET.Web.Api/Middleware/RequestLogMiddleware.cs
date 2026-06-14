using System.Diagnostics;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;

namespace JeeSiteNET.Web.Api.Middleware;

/// <summary>
/// 请求日志中间件。
/// 记录每个 API 请求的方法、路径、耗时、用户信息、IP、设备及异常信息到日志表。
/// </summary>
public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogMiddleware> _logger;

    /// <summary>
    /// 初始化 <see cref="RequestLogMiddleware"/> 的新实例。
    /// </summary>
    /// <param name="next">管道中的下一个委托。</param>
    /// <param name="logger">日志记录器。</param>
    public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 执行中间件逻辑：计时、读取请求体、捕获响应、落库日志。
    /// </summary>
    /// <param name="context">当前 HTTP 上下文。</param>
    /// <param name="db">数据库上下文。</param>
    /// <param name="currentUser">当前登录用户信息。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task InvokeAsync(HttpContext context, JeeSiteDbContext db, ICurrentUser currentUser)
    {
        var sw = Stopwatch.StartNew();
        var request = context.Request;

        if (!request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        var requestBody = await ReadRequestBodyAsync(request);
        var originalBody = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        Exception? capturedException = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            capturedException = ex;
            throw;
        }
        finally
        {
            sw.Stop();
            context.Response.Body = originalBody;
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBody);

            var userAgent = request.Headers.UserAgent.ToString();
            var log = new Log
            {
                LogId = IdGenerator.NewId(),
                LogType = context.Response.StatusCode >= 400 ? "error" : "access",
                LogTitle = $"{request.Method} {request.Path}",
                RequestUri = request.Path,
                RequestMethod = request.Method,
                Params = request.QueryString.ToString(),
                ExecuteTime = sw.ElapsedMilliseconds,
                IsException = capturedException != null ? "1" : "0",
                ExceptionInfo = capturedException?.ToString(),
                UserCode = currentUser.UserCode,
                UserName = currentUser.UserName,
                CreateByName = currentUser.UserName,
                RemoteIp = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = userAgent,
                ServerAddr = System.Net.Dns.GetHostName(),
                DeviceName = ParseDeviceName(userAgent),
                BrowserName = ParseBrowserName(userAgent)
            };

            try { db.Set<Log>().Add(log); await db.SaveChangesAsync(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to save request log"); }
        }
    }

    /// <summary>
    /// 读取请求体文本（启用缓冲以便下游继续使用），超长截断。
    /// </summary>
    /// <param name="request">当前请求。</param>
    /// <returns>请求体文本。</returns>
    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body.Length > 2000 ? body[..2000] + "..." : body;
    }

    /// <summary>
    /// 根据 User-Agent 解析设备类型。
    /// </summary>
    /// <param name="userAgent">User-Agent 头部值。</param>
    /// <returns>设备名称。</returns>
    private static string ParseDeviceName(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        if (userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase)) return "Mobile";
        if (userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase)) return "Tablet";
        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase)) return "Android";
        if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase)) return "iPhone";
        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase)) return "Windows PC";
        if (userAgent.Contains("Macintosh", StringComparison.OrdinalIgnoreCase)) return "Mac";
        if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase)) return "Linux";
        return "Other";
    }

    /// <summary>
    /// 根据 User-Agent 解析浏览器名称。
    /// </summary>
    /// <param name="userAgent">User-Agent 头部值。</param>
    /// <returns>浏览器名称。</returns>
    private static string ParseBrowserName(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        if (userAgent.Contains("Edg", StringComparison.OrdinalIgnoreCase)) return "Edge";
        if (userAgent.Contains("Chrome", StringComparison.OrdinalIgnoreCase) && !userAgent.Contains("Edg", StringComparison.OrdinalIgnoreCase)) return "Chrome";
        if (userAgent.Contains("Firefox", StringComparison.OrdinalIgnoreCase)) return "Firefox";
        if (userAgent.Contains("Safari", StringComparison.OrdinalIgnoreCase) && !userAgent.Contains("Chrome", StringComparison.OrdinalIgnoreCase)) return "Safari";
        return "Other";
    }
}
