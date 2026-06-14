using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>租户管理接口控制器，负责租户的 CRUD 操作。</summary>
[ApiController]
[Route("api/v1/sys/tenant")]
public class TenantController : ControllerBase
{
    private readonly TenantService _tenantService;

    /// <summary>依赖注入构造函数。</summary>
    public TenantController(TenantService tenantService) => _tenantService = tenantService;

    /// <summary>分页查询租户列表。</summary>
    /// <param name="request">分页查询条件。</param>
    /// <returns>分页结果。</returns>
    [HttpPost("list")]
    [Permission("sys:tenant:list")]
    public async Task<ApiResult<PageResult<TenantDto>>> List([FromBody] PageRequest<Tenant> request)
        => ApiResult<PageResult<TenantDto>>.Ok(await _tenantService.FindPageAsync(request));

    /// <summary>根据租户编码获取详情。</summary>
    /// <param name="tenantCode">租户编码。</param>
    /// <returns>租户详情。</returns>
    [HttpGet("get")]
    [Permission("sys:tenant:list")]
    public async Task<ApiResult<TenantDto?>> Get([FromQuery] string tenantCode)
    {
        var entity = await _tenantService.GetAsync(tenantCode);
        return entity == null ? ApiResult<TenantDto?>.NotFound("租户不存在") : ApiResult<TenantDto?>.Ok(entity);
    }

    /// <summary>新增或更新租户信息。</summary>
    /// <param name="dto">租户保存数据。</param>
    /// <returns>操作结果。</returns>
    [HttpPost("save")]
    [Permission("sys:tenant:edit")]
    public async Task<ApiResult> Save([FromBody] TenantSaveDto dto) => await _tenantService.SaveAsync(dto);

    /// <summary>删除指定租户。</summary>
    /// <param name="request">删除请求。</param>
    /// <returns>操作结果。</returns>
    [HttpPost("delete")]
    [Permission("sys:tenant:delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteTenantRequest request) => await _tenantService.DeleteAsync(request.TenantCode);
}

/// <summary>删除租户请求 DTO。</summary>
public class DeleteTenantRequest { public string TenantCode { get; set; } = string.Empty; }
