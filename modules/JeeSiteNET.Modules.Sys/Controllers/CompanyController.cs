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
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/company")]
[Permission("sys:company")]
// 定义class CompanyController
// 定义类：CompanyController

public class CompanyController : ControllerBase
{
    // 字段 _service
    // 字段：_service

    private readonly CompanyService _service;

    // 方法 CompanyController
    // 构造函数：CompanyController

    public CompanyController(CompanyService service)
    {
        _service = service;
    }

    [HttpGet("tree")]
    // 方法 Tree
    // 方法：Tree

    public async Task<ApiResult<List<CompanyDto>>> Tree()
    {
        var list = await _service.TreeAsync();
        // return 返回结果
        return ApiResult<List<CompanyDto>>.Ok(list);
    }

    [HttpGet("{companyCode}")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<CompanyDto>> Get(string companyCode)
    {
        // 缓存：获取值
        var dto = await _service.GetAsync(companyCode);
        // if 条件判断
        if (dto == null) return ApiResult<CompanyDto>.NotFound("公司不存在");
        // return 返回结果
        return ApiResult<CompanyDto>.Ok(dto);
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] CompanySaveDto dto)
    {
        // return 返回结果
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{companyCode}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string companyCode)
    {
        // return 返回结果
        return await _service.DeleteAsync(companyCode);
    }
}
