using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

public class DingTalkOAuth2Provider : IOAuth2Provider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _http;

    public DingTalkOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        var section = configuration.GetSection("OAuth2:DingTalk");
        _clientId = section["ClientId"] ?? "";
        _clientSecret = section["ClientSecret"] ?? "";
        _http = http;
    }

    public string Provider => "dingtalk";

    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        return $"https://login.dingtalk.com/oauth2/auth?client_id={_clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope=openid&state={state}&prompt=consent";
    }

    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        var tokenResponse = await _http.PostAsync(
            "https://api.dingtalk.com/v1.0/oauth2/accessToken",
            new StringContent(JsonSerializer.Serialize(new
            {
                appKey = _clientId,
                appSecret = _clientSecret,
            }), System.Text.Encoding.UTF8, "application/json"));

        tokenResponse.EnsureSuccessStatusCode();
        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
        var accessToken = tokenJson.GetProperty("accessToken").GetString();
        if (string.IsNullOrEmpty(accessToken))
            return null;

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("x-acs-dingtalk-access-token", accessToken);

        var userResponse = await _http.PostAsync(
            "https://api.dingtalk.com/v1.0/oauth2/userinfo",
            new StringContent(JsonSerializer.Serialize(new { }), System.Text.Encoding.UTF8, "application/json"));

        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        return new OAuth2UserInfo
        {
            Provider = "dingtalk",
            ProviderUserId = userJson.GetProperty("openId").GetString() ?? userJson.GetProperty("unionId").GetString() ?? "",
            UserName = userJson.GetProperty("nick").GetString() ?? "",
            Avatar = userJson.TryGetProperty("avatarUrl", out var av) ? av.GetString() : null,
        };
    }
}
