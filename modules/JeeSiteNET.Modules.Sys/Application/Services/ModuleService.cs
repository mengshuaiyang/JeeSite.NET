using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>模块管理服务，负责插件/子模块的信息维护与列表查询。</summary>
public class ModuleService
{
    private readonly IModuleRepository _moduleRepository;

    /// <summary>依赖注入构造函数。</summary>
    public ModuleService(IModuleRepository moduleRepository) => _moduleRepository = moduleRepository;

    /// <summary>根据模块编码获取模块信息。</summary>
    /// <param name="moduleCode">模块编码。</param>
    /// <returns>模块 DTO，不存在时返回 null。</returns>
    public async Task<ModuleDto?> GetAsync(string moduleCode)
    {
        var entity = await _moduleRepository.GetAsync(moduleCode);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按条件分页查询模块列表。</summary>
    /// <param name="request">分页与过滤条件（模块名、启用状态等）。</param>
    /// <returns>模块分页结果。</returns>
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

    /// <summary>新增或更新模块信息（新增默认启用）。</summary>
    /// <param name="dto">模块保存信息。</param>
    /// <returns>保存后的模块 DTO。</returns>
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
            // 新增模块默认启用，避免导入后无法在系统中看到效果
            entity = new Module { ModuleCode = IdGenerator.NewId(), ModuleName = dto.ModuleName, ModuleVersion = dto.ModuleVersion, MainClass = dto.MainClass, IsEnabled = dto.IsEnabled ?? "1", CreateDate = now, UpdateDate = now };
            await _moduleRepository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    /// <summary>删除模块。</summary>
    /// <param name="moduleCode">模块编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string moduleCode)
    {
        var entity = await _moduleRepository.GetAsync(moduleCode);
        if (entity == null) return ApiResult.NotFound("模块不存在");
        await _moduleRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static ModuleDto MapToDto(Module e) => new() { ModuleCode = e.ModuleCode, ModuleName = e.ModuleName, ModuleVersion = e.ModuleVersion, MainClass = e.MainClass, IsEnabled = e.IsEnabled, Status = e.Status };
}
