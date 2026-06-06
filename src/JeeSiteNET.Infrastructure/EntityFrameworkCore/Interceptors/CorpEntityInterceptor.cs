using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

public class CorpEntityInterceptor : SaveChangesInterceptor
{
    private readonly string? _defaultCorpCode;
    private readonly string? _defaultCorpName;

    public CorpEntityInterceptor(string? defaultCorpCode = null, string? defaultCorpName = null)
    {
        _defaultCorpCode = defaultCorpCode ?? "0";
        _defaultCorpName = defaultCorpName;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetCorpFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetCorpFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AutoSetCorpFields(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<ICorpEntity>()
            .Where(e => e.State == EntityState.Added
                && string.IsNullOrEmpty(e.Entity.CorpCode));

        foreach (var entry in entries)
        {
            entry.Entity.CorpCode ??= _defaultCorpCode;
            entry.Entity.CorpName ??= _defaultCorpName;
        }
    }
}
