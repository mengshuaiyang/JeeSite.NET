using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/editor")]
public class EditorController : ControllerBase
{
    private readonly FileService _fileService;

    public EditorController(FileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Vditor file upload — returns Vditor-compatible JSON:
    /// { "msg": "", "code": 0, "data": { "errFiles": [], "succMap": { "filename": "url" } } }
    /// </summary>
    [HttpPost("upload")]
    [Permission("sys:editor:upload")]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string? editor)
    {
        if (file == null || file.Length == 0)
            return Ok(new { msg = "请选择文件", code = -1 });

        var result = await _fileService.UploadAsync(file, "editor", editor ?? "vditor");
        if (result.Code != 200)
            return Ok(new { msg = result.Message, code = -1 });

        var url = result.Data?.FileUrl ?? "";
        return editor?.ToLowerInvariant() switch
        {
            "ueditor" => Ok(new
            {
                state = "SUCCESS",
                url,
                title = result.Data?.FileName ?? "",
                original = result.Data?.FileName ?? "",
            }),
            _ => Ok(new
            {
                msg = "",
                code = 0,
                data = new
                {
                    errFiles = new string[] { },
                    succMap = new Dictionary<string, string>
                    {
                        [result.Data?.FileName ?? ""] = url,
                    },
                },
            }),
        };
    }

    [HttpPost("upload/image")]
    [Permission("sys:editor:upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Ok(new { msg = "请选择图片", code = -1 });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext is not ".jpg" and not ".jpeg" and not ".png" and not ".gif" and not ".webp" and not ".svg" and not ".bmp")
            return Ok(new { msg = "仅支持图片格式", code = -1 });

        var result = await _fileService.UploadAsync(file, "editor", "image");
        if (result.Code != 200)
            return Ok(new { msg = result.Message, code = -1 });

        return Ok(new
        {
            msg = "",
            code = 0,
            data = new
            {
                url = result.Data?.FileUrl ?? "",
                original = result.Data?.FileName ?? "",
            },
        });
    }
}
