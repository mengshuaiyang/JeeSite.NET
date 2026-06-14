using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>系统参数配置服务，负责系统参数配置项的读取与维护。</summary>
public class ConfigService
{
    private readonly IConfigRepository _configRepository;

    /// <summary>依赖注入构造函数。</summary>
    public ConfigService(IConfigRepository configRepository) => _configRepository = configRepository;

    /// <summary>根据配置 Key 获取配置。</summary>
    /// <param name="configKey">配置 Key。</param>
    /// <returns>配置 DTO，不存在时返回 null。</returns>
    public async Task<ConfigDto?> GetAsync(string configKey)
    {
        var entity = await _configRepository.GetAsync(configKey);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按条件分页查询系统参数（按名称/Key/状态过滤）。</summary>
    /// <param name="request">分页及过滤条件。</param>
    /// <returns>配置分页结果。</returns>
    public async Task<PageResult<ConfigDto>> FindPageAsync(PageRequest<Config> request)
    {
        var query = _configRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ConfigName), c => c.ConfigName.Contains(request.Entity!.ConfigName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ConfigKey), c => c.ConfigKey.Contains(request.Entity!.ConfigKey!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), c => c.Status == request.Entity!.Status)
            .OrderBy(c => c.ConfigKey);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<ConfigDto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    /// <summary>新增或更新配置（IsSys 为系统级标记，默认普通配置）。</summary>
    /// <param name="dto">配置保存信息。</param>
    /// <returns>保存后的配置 DTO。</returns>
    public async Task<ApiResult> SaveAsync(ConfigSaveDto dto)
    {
        var now = DateTime.Now;
        Config? entity;
        if (!string.IsNullOrEmpty(dto.ConfigKey))
        {
            entity = await _configRepository.GetAsync(dto.ConfigKey);
            if (entity == null) return ApiResult.NotFound("配置不存在");
            entity.ConfigName = dto.ConfigName; entity.ConfigValue = dto.ConfigValue; entity.IsSys = dto.IsSys; entity.UpdateDate = now;
            await _configRepository.UpdateAsync(entity);
        }
        else
        {
            // 新增时若未指定 Key，默认与名称相同（后续可由业务层调整）
            entity = new Config { ConfigKey = dto.ConfigName, ConfigName = dto.ConfigName, ConfigValue = dto.ConfigValue, IsSys = dto.IsSys ?? "0", CreateDate = now, UpdateDate = now };
            await _configRepository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    /// <summary>删除配置。</summary>
    /// <param name="configKey">配置 Key。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string configKey)
    {
        var entity = await _configRepository.GetAsync(configKey);
        if (entity == null) return ApiResult.NotFound("配置不存在");
        await _configRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static ConfigDto MapToDto(Config e) => new() { ConfigKey = e.ConfigKey, ConfigName = e.ConfigName, ConfigValue = e.ConfigValue, IsSys = e.IsSys, Status = e.Status };
}
