    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
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
[Route("api/v1/sys/role-menu")]
// 定义class RoleMenuController
// 定义类：RoleMenuController

public class RoleMenuController : ControllerBase
{
    // 字段 _roleMenuService
    // 字段：_roleMenuService

    private readonly RoleMenuService _roleMenuService;
    // 构造函数 RoleMenuController
    // 构造函数：RoleMenuController

    public RoleMenuController(RoleMenuService roleMenuService) => _roleMenuService = roleMenuService;

    [Permission("sys:role:list")]
    [HttpGet("get-menu-codes")]
    // 方法 GetMenuCodes
    // 方法：GetMenuCodes

    public async Task<ApiResult<List<string>>> GetMenuCodes([FromQuery] string roleCode)
    {
        // return 返回结果
        return await _roleMenuService.GetMenuCodesAsync(roleCode);
    }

    [Permission("sys:role:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] RoleMenuSaveDto dto)
    {
        // return 返回结果
        return await _roleMenuService.SaveAsync(dto);
    }
}
