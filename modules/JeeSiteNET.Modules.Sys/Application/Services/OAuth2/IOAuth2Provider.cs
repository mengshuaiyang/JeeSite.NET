// 定义 JeeSiteNET.Modules.Sys.Application.Services.OAuth2 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services.OAuth2
namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

// 定义class OAuth2UserInfo
// 定义类：OAuth2UserInfo
public class OAuth2UserInfo
{
    // 属性 Provider
    // 属性：Provider
    public string Provider { get; set; } = string.Empty;
    // 属性 ProviderUserId
    // 属性：ProviderUserId
    public string ProviderUserId { get; set; } = string.Empty;
    // 属性：LoginCode
    public string? LoginCode { get; set; }
    // 属性：UserName
    public string? UserName { get; set; }
    // 属性：Email
    public string? Email { get; set; }
    // 属性：Avatar
    public string? Avatar { get; set; }
}

// 定义接口 IOAuth2Provider
// 定义接口：IOAuth2Provider
public interface IOAuth2Provider
{
    string Provider { get; }
    string GetAuthorizationUrl(string redirectUri, string state);
    Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri);
}
