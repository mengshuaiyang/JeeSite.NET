using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/online")]
[Permission("sys:online:view")]
public class OnlineController : ControllerBase
{
    private readonly IFusionCache _cache;

    public OnlineController(IFusionCache cache)
    {
        _cache = cache;
    }

    [HttpGet("list")]
    public async Task<ApiResult<List<OnlineUserDto>>> GetList()
    {
        var users = new List<OnlineUserDto>();
        var keys = (await _cache.GetOrSetAsync("OnlineKeys", async (ct) => new List<string>(), TimeSpan.FromMinutes(1)))!;
        foreach (var key in keys)
        {
            var token = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{key}");
            if (!string.IsNullOrEmpty(token))
                users.Add(new OnlineUserDto { UserCode = key });
        }
        return ApiResult<List<OnlineUserDto>>.Ok(users);
    }

    [HttpPost("kick")]
    [Permission("sys:online:edit")]
    public async Task<ApiResult> Kick([FromQuery] string userCode)
    {
        var token = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{userCode}");
        if (!string.IsNullOrEmpty(token))
        {
            await _cache.RemoveAsync($"OnlineToken:{userCode}");
            await _cache.SetAsync($"TokenBlacklist:{token}", "kicked", TimeSpan.FromHours(12));
        }
        return ApiResult.Ok();
    }
}

public class OnlineUserDto
{
    public string UserCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string LoginCode { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime LoginTime { get; set; }
    public DateTime? LastActivity { get; set; }
}
