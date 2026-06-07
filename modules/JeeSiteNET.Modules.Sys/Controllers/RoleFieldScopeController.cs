using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role-field-scope")]
[Permission("sys:role:field-scope")]
public class RoleFieldScopeController : ControllerBase
{
    private readonly RoleFieldScopeService _service;

    public RoleFieldScopeController(RoleFieldScopeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ApiResult<List<RoleFieldScopeDto>>> GetByRoleMenu([FromQuery] string roleCode, [FromQuery] string menuCode)
    {
        var list = await _service.GetByRoleMenuAsync(roleCode, menuCode);
        return ApiResult<List<RoleFieldScopeDto>>.Ok(list);
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] RoleFieldScopeSaveDto dto)
    {
        await _service.SaveAsync(dto);
        return ApiResult.Ok();
    }

    [HttpPut("{id}")]
    public async Task<ApiResult> Update(string id, [FromBody] RoleFieldScopeSaveDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return ApiResult.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ApiResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return ApiResult.Ok();
    }
}
