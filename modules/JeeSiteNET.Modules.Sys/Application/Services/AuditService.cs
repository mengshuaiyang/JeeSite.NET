using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>审计日志服务，负责审计信息的分页与列表查询（登录、数据变更等）。</summary>
public class AuditService
{
    private readonly IAuditRepository _auditRepo;

    /// <summary>依赖注入构造函数。</summary>
    public AuditService(IAuditRepository auditRepo) => _auditRepo = auditRepo;

    /// <summary>按条件分页查询审计日志（按审计类型与登录名过滤）。</summary>
    /// <param name="request">分页及过滤条件。</param>
    /// <returns>审计实体分页结果。</returns>
    public async Task<PageResult<Audit>> FindPageAsync(PageRequest<Audit> request)
    {
        var query = _auditRepo.Query();
        if (request.Entity != null)
        {
            if (!string.IsNullOrEmpty(request.Entity.AuditType))
                query = query.Where(a => a.AuditType == request.Entity.AuditType);
            if (!string.IsNullOrEmpty(request.Entity.LoginCode))
                query = query.Where(a => a.LoginCode!.Contains(request.Entity.LoginCode));
        }
        var total = await query.CountAsync();
        var list = await query.OrderByDescending(a => a.CreateDate)
            .Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<Audit> { PageNo = request.PageNo, PageSize = request.PageSize, Total = total, List = list };
    }

    /// <summary>按条件查询审计日志列表。</summary>
    /// <param name="criteria">查询条件（审计类型与登录名）。</param>
    /// <returns>审计实体列表。</returns>
    public async Task<List<Audit>> FindListAsync(Audit criteria)
    {
        var query = _auditRepo.Query();
        if (!string.IsNullOrEmpty(criteria.AuditType))
            query = query.Where(a => a.AuditType == criteria.AuditType);
        if (!string.IsNullOrEmpty(criteria.LoginCode))
            query = query.Where(a => a.LoginCode!.Contains(criteria.LoginCode));
        return await query.OrderByDescending(a => a.CreateDate).ToListAsync();
    }
}
