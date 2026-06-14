    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 ZiggyCreatures.Caching.Fusion 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion
using ZiggyCreatures.Caching.Fusion;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/online")]
[Permission("sys:online:view")]
// 定义class OnlineController
// 定义类：OnlineController

public class OnlineController : ControllerBase
{
    // 字段 _cache
    // 字段：_cache

    private readonly IFusionCache _cache;

    // 方法 OnlineController
    // 构造函数：OnlineController

    public OnlineController(IFusionCache cache)
    {
        _cache = cache;
    }

    [HttpGet("list")]
    // 方法 GetList
    // 方法：GetList

    public async Task<ApiResult<List<OnlineUserDto>>> GetList()
    {
        // 创建 List实例并赋给 users
        var users = new List<OnlineUserDto>();
        var keys = (await _cache.GetOrSetAsync("OnlineKeys", async (ct) => new List<string>(), TimeSpan.FromMinutes(1)))!;
        // foreach 遍历集合
        foreach (var key in keys)
        {
            var token = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{key}");
            // if 条件判断
            if (!string.IsNullOrEmpty(token))
                // 集合操作：添加元素
                users.Add(new OnlineUserDto { UserCode = key });
        }
        // return 返回结果
        return ApiResult<List<OnlineUserDto>>.Ok(users);
    }

    [HttpPost("kick")]
    [Permission("sys:online:edit")]
    // 方法 Kick
    // 方法：Kick

    public async Task<ApiResult> Kick([FromQuery] string userCode)
    {
        var token = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{userCode}");
        // if 条件判断
        if (!string.IsNullOrEmpty(token))
        {
            // await 异步等待
            await _cache.RemoveAsync($"OnlineToken:{userCode}");
            // await 异步等待
            await _cache.SetAsync($"TokenBlacklist:{token}", "kicked", TimeSpan.FromHours(12));
        }
        // return 返回结果
        return ApiResult.Ok();
    }
}

// 定义class OnlineUserDto
// 定义类：OnlineUserDto

public class OnlineUserDto
{
    // 属性 UserCode
    // 属性：UserCode

    public string UserCode { get; set; } = string.Empty;
    // 属性 UserName
    // 属性：UserName

    public string UserName { get; set; } = string.Empty;
    // 属性 LoginCode
    // 属性：LoginCode

    public string LoginCode { get; set; } = string.Empty;
    // 属性 IpAddress
    // 属性：IpAddress

    public string IpAddress { get; set; } = string.Empty;
    // 属性 LoginTime
    // 属性：LoginTime

    public DateTime LoginTime { get; set; }
    // 属性：LastActivity

    public DateTime? LastActivity { get; set; }
}
