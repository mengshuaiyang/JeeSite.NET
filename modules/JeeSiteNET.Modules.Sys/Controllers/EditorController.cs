    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.UEditor 命名空间
// 引入命名空间：JeeSiteNET.Core.UEditor
using JeeSiteNET.Core.UEditor;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Hosting 命名空间
// 引入命名空间：Microsoft.AspNetCore.Hosting
using Microsoft.AspNetCore.Hosting;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/editor")]
// 定义class EditorController
// 定义类：EditorController

public class EditorController : ControllerBase
{
    // 字段 _fileService
    // 字段：_fileService

    private readonly FileService _fileService;
    // 字段 _ueditor
    // 字段：_ueditor

    private readonly UEditorActionHandler _ueditor;

    // 方法 EditorController
    // 构造函数：EditorController

    public EditorController(FileService fileService, IWebHostEnvironment env)
    {
        // null 合并操作 ??（若为 null 则使用右侧值）
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        // 声明并初始化变量：webRoot
        var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        // 创建 UEditorActionHandler实例并赋给 _ueditor
        _ueditor = new UEditorActionHandler(new UEditorOptions(), new LocalFileUploadStore(webRoot, "/"));
    }

    /// <summary>原上传接口，兼容 Vditor 与 UEditor 两种响应格式</summary>
    [HttpPost("upload")]
    [Permission("sys:editor:upload")]
    // 方法 Upload
    // 方法：Upload

    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string? editor)
    {
        // if 条件判断
        if (file == null || file.Length == 0)
            // return 返回结果
            return Ok(new { msg = "请选择文件", code = -1 });

        // null 合并操作 ??（若为 null 则使用右侧值）
        var result = await _fileService.UploadAsync(file, "editor", editor ?? "vditor");
        // if 条件判断
        if (result.Code != 200)
            // return 返回结果
            return Ok(new { msg = result.Message, code = -1 });

        // 声明并初始化变量：url
        var url = result.Data?.FileUrl ?? "";
        // return 返回结果
        return editor?.ToLowerInvariant() switch
        {
            "ueditor" => Ok(new
            {
                state = "SUCCESS",
                url,
                // null 合并操作 ??（若为 null 则使用右侧值）
                title = result.Data?.FileName ?? "",
                // null 合并操作 ??（若为 null 则使用右侧值）
                original = result.Data?.FileName ?? "",
            }),
            _ => Ok(new
            {
                msg = "",
                code = 0,
                data = new
                {
                    // 创建 string实例并赋给 errFiles
                    errFiles = new string[] { },
                    // 创建 Dictionary实例并赋给 succMap
                    succMap = new Dictionary<string, string>
                    {
                        // null 合并操作 ??（若为 null 则使用右侧值）

                        [result.Data?.FileName ?? ""] = url,
                    },
                },
            }),
        };
    }

    [HttpPost("upload/image")]
    [Permission("sys:editor:upload")]
    // 方法 UploadImage
    // 方法：UploadImage

    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        // if 条件判断
        if (file == null || file.Length == 0)
            // return 返回结果
            return Ok(new { msg = "请选择图片", code = -1 });

        // 声明并初始化变量：ext
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        // if 条件判断
        if (ext is not ".jpg" and not ".jpeg" and not ".png" and not ".gif" and not ".webp" and not ".svg" and not ".bmp")
            // return 返回结果
            return Ok(new { msg = "仅支持图片格式", code = -1 });

        var result = await _fileService.UploadAsync(file, "editor", "image");
        // if 条件判断
        if (result.Code != 200)
            // return 返回结果
            return Ok(new { msg = result.Message, code = -1 });

        // return 返回结果
        return Ok(new
        {
            msg = "",
            code = 0,
            data = new
            {
                // null 合并操作 ??（若为 null 则使用右侧值）
                url = result.Data?.FileUrl ?? "",
                // null 合并操作 ??（若为 null 则使用右侧值）
                original = result.Data?.FileName ?? "",
            },
        });
    }

    /// <summary>
    /// UEditor 标准 action 端点。
    /// 
    /// UEditor 前端通过统一 URL: /api/v1/sys/editor?action=xxx 与后端通信
    /// 支持 action：
    ///   config / uploadimage / uploadscrawl / uploadvideo / uploadfile /
    ///   catchimage / listimage / listfile
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("")]
    [Route("ueditor")]
    [Permission("sys:editor:upload")]
    // 方法 UEditor
    // 方法：UEditor

    public async Task<IActionResult> UEditor([FromQuery] string? action)
    {
        // 处理当前 HTTP 上下文
        var json = await _ueditor.HandleAsync(HttpContext);
        // return 返回结果
        return Content(json, "application/json; charset=utf-8");
    }
}
