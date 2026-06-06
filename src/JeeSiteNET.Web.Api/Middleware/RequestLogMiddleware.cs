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

        if (request.Path.StartsWithSegments("/api"))
        {
            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                var log = new Log
                {
                    LogId = IdGenerator.NewId(),
                    LogType = context.Response.StatusCode >= 400 ? "error" : "access",
                    LogTitle = $"{request.Method} {request.Path}",
                    RequestUri = request.Path,
                    RequestMethod = request.Method,
                    ExecuteTime = sw.ElapsedMilliseconds,
                    UserCode = currentUser.UserCode,
                    UserName = currentUser.UserName,
                    RemoteIp = context.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = request.Headers.UserAgent.ToString(),
                    CreateDate = DateTime.Now
                };
                try { db.Set<Log>().Add(log); await db.SaveChangesAsync(); }
                catch (Exception ex) { _logger.LogWarning(ex, "Failed to save request log"); }
            }
        }
        else
        {
            await _next(context);
        }
    }
}
