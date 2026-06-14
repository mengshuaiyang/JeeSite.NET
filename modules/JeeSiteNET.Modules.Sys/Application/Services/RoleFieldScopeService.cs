using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>角色字段权限服务，负责按角色+菜单+实体维度维护字段级可见/可写/必填规则配置。</summary>
public class RoleFieldScopeService
{
    private readonly IRoleFieldScopeRepository _repo;

    /// <summary>依赖注入构造函数。</summary>
    public RoleFieldScopeService(IRoleFieldScopeRepository repo)
    {
        _repo = repo;
    }

    /// <summary>按角色+菜单获取全部字段权限配置。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <param name="menuCode">菜单编码。</param>
    /// <returns>字段权限列表。</returns>
    public async Task<List<RoleFieldScopeDto>> GetByRoleMenuAsync(string roleCode, string menuCode)
    {
        var list = await _repo.GetByRoleMenuAsync(roleCode, menuCode);
        return list.Select(e => e.ToDto()).ToList();
    }

    /// <summary>新增字段权限配置。</summary>
    /// <param name="dto">字段权限信息。</param>
    /// <returns>Task。</returns>
    public async Task SaveAsync(RoleFieldScopeSaveDto dto)
    {
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
    }

    /// <summary>按 ID 更新字段权限配置。</summary>
    /// <param name="id">配置记录 ID。</param>
    /// <param name="dto">更新内容。</param>
    /// <returns>Task，若记录不存在抛异常。</returns>
    public async Task UpdateAsync(string id, RoleFieldScopeSaveDto dto)
    {
        var entity = await _repo.GetAsync(id);
        if (entity == null) throw new Exception("字段权限配置不存在");
        entity.RoleCode = dto.RoleCode;
        entity.MenuCode = dto.MenuCode;
        entity.EntityName = dto.EntityName;
        entity.EntityLabel = dto.EntityLabel;
        entity.EntityClass = dto.EntityClass;
        entity.FieldConfig = dto.FieldConfig;
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
    }

    /// <summary>按 ID 删除字段权限配置。</summary>
    /// <param name="id">配置记录 ID。</param>
    /// <returns>Task。</returns>
    public async Task DeleteAsync(string id)
    {
        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
    }
}
