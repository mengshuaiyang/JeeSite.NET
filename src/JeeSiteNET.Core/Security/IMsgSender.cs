namespace JeeSiteNET.Core.Security;

public interface ISmsSender
{
    Task<bool> SendAsync(string phoneNumber, string message);
}

public interface IEmailSender
{
    Task<bool> SendAsync(string toAddress, string subject, string body, string? cc = null, string? bcc = null);
}
