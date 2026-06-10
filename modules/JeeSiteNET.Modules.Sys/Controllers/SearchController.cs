using JeeSiteNET.Core;
using JeeSiteNET.Core.Search;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/search")]
[Permission("sys:search:view")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService) => _searchService = searchService;

    [HttpPost("query")]
    public async Task<ApiResult<SearchResult<object>>> Search([FromBody] SearchQuery query)
    {
        var result = await _searchService.SearchAsync(query);
        return ApiResult<SearchResult<object>>.Ok(result);
    }

    [Permission("sys:search:admin")]
    [HttpPost("reindex")]
    public async Task<ApiResult> Reindex()
    {
        await _searchService.ReindexAllAsync();
        return ApiResult.Ok("重建索引完成");
    }

    [HttpGet("ping")]
    public async Task<ApiResult<bool>> Ping()
    {
        var ok = await _searchService.PingAsync();
        return ApiResult<bool>.Ok(ok);
    }
}
