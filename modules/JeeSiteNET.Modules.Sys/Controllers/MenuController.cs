using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/menu")]
public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService) => _menuService = menuService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<MenuDto>>> List([FromBody] PageRequest<Menu> request)
    {
        var result = await _menuService.FindPageAsync(request);
        return ApiResult<PageResult<MenuDto>>.Ok(result);
    }

    [HttpGet("tree")]
    public async Task<ApiResult<List<MenuDto>>> Tree([FromQuery] string? moduleCode = null)
    {
        var tree = await _menuService.FindTreeAsync(moduleCode);
        return ApiResult<List<MenuDto>>.Ok(tree);
    }

    [HttpGet("get")]
    public async Task<ApiResult<MenuDto?>> Get([FromQuery] string menuCode)
    {
        var menu = await _menuService.GetAsync(menuCode);
        if (menu == null) return ApiResult<MenuDto?>.NotFound("菜单不存在");
        return ApiResult<MenuDto?>.Ok(menu);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] MenuSaveDto dto)
    {
        return await _menuService.SaveAsync(dto);
    }

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteMenuRequest request)
    {
        return await _menuService.DeleteAsync(request.MenuCode);
    }
}

public class DeleteMenuRequest
{
    public string MenuCode { get; set; } = string.Empty;
}
