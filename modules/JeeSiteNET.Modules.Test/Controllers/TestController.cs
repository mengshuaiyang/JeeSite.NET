using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Test.Application.DTOs;
using JeeSiteNET.Modules.Test.Application.Services;
using JeeSiteNET.Modules.Test.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Test.Controllers;

[ApiController]
[Route("api/v1/test")]
public class TestController : ControllerBase
{
    private readonly TestService _testService;
    public TestController(TestService testService) => _testService = testService;

    // --- Test Data ---

    [Permission("test:data:list")]
    [HttpGet("data/list")]
    public async Task<ApiResult<List<TestDataDto>>> DataList()
        => ApiResult<List<TestDataDto>>.Ok(await _testService.GetDataListAsync());

    [Permission("test:data:list")]
    [HttpGet("data/get")]
    public async Task<ApiResult<TestDataDto?>> DataGet([FromQuery] string id)
    {
        var dto = await _testService.GetDataAsync(id);
        return dto == null ? ApiResult<TestDataDto?>.NotFound("测试数据不存在") : ApiResult<TestDataDto?>.Ok(dto);
    }

    [Permission("test:data:edit")]
    [HttpPost("data/save")]
    public async Task<ApiResult> DataSave([FromBody] TestData entity) => await _testService.SaveDataAsync(entity);

    [Permission("test:data:delete")]
    [HttpPost("data/delete")]
    public async Task<ApiResult> DataDelete([FromBody] DeleteTestDataRequest request) => await _testService.DeleteDataAsync(request.Id);

    // --- Test Tree ---

    [Permission("test:tree:list")]
    [HttpGet("tree/list")]
    public async Task<ApiResult<List<TestTreeDto>>> TreeList()
        => ApiResult<List<TestTreeDto>>.Ok(await _testService.GetTreeAsync());

    [Permission("test:tree:edit")]
    [HttpPost("tree/save")]
    public async Task<ApiResult> TreeSave([FromBody] TestTree entity) => await _testService.SaveTreeAsync(entity);

    [Permission("test:tree:delete")]
    [HttpPost("tree/delete")]
    public async Task<ApiResult> TreeDelete([FromBody] DeleteTestTreeRequest request) => await _testService.DeleteTreeAsync(request.TreeCode);

    // --- Demo Form ---

    [Permission("test:demo:view")]
    [HttpGet("demo/form-config")]
    public ApiResult<object> DemoFormConfig()
    {
        return ApiResult<object>.Ok(new
        {
            layouts = new[]
            {
                new { label = "基础表单", value = "basic", fields = new[] { "text", "textarea", "select", "radio", "checkbox", "date", "switch" } },
                new { label = "高级表单", value = "advanced", fields = new[] { "text", "number", "slider", "rate", "color", "upload", "editor" } },
                new { label = "搜索表单", value = "search", fields = new[] { "keyword", "dateRange", "select", "treeSelect" } }
            }
        });
    }

    // --- Demo Grid ---

    [Permission("test:demo:view")]
    [HttpGet("demo/grid-data")]
    public ApiResult<PageResult<object>> DemoGridData([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20)
    {
        var all = Enumerable.Range(1, 86).Select(i => new
        {
            id = i,
            name = $"演示数据 {i}",
            category = new[] { "技术", "财务", "人事", "行政" }[i % 4],
            status = i % 3 == 0 ? "已审核" : i % 3 == 1 ? "待审核" : "草稿",
            amount = Math.Round(Random.Shared.NextDouble() * 10000, 2),
            createDate = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"),
            creator = new[] { "张三", "李四", "王五", "赵六" }[i % 4]
        }).ToList<object>();

        var paged = all.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<object>();
        return ApiResult<PageResult<object>>.Ok(new PageResult<object>
        {
            List = paged,
            Total = all.Count,
            PageNo = pageNo,
            PageSize = pageSize
        });
    }
}

public class DeleteTestDataRequest { public string Id { get; set; } = string.Empty; }
public class DeleteTestTreeRequest { public string TreeCode { get; set; } = string.Empty; }
