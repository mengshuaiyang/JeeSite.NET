using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Web.Api.Controllers;

/// <summary>
/// 健康检查控制器。
/// 用于负载均衡、容器探活等场景，返回健康状态与当前时间。
/// </summary>
[ApiController]
[Route("api/v1/health")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    /// <summary>
    /// 返回健康检查响应。
    /// </summary>
    /// <returns>包含健康状态对象与 HTTP 200 OK。</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.Now });
    }
}
