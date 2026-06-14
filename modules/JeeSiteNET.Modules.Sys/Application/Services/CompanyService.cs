using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>公司管理服务，负责公司树、公司信息与机构关联维护。</summary>
public class CompanyService
{
    private readonly ICompanyRepository _repo;
    private readonly JeeSiteDbContext _db;

    /// <summary>依赖注入构造函数。</summary>
    public CompanyService(ICompanyRepository repo, JeeSiteDbContext db)
    {
        _repo = repo;
        _db = db;
    }

    /// <summary>按树形排序获取全部公司列表，并附加关联机构编码。</summary>
    /// <returns>公司 DTO 列表（已携带 OfficeCodes）。</returns>
    public async Task<List<CompanyDto>> TreeAsync()
    {
        var list = await _repo.Query().OrderBy(c => c.TreeSorts).ToListAsync();
        // 一次性读取所有公司-机构关系，按 CompanyCode 聚合，避免 N+1 查询
        var officesByCompany = (await _db.Set<CompanyOffice>().ToListAsync())
            .GroupBy(o => o.CompanyCode)
            .ToDictionary(g => g.Key, g => g.Select(x => x.OfficeCode).ToList());

        return list.Select(c => c.ToDto(officesByCompany.GetValueOrDefault(c.CompanyCode, new()))).ToList();
    }

    /// <summary>根据公司编码获取单个公司及其关联机构。</summary>
    /// <param name="companyCode">公司编码。</param>
    /// <returns>公司 DTO，不存在时返回 null。</returns>
    public async Task<CompanyDto?> GetAsync(string companyCode)
    {
        var c = await _repo.GetAsync(companyCode);
        if (c == null) return null;
        var offices = await _repo.GetOfficesAsync(companyCode);
        return c.ToDto(offices.Select(o => o.OfficeCode).ToList());
    }

    /// <summary>保存公司（带 CompanyCode 更新，不带则新增）并维护关联机构列表。</summary>
    /// <param name="dto">公司保存信息，包含 OfficeCodes 数组。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SaveAsync(CompanySaveDto dto)
    {
        var now = DateTime.Now;
        Company? c;

        if (!string.IsNullOrEmpty(dto.CompanyCode))
        {
            c = await _repo.GetAsync(dto.CompanyCode);
            if (c == null) return ApiResult.NotFound("公司不存在");
            c.ViewCode = dto.ViewCode;
            c.CompanyName = dto.CompanyName;
            c.FullName = dto.FullName;
            c.AreaCode = dto.AreaCode;
            c.ParentCode = dto.ParentCode ?? "0";
            c.Remarks = dto.Remarks;
            c.UpdateDate = now;
            await _repo.UpdateAsync(c);
        }
        else
        {
            c = dto.ToEntity();
            await _repo.AddAsync(c);
        }

        // 先清除旧的公司-机构关联，再重新写入当前提交的列表，保证最终一致
        await _repo.DeleteOfficesAsync(c.CompanyCode);
        if (dto.OfficeCodes != null)
        {
            foreach (var oc in dto.OfficeCodes.Where(x => !string.IsNullOrEmpty(x)))
            {
                await _repo.AddOfficeAsync(new CompanyOffice
                {
                    Id = IdGenerator.NewId(),
                    CompanyCode = c.CompanyCode,
                    OfficeCode = oc
                });
            }
        }

        await _repo.SaveChangesAsync();
        return ApiResult.Ok();
    }

    /// <summary>删除公司（按编码级联删除关联信息）。</summary>
    /// <param name="companyCode">公司编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string companyCode)
    {
        var c = await _repo.GetAsync(companyCode);
        if (c == null) return ApiResult.NotFound("公司不存在");
        await _repo.DeleteAsync(companyCode);
        await _repo.SaveChangesAsync();
        return ApiResult.Ok();
    }
}
