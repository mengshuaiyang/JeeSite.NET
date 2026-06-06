using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

public class TreeEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetTreeFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetTreeFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void AutoSetTreeFields(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<ITreeEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            if (entity.ParentCode == "0" || string.IsNullOrEmpty(entity.ParentCode))
            {
                entity.TreeLevel = 0;
                entity.ParentCodes = string.Empty;
                entity.TreeNames = entity.GetName();
                entity.TreeSorts = entity.TreeSort.ToString("D10");
            }
            else
            {
                var parent = context.Find(entity.GetType(), entity.ParentCode) as ITreeEntity;
                if (parent != null)
                {
                    entity.TreeLevel = parent.TreeLevel + 1;
                    entity.ParentCodes = parent.ParentCodes + entity.ParentCode + ",";
                    entity.TreeNames = parent.TreeNames + "/" + entity.GetName();
                    entity.TreeSorts = parent.TreeSorts + entity.TreeSort.ToString("D10");
                }
            }
        }
    }
}
