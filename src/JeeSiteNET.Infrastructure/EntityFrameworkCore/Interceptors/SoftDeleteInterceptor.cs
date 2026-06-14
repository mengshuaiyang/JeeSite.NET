using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

/// <summary>
/// 软删除拦截器：将对 IDataEntity 的物理删除转换为逻辑删除（Status = "1"，状态变为 Modified）
/// </summary>
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// 同步保存前拦截：拦截删除操作改为逻辑删除
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <returns>拦截结果</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// 异步保存前拦截：拦截删除操作改为逻辑删除
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>拦截结果</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSoftDelete(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// 将 Deleted 状态的 IDataEntity 改为 Modified 并设置 Status = "1"，实现逻辑删除
    /// </summary>
    /// <param name="context">DbContext 上下文</param>
    private static void AutoSoftDelete(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<IDataEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.Status = "1";
        }
    }
}
