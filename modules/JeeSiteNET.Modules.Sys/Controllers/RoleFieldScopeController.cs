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
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role-field-scope")]
[Permission("sys:role:field-scope")]
// 定义class RoleFieldScopeController
// 定义类：RoleFieldScopeController

public class RoleFieldScopeController : ControllerBase
{
    // 字段 _service
    // 字段：_service

    private readonly RoleFieldScopeService _service;

    // 方法 RoleFieldScopeController
    // 构造函数：RoleFieldScopeController

    public RoleFieldScopeController(RoleFieldScopeService service)
    {
        _service = service;
    }

    [HttpGet]
    // 方法 GetByRoleMenu
    // 方法：GetByRoleMenu

    public async Task<ApiResult<List<RoleFieldScopeDto>>> GetByRoleMenu([FromQuery] string roleCode, [FromQuery] string menuCode)
    {
        var list = await _service.GetByRoleMenuAsync(roleCode, menuCode);
        // return 返回结果
        return ApiResult<List<RoleFieldScopeDto>>.Ok(list);
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] RoleFieldScopeSaveDto dto)
    {
        // await 异步等待
        await _service.SaveAsync(dto);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpPut("{id}")]
    // 方法 Update
    // 方法：Update

    public async Task<ApiResult> Update(string id, [FromBody] RoleFieldScopeSaveDto dto)
    {
        // await 异步等待
        await _service.UpdateAsync(id, dto);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpDelete("{id}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string id)
    {
        // await 异步等待
        await _service.DeleteAsync(id);
        // return 返回结果
        return ApiResult.Ok();
    }
}
