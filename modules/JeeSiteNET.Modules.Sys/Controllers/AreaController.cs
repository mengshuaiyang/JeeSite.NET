    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/area")]
[Permission("sys:area")]
// 定义class AreaController
// 定义类：AreaController

public class AreaController : ControllerBase
{
    // 字段 _service
    // 字段：_service

    private readonly AreaService _service;

    // 方法 AreaController
    // 构造函数：AreaController

    public AreaController(AreaService service)
    {
        _service = service;
    }

    [HttpGet("tree")]
    // 方法 Tree
    // 方法：Tree

    public async Task<ApiResult<List<AreaDto>>> Tree()
    {
        var list = await _service.TreeAsync();
        // return 返回结果
        return ApiResult<List<AreaDto>>.Ok(list);
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] Area dto)
    {
        // return 返回结果
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{areaCode}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string areaCode)
    {
        // return 返回结果
        return await _service.DeleteAsync(areaCode);
    }
}
