using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly string? _currentUserCode;

    public AuditInterceptor(string? currentUserCode = null)
    {
        _currentUserCode = currentUserCode;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AutoSetAuditFields(DbContext? context)
    {
        if (context == null) return;

        var now = DateTime.Now;
        var entries = context.ChangeTracker.Entries<IBaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateBy = _currentUserCode;
                entry.Entity.CreateDate = now;
                entry.Entity.UpdateBy = _currentUserCode;
                entry.Entity.UpdateDate = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateBy = _currentUserCode;
                entry.Entity.UpdateDate = now;
            }
        }
    }
}
