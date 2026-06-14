using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>服务器监控接口控制器，提供 CPU、内存、磁盘等服务器运行信息查询。</summary>
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

    /// <summary>HTTP GET - 获取服务器 CPU、内存、磁盘等运行状态信息。</summary>
    [HttpGet("server")]
    public ApiResult<ServerInfo> GetServerInfo()
    {
        return ApiResult<ServerInfo>.Ok(_monitorService.GetServerInfo());
    }
}
