using JeeSiteNET.Core;
using Microsoft.AspNetCore.SignalR;
using JeeSiteNET.Web.Api.Hubs;

namespace JeeSiteNET.Web.Api.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

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
