using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;
    public CategoryService(ICategoryRepository repository) => _repository = repository;

    public async Task<List<CategoryDto>> GetBySiteAsync(string siteCode)
    {
        var list = await _repository.Query().Where(c => c.SiteCode == siteCode && c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();
        return list.Select(CategoryDto.FromEntity).ToList();
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var list = await _repository.Query().Where(c => c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();
        return list.Select(CategoryDto.FromEntity).ToList();
    }

    public async Task<List<CategoryDto>> FindListAsync()
    {
        var list = await _repository.Query().OrderBy(c => c.TreeSort).ToListAsync();
        return list.Select(CategoryDto.FromEntity).ToList();
    }

    public async Task<List<CategoryDto>> FindTreeAsync()
    {
        var list = await _repository.FindTreeAsync();
        return BuildTree(list.Select(CategoryDto.FromEntity).ToList(), "0");
    }

    public async Task<CategoryDto?> GetAsync(string categoryCode)
    {
        var entity = await _repository.GetAsync(categoryCode);
        return entity == null ? null : CategoryDto.FromEntity(entity);
    }

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
