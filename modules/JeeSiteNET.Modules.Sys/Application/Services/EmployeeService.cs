using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>员工管理服务，负责员工基础信息、岗位绑定与附属机构维护。</summary>
public class EmployeeService
{
    private readonly IEmployeeRepository _empRepo;
    private readonly JeeSiteDbContext _db;

    /// <summary>依赖注入构造函数。</summary>
    public EmployeeService(IEmployeeRepository empRepo, JeeSiteDbContext db)
    {
        _empRepo = empRepo;
        _db = db;
    }

    /// <summary>根据员工编码获取员工信息，包含关联岗位与附属机构列表。</summary>
    /// <param name="empCode">员工编码。</param>
    /// <returns>员工 DTO，不存在时返回 null。</returns>
    public async Task<EmployeeDto?> GetAsync(string empCode)
    {
        var emp = await _empRepo.GetAsync(empCode);
        if (emp == null) return null;
        var posts = await _empRepo.GetPostsAsync(empCode);
        var offices = await _empRepo.GetOfficesAsync(empCode);
        var officeCodes = offices.Select(o => o.OfficeCode).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();
        // 一次性读取关联机构名称，避免逐行查询
        var orgs = officeCodes.Count > 0
            ? await _db.Set<Organization>().Where(o => officeCodes.Contains(o.OrgCode)).AsNoTracking().ToDictionaryAsync(o => o.OrgCode, o => o.OrgName)
            : [];
        var officeDtos = offices.Select(o => new EmployeeOfficeDto
        {
            Id = o.Id,
            EmpCode = o.EmpCode,
            OfficeCode = o.OfficeCode,
            OfficeName = orgs.GetValueOrDefault(o.OfficeCode, string.Empty),
            PostCode = o.PostCode
        }).ToList();
        return emp.ToDto(posts.Select(p => p.PostCode).ToList(), officeDtos);
    }

    /// <summary>按条件分页查询员工列表，批量加载岗位与机构信息以避免 N+1。</summary>
    /// <param name="request">分页及过滤条件（员工名、工号）。</param>
    /// <returns>分页结果。</returns>
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
        // 批量读取岗位、机构与机构名称，降低数据往返次数
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

    /// <summary>新增或更新员工信息，同时重置岗位与附属机构关联。</summary>
    /// <param name="dto">员工保存信息（包含岗位列表与机构列表）。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SaveAsync(EmployeeSaveDto dto)
    {
        var now = DateTime.Now;
        Employee? emp;

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

        // 先清除旧的员工-岗位关系，再按当前提交重新写入
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

        // 先清除旧的员工-附属机构关系，再按当前提交重新写入
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

    /// <summary>删除员工。</summary>
    /// <param name="empCode">员工编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string empCode)
    {
        var emp = await _empRepo.GetAsync(empCode);
        if (emp == null) return ApiResult.NotFound("员工不存在");
        await _empRepo.DeleteAsync(empCode);
        await _empRepo.SaveChangesAsync();
        return ApiResult.Ok();
    }
}
