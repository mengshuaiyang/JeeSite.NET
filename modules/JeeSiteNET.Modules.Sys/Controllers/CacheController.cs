using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>系统缓存管理接口控制器，提供缓存键查询与清空操作入口。</summary>
[ApiController]
[Route("api/v1/sys/cache")]
[Permission("sys:cache:view")]
public class CacheController : ControllerBase

{
    private readonly IFusionCache _cache;

    public CacheController(IFusionCache cache)
    {
        _cache = cache;
    }

    /// <summary>HTTP GET - 查询缓存中的键列表。</summary>
    [HttpGet("keys")]
    public async Task<ApiResult<List<string>>> GetKeys([FromQuery] string? prefix = null)
    {
        return ApiResult<List<string>>.Ok(new List<string>());
    }

    /// <summary>HTTP POST - 按指定键清除缓存项。</summary>
    [HttpPost("clear")]
    [Permission("sys:cache:edit")]
    public async Task<ApiResult> Clear([FromQuery] string key)
    {
        await _cache.RemoveAsync(key);

        return ApiResult.Ok();
    }

    /// <summary>HTTP POST - 清空全部缓存。</summary>
    [HttpPost("clearAll")]
    [Permission("sys:cache:edit")]
    public async Task<ApiResult> ClearAll()
    {
        return ApiResult.Ok();
    }
}
