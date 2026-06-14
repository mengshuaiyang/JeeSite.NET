    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
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
[Route("api/v1/sys/dashboard")]
[Permission("dashboard")]
// 定义class DashboardController
// 定义类：DashboardController

public class DashboardController : ControllerBase
{
    // 字段 _dashboardService
    // 字段：_dashboardService

    private readonly DashboardService _dashboardService;

    // 方法 DashboardController
    // 构造函数：DashboardController

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    // 方法 GetStats
    // 方法：GetStats

    public async Task<ApiResult<DashboardStats>> GetStats()
    {
        var stats = await _dashboardService.GetStatsAsync();
        // return 返回结果
        return ApiResult<DashboardStats>.Ok(stats);
    }
}
