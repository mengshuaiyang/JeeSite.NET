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
[Route("api/v1/sys/role-data-scope")]
[Permission("sys:role:data-scope")]
// 定义class RoleDataScopeController
// 定义类：RoleDataScopeController

public class RoleDataScopeController : ControllerBase
{
    // 字段 _service
    // 字段：_service

    private readonly RoleDataScopeService _service;

    // 方法 RoleDataScopeController
    // 构造函数：RoleDataScopeController

    public RoleDataScopeController(RoleDataScopeService service)
    {
        _service = service;
    }

    [HttpGet("role/{roleCode}")]
    // 方法 GetByRole
    // 方法：GetByRole

    public async Task<ApiResult<List<RoleDataScopeDto>>> GetByRole(string roleCode)
    {
        var list = await _service.GetByRoleAsync(roleCode);
        // return 返回结果
        return ApiResult<List<RoleDataScopeDto>>.Ok(list);
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] RoleDataScopeSaveDto dto)
    {
        // await 异步等待
        await _service.SaveAsync(dto);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpDelete("{roleCode}/{menuCode}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string roleCode, string menuCode)
    {
        // await 异步等待
        await _service.DeleteAsync(roleCode, menuCode);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpDelete("role/{roleCode}")]
    // 方法 DeleteByRole
    // 方法：DeleteByRole

    public async Task<ApiResult> DeleteByRole(string roleCode)
    {
        // await 异步等待
        await _service.DeleteByRoleAsync(roleCode);
        // return 返回结果
        return ApiResult.Ok();
    }
}
