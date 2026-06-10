using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/preview")]
public class PreviewController : ControllerBase
{
    private readonly PreviewService _previewService;

    public PreviewController(PreviewService previewService) => _previewService = previewService;

    [HttpGet("{uploadId}")]
    public async Task<IActionResult> Preview(string uploadId)
    {
        var result = await _previewService.GetPreviewAsync(uploadId);
        if (result == null) return NotFound();
        return File(result.Stream, result.ContentType);
    }

    [HttpGet("{uploadId}/thumbnail")]
    public async Task<IActionResult> Thumbnail(string uploadId, [FromQuery] int width = 200, [FromQuery] int height = 200)
    {
        var result = await _previewService.GetThumbnailAsync(uploadId, width, height);
        if (result == null) return NotFound();
        return File(result.Stream, result.ContentType);
    }

    [HttpGet("{uploadId}/pdf")]
    public async Task<IActionResult> Pdf(string uploadId)
    {
        try
        {
            var result = await _previewService.ConvertToPdfAsync(uploadId);
            if (result == null) return NotFound();
            return File(result.Stream, result.ContentType);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(501, ApiResult.Fail(501, ex.Message));
        }
    }
}
