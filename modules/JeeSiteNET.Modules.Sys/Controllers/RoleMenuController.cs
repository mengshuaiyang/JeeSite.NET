using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role-menu")]
public class RoleMenuController : ControllerBase
{
    private readonly RoleMenuService _roleMenuService;
    public RoleMenuController(RoleMenuService roleMenuService) => _roleMenuService = roleMenuService;

    [HttpGet("get-menu-codes")]
    public async Task<ApiResult<List<string>>> GetMenuCodes([FromQuery] string roleCode)
    {
        return await _roleMenuService.GetMenuCodesAsync(roleCode);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] RoleMenuSaveDto dto)
    {
        return await _roleMenuService.SaveAsync(dto);
    }
}
