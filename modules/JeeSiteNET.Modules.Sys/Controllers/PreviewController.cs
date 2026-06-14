    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/preview")]
// 定义class PreviewController
// 定义类：PreviewController

public class PreviewController : ControllerBase
{
    // 字段 _previewService
    // 字段：_previewService

    private readonly PreviewService _previewService;

    // 构造函数 PreviewController
    // 构造函数：PreviewController

    public PreviewController(PreviewService previewService) => _previewService = previewService;

    [HttpGet("{uploadId}")]
    // 方法 Preview
    // 方法：Preview

    public async Task<IActionResult> Preview(string uploadId)
    {
        var result = await _previewService.GetPreviewAsync(uploadId);
        // if 条件判断
        if (result == null) return NotFound();
        // return 返回结果
        return File(result.Stream, result.ContentType);
    }

    [HttpGet("{uploadId}/thumbnail")]
    // 方法 Thumbnail
    // 方法：Thumbnail

    public async Task<IActionResult> Thumbnail(string uploadId, [FromQuery] int width = 200, [FromQuery] int height = 200)
    {
        var result = await _previewService.GetThumbnailAsync(uploadId, width, height);
        // if 条件判断
        if (result == null) return NotFound();
        // return 返回结果
        return File(result.Stream, result.ContentType);
    }

    [HttpGet("{uploadId}/pdf")]
    // 方法 Pdf
    // 方法：Pdf

    public async Task<IActionResult> Pdf(string uploadId)
    {
        // try 异常捕获开始
        try
        {
            var result = await _previewService.ConvertToPdfAsync(uploadId);
            // if 条件判断
            if (result == null) return NotFound();
            // return 返回结果
            return File(result.Stream, result.ContentType);
        }
        // catch 捕获异常
        catch (InvalidOperationException ex)
        {
            // return 返回结果
            return StatusCode(501, ApiResult.Fail(501, ex.Message));
        }
    }
}
