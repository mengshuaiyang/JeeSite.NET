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
}

public class DeleteTestDataRequest { public string Id { get; set; } = string.Empty; }
public class DeleteTestTreeRequest { public string TreeCode { get; set; } = string.Empty; }
