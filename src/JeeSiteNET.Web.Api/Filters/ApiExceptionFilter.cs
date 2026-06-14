using JeeSiteNET.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JeeSiteNET.Web.Api.Filters;

/// <summary>
/// 全局异常过滤器。
/// 捕获控制器层未处理的异常，写入日志并返回统一格式的错误响应，
/// 开发环境返回原始异常消息，生产环境返回通用服务器错误消息。
/// </summary>
public class ApiExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// 初始化 <see cref="ApiExceptionFilter"/> 的新实例。
    /// </summary>
    /// <param name="logger">日志记录器。</param>
    /// <param name="env">Web 宿主环境，用于判断开发环境判断。</param>
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// 同步处理异常上下文，记录日志并返回统一错误响应。
    /// </summary>
    /// <param name="context">异常上下文。</param>
    public void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    /// <summary>
    /// 异步处理异常上下文，记录日志并返回统一错误响应。
    /// </summary>
    /// <param name="context">异常上下文。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        HandleException(context);
        await Task.CompletedTask;
    }

    /// <summary>
    /// 统一异常处理逻辑：记录错误日志并封装统一响应。
    /// </summary>
    /// <param name="context">异常上下文。</param>
    private void HandleException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception: {Message}", context.Exception.Message);

        var message = _env.IsDevelopment()
            ? context.Exception.Message
            : "服务器内部错误，请稍后重试";

        context.Result = new JsonResult(ApiResult.Error(message))
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}
