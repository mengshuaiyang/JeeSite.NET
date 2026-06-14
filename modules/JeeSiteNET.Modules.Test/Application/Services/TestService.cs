    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Test.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Application.DTOs
using JeeSiteNET.Modules.Test.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Test.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Interfaces
using JeeSiteNET.Modules.Test.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Test.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Application.Services
namespace JeeSiteNET.Modules.Test.Application.Services;

// 定义class TestService
// 定义类：TestService
public class TestService
{
    // 字段 _testDataRepository
    // 字段：_testDataRepository
    private readonly ITestDataRepository _testDataRepository;
    // 字段 _testTreeRepository
    // 字段：_testTreeRepository
    private readonly ITestTreeRepository _testTreeRepository;

    // 方法 TestService
    // 构造函数：TestService
    public TestService(ITestDataRepository testDataRepository, ITestTreeRepository testTreeRepository)
    {
        _testDataRepository = testDataRepository;
        _testTreeRepository = testTreeRepository;
    }

    // --- TestData ---

    // 方法 GetDataListAsync
    // 方法：GetDataListAsync
    public async Task<List<TestDataDto>> GetDataListAsync()
    {
        var list = await _testDataRepository.FindListAsync();
        // return 返回结果
        return list.Select(TestDataDto.FromEntity).ToList();
    }

    // 方法 GetDataAsync
    // 方法：GetDataAsync
    public async Task<TestDataDto?> GetDataAsync(string id)
    {
        // 缓存：获取值
        var entity = await _testDataRepository.GetAsync(id);
        // return 返回结果
        return entity == null ? null : TestDataDto.FromEntity(entity);
    }

    // 方法 SaveDataAsync
    // 方法：SaveDataAsync
    public async Task<ApiResult> SaveDataAsync(TestData entity)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // if 条件判断
        if (!string.IsNullOrEmpty(entity.Id))
        {
            // 缓存：获取值
            var existing = await _testDataRepository.GetAsync(entity.Id);
            // if 条件判断
            if (existing == null) return ApiResult.NotFound("测试数据不存在");
            entity.UpdateDate = now;
            entity.CreateBy = existing.CreateBy;
            entity.CreateDate = existing.CreateDate;
            // await 异步等待
            await _testDataRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 调用 ToString
            entity.Id = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            // await 异步等待
            await _testDataRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(TestDataDto.FromEntity(entity));
    }

    // 方法 DeleteDataAsync
    // 方法：DeleteDataAsync
    public async Task<ApiResult> DeleteDataAsync(string id)
    {
        // 缓存：获取值
        var entity = await _testDataRepository.GetAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("测试数据不存在");
        // await 异步等待
        await _testDataRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- TestTree ---

    // 方法 GetTreeAsync
    // 方法：GetTreeAsync
    public async Task<List<TestTreeDto>> GetTreeAsync()
    {
        var list = await _testTreeRepository.FindListAsync();
        // 数据库操作：投影选择
        var dtos = list.Select(TestTreeDto.FromEntity).ToList();
        // return 返回结果
        return BuildTree(dtos, "0");
    }

    // 方法 BuildTree
    // 方法：BuildTree
    private List<TestTreeDto> BuildTree(List<TestTreeDto> all, string parentCode)
    {
        // return 返回结果
        return all.Where(e => e.ParentCode == parentCode).Select(e =>
        {
            e.Children = BuildTree(all, e.TreeCode);
            // return 返回结果
            return e;
        // 数据库操作：升序排序
        }).OrderBy(e => e.TreeSort).ToList();
    }

    // 方法 SaveTreeAsync
    // 方法：SaveTreeAsync
    public async Task<ApiResult> SaveTreeAsync(TestTree entity)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // if 条件判断
        if (!string.IsNullOrEmpty(entity.TreeCode))
        {
            // 缓存：获取值
            var existing = await _testTreeRepository.GetAsync(entity.TreeCode);
            // if 条件判断
            if (existing == null) return ApiResult.NotFound("测试树不存在");
            entity.UpdateDate = now;
            // await 异步等待
            await _testTreeRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 调用 ToString
            entity.TreeCode = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            // await 异步等待
            await _testTreeRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(TestTreeDto.FromEntity(entity));
    }

    // 方法 DeleteTreeAsync
    // 方法：DeleteTreeAsync
    public async Task<ApiResult> DeleteTreeAsync(string treeCode)
    {
        // 缓存：获取值
        var entity = await _testTreeRepository.GetAsync(treeCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("测试树不存在");
        // await 异步等待
        await _testTreeRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }
}
