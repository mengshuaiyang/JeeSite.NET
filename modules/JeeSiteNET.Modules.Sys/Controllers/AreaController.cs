using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/area")]
[Permission("sys:area")]
public class AreaController : ControllerBase
{
    private readonly AreaService _service;

    public AreaController(AreaService service)
    {
        _service = service;
    }

    [HttpGet("tree")]
    public async Task<ApiResult<List<AreaDto>>> Tree()
    {
        var list = await _service.TreeAsync();
        return ApiResult<List<AreaDto>>.Ok(list);
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] Area dto)
    {
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{areaCode}")]
    public async Task<ApiResult> Delete(string areaCode)
    {
        return await _service.DeleteAsync(areaCode);
    }
}
