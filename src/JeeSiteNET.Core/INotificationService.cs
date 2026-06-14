namespace JeeSiteNET.Core;

/// <summary>
/// 站内通知服务接口：支持向单个或多个用户发送系统消息、WebSocket 推送等
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// 向单个用户发送通知消息
    /// </summary>
    /// <param name="userCode">接收用户编码</param>
    /// <param name="title">消息标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="type">消息类型（如 "system" / "todo" / "notice"）</param>
    /// <param name="data">附加业务数据（将序列化为 JSON）</param>
    /// <returns>Task</returns>
    Task SendToUserAsync(string userCode, string title, string message, string? type = null, object? data = null);

    /// <summary>
    /// 向多个用户批量发送通知消息
    /// </summary>
    /// <param name="userCodes">接收用户编码集合</param>
    /// <param name="title">消息标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="type">消息类型</param>
    /// <param name="data">附加业务数据</param>
    /// <returns>Task</returns>
    Task SendToUsersAsync(IEnumerable<string> userCodes, string title, string message, string? type = null, object? data = null);
}
