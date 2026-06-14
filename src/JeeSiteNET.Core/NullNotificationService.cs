namespace JeeSiteNET.Core;

/// <summary>
/// 空对象模式通知服务：在未配置真实通知通道时使用，不执行任何操作直接返回完成态
/// </summary>
public class NullNotificationService : INotificationService
{
    /// <summary>
    /// 空实现：直接返回 Task.CompletedTask，不产生副作用
    /// </summary>
    /// <param name="userCode">接收用户编码（忽略）</param>
    /// <param name="title">消息标题（忽略）</param>
    /// <param name="message">消息内容（忽略）</param>
    /// <param name="type">消息类型（忽略）</param>
    /// <param name="data">附加数据（忽略）</param>
    /// <returns>Task.CompletedTask</returns>
    public Task SendToUserAsync(string userCode, string title, string message, string? type = null, object? data = null) => Task.CompletedTask;

    /// <summary>
    /// 空实现：直接返回 Task.CompletedTask，不产生副作用
    /// </summary>
    /// <param name="userCodes">接收用户编码集合（忽略）</param>
    /// <param name="title">消息标题（忽略）</param>
    /// <param name="message">消息内容（忽略）</param>
    /// <param name="type">消息类型（忽略）</param>
    /// <param name="data">附加数据（忽略）</param>
    /// <returns>Task.CompletedTask</returns>
    public Task SendToUsersAsync(IEnumerable<string> userCodes, string title, string message, string? type = null, object? data = null) => Task.CompletedTask;
}
