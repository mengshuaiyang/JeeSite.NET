namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

public class OAuth2UserInfo
{
    public string Provider { get; set; } = string.Empty;
    public string ProviderUserId { get; set; } = string.Empty;
    public string? LoginCode { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
}

public interface IOAuth2Provider
{
    string Provider { get; }
    string GetAuthorizationUrl(string redirectUri, string state);
    Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri);
}
