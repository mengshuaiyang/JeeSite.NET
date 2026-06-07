using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class DictDataService
{
    private readonly IDictDataRepository _dictDataRepository;
    private readonly IFusionCache _cache;

    public DictDataService(IDictDataRepository dictDataRepository, IFusionCache cache)
    {
        _dictDataRepository = dictDataRepository;
        _cache = cache;
    }

    public async Task<DictDataDto?> GetAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<List<DictDataDto>> TreeAsync(string dictType)
    {
        var all = await _dictDataRepository.Query()
            .Where(d => d.DictType == dictType)
            .OrderBy(d => d.TreeSorts)
            .ToListAsync();
        return BuildTree(all, null);
    }

    private static List<DictDataDto> BuildTree(List<DictData> all, string? parentCode)
    {
        return all
            .Where(d => d.ParentCode == parentCode)
            .Select(d =>
            {
                var dto = MapToDto(d);
                dto.Children = BuildTree(all, d.DictCode);
                return dto;
            })
            .ToList();
    }

    public async Task<PageResult<DictDataDto>> FindPageAsync(PageRequest<DictData> request)
    {
        var query = _dictDataRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.DictType),
                d => d.DictType == request.Entity!.DictType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.DictLabel),
                d => d.DictLabel.Contains(request.Entity!.DictLabel!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status),
                d => d.Status == request.Entity!.Status)
            .OrderBy(d => d.Sort);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResult<DictDataDto>
        {
            List = list.Select(MapToDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    public async Task<List<DictDataDto>> GetByTypeAsync(string dictType)
    {
        return await _cache.GetOrSetAsync(
            CacheKeys.DictByType(dictType),
            async (ct) =>
            {
                var list = await _dictDataRepository.GetByTypeAsync(dictType);
                return list.Select(MapToDto).ToList();
            },
            TimeSpan.FromMinutes(30));
    }

    public async Task<ApiResult> SaveAsync(DictDataSaveDto dto)
    {
        var now = DateTime.Now;
        DictData? entity;

        if (!string.IsNullOrEmpty(dto.DictCode))
        {
            entity = await _dictDataRepository.GetAsync(dto.DictCode);
            if (entity == null) return ApiResult.NotFound("字典数据不存在");
            entity.DictType = dto.DictType;
            entity.DictLabel = dto.DictLabel;
            entity.DictValue = dto.DictValue;
            entity.ParentCode = dto.ParentCode;
            entity.Sort = dto.Sort;
            entity.TreeLeaf = "1";
            entity.UpdateDate = now;
            await _dictDataRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new DictData
            {
                DictCode = IdGenerator.NewId(),
                DictType = dto.DictType,
                DictLabel = dto.DictLabel,
                DictValue = dto.DictValue,
                ParentCode = dto.ParentCode,
                Sort = dto.Sort,
                TreeLeaf = "1",
                CreateDate = now,
                UpdateDate = now
            };
            await _dictDataRepository.AddAsync(entity);
        }

        if (!string.IsNullOrEmpty(entity.ParentCode))
        {
            var parent = await _dictDataRepository.GetAsync(entity.ParentCode);
            if (parent != null)
            {
                parent.TreeLeaf = "0";
                await _dictDataRepository.UpdateAsync(parent);
            }
        }

        await _cache.RemoveAsync(CacheKeys.DictByType(dto.DictType));
        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        if (entity == null) return ApiResult.NotFound("字典数据不存在");
        await _dictDataRepository.DeleteAsync(entity);
        if (entity.DictType != null)
            await _cache.RemoveAsync(CacheKeys.DictByType(entity.DictType));
        return ApiResult.Ok();
    }

    private static DictDataDto MapToDto(DictData entity) => new()
    {
        DictCode = entity.DictCode,
        DictType = entity.DictType,
        DictLabel = entity.DictLabel,
        DictValue = entity.DictValue,
        Sort = entity.Sort,
        ParentCode = entity.ParentCode,
        TreeLeaf = entity.TreeLeaf,
        Status = entity.Status
    };
}
