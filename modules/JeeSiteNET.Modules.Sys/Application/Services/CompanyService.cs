using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class CompanyService
{
    private readonly ICompanyRepository _repo;
    private readonly DbContext _db;

    public CompanyService(ICompanyRepository repo, DbContext db)
    {
        _repo = repo;
        _db = db;
    }

    public async Task<List<CompanyDto>> TreeAsync()
    {
        var list = await _repo.Query().OrderBy(c => c.TreeSorts).ToListAsync();
        var officesByCompany = (await _db.Set<CompanyOffice>().ToListAsync())
            .GroupBy(o => o.CompanyCode)
            .ToDictionary(g => g.Key, g => g.Select(x => x.OfficeCode).ToList());

        return list.Select(c => c.ToDto(officesByCompany.GetValueOrDefault(c.CompanyCode, new()))).ToList();
    }

    public async Task<CompanyDto?> GetAsync(string companyCode)
    {
        var c = await _repo.GetAsync(companyCode);
        if (c == null) return null;
        var offices = await _repo.GetOfficesAsync(companyCode);
        return c.ToDto(offices.Select(o => o.OfficeCode).ToList());
    }

    public async Task<ApiResult> SaveAsync(CompanySaveDto dto)
    {
        var now = DateTime.Now;
        Company c;

        if (!string.IsNullOrEmpty(dto.CompanyCode))
        {
            c = await _repo.GetAsync(dto.CompanyCode);
            if (c == null) return ApiResult.NotFound("公司不存在");
            c.ViewCode = dto.ViewCode;
            c.CompanyName = dto.CompanyName;
            c.FullName = dto.FullName;
            c.AreaCode = dto.AreaCode;
            c.ParentCode = dto.ParentCode;
            c.Remarks = dto.Remarks;
            c.UpdateDate = now;
            await _repo.UpdateAsync(c);
        }
        else
        {
            c = dto.ToEntity();
            await _repo.AddAsync(c);
        }

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

    public async Task<ApiResult> DeleteAsync(string companyCode)
    {
        var c = await _repo.GetAsync(companyCode);
        if (c == null) return ApiResult.NotFound("公司不存在");
        await _repo.DeleteAsync(companyCode);
        await _repo.SaveChangesAsync();
        return ApiResult.Ok();
    }
}
