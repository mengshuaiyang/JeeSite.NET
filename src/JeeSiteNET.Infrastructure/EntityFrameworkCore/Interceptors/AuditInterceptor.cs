using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore.Interceptors;

/// <summary>
/// 审计拦截器：在 SaveChanges 执行前自动填充 IBaseEntity 的 CreateBy/CreateDate/UpdateBy/UpdateDate 字段
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// 服务提供程序（用于获取当前用户 ICurrentUser）
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider，用于解析 ICurrentUser</param>
    public AuditInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 同步保存前拦截：自动填充审计字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <returns>拦截结果</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AutoSetAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// 异步保存前拦截：自动填充审计字段
    /// </summary>
    /// <param name="eventData">DbContext 事件数据</param>
    /// <param name="result">拦截结果</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>拦截结果</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AutoSetAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// 为所有 IBaseEntity 按状态自动填充创建/更新人员与时间；新增时同时填充两类字段，更新时仅更新类字段
    /// </summary>
    /// <param name="context">DbContext 上下文</param>
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
