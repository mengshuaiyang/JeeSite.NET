// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义接口 IArticleAuthService
// 定义接口：IArticleAuthService
public interface IArticleAuthService
{
    Task<bool> CanViewAsync(string articleCode, string? userCode);
    Task<bool> CanEditAsync(string articleCode, string? userCode);
}

// 定义class DefaultArticleAuthService
// 定义类：DefaultArticleAuthService
public class DefaultArticleAuthService : IArticleAuthService
{
    // 方法：CanViewAsync
    public Task<bool> CanViewAsync(string articleCode, string? userCode) => Task.FromResult(true);
    // 方法：CanEditAsync
    public Task<bool> CanEditAsync(string articleCode, string? userCode) => Task.FromResult(false);
}
