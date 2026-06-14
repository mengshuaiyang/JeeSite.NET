using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>角色数据权限服务，负责按角色+菜单维度维护数据可见范围规则（如"仅看本部门"、"看全部"等）。</summary>
public class RoleDataScopeService
{
    private readonly IRoleDataScopeRepository _repository;

    /// <summary>依赖注入构造函数。</summary>
    public RoleDataScopeService(IRoleDataScopeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>根据角色编码获取其所有数据范围配置。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <returns>数据范围配置列表。</returns>
    public async Task<List<RoleDataScopeDto>> GetByRoleAsync(string roleCode)
    {
        var list = await _repository.FindByRoleAsync(roleCode);
        return list.Select(MapToDto).ToList();
    }

    /// <summary>新增或更新单条数据范围规则（以 roleCode + menuCode 为唯一键）。</summary>
    /// <param name="dto">数据范围保存信息。</param>
    /// <returns>Task。</returns>
    public async Task SaveAsync(RoleDataScopeSaveDto dto)
    {
        var existing = await _repository.GetAsync(dto.RoleCode, dto.MenuCode);
        if (existing != null)
        {
            // 已有规则 → 更新配置字段
            existing.RuleName = dto.RuleName;
            existing.RuleType = dto.RuleType;
            existing.RuleConfig = dto.RuleConfig;
            await _repository.UpdateAsync(existing);
        }
        else
        {
            var entity = new RoleDataScope
            {
                RoleCode = dto.RoleCode,
                MenuCode = dto.MenuCode,
                RuleName = dto.RuleName,
                RuleType = dto.RuleType,
                RuleConfig = dto.RuleConfig
            };
            await _repository.AddAsync(entity);
        }
        await _repository.SaveChangesAsync();
    }

    /// <summary>按角色+菜单删除单条数据范围规则。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <param name="menuCode">菜单编码。</param>
    /// <returns>Task。</returns>
    public async Task DeleteAsync(string roleCode, string menuCode)
    {
        await _repository.DeleteAsync(roleCode, menuCode);
        await _repository.SaveChangesAsync();
    }

    /// <summary>删除指定角色下的全部数据范围配置。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <returns>Task。</returns>
    public async Task DeleteByRoleAsync(string roleCode)
    {
        await _repository.DeleteByRoleAsync(roleCode);
        await _repository.SaveChangesAsync();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static RoleDataScopeDto MapToDto(RoleDataScope e) => new()
    {
        RoleCode = e.RoleCode,
        MenuCode = e.MenuCode,
        RuleName = e.RuleName,
        RuleType = e.RuleType,
        RuleConfig = e.RuleConfig
    };
}
