using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
using JeeSiteNET.Modules.CodeGen.Application.Services;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.CodeGen.Controllers;

/// <summary>代码生成配置接口控制器，负责表配置 CRUD、数据库表与列查询、预览、生成、下载代码包等。</summary>
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

    /// <summary>分页查询代码生成表配置列表。</summary>
    /// <param name="request">分页查询条件。</param>
    /// <returns>分页结果。</returns>
    [Permission("codegen:table:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<GenTableDto>>> List([FromBody] PageRequest<GenTable> request)
        => ApiResult<PageResult<GenTableDto>>.Ok(await _genTableService.FindPageAsync(request));

    /// <summary>根据表名获取单条表配置详情。</summary>
    /// <param name="tableName">表名。</param>
    /// <returns>表配置详情，不存在时返回 NotFound。</returns>
    [Permission("codegen:table:list")]
    [HttpGet("get")]
    public async Task<ApiResult<GenTableDto?>> Get([FromQuery] string tableName)
    {
        var dto = await _genTableService.GetAsync(tableName);
        return dto == null ? ApiResult<GenTableDto?>.NotFound("表配置不存在") : ApiResult<GenTableDto?>.Ok(dto);
    }

    /// <summary>新增或更新表配置信息。</summary>
    /// <param name="dto">表配置数据。</param>
    /// <returns>操作结果。</returns>
    [Permission("codegen:table:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] GenTableSaveDto dto) => await _genTableService.SaveAsync(dto);

    /// <summary>删除指定表配置。</summary>
    /// <param name="request">删除请求，包含表名。</param>
    /// <returns>操作结果。</returns>
    [Permission("codegen:table:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteGenTableRequest request) => await _genTableService.DeleteAsync(request.TableName);

    /// <summary>获取数据库中可用于代码生成的表列表。</summary>
    /// <returns>数据库表信息列表。</returns>
    [Permission("codegen:table:import")]
    [HttpGet("db/tables")]
    public async Task<ApiResult<List<DbTableInfo>>> GetDbTables()
        => ApiResult<List<DbTableInfo>>.Ok(await _codeGenService.FindDbTablesAsync());

    /// <summary>获取指定表的列信息。</summary>
    /// <param name="tableName">表名。</param>
    /// <returns>列信息列表。</returns>
    [Permission("codegen:table:import")]
    [HttpGet("db/columns")]
    public async Task<ApiResult<List<ColumnInfo>>> GetDbColumns([FromQuery] string tableName)
        => ApiResult<List<ColumnInfo>>.Ok(await _codeGenService.FindDbColumnsAsync(tableName));

    /// <summary>导入数据库表配置（从数据库读取表结构）。</summary>
    /// <param name="request">导入请求。</param>
    /// <returns>操作结果。</returns>
    [Permission("codegen:table:import")]
    [HttpPost("import")]
    public async Task<ApiResult> ImportTables([FromBody] ImportTableRequest request) => await _codeGenService.ImportTablesAsync(request);

    /// <summary>预览指定表的生成代码。</summary>
    /// <param name="tableName">表名。</param>
    /// <returns>预览项列表。</returns>
    [Permission("codegen:table:list")]
    [HttpGet("preview")]
    public async Task<ApiResult<List<GenPreviewItem>>> Preview([FromQuery] string tableName)
        => ApiResult<List<GenPreviewItem>>.Ok(await _codeGenService.PreviewAsync(tableName));

    /// <summary>根据表配置生成代码。</summary>
    /// <param name="request">生成请求。</param>
    /// <returns>操作结果。</returns>
    [Permission("codegen:table:edit")]
    [HttpPost("generate")]
    public async Task<ApiResult> Generate([FromBody] GenerateRequest request)
        => await _codeGenService.GenerateAsync(request.TableName, request.OutputDir);

    /// <summary>下载生成的代码 ZIP 包。</summary>
    /// <param name="tableName">表名。</param>
    /// <returns>ZIP 文件流。</returns>
    [Permission("codegen:table:edit")]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string tableName)
    {
        var bytes = await _codeGenService.DownloadAsync(tableName);
        if (bytes.Length == 0) return NotFound();
        return File(bytes, "application/zip", $"{tableName}.zip");
    }
}

/// <summary>删除代码生成配置请求 DTO。</summary>
public class DeleteGenTableRequest { public string TableName { get; set; } = string.Empty; }

/// <summary>生成代码请求 DTO。</summary>
public class GenerateRequest { public string TableName { get; set; } = string.Empty; public string? OutputDir { get; set; } }
