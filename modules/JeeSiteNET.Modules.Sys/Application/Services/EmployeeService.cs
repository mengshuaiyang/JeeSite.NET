using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _empRepo;
    private readonly DbContext _db;

    public EmployeeService(IEmployeeRepository empRepo, DbContext db)
    {
        _empRepo = empRepo;
        _db = db;
    }

    public async Task<EmployeeDto?> GetAsync(string empCode)
    {
        var emp = await _empRepo.GetAsync(empCode);
        if (emp == null) return null;
        var posts = await _empRepo.GetPostsAsync(empCode);
        var offices = await _empRepo.GetOfficesAsync(empCode);
        var officeDtos = offices.Select(o => new EmployeeOfficeDto
        {
            Id = o.Id,
            EmpCode = o.EmpCode,
            OfficeCode = o.OfficeCode,
            OfficeName = _db.Set<Organization>().FirstOrDefault(x => x.OrgCode == o.OfficeCode)?.OrgName ?? string.Empty,
            PostCode = o.PostCode
        }).ToList();
        return emp.ToDto(posts.Select(p => p.PostCode).ToList(), officeDtos);
    }

    public async Task<PageResult<EmployeeDto>> FindPageAsync(PageRequest<Employee> request)
    {
        var query = _empRepo.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.EmpName),
                e => e.EmpName.Contains(request.Entity!.EmpName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.EmpNo),
                e => e.EmpNo == request.Entity!.EmpNo)
            .OrderByDescending(e => e.CreateDate);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var codes = list.Select(e => e.EmpCode).ToList();
        var posts = await _db.Set<EmployeePost>()
            .Where(p => codes.Contains(p.EmpCode))
            .ToListAsync();
        var offices = await _db.Set<EmployeeOffice>()
            .Where(o => codes.Contains(o.EmpCode))
            .ToListAsync();

        var orgs = await _db.Set<Organization>()
            .Where(o => offices.Select(x => x.OfficeCode).Contains(o.OrgCode))
            .ToDictionaryAsync(o => o.OrgCode, o => o.OrgName);

        return new PageResult<EmployeeDto>
        {
            List = list.Select(e => e.ToDto(
                posts.Where(p => p.EmpCode == e.EmpCode).Select(p => p.PostCode).ToList(),
                offices.Where(o => o.EmpCode == e.EmpCode)
                    .Select(o => new EmployeeOfficeDto
                    {
                        Id = o.Id,
                        EmpCode = o.EmpCode,
                        OfficeCode = o.OfficeCode,
                        OfficeName = orgs.GetValueOrDefault(o.OfficeCode, string.Empty),
                        PostCode = o.PostCode
                    }).ToList()
            )).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    public async Task<ApiResult> SaveAsync(EmployeeSaveDto dto)
    {
        var now = DateTime.Now;
        Employee emp;

        if (!string.IsNullOrEmpty(dto.EmpCode))
        {
            emp = await _empRepo.GetAsync(dto.EmpCode);
            if (emp == null) return ApiResult.NotFound("员工不存在");
            emp.EmpNo = dto.EmpNo;
            emp.EmpName = dto.EmpName;
            emp.EmpNameEn = dto.EmpNameEn;
            emp.OfficeCode = dto.OfficeCode;
            emp.CompanyCode = dto.CompanyCode;
            emp.Remarks = dto.Remarks;
            emp.UpdateDate = now;
            await _empRepo.UpdateAsync(emp);
        }
        else
        {
            emp = dto.ToEntity();
            emp.CreateDate = now;
            await _empRepo.AddAsync(emp);
        }

        // 更新岗位
        await _empRepo.DeletePostsAsync(emp.EmpCode);
        if (dto.PostCodes != null)
        {
            foreach (var pc in dto.PostCodes.Where(c => !string.IsNullOrEmpty(c)))
            {
                await _empRepo.AddPostAsync(new EmployeePost
                {
                    EmpCode = emp.EmpCode,
                    PostCode = pc
                });
            }
        }

        // 更新附属机构
        await _empRepo.DeleteOfficesAsync(emp.EmpCode);
        if (dto.Offices != null)
        {
            foreach (var o in dto.Offices.Where(x => !string.IsNullOrEmpty(x.OfficeCode)))
            {
                await _empRepo.AddOfficeAsync(new EmployeeOffice
                {
                    Id = o.Id ?? IdGenerator.NewId(),
                    EmpCode = emp.EmpCode,
                    OfficeCode = o.OfficeCode,
                    PostCode = o.PostCode
                });
            }
        }

        await _empRepo.SaveChangesAsync();
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteAsync(string empCode)
    {
        var emp = await _empRepo.GetAsync(empCode);
        if (emp == null) return ApiResult.NotFound("员工不存在");
        await _empRepo.DeleteAsync(empCode);
        await _empRepo.SaveChangesAsync();
        return ApiResult.Ok();
    }
}
