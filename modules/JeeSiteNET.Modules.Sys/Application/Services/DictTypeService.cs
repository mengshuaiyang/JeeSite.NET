using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>字典类型服务，负责字典分类的增删改查与列表查询。</summary>
public class DictTypeService
{
    private readonly IDictTypeRepository _dictTypeRepository;

    /// <summary>依赖注入构造函数。</summary>
    public DictTypeService(IDictTypeRepository dictTypeRepository)
    {
        _dictTypeRepository = dictTypeRepository;
    }

    /// <summary>根据字典类型编码获取字典。</summary>
    /// <param name="dictTypeCode">字典类型编码。</param>
    /// <returns>字典类型 DTO，不存在时返回 null。</returns>
    public async Task<DictTypeDto?> GetAsync(string dictTypeCode)
    {
        var entity = await _dictTypeRepository.GetAsync(dictTypeCode);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按条件分页查询字典类型。</summary>
    /// <param name="request">分页及过滤条件（字典名、状态）。</param>
    /// <returns>分页结果。</returns>
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

    /// <summary>获取全部字典类型列表，适合前端下拉框。</summary>
    /// <returns>字典类型 DTO 列表。</returns>
    public async Task<List<DictTypeDto>> FindListAsync()
    {
        var list = await _dictTypeRepository.FindListAsync();
        return list.Select(MapToDto).ToList();
    }

    /// <summary>新增或更新字典类型。</summary>
    /// <param name="dto">字典类型保存信息。</param>
    /// <returns>保存后的字典类型 DTO。</returns>
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
            // 新增时默认非系统字典，系统字典（IsSys = "1"）一般由平台预置
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

    /// <summary>删除字典类型。</summary>
    /// <param name="dictTypeCode">字典类型编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string dictTypeCode)
    {
        var entity = await _dictTypeRepository.GetAsync(dictTypeCode);
        if (entity == null) return ApiResult.NotFound("字典类型不存在");
        await _dictTypeRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static DictTypeDto MapToDto(DictType entity) => new()
    {
        DictTypeCode = entity.DictTypeCode,
        DictName = entity.DictName,
        IsSys = entity.IsSys,
        Sort = entity.Sort,
        Status = entity.Status
    };
}
