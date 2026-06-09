namespace JeeSiteNET.Core;

public class NullNotificationService : INotificationService
{
    public Task SendToUserAsync(string userCode, string title, string message, string? type = null, object? data = null) => Task.CompletedTask;
    public Task SendToUsersAsync(IEnumerable<string> userCodes, string title, string message, string? type = null, object? data = null) => Task.CompletedTask;
}
