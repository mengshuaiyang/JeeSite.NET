using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class DictTypeService
{
    private readonly IDictTypeRepository _dictTypeRepository;

    public DictTypeService(IDictTypeRepository dictTypeRepository)
    {
        _dictTypeRepository = dictTypeRepository;
    }

    public async Task<DictTypeDto?> GetAsync(string dictTypeCode)
    {
        var entity = await _dictTypeRepository.GetAsync(dictTypeCode);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<PageResult<DictTypeDto>> FindPageAsync(PageRequest<DictType> request)
    {
        var query = _dictTypeRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.DictName),
                d => d.DictName.Contains(request.Entity!.DictName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status),
                d => d.Status == request.Entity!.Status)
            .OrderBy(d => d.Sort);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResult<DictTypeDto>
        {
            List = list.Select(MapToDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    public async Task<List<DictTypeDto>> FindListAsync()
    {
        var list = await _dictTypeRepository.FindListAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<ApiResult> SaveAsync(DictTypeSaveDto dto)
    {
        var now = DateTime.Now;
        DictType? entity;

        if (!string.IsNullOrEmpty(dto.DictTypeCode))
        {
            entity = await _dictTypeRepository.GetAsync(dto.DictTypeCode);
            if (entity == null) return ApiResult.NotFound("字典类型不存在");
            entity.DictName = dto.DictName;
            entity.IsSys = dto.IsSys;
            entity.Sort = dto.Sort;
            entity.UpdateDate = now;
            await _dictTypeRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new DictType
            {
                DictTypeCode = IdGenerator.NewId(),
                DictName = dto.DictName,
                IsSys = dto.IsSys ?? "0",
                Sort = dto.Sort,
                CreateDate = now,
                UpdateDate = now
            };
            await _dictTypeRepository.AddAsync(entity);
        }

        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync(string dictTypeCode)
    {
        var entity = await _dictTypeRepository.GetAsync(dictTypeCode);
        if (entity == null) return ApiResult.NotFound("字典类型不存在");
        await _dictTypeRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static DictTypeDto MapToDto(DictType entity) => new()
    {
        DictTypeCode = entity.DictTypeCode,
        DictName = entity.DictName,
        IsSys = entity.IsSys,
        Sort = entity.Sort,
        Status = entity.Status
    };
}
