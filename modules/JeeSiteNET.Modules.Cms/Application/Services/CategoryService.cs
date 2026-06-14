using JeeSiteNET.Core;

using JeeSiteNET.Core.Utils;

using JeeSiteNET.Modules.Cms.Application.DTOs;

using JeeSiteNET.Modules.Cms.Domain.Entities;

using JeeSiteNET.Modules.Cms.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;
/// <summary>CMS 栏目领域服务，提供栏目列表、树状结构构建、按站点过滤、CRUD 等业务操作。</summary>
public class CategoryService

{

    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository) => _repository = repository;
    /// <summary>按站点查询栏目列表。</summary>
    public async Task<List<CategoryDto>> GetBySiteAsync(string siteCode)

    {

        var list = await _repository.Query().Where(c => c.SiteCode == siteCode && c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();

        return list.Select(CategoryDto.FromEntity).ToList();

    }
    /// <summary>查询全部数据。</summary>
    public async Task<List<CategoryDto>> GetAllAsync()

    {

        var list = await _repository.Query().Where(c => c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();

        return list.Select(CategoryDto.FromEntity).ToList();

    }
    /// <summary>列表查询方法（不分页）。</summary>
    public async Task<List<CategoryDto>> FindListAsync()

    {

        var list = await _repository.Query().OrderBy(c => c.TreeSort).ToListAsync();

        return list.Select(CategoryDto.FromEntity).ToList();

    }
    /// <summary>树形结构查询方法。</summary>
    public async Task<List<CategoryDto>> FindTreeAsync()

    {

        var list = await _repository.FindTreeAsync();

        return BuildTree(list.Select(CategoryDto.FromEntity).ToList(), "0");

    }
    /// <summary>获取单条数据的异步方法（详情查询）。</summary>
    public async Task<CategoryDto?> GetAsync(string categoryCode)

    {

        var entity = await _repository.GetAsync(categoryCode);

        return entity == null ? null : CategoryDto.FromEntity(entity);

    }
    /// <summary>保存方法（新增或更新）。</summary>
    public async Task<ApiResult> SaveAsync(CategorySaveDto dto)

    {

        var now = DateTime.Now;

        Category? entity;

        if (!string.IsNullOrEmpty(dto.CategoryCode))

        {

            entity = await _repository.GetAsync(dto.CategoryCode);

            if (entity == null) return ApiResult.NotFound("栏目不存在");

            entity.CategoryName = dto.CategoryName;

            entity.CategoryType = dto.CategoryType ?? "article";

            entity.Image = dto.Image;

            entity.Link = dto.Link;

            entity.Keywords = dto.Keywords;

            entity.Description = dto.Description;

            entity.IsShow = dto.IsShow ?? "1";

            entity.SiteCode = dto.SiteCode;

            entity.ParentCode = dto.ParentCode ?? "0";

            entity.TreeSort = dto.TreeSort;

            entity.UpdateDate = now;

            await _repository.UpdateAsync(entity);

        }

        else

        {

            entity = new Category

            {

                CategoryCode = IdGenerator.NewId(),

                CategoryName = dto.CategoryName,

                CategoryType = dto.CategoryType ?? "article",

                Image = dto.Image,

                Link = dto.Link,

                Keywords = dto.Keywords,

                Description = dto.Description,

                IsShow = dto.IsShow ?? "1",

                SiteCode = dto.SiteCode,

                ParentCode = dto.ParentCode ?? "0",

                TreeSort = dto.TreeSort,

                CreateDate = now,

                UpdateDate = now

            };

            await _repository.AddAsync(entity);

        }

        return ApiResult.Ok(CategoryDto.FromEntity(entity));

    }
    /// <summary>删除方法。</summary>
    public async Task<ApiResult> DeleteAsync(string categoryCode)

    {

        var entity = await _repository.GetAsync(categoryCode);

        if (entity == null) return ApiResult.NotFound("栏目不存在");

        await _repository.DeleteAsync(entity);

        return ApiResult.Ok();

    }

    private static List<CategoryDto> BuildTree(List<CategoryDto> all, string parentCode)

    {

        return all.Where(c => c.ParentCode == parentCode).OrderBy(c => c.TreeSort)

            .Select(c => { c.Children = BuildTree(all, c.CategoryCode); return c; })

            .ToList();

    }

}
