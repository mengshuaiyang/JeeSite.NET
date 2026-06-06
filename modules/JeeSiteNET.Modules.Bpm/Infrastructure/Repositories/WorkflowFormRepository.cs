using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;

public class WorkflowFormRepository : IWorkflowFormRepository
{
    private readonly JeeSiteDbContext _db;
    public WorkflowFormRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<WorkflowForm> Query() => _db.Set<WorkflowForm>().AsNoTracking();
    public async Task<WorkflowForm?> GetAsync(object id) => await _db.Set<WorkflowForm>().FindAsync(id);
    public async Task<List<WorkflowForm>> FindListAsync() => await _db.Set<WorkflowForm>().AsNoTracking().ToListAsync();
    public async Task AddAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<WorkflowForm?> FindByBusinessKeyAsync(string businessKey)
        => await _db.Set<WorkflowForm>().FirstOrDefaultAsync(f => f.BusinessKey == businessKey);
}
