using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

public class GitHubOAuth2Provider : IOAuth2Provider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _scope;
    private readonly HttpClient _http;

    public GitHubOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        var section = configuration.GetSection("OAuth2:GitHub");
        _clientId = section["ClientId"] ?? "";
        _clientSecret = section["ClientSecret"] ?? "";
        _scope = section["Scope"] ?? "user:email";
        _http = http;
    }

    public string Provider => "github";

    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        return $"https://github.com/login/oauth/authorize?client_id={_clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(_scope)}&state={state}";
    }

    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        var tokenResponse = await _http.PostAsync(
            "https://github.com/login/oauth/access_token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["code"] = code,
                ["redirect_uri"] = redirectUri,
            }));

        tokenResponse.EnsureSuccessStatusCode();
        var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
        var accessToken = ParseQueryString(tokenBody, "access_token");
        if (string.IsNullOrEmpty(accessToken))
            return null;

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        _http.DefaultRequestHeaders.Add("User-Agent", "JeeSite.NET");

        var userResponse = await _http.GetAsync("https://api.github.com/user");
        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        var emailsResponse = await _http.GetAsync("https://api.github.com/user/emails");
        var emailsJson = await emailsResponse.Content.ReadFromJsonAsync<JsonElement[]>();
        var primaryEmail = emailsJson?.FirstOrDefault(e =>
            e.GetProperty("primary").GetBoolean()) ?? emailsJson?.FirstOrDefault();

        return new OAuth2UserInfo
        {
            Provider = "github",
            ProviderUserId = userJson.GetProperty("id").GetInt64().ToString(),
            LoginCode = userJson.GetProperty("login").GetString(),
            UserName = userJson.GetProperty("name").GetString() ?? userJson.GetProperty("login").GetString(),
            Email = primaryEmail?.GetProperty("email").GetString(),
            Avatar = userJson.GetProperty("avatar_url").GetString(),
        };
    }

    private static string? ParseQueryString(string query, string key)
    {
        foreach (var part in query.Split('&'))
        {
            var eq = part.IndexOf('=');
            if (eq > 0 && part[..eq] == key)
                return Uri.UnescapeDataString(part[(eq + 1)..]);
        }
        return null;
    }
}
