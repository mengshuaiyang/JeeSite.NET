using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

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

    [HttpGet("keys")]
    public async Task<ApiResult<List<string>>> GetKeys([FromQuery] string? prefix = null)
    {
        return ApiResult<List<string>>.Ok(new List<string>());
    }

    [HttpPost("clear")]
    [Permission("sys:cache:edit")]
    public async Task<ApiResult> Clear([FromQuery] string key)
    {
        await _cache.RemoveAsync(key);
        return ApiResult.Ok();
    }

    [HttpPost("clearAll")]
    [Permission("sys:cache:edit")]
    public async Task<ApiResult> ClearAll()
    {
        return ApiResult.Ok();
    }
}
