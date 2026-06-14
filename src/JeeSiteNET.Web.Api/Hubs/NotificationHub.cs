using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace JeeSiteNET.Web.Api.Hubs;

/// <summary>
/// 实时通知 SignalR Hub。
/// 客户端连接后自动加入以用户编码命名的组，便于按用户发送通知。
/// </summary>
[Authorize]
public class NotificationHub : Hub
{
    /// <summary>
    /// 客户端连接成功时，将连接加入用户编码对应的分组。
    /// </summary>
    /// <returns>表示异步操作的任务。</returns>
    public override async Task OnConnectedAsync()
    {
        var userCode = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userCode))
            await Groups.AddToGroupAsync(Context.ConnectionId, userCode);
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// 客户端断开连接时，将连接从用户编码分组中移除。
    /// </summary>
    /// <param name="exception">导致断开原因（若有）。</param>
    /// <returns>表示异步操作的任务。</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userCode = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userCode))
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userCode);
        await base.OnDisconnectedAsync(exception);
    }
}
