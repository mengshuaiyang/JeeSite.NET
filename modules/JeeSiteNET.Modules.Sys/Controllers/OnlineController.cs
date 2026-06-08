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
        return ApiResult<List<OnlineUserDto>>.Ok(new List<OnlineUserDto>());
    }

    [HttpPost("kick")]
    [Permission("sys:online:edit")]
    public async Task<ApiResult> Kick([FromQuery] string userCode)
    {
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
