namespace JeeSiteNET.Core;

public interface INotificationService
{
    Task SendToUserAsync(string userCode, string title, string message, string? type = null, object? data = null);
    Task SendToUsersAsync(IEnumerable<string> userCodes, string title, string message, string? type = null, object? data = null);
}
