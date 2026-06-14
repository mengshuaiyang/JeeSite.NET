    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/employee")]
[Permission("sys:employee")]
// 定义class EmployeeController
// 定义类：EmployeeController

public class EmployeeController : ControllerBase
{
    // 字段 _service
    // 字段：_service

    private readonly EmployeeService _service;

    // 方法 EmployeeController
    // 构造函数：EmployeeController

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [HttpGet("{empCode}")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<EmployeeDto>> Get(string empCode)
    {
        // 缓存：获取值
        var dto = await _service.GetAsync(empCode);
        // if 条件判断
        if (dto == null) return ApiResult<EmployeeDto>.NotFound("员工不存在");
        // return 返回结果
        return ApiResult<EmployeeDto>.Ok(dto);
    }

    [HttpPost("page")]
    // 方法 Page
    // 方法：Page

    public async Task<ApiResult<PageResult<EmployeeDto>>> Page([FromBody] PageRequest<Employee> request)
    {
        var page = await _service.FindPageAsync(request);
        // return 返回结果
        return ApiResult<PageResult<EmployeeDto>>.Ok(page);
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] EmployeeSaveDto dto)
    {
        // return 返回结果
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{empCode}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string empCode)
    {
        // return 返回结果
        return await _service.DeleteAsync(empCode);
    }
}
