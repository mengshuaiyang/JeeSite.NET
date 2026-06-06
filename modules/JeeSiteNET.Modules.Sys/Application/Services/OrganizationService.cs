using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class OrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<OrganizationDto?> GetAsync(string orgCode)
    {
        var org = await _organizationRepository.GetAsync(orgCode);
        return org == null ? null : MapToDto(org);
    }

    public async Task<List<OrganizationDto>> FindTreeAsync(string? orgType = null)
    {
        var query = _organizationRepository.Query().Where(o => o.Status == "0");
        if (!string.IsNullOrEmpty(orgType))
            query = query.Where(o => o.OrgType == orgType);

        var list = await query.OrderBy(o => o.TreeSort).ToListAsync();
        return BuildTree(list, "0");
    }

    public async Task<ApiResult> SaveAsync(OrganizationSaveDto dto)
    {
        var now = DateTime.Now;
        Organization? org;

        if (!string.IsNullOrEmpty(dto.OrgCode))
        {
            org = await _organizationRepository.GetAsync(dto.OrgCode);
            if (org == null) return ApiResult.NotFound("机构不存在");
            org.OrgName = dto.OrgName;
            org.ViewCode = dto.ViewCode;
            org.FullName = dto.FullName;
            org.OrgType = dto.OrgType;
            org.Leader = dto.Leader;
            org.Phone = dto.Phone;
            org.ParentCode = dto.ParentCode;
            org.TreeSort = dto.TreeSort;
            org.UpdateDate = now;
            await _organizationRepository.UpdateAsync(org);
        }
        else
        {
            org = new Organization
            {
                OrgCode = IdGenerator.NewId(),
                OrgName = dto.OrgName,
                ViewCode = dto.ViewCode,
                FullName = dto.FullName,
                OrgType = dto.OrgType,
                Leader = dto.Leader,
                Phone = dto.Phone,
                ParentCode = dto.ParentCode,
                TreeSort = dto.TreeSort,
                CreateDate = now,
                UpdateDate = now
            };
            await _organizationRepository.AddAsync(org);
        }

        return ApiResult.Ok(MapToDto(org));
    }

    public async Task<ApiResult> DeleteAsync(string orgCode)
    {
        var org = await _organizationRepository.GetAsync(orgCode);
        if (org == null) return ApiResult.NotFound("机构不存在");
        await _organizationRepository.DeleteAsync(org);
        return ApiResult.Ok();
    }

    private static OrganizationDto MapToDto(Organization org) => new()
    {
        OrgCode = org.OrgCode,
        OrgName = org.OrgName,
        ViewCode = org.ViewCode,
        FullName = org.FullName,
        OrgType = org.OrgType,
        Leader = org.Leader,
        Phone = org.Phone,
        ParentCode = org.ParentCode,
        ParentCodes = org.ParentCodes,
        TreeSort = org.TreeSort,
        TreeNames = org.TreeNames,
        TreeLevel = org.TreeLevel,
        TreeLeaf = org.TreeLeaf,
        Status = org.Status
    };

    private static List<OrganizationDto> BuildTree(List<Organization> allOrgs, string parentCode)
    {
        return allOrgs
            .Where(o => o.ParentCode == parentCode)
            .OrderBy(o => o.TreeSort)
            .Select(o =>
            {
                var dto = MapToDto(o);
                dto.Children = BuildTree(allOrgs, o.OrgCode);
                return dto;
            })
            .ToList();
    }
}
