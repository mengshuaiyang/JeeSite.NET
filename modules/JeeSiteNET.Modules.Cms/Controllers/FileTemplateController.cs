    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Cms.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Controllers
namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/file-template")]
// 定义class FileTemplateController
// 定义类：FileTemplateController

public class FileTemplateController : ControllerBase
{
    private static readonly string TplDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "modules", "JeeSiteNET.Modules.Cms", "FileTemplates");

    // 方法 FileTemplateController
    // 构造函数：FileTemplateController

    public FileTemplateController()
    {
        // if 条件判断
        if (!Directory.Exists(TplDir))
            Directory.CreateDirectory(TplDir);
    }

    [Permission("cms:file-template:list")]
    [HttpGet("list")]
    // 方法：List

    public ApiResult<List<object>> List()
    {
        // if 条件判断
        if (!Directory.Exists(TplDir))
            // return 返回结果
            return ApiResult<List<object>>.Ok(new List<object>());

        // 数据库操作：投影选择
        var files = Directory.GetFiles(TplDir, "*.html").Select(f => new
        {
            name = Path.GetFileNameWithoutExtension(f),
            fileName = Path.GetFileName(f),
            // 创建 FileInfo实例并赋给 size
            size = new FileInfo(f).Length,
            // 调用 ToString
            updateDate = System.IO.File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss")
        // 数据库操作：查询为列表
        } as object).ToList();

        // return 返回结果
        return ApiResult<List<object>>.Ok(files);
    }

    [Permission("cms:file-template:view")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public ApiResult<object> Get([FromQuery] string name)
    {
        // 声明并初始化变量：path
        var path = Path.Combine(TplDir, $"{name}.html");
        // if 条件判断
        if (!System.IO.File.Exists(path))
            // return 返回结果
            return ApiResult<object>.NotFound("模板不存在");

        // return 返回结果
        return ApiResult<object>.Ok(new
        {
            name,
            // 文件/流操作：读取文件全部文本
            content = System.IO.File.ReadAllText(path),
            fileName = $"{name}.html"
        });
    }

    [Permission("cms:file-template:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public ApiResult Save([FromBody] SaveFileTemplateRequest request)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(request.Name))
            // return 返回结果
            return ApiResult.Fail(400, "模板名称不能为空");

        // 声明并初始化变量：path
        var path = Path.Combine(TplDir, $"{request.Name}.html");
        // 文件/流操作：写入文本到文件
        System.IO.File.WriteAllText(path, request.Content ?? "");
        // return 返回结果
        return ApiResult.Ok();
    }

    [Permission("cms:file-template:delete")]
    [HttpPost("delete")]
    // 方法 Delete
    // 方法：Delete

    public ApiResult Delete([FromBody] DeleteFileTemplateRequest request)
    {
        // 声明并初始化变量：path
        var path = Path.Combine(TplDir, $"{request.Name}.html");
        // if 条件判断
        if (!System.IO.File.Exists(path))
            // return 返回结果
            return ApiResult.NotFound("模板不存在");

        // 数据库操作：删除
        System.IO.File.Delete(path);
        // return 返回结果
        return ApiResult.Ok();
    }
}

// 定义class SaveFileTemplateRequest
// 定义类：SaveFileTemplateRequest

public class SaveFileTemplateRequest
{
    // 属性 Name
    // 属性：Name

    public string Name { get; set; } = string.Empty;
    // 属性：Content

    public string? Content { get; set; }
}

// 定义class DeleteFileTemplateRequest
// 定义类：DeleteFileTemplateRequest

public class DeleteFileTemplateRequest
{
    // 属性 Name
    // 属性：Name

    public string Name { get; set; } = string.Empty;
}
