using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class RoleFieldScopeService
{
    private readonly IRoleFieldScopeRepository _repo;

    public RoleFieldScopeService(IRoleFieldScopeRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<RoleFieldScopeDto>> GetByRoleMenuAsync(string roleCode, string menuCode)
    {
        var list = await _repo.GetByRoleMenuAsync(roleCode, menuCode);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task SaveAsync(RoleFieldScopeSaveDto dto)
    {
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
    }

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

    public async Task DeleteAsync(string id)
    {
        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
    }
}
