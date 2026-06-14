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
[Route("api/v1/sys/dict-data")]
// 定义class DictDataController
// 定义类：DictDataController

public class DictDataController : ControllerBase
{
    // 字段 _dictDataService
    // 字段：_dictDataService

    private readonly DictDataService _dictDataService;

    // 构造函数 DictDataController
    // 构造函数：DictDataController

    public DictDataController(DictDataService dictDataService) => _dictDataService = dictDataService;

    [Permission("sys:dict:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<DictDataDto>>> List([FromBody] PageRequest<DictData> request)
    {
        var result = await _dictDataService.FindPageAsync(request);
        // return 返回结果
        return ApiResult<PageResult<DictDataDto>>.Ok(result);
    }

    [Permission("sys:dict:list")]
    [HttpGet("by-type")]
    // 方法 GetByType
    // 方法：GetByType

    public async Task<ApiResult<List<DictDataDto>>> GetByType([FromQuery] string dictType)
    {
        var list = await _dictDataService.GetByTypeAsync(dictType);
        // return 返回结果
        return ApiResult<List<DictDataDto>>.Ok(list);
    }

    [Permission("sys:dict:list")]
    [HttpGet("tree")]
    // 方法 Tree
    // 方法：Tree

    public async Task<ApiResult<List<DictDataDto>>> Tree([FromQuery] string dictType)
    {
        var tree = await _dictDataService.TreeAsync(dictType);
        // return 返回结果
        return ApiResult<List<DictDataDto>>.Ok(tree);
    }

    [Permission("sys:dict:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<DictDataDto?>> Get([FromQuery] string dictCode)
    {
        // 缓存：获取值
        var entity = await _dictDataService.GetAsync(dictCode);
        // if 条件判断
        if (entity == null) return ApiResult<DictDataDto?>.NotFound("字典数据不存在");
        // return 返回结果
        return ApiResult<DictDataDto?>.Ok(entity);
    }

    [Permission("sys:dict:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] DictDataSaveDto dto)
    {
        // return 返回结果
        return await _dictDataService.SaveAsync(dto);
    }

    [Permission("sys:dict:delete")]
    [HttpPost("delete")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteDictDataRequest request)
    {
        // return 返回结果
        return await _dictDataService.DeleteAsync(request.DictCode);
    }
}

// 定义class DeleteDictDataRequest
// 定义类：DeleteDictDataRequest

public class DeleteDictDataRequest
{
    // 属性 DictCode
    // 属性：DictCode

    public string DictCode { get; set; } = string.Empty;
}
