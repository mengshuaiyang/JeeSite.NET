using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;
    public CategoryService(ICategoryRepository repository) => _repository = repository;

    public async Task<List<Application.DTOs.CategoryDto>> GetBySiteAsync(string siteCode)
    {
        var list = await _repository.Query().Where(c => c.SiteCode == siteCode && c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<List<Application.DTOs.CategoryDto>> GetAllAsync()
    {
        var list = await _repository.Query().Where(c => c.Status == "0").OrderBy(c => c.TreeSort).ToListAsync();
        return list.Select(MapToDto).ToList();
    }

    private static Application.DTOs.CategoryDto MapToDto(Category e) => new()
    {
        CategoryCode = e.CategoryCode,
        CategoryName = e.CategoryName,
        CategoryType = e.CategoryType,
        Image = e.Image,
        Link = e.Link,
        Keywords = e.Keywords,
        Description = e.Description,
        IsShow = e.IsShow,
        SiteCode = e.SiteCode,
        ParentCode = e.ParentCode,
        TreeSort = e.TreeSort,
        TreeLeaf = e.TreeLeaf,
        TreeLevel = e.TreeLevel,
        Status = e.Status
    };
}
