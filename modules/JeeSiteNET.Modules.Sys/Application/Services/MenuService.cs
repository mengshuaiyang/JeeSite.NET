using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>菜单管理服务，负责菜单树构建、权限过滤、维护与树结构修复。</summary>
public class MenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IFusionCache _cache;
    private readonly ICurrentUser _currentUser;

    /// <summary>依赖注入构造函数。</summary>
    public MenuService(IMenuRepository menuRepository, IFusionCache cache, ICurrentUser currentUser)
    {
        _menuRepository = menuRepository;
        _cache = cache;
        _currentUser = currentUser;
    }

    /// <summary>获取当前登录用户有权访问的菜单树（可按子系统编码过滤）。</summary>
    /// <param name="sysCode">子系统编码，为空返回全部可见菜单。</param>
    /// <returns>菜单树 DTO 列表。</returns>
    public async Task<List<MenuDto>> GetUserMenusAsync(string? sysCode = null)
    {
        var query = _menuRepository.Query()
            .Where(m => m.Status == "0" && m.IsShow != "0");

        if (!string.IsNullOrEmpty(sysCode))
            query = query.Where(m => m.SysCode == sysCode);

        var all = await query.OrderBy(m => m.TreeSort).ToListAsync();

        // 超级管理员或未配置权限时直接展示全部可见菜单
        if (_currentUser.IsSuperAdmin || _currentUser.Permissions.Count == 0)
            return BuildTree(all, "0");

        var perms = new HashSet<string>(_currentUser.Permissions);
        // 未配置 Permission 的菜单项视为公共可见，直接放过
        var filtered = all.Where(m =>
            string.IsNullOrEmpty(m.Permission) || perms.Contains(m.Permission)).ToList();
        return BuildTree(filtered, "0");
    }

    /// <summary>获取菜单中配置的全部子系统编码（去重）。</summary>
    /// <returns>子系统编码列表。</returns>
    public async Task<List<string>> GetSysCodesAsync()
    {
        var codes = await _menuRepository.Query()
            .Where(m => m.Status == "0" && !string.IsNullOrEmpty(m.SysCode))
            .Select(m => m.SysCode!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
        return codes;
    }

    /// <summary>根据菜单编码获取菜单 DTO。</summary>
    /// <param name="menuCode">菜单编码。</param>
    /// <returns>菜单 DTO，不存在时返回 null。</returns>
    public async Task<MenuDto?> GetAsync(string menuCode)
    {
        var menu = await _menuRepository.GetAsync(menuCode);
        return menu == null ? null : MapToDto(menu);
    }

    /// <summary>按条件分页查询菜单列表。</summary>
    /// <param name="request">分页及过滤条件（菜单名、模块、状态）。</param>
    /// <returns>分页结果。</returns>
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

    /// <summary>返回菜单树（按模块过滤，带 10 分钟缓存），适合页面初始化时一次性读取。</summary>
    /// <param name="moduleCode">模块编码，为空返回全部。</param>
    /// <returns>菜单树 DTO 列表。</returns>
    public async Task<List<MenuDto>> FindTreeAsync(string? moduleCode = null)
    {
        // 以模块编码作为缓存 Key，避免不同模块间菜单树混存
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

    /// <summary>新增或保存菜单：保存后会清除对应模块的菜单缓存。</summary>
    /// <param name="dto">菜单保存信息。</param>
    /// <returns>保存后的菜单 DTO。</returns>
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
            // 新增菜单默认显示（IsShow = "1"），避免前端不可见导致调试困难
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

        // 菜单变更后清除缓存确保用户下次获取最新结构
        await _cache.RemoveAsync(CacheKeys.MenuTree(menu.ModuleCode ?? "all"));
        return ApiResult.Ok(MapToDto(menu));
    }

    /// <summary>删除菜单，删除后清除对应模块菜单缓存。</summary>
    /// <param name="menuCode">菜单编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string menuCode)
    {
        var menu = await _menuRepository.GetAsync(menuCode);
        if (menu == null) return ApiResult.NotFound("菜单不存在");
        await _menuRepository.DeleteAsync(menu);
        await _cache.RemoveAsync(CacheKeys.MenuTree(menu.ModuleCode ?? "all"));
        return ApiResult.Ok();
    }

    /// <summary>修复菜单树结构，重新计算 TreeLevel/TreeNames/ParentCodes/TreSort，并清缓存。</summary>
    /// <returns>修复完成结果。</returns>
    public async Task<ApiResult> FixTreeDataAsync()
    {
        var all = await _menuRepository.Query()
            .OrderBy(m => m.TreeSort)
            .ToListAsync();

        // 将菜单扁平数据转换为通用树形节点模型，复用 TreeFixUtil 的统一修复逻辑
        var nodes = all.Select(m => new TreeFixNode
        {
            Id = m.MenuCode,
            ParentId = m.ParentCode ?? "0",
            Name = m.MenuName ?? "",
            TreeSort = m.TreeSort
        }).ToList();

        var fixedNodes = TreeFixUtil.FixTreeData(nodes);
        var nodeMap = fixedNodes.ToDictionary(n => n.Id);

        foreach (var menu in all)
        {
            if (nodeMap.TryGetValue(menu.MenuCode, out var fixedNode))
            {
                menu.ParentCodes = fixedNode.ParentCodes;
                menu.TreeSort = (decimal)fixedNode.TreeSort;
                menu.TreeLevel = fixedNode.TreeLevel;
                menu.TreeNames = fixedNode.TreeNames;
            }
        }

        foreach (var menu in all)
            await _menuRepository.UpdateAsync(menu);

        // 树结构修复后需全部清空菜单缓存，防止前端获取到过期的层级信息
        await _cache.RemoveAsync(CacheKeys.MenuTree("all"));

        return ApiResult.Ok("树结构修复完成");
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
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
        SysCode = menu.SysCode,
        ModuleCode = menu.ModuleCode,
        ParentCode = menu.ParentCode,
        ParentCodes = menu.ParentCodes,
        TreeSort = menu.TreeSort,
        TreeNames = menu.TreeNames,
        TreeLevel = menu.TreeLevel,
        TreeLeaf = menu.TreeLeaf,
        Status = menu.Status
    };

    /// <summary>根据父子关系递归构建菜单树。</summary>
    /// <param name="allMenus">扁平菜单列表。</param>
    /// <param name="parentCode">起始父菜单编码（根为 "0"）。</param>
    /// <returns>递归嵌套的菜单树。</returns>
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
