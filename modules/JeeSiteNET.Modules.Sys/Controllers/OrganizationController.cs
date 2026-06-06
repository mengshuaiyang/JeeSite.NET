using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/org")]
public class OrganizationController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationController(OrganizationService organizationService) => _organizationService = organizationService;

    [Permission("sys:org:list")]
    [HttpGet("tree")]
    public async Task<ApiResult<List<OrganizationDto>>> Tree([FromQuery] string? orgType = null)
    {
        var tree = await _organizationService.FindTreeAsync(orgType);
        return ApiResult<List<OrganizationDto>>.Ok(tree);
    }

    [Permission("sys:org:list")]
    [HttpGet("get")]
    public async Task<ApiResult<OrganizationDto?>> Get([FromQuery] string orgCode)
    {
        var org = await _organizationService.GetAsync(orgCode);
        if (org == null) return ApiResult<OrganizationDto?>.NotFound("机构不存在");
        return ApiResult<OrganizationDto?>.Ok(org);
    }

    [Permission("sys:org:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] OrganizationSaveDto dto)
    {
        return await _organizationService.SaveAsync(dto);
    }

    [Permission("sys:org:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteOrgRequest request)
    {
        return await _organizationService.DeleteAsync(request.OrgCode);
    }
}

public class DeleteOrgRequest
{
    public string OrgCode { get; set; } = string.Empty;
}
