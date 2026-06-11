using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>菜单管理接口</summary>
[ApiController]
[Route("api/v1/sys/menu")]
public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService) => _menuService = menuService;

    /// <summary>分页查询菜单</summary>
    [Permission("sys:menu:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<MenuDto>>> List([FromBody] PageRequest<Menu> request)
    {
        var result = await _menuService.FindPageAsync(request);
        return ApiResult<PageResult<MenuDto>>.Ok(result);
    }

    /// <summary>获取菜单树（含子节点）</summary>
    [Permission("sys:menu:list")]
    [HttpGet("tree")]
    public async Task<ApiResult<List<MenuDto>>> Tree([FromQuery] string? moduleCode = null)
    {
        var tree = await _menuService.FindTreeAsync(moduleCode);
        return ApiResult<List<MenuDto>>.Ok(tree);
    }

    /// <summary>获取单条菜单</summary>
    [Permission("sys:menu:list")]
    [HttpGet("get")]
    public async Task<ApiResult<MenuDto?>> Get([FromQuery] string menuCode)
    {
        var menu = await _menuService.GetAsync(menuCode);
        if (menu == null) return ApiResult<MenuDto?>.NotFound("菜单不存在");
        return ApiResult<MenuDto?>.Ok(menu);
    }

    /// <summary>保存菜单（新增/编辑）</summary>
    [Permission("sys:menu:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] MenuSaveDto dto)
    {
        return await _menuService.SaveAsync(dto);
    }

    /// <summary>删除菜单</summary>
    [Permission("sys:menu:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteMenuRequest request)
    {
        return await _menuService.DeleteAsync(request.MenuCode);
    }

    /// <summary>获取当前用户菜单树（按权限过滤）</summary>
    [Authorize]
    [HttpGet("user-menus")]
    public async Task<ApiResult<List<MenuDto>>> UserMenus([FromQuery] string? sysCode = null)
    {
        var tree = await _menuService.GetUserMenusAsync(sysCode);
        return ApiResult<List<MenuDto>>.Ok(tree);
    }

    /// <summary>获取当前用户可用的系统编码列表</summary>
    [Authorize]
    [HttpGet("sys-codes")]
    public async Task<ApiResult<List<string>>> SysCodes()
    {
        var codes = await _menuService.GetSysCodesAsync();
        return ApiResult<List<string>>.Ok(codes);
    }

    /// <summary>修复菜单树数据</summary>
    [Permission("sys:menu:edit")]
    [HttpPost("fix-tree")]
    public async Task<ApiResult> FixTree()
    {
        var result = await _menuService.FixTreeDataAsync();
        return result;
    }
}

public class DeleteMenuRequest
{
    public string MenuCode { get; set; } = string.Empty;
}
