using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/company")]
[Permission("sys:company")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _service;

    public CompanyController(CompanyService service)
    {
        _service = service;
    }

    [HttpGet("tree")]
    public async Task<ApiResult<List<CompanyDto>>> Tree()
    {
        var list = await _service.TreeAsync();
        return ApiResult<List<CompanyDto>>.Ok(list);
    }

    [HttpGet("{companyCode}")]
    public async Task<ApiResult<CompanyDto>> Get(string companyCode)
    {
        var dto = await _service.GetAsync(companyCode);
        if (dto == null) return ApiResult<CompanyDto>.NotFound("公司不存在");
        return ApiResult<CompanyDto>.Ok(dto);
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] CompanySaveDto dto)
    {
        return await _service.SaveAsync(dto);
    }

    [HttpDelete("{companyCode}")]
    public async Task<ApiResult> Delete(string companyCode)
    {
        return await _service.DeleteAsync(companyCode);
    }
}
