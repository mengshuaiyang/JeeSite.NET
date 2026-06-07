using JeeSiteNET.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JeeSiteNET.Web.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        HandleException(context);
        await Task.CompletedTask;
    }

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
