    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class BizCategoryService
// 定义类：BizCategoryService
public class BizCategoryService
{
    // 字段 _repository
    // 字段：_repository
    private readonly IBizCategoryRepository _repository;

    // 方法 BizCategoryService
    // 构造函数：BizCategoryService
    public BizCategoryService(IBizCategoryRepository repository)
    {
        _repository = repository;
    }

    // 方法 GetTreeAsync
    // 方法：GetTreeAsync
    public async Task<List<BizCategoryDto>> GetTreeAsync()
    {
        var list = await _repository.FindListAsync();
        // 数据库操作：投影选择
        var dtos = list.Select(BizCategoryDto.FromEntity).ToList();
        // return 返回结果
        return BuildTree(dtos, "0");
    }

    // 方法 BuildTree
    // 方法：BuildTree
    private List<BizCategoryDto> BuildTree(List<BizCategoryDto> all, string parentCode)
    {
        // return 返回结果
        return all.Where(e => e.ParentCode == parentCode).Select(e =>
        {
            e.Children = BuildTree(all, e.CategoryCode);
            // return 返回结果
            return e;
        // 数据库操作：升序排序
        }).OrderBy(e => e.TreeSort).ToList();
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(BizCategory entity)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // if 条件判断
        if (!string.IsNullOrEmpty(entity.CategoryCode))
        {
            // 缓存：获取值
            var existing = await _repository.GetAsync(entity.CategoryCode);
            // if 条件判断
            if (existing == null) return ApiResult.NotFound("业务分类不存在");
            entity.UpdateDate = now;
            // await 异步等待
            await _repository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 调用 ToString
            entity.CategoryCode = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            // await 异步等待
            await _repository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(BizCategoryDto.FromEntity(entity));
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task<ApiResult> DeleteAsync(string categoryCode)
    {
        // 缓存：获取值
        var entity = await _repository.GetAsync(categoryCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("业务分类不存在");
        // await 异步等待
        await _repository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }
}
