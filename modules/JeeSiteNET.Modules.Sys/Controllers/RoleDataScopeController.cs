using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role-data-scope")]
[Permission("sys:role:data-scope")]
public class RoleDataScopeController : ControllerBase
{
    private readonly RoleDataScopeService _service;

    public RoleDataScopeController(RoleDataScopeService service)
    {
        _service = service;
    }

    [HttpGet("role/{roleCode}")]
    public async Task<ApiResult<List<RoleDataScopeDto>>> GetByRole(string roleCode)
    {
        var list = await _service.GetByRoleAsync(roleCode);
        return ApiResult<List<RoleDataScopeDto>>.Ok(list);
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] RoleDataScopeSaveDto dto)
    {
        await _service.SaveAsync(dto);
        return ApiResult.Ok();
    }

    [HttpDelete("{roleCode}/{menuCode}")]
    public async Task<ApiResult> Delete(string roleCode, string menuCode)
    {
        await _service.DeleteAsync(roleCode, menuCode);
        return ApiResult.Ok();
    }

    [HttpDelete("role/{roleCode}")]
    public async Task<ApiResult> DeleteByRole(string roleCode)
    {
        await _service.DeleteByRoleAsync(roleCode);
        return ApiResult.Ok();
    }
}
