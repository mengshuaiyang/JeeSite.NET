using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/tenant")]
public class TenantController : ControllerBase
{
    private readonly TenantService _tenantService;

    public TenantController(TenantService tenantService) => _tenantService = tenantService;

    [HttpPost("list")]
    [Permission("sys:tenant:list")]
    public async Task<ApiResult<PageResult<TenantDto>>> List([FromBody] PageRequest<Tenant> request)
    {
        return ApiResult<PageResult<TenantDto>>.Ok(await _tenantService.FindPageAsync(request));
    }

    [HttpGet("get")]
    [Permission("sys:tenant:list")]
    public async Task<ApiResult<TenantDto?>> Get([FromQuery] string tenantCode)
    {
        var entity = await _tenantService.GetAsync(tenantCode);
        return entity == null ? ApiResult<TenantDto?>.NotFound("租户不存在") : ApiResult<TenantDto?>.Ok(entity);
    }

    [HttpPost("save")]
    [Permission("sys:tenant:edit")]
    public async Task<ApiResult> Save([FromBody] TenantSaveDto dto) => await _tenantService.SaveAsync(dto);

    [HttpPost("delete")]
    [Permission("sys:tenant:delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteTenantRequest request) => await _tenantService.DeleteAsync(request.TenantCode);
}

public class DeleteTenantRequest { public string TenantCode { get; set; } = string.Empty; }
