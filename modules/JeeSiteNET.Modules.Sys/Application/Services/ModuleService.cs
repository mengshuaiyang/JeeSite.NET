using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class ModuleService
{
    private readonly IModuleRepository _moduleRepository;
    public ModuleService(IModuleRepository moduleRepository) => _moduleRepository = moduleRepository;

    public async Task<ModuleDto?> GetAsync(string moduleCode)
    {
        var entity = await _moduleRepository.GetAsync(moduleCode);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<PageResult<ModuleDto>> FindPageAsync(PageRequest<Module> request)
    {
        var query = _moduleRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ModuleName), m => m.ModuleName.Contains(request.Entity!.ModuleName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsEnabled), m => m.IsEnabled == request.Entity!.IsEnabled)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), m => m.Status == request.Entity!.Status)
            .OrderBy(m => m.ModuleName);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<ModuleDto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    public async Task<ApiResult> SaveAsync(ModuleSaveDto dto)
    {
        var now = DateTime.Now;
        Module? entity;
        if (!string.IsNullOrEmpty(dto.ModuleCode))
        {
            entity = await _moduleRepository.GetAsync(dto.ModuleCode);
            if (entity == null) return ApiResult.NotFound("模块不存在");
            entity.ModuleName = dto.ModuleName; entity.ModuleVersion = dto.ModuleVersion; entity.MainClass = dto.MainClass; entity.IsEnabled = dto.IsEnabled; entity.UpdateDate = now;
            await _moduleRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Module { ModuleCode = IdGenerator.NewId(), ModuleName = dto.ModuleName, ModuleVersion = dto.ModuleVersion, MainClass = dto.MainClass, IsEnabled = dto.IsEnabled ?? "1", CreateDate = now, UpdateDate = now };
            await _moduleRepository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync(string moduleCode)
    {
        var entity = await _moduleRepository.GetAsync(moduleCode);
        if (entity == null) return ApiResult.NotFound("模块不存在");
        await _moduleRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static ModuleDto MapToDto(Module e) => new() { ModuleCode = e.ModuleCode, ModuleName = e.ModuleName, ModuleVersion = e.ModuleVersion, MainClass = e.MainClass, IsEnabled = e.IsEnabled, Status = e.Status };
}
