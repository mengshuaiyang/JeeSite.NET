using System.Diagnostics;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;

namespace JeeSiteNET.Web.Api.Middleware;

public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogMiddleware> _logger;

    public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body.Length > 2000 ? body[..2000] + "..." : body;
    }

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
