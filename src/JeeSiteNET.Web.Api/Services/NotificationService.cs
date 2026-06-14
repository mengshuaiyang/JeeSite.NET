using JeeSiteNET.Core;
using Microsoft.AspNetCore.SignalR;
using JeeSiteNET.Web.Api.Hubs;

namespace JeeSiteNET.Web.Api.Services;

/// <summary>
/// 基于 SignalR 的通知服务实现。
/// 通过以用户编码命名的组向特定用户/全体用户发送实时消息。
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    /// <summary>
    /// 初始化 <see cref="NotificationService"/> 的新实例。
    /// </summary>
    /// <param name="hubContext">SignalR Hub 上下文。</param>
    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// 向单个用户发送通知。
    /// </summary>
    /// <param name="userCode">目标用户编码。</param>
    /// <param name="title">通知标题。</param>
    /// <param name="message">通知正文。</param>
    /// <param name="type">消息类型（info/warning/error 等，默认 info。</param>
    /// <param name="data">附加业务数据。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task SendToUserAsync(string userCode, string title, string message, string? type = null, object? data = null)
    {
        await _hubContext.Clients.Group(userCode).SendAsync("ReceiveNotification", new
        {
            title,
            message,
            type = type ?? "info",
            data,
            timestamp = DateTime.Now
        });
    }

    /// <summary>
    /// 向多个用户发送通知。
    /// </summary>
    /// <param name="userCodes">目标用户编码集合。</param>
    /// <param name="title">通知标题。</param>
    /// <param name="message">通知正文。</param>
    /// <param name="type">消息类型（info/warning/error 等，默认 info。</param>
    /// <param name="data">附加业务数据。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task SendToUsersAsync(IEnumerable<string> userCodes, string title, string message, string? type = null, object? data = null)
    {
        await _hubContext.Clients.Groups(userCodes).SendAsync("ReceiveNotification", new
        {
            title,
            message,
            type = type ?? "info",
            data,
            timestamp = DateTime.Now
        });
    }

    /// <summary>
    /// 向所有已连接客户端广播通知。
    /// </summary>
    /// <param name="title">通知标题。</param>
    /// <param name="message">通知正文。</param>
    /// <param name="type">消息类型（info/warning/error 等，默认 info。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task BroadcastAsync(string title, string message, string? type = null)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
        {
            title,
            message,
            type = type ?? "info",
            data = (object?)null,
            timestamp = DateTime.Now
        });
    }
}
