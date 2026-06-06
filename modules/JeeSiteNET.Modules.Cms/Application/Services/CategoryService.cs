using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<CategoryDto?> GetAsync(string categoryCode)
    {
        var entity = await _categoryRepository.GetAsync(categoryCode);
        return entity == null ? null : CategoryDto.FromEntity(entity);
    }

    public async Task<List<CategoryDto>> FindTreeAsync()
    {
        var list = await _categoryRepository.FindTreeAsync();
        var dtos = list.Select(CategoryDto.FromEntity).ToList();
        return BuildTree(dtos, "0");
    }

    public async Task<List<CategoryDto>> FindListAsync()
    {
        var list = await _categoryRepository.FindListAsync();
        return list.Select(CategoryDto.FromEntity).ToList();
    }

    public async Task<ApiResult> SaveAsync(CategorySaveDto dto)
    {
        var now = DateTime.Now;
        Category? entity;
        if (!string.IsNullOrEmpty(dto.CategoryCode))
        {
            entity = await _categoryRepository.GetAsync(dto.CategoryCode);
            if (entity == null) return ApiResult.NotFound("栏目不存在");
            entity.CategoryName = dto.CategoryName; entity.CategoryType = dto.CategoryType;
            entity.Image = dto.Image; entity.Link = dto.Link; entity.Keywords = dto.Keywords;
            entity.Description = dto.Description; entity.IsShow = dto.IsShow; entity.SiteCode = dto.SiteCode;
            entity.ParentCode = dto.ParentCode; entity.TreeSort = dto.TreeSort; entity.UpdateDate = now;
            await _categoryRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Category { CategoryCode = dto.CategoryName, CategoryName = dto.CategoryName,
                CategoryType = dto.CategoryType ?? "article", Image = dto.Image, Link = dto.Link,
                Keywords = dto.Keywords, Description = dto.Description, IsShow = dto.IsShow ?? "1",
                SiteCode = dto.SiteCode, ParentCode = dto.ParentCode, TreeSort = dto.TreeSort,
                CreateDate = now, UpdateDate = now };
            await _categoryRepository.AddAsync(entity);
        }
        return ApiResult.Ok(CategoryDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string categoryCode)
    {
        var entity = await _categoryRepository.GetAsync(categoryCode);
        if (entity == null) return ApiResult.NotFound("栏目不存在");
        await _categoryRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static List<CategoryDto> BuildTree(List<CategoryDto> nodes, string parentCode)
    {
        var tree = nodes.Where(n => n.ParentCode == parentCode).OrderBy(n => n.TreeSort).ToList();
        foreach (var node in tree)
        {
            var children = BuildTree(nodes, node.CategoryCode);
            if (children.Count > 0)
            {
                node.Children = children;
                node.TreeLeaf = "0";
            }
        }
        return tree;
    }
}
