using JeeSiteNET.Core;
using JeeSiteNET.Modules.Test.Application.DTOs;
using JeeSiteNET.Modules.Test.Domain.Entities;
using JeeSiteNET.Modules.Test.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Test.Application.Services;

public class TestService
{
    private readonly ITestDataRepository _testDataRepository;
    private readonly ITestTreeRepository _testTreeRepository;

    public TestService(ITestDataRepository testDataRepository, ITestTreeRepository testTreeRepository)
    {
        _testDataRepository = testDataRepository;
        _testTreeRepository = testTreeRepository;
    }

    // --- TestData ---

    public async Task<List<TestDataDto>> GetDataListAsync()
    {
        var list = await _testDataRepository.FindListAsync();
        return list.Select(TestDataDto.FromEntity).ToList();
    }

    public async Task<TestDataDto?> GetDataAsync(string id)
    {
        var entity = await _testDataRepository.GetAsync(id);
        return entity == null ? null : TestDataDto.FromEntity(entity);
    }

    public async Task<ApiResult> SaveDataAsync(TestData entity)
    {
        var now = DateTime.Now;
        if (!string.IsNullOrEmpty(entity.Id))
        {
            var existing = await _testDataRepository.GetAsync(entity.Id);
            if (existing == null) return ApiResult.NotFound("测试数据不存在");
            entity.UpdateDate = now;
            entity.CreateBy = existing.CreateBy;
            entity.CreateDate = existing.CreateDate;
            await _testDataRepository.UpdateAsync(entity);
        }
        else
        {
            entity.Id = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            await _testDataRepository.AddAsync(entity);
        }
        return ApiResult.Ok(TestDataDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteDataAsync(string id)
    {
        var entity = await _testDataRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("测试数据不存在");
        await _testDataRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- TestTree ---

    public async Task<List<TestTreeDto>> GetTreeAsync()
    {
        var list = await _testTreeRepository.FindListAsync();
        var dtos = list.Select(TestTreeDto.FromEntity).ToList();
        return BuildTree(dtos, "0");
    }

    private List<TestTreeDto> BuildTree(List<TestTreeDto> all, string parentCode)
    {
        return all.Where(e => e.ParentCode == parentCode).Select(e =>
        {
            e.Children = BuildTree(all, e.TreeCode);
            return e;
        }).OrderBy(e => e.TreeSort).ToList();
    }

    public async Task<ApiResult> SaveTreeAsync(TestTree entity)
    {
        var now = DateTime.Now;
        if (!string.IsNullOrEmpty(entity.TreeCode))
        {
            var existing = await _testTreeRepository.GetAsync(entity.TreeCode);
            if (existing == null) return ApiResult.NotFound("测试树不存在");
            entity.UpdateDate = now;
            await _testTreeRepository.UpdateAsync(entity);
        }
        else
        {
            entity.TreeCode = Guid.NewGuid().ToString("N")[..20];
            entity.CreateDate = now;
            entity.UpdateDate = now;
            entity.Status = "0";
            await _testTreeRepository.AddAsync(entity);
        }
        return ApiResult.Ok(TestTreeDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteTreeAsync(string treeCode)
    {
        var entity = await _testTreeRepository.GetAsync(treeCode);
        if (entity == null) return ApiResult.NotFound("测试树不存在");
        await _testTreeRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }
}
