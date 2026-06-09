using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace JeeSiteNET.Web.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userCode = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userCode))
            await Groups.AddToGroupAsync(Context.ConnectionId, userCode);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userCode = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userCode))
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userCode);
        await base.OnDisconnectedAsync(exception);
    }
}
