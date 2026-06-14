    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Test.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Application.DTOs
using JeeSiteNET.Modules.Test.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Test.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Application.Services
using JeeSiteNET.Modules.Test.Application.Services;
    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Test.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Controllers
namespace JeeSiteNET.Modules.Test.Controllers;

[ApiController]
[Route("api/v1/test")]
// 定义class TestController
// 定义类：TestController

public class TestController : ControllerBase
{
    // 字段 _testService
    // 字段：_testService

    private readonly TestService _testService;
    // 构造函数 TestController
    // 构造函数：TestController

    public TestController(TestService testService) => _testService = testService;

    // --- Test Data ---

    [Permission("test:data:list")]
    [HttpGet("data/list")]
    // 方法 DataList
    // 方法：DataList

    public async Task<ApiResult<List<TestDataDto>>> DataList()
        => ApiResult<List<TestDataDto>>.Ok(await _testService.GetDataListAsync());

    [Permission("test:data:list")]
    [HttpGet("data/get")]
    // 方法 DataGet
    // 方法：DataGet

    public async Task<ApiResult<TestDataDto?>> DataGet([FromQuery] string id)
    {
        var dto = await _testService.GetDataAsync(id);
        // return 返回结果
        return dto == null ? ApiResult<TestDataDto?>.NotFound("测试数据不存在") : ApiResult<TestDataDto?>.Ok(dto);
    }

    [Permission("test:data:edit")]
    [HttpPost("data/save")]
    // 方法：DataSave

    public async Task<ApiResult> DataSave([FromBody] TestData entity) => await _testService.SaveDataAsync(entity);

    [Permission("test:data:delete")]
    [HttpPost("data/delete")]
    // 方法：DataDelete

    public async Task<ApiResult> DataDelete([FromBody] DeleteTestDataRequest request) => await _testService.DeleteDataAsync(request.Id);

    // --- Test Tree ---

    [Permission("test:tree:list")]
    [HttpGet("tree/list")]
    // 方法 TreeList
    // 方法：TreeList

    public async Task<ApiResult<List<TestTreeDto>>> TreeList()
        => ApiResult<List<TestTreeDto>>.Ok(await _testService.GetTreeAsync());

    [Permission("test:tree:edit")]
    [HttpPost("tree/save")]
    // 方法：TreeSave

    public async Task<ApiResult> TreeSave([FromBody] TestTree entity) => await _testService.SaveTreeAsync(entity);

    [Permission("test:tree:delete")]
    [HttpPost("tree/delete")]
    // 方法：TreeDelete

    public async Task<ApiResult> TreeDelete([FromBody] DeleteTestTreeRequest request) => await _testService.DeleteTreeAsync(request.TreeCode);

    // --- Demo Form ---

    [Permission("test:demo:view")]
    [HttpGet("demo/form-config")]
    // 方法 DemoFormConfig
    // 方法：DemoFormConfig

    public ApiResult<object> DemoFormConfig()
    {
        // return 返回结果
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
    // 方法 DemoGridData
    // 方法：DemoGridData

    public ApiResult<PageResult<object>> DemoGridData([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20)
    {
        // 数据库操作：投影选择
        var all = Enumerable.Range(1, 86).Select(i => new
        {
            id = i,
            name = $"演示数据 {i}",
            category = new[] { "技术", "财务", "人事", "行政" }[i % 4],
            status = i % 3 == 0 ? "已审核" : i % 3 == 1 ? "待审核" : "草稿",
            amount = Math.Round(Random.Shared.NextDouble() * 10000, 2),
            // 调用 ToString
            createDate = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"),
            creator = new[] { "张三", "李四", "王五", "赵六" }[i % 4]
        }).ToList<object>();

        // 调用 Skip
        var paged = all.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<object>();
        // return 返回结果
        return ApiResult<PageResult<object>>.Ok(new PageResult<object>
        {
            List = paged,
            Total = all.Count,
            PageNo = pageNo,
            PageSize = pageSize
        });
    }
}

// 定义class DeleteTestDataRequest
// 定义类：DeleteTestDataRequest

public class DeleteTestDataRequest { public string Id { get; set; } = string.Empty; }
// 定义class DeleteTestTreeRequest
// 定义类：DeleteTestTreeRequest

public class DeleteTestTreeRequest { public string TreeCode { get; set; } = string.Empty; }
