using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public AuditInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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

        string? userCode = null;
        try
        {
            var currentUser = _serviceProvider.GetService(typeof(ICurrentUser)) as ICurrentUser;
            userCode = currentUser?.UserCode;
        }
        catch
        {
            // Scoped service not available (e.g. background jobs)
        }

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateBy = userCode;
                entry.Entity.CreateDate = now;
                entry.Entity.UpdateBy = userCode;
                entry.Entity.UpdateDate = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateBy = userCode;
                entry.Entity.UpdateDate = now;
            }
        }
    }
}
