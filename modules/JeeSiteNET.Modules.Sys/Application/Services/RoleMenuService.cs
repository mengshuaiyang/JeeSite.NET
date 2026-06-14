    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class RoleMenuService
// 定义类：RoleMenuService
public class RoleMenuService
{
    // 字段 _roleMenuRepository
    // 字段：_roleMenuRepository
    private readonly IRoleMenuRepository _roleMenuRepository;

    // 方法 RoleMenuService
    // 构造函数：RoleMenuService
    public RoleMenuService(IRoleMenuRepository roleMenuRepository)
    {
        _roleMenuRepository = roleMenuRepository;
    }

    // 方法 GetMenuCodesAsync
    // 方法：GetMenuCodesAsync
    public async Task<ApiResult<List<string>>> GetMenuCodesAsync(string roleCode)
    {
        var menuCodes = await _roleMenuRepository.GetMenuCodesByRoleAsync(roleCode);
        // return 返回结果
        return ApiResult<List<string>>.Ok(menuCodes);
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(RoleMenuSaveDto dto)
    {
        // await 异步等待
        await _roleMenuRepository.SaveRoleMenusAsync(dto.RoleCode, dto.MenuCodes);
        // return 返回结果
        return ApiResult.Ok();
    }
}
