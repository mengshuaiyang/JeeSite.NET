using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>岗位（职位）管理服务，负责岗位的增删改查。</summary>
public class PostService
{
    private readonly IPostRepository _postRepository;

    /// <summary>依赖注入构造函数。</summary>
    public PostService(IPostRepository postRepository) => _postRepository = postRepository;

    /// <summary>根据岗位编码获取岗位。</summary>
    /// <param name="postCode">岗位编码。</param>
    /// <returns>岗位 DTO，不存在时返回 null。</returns>
    public async Task<PostDto?> GetAsync(string postCode)
    {
        var entity = await _postRepository.GetAsync(postCode);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按条件分页查询岗位（按岗位名/机构/状态过滤）。</summary>
    /// <param name="request">分页与过滤条件。</param>
    /// <returns>岗位分页结果。</returns>
    public async Task<PageResult<PostDto>> FindPageAsync(PageRequest<Post> request)
    {
        var query = _postRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.PostName), p => p.PostName.Contains(request.Entity!.PostName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.OrgCode), p => p.OrgCode == request.Entity!.OrgCode)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), p => p.Status == request.Entity!.Status)
            .OrderBy(p => p.PostSort);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<PostDto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    /// <summary>新增或保存岗位。</summary>
    /// <param name="dto">岗位保存信息。</param>
    /// <returns>保存后的岗位 DTO。</returns>
    public async Task<ApiResult> SaveAsync(PostSaveDto dto)
    {
        var now = DateTime.Now;
        Post? entity;
        if (!string.IsNullOrEmpty(dto.PostCode))
        {
            entity = await _postRepository.GetAsync(dto.PostCode);
            if (entity == null) return ApiResult.NotFound("岗位不存在");
            entity.PostName = dto.PostName; entity.OrgCode = dto.OrgCode; entity.PostSort = dto.Sort; entity.UpdateDate = now;
            await _postRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Post { PostCode = IdGenerator.NewId(), PostName = dto.PostName, OrgCode = dto.OrgCode, PostSort = dto.Sort, CreateDate = now, UpdateDate = now };
            await _postRepository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    /// <summary>删除岗位。</summary>
    /// <param name="postCode">岗位编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string postCode)
    {
        var entity = await _postRepository.GetAsync(postCode);
        if (entity == null) return ApiResult.NotFound("岗位不存在");
        await _postRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static PostDto MapToDto(Post e) => new() { PostCode = e.PostCode, PostName = e.PostName, OrgCode = e.OrgCode, Sort = e.PostSort, Status = e.Status };
}
