using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class BizCategoryService
{
    private readonly IBizCategoryRepository _repository;

    public BizCategoryService(IBizCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BizCategoryDto>> GetTreeAsync()
    {
        var list = await _repository.FindListAsync();
        var dtos = list.Select(BizCategoryDto.FromEntity).ToList();
        return BuildTree(dtos, "0");
    }

    private List<BizCategoryDto> BuildTree(List<BizCategoryDto> all, string parentCode)
    {
        return all.Where(e => e.ParentCode == parentCode).Select(e =>
        {
            e.Children = BuildTree(all, e.CategoryCode);
            return e;
        }).OrderBy(e => e.TreeSort).ToList();
    }

    public async Task<ApiResult> SaveAsync(BizCategory entity)
    {
        var now = DateTime.Now;
        if (!string.IsNullOrEmpty(entity.CategoryCode))
        {
            var existing = await _repository.GetAsync(entity.CategoryCode);
            if (existing == null) return ApiResult.NotFound("业务分类不存在");
            entity.UpdateDate = now;
            await _repository.UpdateAsync(entity);
        }
        else
        {
            entity.CategoryCode = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            await _repository.AddAsync(entity);
        }
        return ApiResult.Ok(BizCategoryDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string categoryCode)
    {
        var entity = await _repository.GetAsync(categoryCode);
        if (entity == null) return ApiResult.NotFound("业务分类不存在");
        await _repository.DeleteAsync(entity);
        return ApiResult.Ok();
    }
}
