    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Search 命名空间
// 引入命名空间：JeeSiteNET.Core.Search
using JeeSiteNET.Core.Search;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/search")]
[Permission("sys:search:view")]
// 定义class SearchController
// 定义类：SearchController

public class SearchController : ControllerBase
{
    // 字段 _searchService
    // 字段：_searchService

    private readonly SearchService _searchService;

    // 构造函数 SearchController
    // 构造函数：SearchController

    public SearchController(SearchService searchService) => _searchService = searchService;

    [HttpPost("query")]
    // 方法 Search
    // 方法：Search

    public async Task<ApiResult<SearchResult<object>>> Search([FromBody] SearchQuery query)
    {
        var result = await _searchService.SearchAsync(query);
        // return 返回结果
        return ApiResult<SearchResult<object>>.Ok(result);
    }

    [Permission("sys:search:admin")]
    [HttpPost("reindex")]
    // 方法 Reindex
    // 方法：Reindex

    public async Task<ApiResult> Reindex()
    {
        // await 异步等待
        await _searchService.ReindexAllAsync();
        // return 返回结果
        return ApiResult.Ok("重建索引完成");
    }

    [HttpGet("ping")]
    // 方法 Ping
    // 方法：Ping

    public async Task<ApiResult<bool>> Ping()
    {
        var ok = await _searchService.PingAsync();
        // return 返回结果
        return ApiResult<bool>.Ok(ok);
    }
}
