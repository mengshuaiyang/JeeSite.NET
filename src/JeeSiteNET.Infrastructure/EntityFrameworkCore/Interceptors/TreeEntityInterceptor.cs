using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

/// <summary>
/// 树形实体拦截器：保存前自动填充 ITreeEntity 的 TreeLevel/ParentCodes/TreeNames/TreeSorts 等层级字段
/// </summary>
public class TreeEntityInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// 同步保存前拦截：自动填充树形字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <returns>拦截结果</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetTreeFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// 异步保存前拦截：自动填充树形字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>拦截结果</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetTreeFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// 根据 ParentCode 递归推导节点层级与路径字符串；根节点(ParentCode=0/空)层级为 0
    /// </summary>
    /// <param name="context">DbContext 上下文（用于查询父节点）</param>
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
                entity.TreeSorts = entity.TreeSort.ToString("0000000000");
            }
            else
            {
                var parent = context.Find(entity.GetType(), entity.ParentCode) as ITreeEntity;
                if (parent != null)
                {
                    entity.TreeLevel = parent.TreeLevel + 1;
                    entity.ParentCodes = parent.ParentCodes + entity.ParentCode + ",";
                    entity.TreeNames = parent.TreeNames + "/" + entity.GetName();
                    entity.TreeSorts = parent.TreeSorts + entity.TreeSort.ToString("0000000000");
                }
            }
        }
    }
}
