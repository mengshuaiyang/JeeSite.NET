using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class RoleDataScopeService
{
    private readonly IRoleDataScopeRepository _repository;

    public RoleDataScopeService(IRoleDataScopeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoleDataScopeDto>> GetByRoleAsync(string roleCode)
    {
        var list = await _repository.FindByRoleAsync(roleCode);
        return list.Select(MapToDto).ToList();
    }

    public async Task SaveAsync(RoleDataScopeSaveDto dto)
    {
        var existing = await _repository.GetAsync(dto.RoleCode, dto.MenuCode);
        if (existing != null)
        {
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

    public async Task DeleteAsync(string roleCode, string menuCode)
    {
        await _repository.DeleteAsync(roleCode, menuCode);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteByRoleAsync(string roleCode)
    {
        await _repository.DeleteByRoleAsync(roleCode);
        await _repository.SaveChangesAsync();
    }

    private static RoleDataScopeDto MapToDto(RoleDataScope e) => new()
    {
        RoleCode = e.RoleCode,
        MenuCode = e.MenuCode,
        RuleName = e.RuleName,
        RuleType = e.RuleType,
        RuleConfig = e.RuleConfig
    };
}
