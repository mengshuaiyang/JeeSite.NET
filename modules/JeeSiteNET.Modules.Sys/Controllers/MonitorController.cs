using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/monitor")]
[Permission("sys:monitor:view")]
public class MonitorController : ControllerBase
{
    private readonly MonitorService _monitorService;

    public MonitorController(MonitorService monitorService)
    {
        _monitorService = monitorService;
    }

    [HttpGet("server")]
    public ApiResult<ServerInfo> GetServerInfo()
    {
        return ApiResult<ServerInfo>.Ok(_monitorService.GetServerInfo());
    }
}
