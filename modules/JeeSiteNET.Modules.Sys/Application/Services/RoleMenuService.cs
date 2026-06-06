using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class RoleMenuService
{
    private readonly IRoleMenuRepository _roleMenuRepository;

    public RoleMenuService(IRoleMenuRepository roleMenuRepository)
    {
        _roleMenuRepository = roleMenuRepository;
    }

    public async Task<ApiResult<List<string>>> GetMenuCodesAsync(string roleCode)
    {
        var menuCodes = await _roleMenuRepository.GetMenuCodesByRoleAsync(roleCode);
        return ApiResult<List<string>>.Ok(menuCodes);
    }

    public async Task<ApiResult> SaveAsync(RoleMenuSaveDto dto)
    {
        await _roleMenuRepository.SaveRoleMenusAsync(dto.RoleCode, dto.MenuCodes);
        return ApiResult.Ok();
    }
}
