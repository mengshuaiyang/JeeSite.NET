using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class ConfigService
{
    private readonly IConfigRepository _configRepository;
    public ConfigService(IConfigRepository configRepository) => _configRepository = configRepository;

    public async Task<ConfigDto?> GetAsync(string configKey)
    {
        var entity = await _configRepository.GetAsync(configKey);
        return entity == null ? null : MapToDto(entity);
    }

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
            entity = new Config { ConfigKey = dto.ConfigName, ConfigName = dto.ConfigName, ConfigValue = dto.ConfigValue, IsSys = dto.IsSys ?? "0", CreateDate = now, UpdateDate = now };
            await _configRepository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync(string configKey)
    {
        var entity = await _configRepository.GetAsync(configKey);
        if (entity == null) return ApiResult.NotFound("配置不存在");
        await _configRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static ConfigDto MapToDto(Config e) => new() { ConfigKey = e.ConfigKey, ConfigName = e.ConfigName, ConfigValue = e.ConfigValue, IsSys = e.IsSys, Status = e.Status };
}
