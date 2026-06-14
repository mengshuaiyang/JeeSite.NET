using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>字典数据服务，负责字典数据的增删改查与树型构建，并按类型做本地缓存。</summary>
public class DictDataService
{
    private readonly IDictDataRepository _dictDataRepository;
    private readonly IFusionCache _cache;

    /// <summary>依赖注入构造函数。</summary>
    public DictDataService(IDictDataRepository dictDataRepository, IFusionCache cache)
    {
        _dictDataRepository = dictDataRepository;
        _cache = cache;
    }

    /// <summary>根据字典编码获取字典项。</summary>
    /// <param name="dictCode">字典编码。</param>
    /// <returns>字典 DTO，不存在时返回 null。</returns>
    public async Task<DictDataDto?> GetAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按字典类型返回树形结构字典数据。</summary>
    /// <param name="dictType">字典类型编码。</param>
    /// <returns>字典 DTO 树。</returns>
    public async Task<List<DictDataDto>> TreeAsync(string dictType)
    {
        var all = await _dictDataRepository.Query()
            .Where(d => d.DictType == dictType)
            .OrderBy(d => d.TreeSorts)
            .ToListAsync();
        return BuildTree(all, null);
    }

    /// <summary>递归构建父子关系字典树。</summary>
    /// <param name="all">扁平字典数据列表。</param>
    /// <param name="parentCode">上级字典编码，根级为 null。</param>
    /// <returns>递归嵌套的字典 DTO 列表。</returns>
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

    /// <summary>按条件分页查询字典数据。</summary>
    /// <param name="request">分页及过滤条件（字典类型、标签、状态）。</param>
    /// <returns>分页结果。</returns>
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

    /// <summary>按字典类型获取全部字典项（带 30 分钟 FusionCache 缓存）。</summary>
    /// <param name="dictType">字典类型编码。</param>
    /// <returns>字典 DTO 列表。</returns>
    public async Task<List<DictDataDto>> GetByTypeAsync(string dictType)
    {
        // 字典变动频率低，设置 30 分钟缓存；保存/删除时会主动清除对应类型缓存
        return await _cache.GetOrSetAsync(
            CacheKeys.DictByType(dictType),
            async (ct) =>
            {
                var list = await _dictDataRepository.GetByTypeAsync(dictType);
                return list.Select(MapToDto).ToList();
            },
            TimeSpan.FromMinutes(30));
    }

    /// <summary>新增或更新字典数据，保存后将父节点标记为非叶子并清除缓存。</summary>
    /// <param name="dto">字典保存信息。</param>
    /// <returns>保存后的字典 DTO。</returns>
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
            entity.ParentCode = dto.ParentCode ?? "0";
            entity.Sort = dto.Sort;
            // 新插入节点默认作为叶子节点，后续若有子节点追加会更新父节点为 "0"
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
                ParentCode = dto.ParentCode ?? "0",
                Sort = dto.Sort,
                TreeLeaf = "1",
                CreateDate = now,
                UpdateDate = now
            };
            await _dictDataRepository.AddAsync(entity);
        }

        // 若指定了父节点，则更新父节点为非叶子，保证前端树型结构正常显示
        if (!string.IsNullOrEmpty(entity.ParentCode))
        {
            var parent = await _dictDataRepository.GetAsync(entity.ParentCode);
            if (parent != null)
            {
                parent.TreeLeaf = "0";
                await _dictDataRepository.UpdateAsync(parent);
            }
        }

        // 保存后清除该类型缓存，保证下次读取为最新数据
        await _cache.RemoveAsync(CacheKeys.DictByType(dto.DictType));
        return ApiResult.Ok(MapToDto(entity));
    }

    /// <summary>删除字典数据，删除后清除对应类型缓存。</summary>
    /// <param name="dictCode">字典编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string dictCode)
    {
        var entity = await _dictDataRepository.GetAsync(dictCode);
        if (entity == null) return ApiResult.NotFound("字典数据不存在");
        await _dictDataRepository.DeleteAsync(entity);
        if (entity.DictType != null)
            await _cache.RemoveAsync(CacheKeys.DictByType(entity.DictType));
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
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
