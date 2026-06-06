using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/dict-type")]
public class DictTypeController : ControllerBase
{
    private readonly DictTypeService _dictTypeService;

    public DictTypeController(DictTypeService dictTypeService) => _dictTypeService = dictTypeService;

    [Permission("sys:dict:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<DictTypeDto>>> List([FromBody] PageRequest<DictType> request)
    {
        var result = await _dictTypeService.FindPageAsync(request);
        return ApiResult<PageResult<DictTypeDto>>.Ok(result);
    }

    [Permission("sys:dict:list")]
    [HttpGet("get")]
    public async Task<ApiResult<DictTypeDto?>> Get([FromQuery] string dictTypeCode)
    {
        var entity = await _dictTypeService.GetAsync(dictTypeCode);
        if (entity == null) return ApiResult<DictTypeDto?>.NotFound("字典类型不存在");
        return ApiResult<DictTypeDto?>.Ok(entity);
    }

    [Permission("sys:dict:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] DictTypeSaveDto dto)
    {
        return await _dictTypeService.SaveAsync(dto);
    }

    [Permission("sys:dict:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteDictTypeRequest request)
    {
        return await _dictTypeService.DeleteAsync(request.DictTypeCode);
    }
}

public class DeleteDictTypeRequest
{
    public string DictTypeCode { get; set; } = string.Empty;
}
