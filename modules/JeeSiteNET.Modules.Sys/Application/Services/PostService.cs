using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class PostService
{
    private readonly IPostRepository _postRepository;
    public PostService(IPostRepository postRepository) => _postRepository = postRepository;

    public async Task<PostDto?> GetAsync(string postCode)
    {
        var entity = await _postRepository.GetAsync(postCode);
        return entity == null ? null : MapToDto(entity);
    }

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

    public async Task<ApiResult> DeleteAsync(string postCode)
    {
        var entity = await _postRepository.GetAsync(postCode);
        if (entity == null) return ApiResult.NotFound("岗位不存在");
        await _postRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static PostDto MapToDto(Post e) => new() { PostCode = e.PostCode, PostName = e.PostName, OrgCode = e.OrgCode, Sort = e.PostSort, Status = e.Status };
}
