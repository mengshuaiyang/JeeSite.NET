    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/dict-type")]
// 定义class DictTypeController
// 定义类：DictTypeController

public class DictTypeController : ControllerBase
{
    // 字段 _dictTypeService
    // 字段：_dictTypeService

    private readonly DictTypeService _dictTypeService;

    // 构造函数 DictTypeController
    // 构造函数：DictTypeController

    public DictTypeController(DictTypeService dictTypeService) => _dictTypeService = dictTypeService;

    [Permission("sys:dict:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<DictTypeDto>>> List([FromBody] PageRequest<DictType> request)
    {
        var result = await _dictTypeService.FindPageAsync(request);
        // return 返回结果
        return ApiResult<PageResult<DictTypeDto>>.Ok(result);
    }

    [Permission("sys:dict:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<DictTypeDto?>> Get([FromQuery] string dictTypeCode)
    {
        // 缓存：获取值
        var entity = await _dictTypeService.GetAsync(dictTypeCode);
        // if 条件判断
        if (entity == null) return ApiResult<DictTypeDto?>.NotFound("字典类型不存在");
        // return 返回结果
        return ApiResult<DictTypeDto?>.Ok(entity);
    }

    [Permission("sys:dict:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] DictTypeSaveDto dto)
    {
        // return 返回结果
        return await _dictTypeService.SaveAsync(dto);
    }

    [Permission("sys:dict:delete")]
    [HttpPost("delete")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteDictTypeRequest request)
    {
        // return 返回结果
        return await _dictTypeService.DeleteAsync(request.DictTypeCode);
    }
}

// 定义class DeleteDictTypeRequest
// 定义类：DeleteDictTypeRequest

public class DeleteDictTypeRequest
{
    // 属性 DictTypeCode
    // 属性：DictTypeCode

    public string DictTypeCode { get; set; } = string.Empty;
}
