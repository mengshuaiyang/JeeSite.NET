using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>机构（组织）管理服务，负责机构树查询、保存与删除。</summary>
public class OrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>依赖注入构造函数。</summary>
    public OrganizationService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    /// <summary>根据机构编码获取机构 DTO。</summary>
    /// <param name="orgCode">机构编码。</param>
    /// <returns>机构 DTO，不存在时返回 null。</returns>
    public async Task<OrganizationDto?> GetAsync(string orgCode)
    {
        var org = await _organizationRepository.GetAsync(orgCode);
        return org == null ? null : MapToDto(org);
    }

    /// <summary>按条件查询机构树（可按机构类型过滤）。</summary>
    /// <param name="orgType">机构类型过滤，为空返回全部。</param>
    /// <returns>机构树 DTO 列表。</returns>
    public async Task<List<OrganizationDto>> FindTreeAsync(string? orgType = null)
    {
        var query = _organizationRepository.Query().Where(o => o.Status == "0");
        if (!string.IsNullOrEmpty(orgType))
            query = query.Where(o => o.OrgType == orgType);

        var list = await query.OrderBy(o => o.TreeSort).ToListAsync();
        return BuildTree(list, "0");
    }

    /// <summary>新增或更新机构。</summary>
    /// <param name="dto">机构保存信息。</param>
    /// <returns>保存后的机构 DTO。</returns>
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

    /// <summary>删除机构。</summary>
    /// <param name="orgCode">机构编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string orgCode)
    {
        var org = await _organizationRepository.GetAsync(orgCode);
        if (org == null) return ApiResult.NotFound("机构不存在");
        await _organizationRepository.DeleteAsync(org);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
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

    /// <summary>根据父子关系递归构建机构树。</summary>
    /// <param name="allOrgs">扁平机构列表。</param>
    /// <param name="parentCode">起始父机构编码（根为 "0"）。</param>
    /// <returns>机构树 DTO 列表。</returns>
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
