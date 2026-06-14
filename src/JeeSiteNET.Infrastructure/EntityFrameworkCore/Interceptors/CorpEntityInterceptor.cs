using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

/// <summary>
/// 公司实体拦截器：新增 ICorpEntity 时，若 CorpCode / CorpName 为空则自动填充默认值
/// </summary>
public class CorpEntityInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// 默认公司编号（null 时为 "0"）
    /// </summary>
    private readonly string? _defaultCorpCode;
    /// <summary>
    /// 默认公司名称
    /// </summary>
    private readonly string? _defaultCorpName;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="defaultCorpCode">默认公司编号，缺省为 "0"</param>
    /// <param name="defaultCorpName">默认公司名称</param>
    public CorpEntityInterceptor(string? defaultCorpCode = null, string? defaultCorpName = null)
    {
        _defaultCorpCode = defaultCorpCode ?? "0";
        _defaultCorpName = defaultCorpName;
    }

    /// <summary>
    /// 同步保存前拦截：自动填充公司字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <returns>拦截结果</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetCorpFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// 异步保存前拦截：自动填充公司字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>拦截结果</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetCorpFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// 为状态为 Added 且 CorpCode 为空的 ICorpEntity 实体填充默认 CorpCode / CorpName
    /// </summary>
    /// <param name="context">DbContext 上下文</param>
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
