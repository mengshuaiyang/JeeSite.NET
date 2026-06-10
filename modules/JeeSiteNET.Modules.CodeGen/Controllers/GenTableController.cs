using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
using JeeSiteNET.Modules.CodeGen.Application.Services;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.CodeGen.Controllers;

[ApiController]
[Route("api/v1/codegen/table")]
public class GenTableController : ControllerBase
{
    private readonly GenTableService _genTableService;
    private readonly CodeGenService _codeGenService;

    public GenTableController(GenTableService genTableService, CodeGenService codeGenService)
    {
        _genTableService = genTableService;
        _codeGenService = codeGenService;
    }

    [Permission("codegen:table:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<GenTableDto>>> List([FromBody] PageRequest<GenTable> request)
        => ApiResult<PageResult<GenTableDto>>.Ok(await _genTableService.FindPageAsync(request));

    [Permission("codegen:table:list")]
    [HttpGet("get")]
    public async Task<ApiResult<GenTableDto?>> Get([FromQuery] string tableName)
    {
        var dto = await _genTableService.GetAsync(tableName);
        return dto == null ? ApiResult<GenTableDto?>.NotFound("表配置不存在") : ApiResult<GenTableDto?>.Ok(dto);
    }

    [Permission("codegen:table:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] GenTableSaveDto dto) => await _genTableService.SaveAsync(dto);

    [Permission("codegen:table:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteGenTableRequest request) => await _genTableService.DeleteAsync(request.TableName);

    [Permission("codegen:table:import")]
    [HttpGet("db/tables")]
    public async Task<ApiResult<List<DbTableInfo>>> GetDbTables()
        => ApiResult<List<DbTableInfo>>.Ok(await _codeGenService.FindDbTablesAsync());

    [Permission("codegen:table:import")]
    [HttpGet("db/columns")]
    public async Task<ApiResult<List<ColumnInfo>>> GetDbColumns([FromQuery] string tableName)
        => ApiResult<List<ColumnInfo>>.Ok(await _codeGenService.FindDbColumnsAsync(tableName));

    [Permission("codegen:table:import")]
    [HttpPost("import")]
    public async Task<ApiResult> ImportTables([FromBody] ImportTableRequest request) => await _codeGenService.ImportTablesAsync(request);

    [Permission("codegen:table:list")]
    [HttpGet("preview")]
    public async Task<ApiResult<List<GenPreviewItem>>> Preview([FromQuery] string tableName)
        => ApiResult<List<GenPreviewItem>>.Ok(await _codeGenService.PreviewAsync(tableName));

    [Permission("codegen:table:edit")]
    [HttpPost("generate")]
    public async Task<ApiResult> Generate([FromBody] GenerateRequest request)
        => await _codeGenService.GenerateAsync(request.TableName, request.OutputDir);

    [Permission("codegen:table:edit")]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string tableName)
    {
        var bytes = await _codeGenService.DownloadAsync(tableName);
        if (bytes.Length == 0) return NotFound();
        return File(bytes, "application/zip", $"{tableName}.zip");
    }
}

public class DeleteGenTableRequest { public string TableName { get; set; } = string.Empty; }
public class GenerateRequest { public string TableName { get; set; } = string.Empty; public string? OutputDir { get; set; } }
