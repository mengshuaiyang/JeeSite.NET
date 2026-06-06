using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class MenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IFusionCache _cache;

    public MenuService(IMenuRepository menuRepository, IFusionCache cache)
    {
        _menuRepository = menuRepository;
        _cache = cache;
    }

    public async Task<MenuDto?> GetAsync(string menuCode)
    {
        var menu = await _menuRepository.GetAsync(menuCode);
        return menu == null ? null : MapToDto(menu);
    }

    public async Task<PageResult<MenuDto>> FindPageAsync(PageRequest<Menu> request)
    {
        var query = _menuRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.MenuName),
                m => m.MenuName.Contains(request.Entity!.MenuName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ModuleCode),
                m => m.ModuleCode == request.Entity!.ModuleCode)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status),
                m => m.Status == request.Entity!.Status)
            .OrderBy(m => m.TreeSort);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResult<MenuDto>
        {
            List = list.Select(MapToDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    public async Task<List<MenuDto>> FindTreeAsync(string? moduleCode = null)
    {
        var key = CacheKeys.MenuTree(moduleCode ?? "all");
        return await _cache.GetOrSetAsync(key, async (ct) =>
        {
            var query = _menuRepository.Query().Where(m => m.Status == "0");
            if (!string.IsNullOrEmpty(moduleCode))
                query = query.Where(m => m.ModuleCode == moduleCode);

            var list = await query.OrderBy(m => m.TreeSort).ToListAsync();
            return BuildTree(list, "0");
        }, TimeSpan.FromMinutes(10));
    }

    public async Task<ApiResult> SaveAsync(MenuSaveDto dto)
    {
        var now = DateTime.Now;
        Menu? menu;

        if (!string.IsNullOrEmpty(dto.MenuCode))
        {
            menu = await _menuRepository.GetAsync(dto.MenuCode);
            if (menu == null) return ApiResult.NotFound("菜单不存在");
            menu.MenuName = dto.MenuName;
            menu.MenuHref = dto.MenuHref;
            menu.MenuTarget = dto.MenuTarget;
            menu.MenuIcon = dto.MenuIcon;
            menu.Permission = dto.Permission;
            menu.Weight = dto.Weight;
            menu.IsShow = dto.IsShow;
            menu.ModuleCode = dto.ModuleCode;
            menu.ParentCode = dto.ParentCode;
            menu.TreeSort = dto.TreeSort;
            menu.UpdateDate = now;
            await _menuRepository.UpdateAsync(menu);
        }
        else
        {
            menu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = dto.MenuName,
                MenuHref = dto.MenuHref,
                MenuTarget = dto.MenuTarget,
                MenuIcon = dto.MenuIcon,
                Permission = dto.Permission,
                Weight = dto.Weight,
                IsShow = dto.IsShow ?? "1",
                ModuleCode = dto.ModuleCode,
                ParentCode = dto.ParentCode,
                TreeSort = dto.TreeSort,
                CreateDate = now,
                UpdateDate = now
            };
            await _menuRepository.AddAsync(menu);
        }

        await _cache.RemoveAsync(CacheKeys.MenuTree(menu.ModuleCode ?? "all"));
        return ApiResult.Ok(MapToDto(menu));
    }

    public async Task<ApiResult> DeleteAsync(string menuCode)
    {
        var menu = await _menuRepository.GetAsync(menuCode);
        if (menu == null) return ApiResult.NotFound("菜单不存在");
        await _menuRepository.DeleteAsync(menu);
        await _cache.RemoveAsync(CacheKeys.MenuTree(menu.ModuleCode ?? "all"));
        return ApiResult.Ok();
    }

    private static MenuDto MapToDto(Menu menu) => new()
    {
        MenuCode = menu.MenuCode,
        MenuName = menu.MenuName,
        MenuHref = menu.MenuHref,
        MenuTarget = menu.MenuTarget,
        MenuIcon = menu.MenuIcon,
        Permission = menu.Permission,
        Weight = menu.Weight,
        IsShow = menu.IsShow,
        ModuleCode = menu.ModuleCode,
        ParentCode = menu.ParentCode,
        ParentCodes = menu.ParentCodes,
        TreeSort = menu.TreeSort,
        TreeNames = menu.TreeNames,
        TreeLevel = menu.TreeLevel,
        TreeLeaf = menu.TreeLeaf,
        Status = menu.Status
    };

    private static List<MenuDto> BuildTree(List<Menu> allMenus, string parentCode)
    {
        return allMenus
            .Where(m => m.ParentCode == parentCode)
            .OrderBy(m => m.TreeSort)
            .Select(m =>
            {
                var dto = MapToDto(m);
                dto.Children = BuildTree(allMenus, m.MenuCode);
                return dto;
            })
            .ToList();
    }
}
