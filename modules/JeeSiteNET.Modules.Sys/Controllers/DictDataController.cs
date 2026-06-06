using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/dict-data")]
public class DictDataController : ControllerBase
{
    private readonly DictDataService _dictDataService;

    public DictDataController(DictDataService dictDataService) => _dictDataService = dictDataService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<DictDataDto>>> List([FromBody] PageRequest<DictData> request)
    {
        var result = await _dictDataService.FindPageAsync(request);
        return ApiResult<PageResult<DictDataDto>>.Ok(result);
    }

    [HttpGet("by-type")]
    public async Task<ApiResult<List<DictDataDto>>> GetByType([FromQuery] string dictType)
    {
        var list = await _dictDataService.GetByTypeAsync(dictType);
        return ApiResult<List<DictDataDto>>.Ok(list);
    }

    [HttpGet("get")]
    public async Task<ApiResult<DictDataDto?>> Get([FromQuery] string dictCode)
    {
        var entity = await _dictDataService.GetAsync(dictCode);
        if (entity == null) return ApiResult<DictDataDto?>.NotFound("字典数据不存在");
        return ApiResult<DictDataDto?>.Ok(entity);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] DictDataSaveDto dto)
    {
        return await _dictDataService.SaveAsync(dto);
    }

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteDictDataRequest request)
    {
        return await _dictDataService.DeleteAsync(request.DictCode);
    }
}

public class DeleteDictDataRequest
{
    public string DictCode { get; set; } = string.Empty;
}
