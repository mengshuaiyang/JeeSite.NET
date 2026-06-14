    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/excel")]
// 定义class ExcelController
// 定义类：ExcelController

public class ExcelController : ControllerBase
{
    private static readonly Dictionary<string, Type> KnownTypes = new()
    {
        ["UserDto"] = typeof(Application.DTOs.UserDto),
        ["UserSaveDto"] = typeof(Application.DTOs.UserSaveDto),
        ["RoleDto"] = typeof(Application.DTOs.RoleDto),
        ["RoleSaveDto"] = typeof(Application.DTOs.RoleSaveDto),
        ["PostDto"] = typeof(Application.DTOs.PostDto),
        ["DictDataDto"] = typeof(Application.DTOs.DictDataDto),
        ["DictTypeDto"] = typeof(Application.DTOs.DictTypeDto),
        ["ConfigDto"] = typeof(Application.DTOs.ConfigDto),
        ["EmployeeDto"] = typeof(Application.DTOs.EmployeeDto),
        ["LogDto"] = typeof(Application.DTOs.LogDto),
        ["ModuleDto"] = typeof(Application.DTOs.ModuleDto),
        ["BizCategoryDto"] = typeof(Application.DTOs.BizCategoryDto),
        ["CompanyDto"] = typeof(Application.DTOs.CompanyDto),
        ["OfficeDto"] = typeof(Application.DTOs.OrganizationDto),
        ["AreaDto"] = typeof(Application.DTOs.AreaDto),
    };

    // 字段 _excelService
    // 字段：_excelService

    private readonly ExcelService _excelService;

    // 构造函数 ExcelController
    // 构造函数：ExcelController

    public ExcelController(ExcelService excelService) => _excelService = excelService;

    [HttpPost("export")]
    // 方法 Export
    // 方法：Export

    public async Task<IActionResult> Export([FromQuery] string type, [FromBody] JsonElement data)
    {
        // if 条件判断
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            // return 返回结果
            return BadRequest(new { code = 400, message = $"未知类型: {type}" });

        // 声明并初始化变量：items
        var items = JsonSerializer.Deserialize(data.GetRawText(), typeof(List<>).MakeGenericType(dtoType));
        // if 条件判断
        if (items == null) return NotFound();

        // 声明并初始化变量：exportMethod
        var exportMethod = typeof(ExcelService).GetMethod("Export")!.MakeGenericMethod(dtoType);
        // 调用 Run
        var bytes = await Task.Run(() => (byte[])exportMethod.Invoke(_excelService, [items, "Sheet1"])!);

        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{type}.xlsx");
    }

    [HttpGet("template")]
    // 方法 Template
    // 方法：Template

    public IActionResult Template([FromQuery] string type)
    {
        // if 条件判断
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            // return 返回结果
            return BadRequest(new { code = 400, message = $"未知类型: {type}" });

        // 声明并初始化变量：method
        var method = typeof(ExcelService).GetMethod("ExportTemplate")!.MakeGenericMethod(dtoType);
        // 声明并初始化变量：bytes
        var bytes = (byte[])method.Invoke(_excelService, ["Sheet1"])!;

        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{type}_template.xlsx");
    }

    [HttpPost("import")]
    // 方法 Import
    // 方法：Import

    public async Task<ApiResult<List<object>>> Import([FromQuery] string type, IFormFile file)
    {
        // if 条件判断
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            // return 返回结果
            return ApiResult<List<object>>.Fail(400, $"未知类型: {type}");

    // 引入 var ms 命名空间
        using var ms = new MemoryStream();
        // await 异步等待
        await file.CopyToAsync(ms);
        // 调用 ToArray
        var bytes = ms.ToArray();

        // 声明并初始化变量：importMethod
        var importMethod = typeof(ExcelService).GetMethod("Import")!.MakeGenericMethod(dtoType);
        // 调用 Run
        var result = await Task.Run(() => importMethod.Invoke(_excelService, [bytes, null]));

        // return 返回结果
        return ApiResult<List<object>>.Ok(((IEnumerable<object>)result!).ToList());
    }
}
