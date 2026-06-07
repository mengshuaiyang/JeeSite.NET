using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/employee")]
[Permission("sys:employee")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _service;

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [HttpGet("{empCode}")]
    public async Task<ApiResult<EmployeeDto>> Get(string empCode)
    {
        var dto = await _service.GetAsync(empCode);
        if (dto == null) return ApiResult<EmployeeDto>.NotFound("员工不存在");
        return ApiResult<EmployeeDto>.Ok(dto);
    }

    [HttpPost("page")]
    public async Task<ApiResult<PageResult<EmployeeDto>>> Page([FromBody] PageRequest<Employee> request)
    {
        var page = await _service.FindPageAsync(request);
        return ApiResult<PageResult<EmployeeDto>>.Ok(page);
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] EmployeeSaveDto dto)
    {
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{empCode}")]
    public async Task<ApiResult> Delete(string empCode)
    {
        return await _service.DeleteAsync(empCode);
    }
}
