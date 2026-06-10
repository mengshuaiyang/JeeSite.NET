using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class AuditService
{
    private readonly IAuditRepository _auditRepo;

    public AuditService(IAuditRepository auditRepo) => _auditRepo = auditRepo;

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
