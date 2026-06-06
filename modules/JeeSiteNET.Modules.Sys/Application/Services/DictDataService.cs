using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class DictDataService
{
    private readonly IDictDataRepository _dictDataRepository;

    public DictDataService(IDictDataRepository dictDataRepository)
    {
        _dictDataRepository = dictDataRepository;
    }

    public async Task<DictDataDto?> GetAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        return entity == null ? null : MapToDto(entity);
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
        var list = await _dictDataRepository.GetByTypeAsync(dictType);
        return list.Select(MapToDto).ToList();
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
            entity.Sort = dto.Sort;
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
                Sort = dto.Sort,
                CreateDate = now,
                UpdateDate = now
            };
            await _dictDataRepository.AddAsync(entity);
        }

        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        if (entity == null) return ApiResult.NotFound("字典数据不存在");
        await _dictDataRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static DictDataDto MapToDto(DictData entity) => new()
    {
        DictCode = entity.DictCode,
        DictType = entity.DictType,
        DictLabel = entity.DictLabel,
        DictValue = entity.DictValue,
        Sort = entity.Sort,
        Status = entity.Status
    };
}
