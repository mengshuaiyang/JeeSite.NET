using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/excel")]
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

    private readonly ExcelService _excelService;

    public ExcelController(ExcelService excelService) => _excelService = excelService;

    [HttpPost("export")]
    public async Task<IActionResult> Export([FromQuery] string type, [FromBody] JsonElement data)
    {
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            return BadRequest(new { code = 400, message = $"未知类型: {type}" });

        var items = JsonSerializer.Deserialize(data.GetRawText(), typeof(List<>).MakeGenericType(dtoType));
        if (items == null) return NotFound();

        var exportMethod = typeof(ExcelService).GetMethod("Export")!.MakeGenericMethod(dtoType);
        var bytes = await Task.Run(() => (byte[])exportMethod.Invoke(_excelService, [items, "Sheet1"])!);

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{type}.xlsx");
    }

    [HttpGet("template")]
    public IActionResult Template([FromQuery] string type)
    {
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            return BadRequest(new { code = 400, message = $"未知类型: {type}" });

        var method = typeof(ExcelService).GetMethod("ExportTemplate")!.MakeGenericMethod(dtoType);
        var bytes = (byte[])method.Invoke(_excelService, ["Sheet1"])!;

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{type}_template.xlsx");
    }

    [HttpPost("import")]
    public async Task<ApiResult<List<object>>> Import([FromQuery] string type, IFormFile file)
    {
        if (!KnownTypes.TryGetValue(type, out var dtoType))
            return ApiResult<List<object>>.Fail(400, $"未知类型: {type}");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var importMethod = typeof(ExcelService).GetMethod("Import")!.MakeGenericMethod(dtoType);
        var result = await Task.Run(() => importMethod.Invoke(_excelService, [bytes, null]));

        return ApiResult<List<object>>.Ok(((IEnumerable<object>)result!).ToList());
    }
}
