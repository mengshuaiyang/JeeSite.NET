using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/dashboard")]
[Permission("dashboard")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    public async Task<ApiResult<DashboardStats>> GetStats()
    {
        var stats = await _dashboardService.GetStatsAsync();
        return ApiResult<DashboardStats>.Ok(stats);
    }
}
