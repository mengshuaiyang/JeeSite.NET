using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/file-template")]
public class FileTemplateController : ControllerBase
{
    private static readonly string TplDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "modules", "JeeSiteNET.Modules.Cms", "FileTemplates");

    public FileTemplateController()
    {
        if (!Directory.Exists(TplDir))
            Directory.CreateDirectory(TplDir);
    }

    [Permission("cms:file-template:list")]
    [HttpGet("list")]
    public ApiResult<List<object>> List()
    {
        if (!Directory.Exists(TplDir))
            return ApiResult<List<object>>.Ok(new List<object>());

        var files = Directory.GetFiles(TplDir, "*.html").Select(f => new
        {
            name = Path.GetFileNameWithoutExtension(f),
            fileName = Path.GetFileName(f),
            size = new FileInfo(f).Length,
            updateDate = System.IO.File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss")
        } as object).ToList();

        return ApiResult<List<object>>.Ok(files);
    }

    [Permission("cms:file-template:view")]
    [HttpGet("get")]
    public ApiResult<object> Get([FromQuery] string name)
    {
        var path = Path.Combine(TplDir, $"{name}.html");
        if (!System.IO.File.Exists(path))
            return ApiResult<object>.NotFound("模板不存在");

        return ApiResult<object>.Ok(new
        {
            name,
            content = System.IO.File.ReadAllText(path),
            fileName = $"{name}.html"
        });
    }

    [Permission("cms:file-template:edit")]
    [HttpPost("save")]
    public ApiResult Save([FromBody] SaveFileTemplateRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
            return ApiResult.Fail(400, "模板名称不能为空");

        var path = Path.Combine(TplDir, $"{request.Name}.html");
        System.IO.File.WriteAllText(path, request.Content ?? "");
        return ApiResult.Ok();
    }

    [Permission("cms:file-template:delete")]
    [HttpPost("delete")]
    public ApiResult Delete([FromBody] DeleteFileTemplateRequest request)
    {
        var path = Path.Combine(TplDir, $"{request.Name}.html");
        if (!System.IO.File.Exists(path))
            return ApiResult.NotFound("模板不存在");

        System.IO.File.Delete(path);
        return ApiResult.Ok();
    }
}

public class SaveFileTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Content { get; set; }
}

public class DeleteFileTemplateRequest
{
    public string Name { get; set; } = string.Empty;
}
