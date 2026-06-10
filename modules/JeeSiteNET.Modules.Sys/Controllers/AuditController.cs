using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/audit")]
[Permission("sys:audit:view")]
public class AuditController : ControllerBase
{
    private readonly AuditService _auditService;

    public AuditController(AuditService auditService) => _auditService = auditService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<Audit>>> List([FromBody] PageRequest<Audit> request)
    {
        var result = await _auditService.FindPageAsync(request);
        return ApiResult<PageResult<Audit>>.Ok(result);
    }

    [HttpGet("list")]
    public async Task<ApiResult<PageResult<Audit>>> GetList([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? auditType = null, [FromQuery] string? loginCode = null)
    {
        var request = new PageRequest<Audit>
        {
            PageNo = pageNo,
            PageSize = pageSize,
            Entity = new Audit { AuditType = auditType, LoginCode = loginCode }
        };
        var result = await _auditService.FindPageAsync(request);
        return ApiResult<PageResult<Audit>>.Ok(result);
    }
}
