namespace JeeSiteNET.Modules.Cms.Application.Services;

public interface IArticleAuthService
{
    Task<bool> CanViewAsync(string articleCode, string? userCode);
    Task<bool> CanEditAsync(string articleCode, string? userCode);
}

public class DefaultArticleAuthService : IArticleAuthService
{
    public Task<bool> CanViewAsync(string articleCode, string? userCode) => Task.FromResult(true);
    public Task<bool> CanEditAsync(string articleCode, string? userCode) => Task.FromResult(false);
}
