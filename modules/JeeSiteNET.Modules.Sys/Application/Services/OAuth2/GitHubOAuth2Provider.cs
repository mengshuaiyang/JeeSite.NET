using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

// ================================================================
// GitHub OAuth2 登录提供者
//
// 流程：
//   ① 用户点击"用 GitHub 登录" → 跳转到 GitHub 授权页
//   ② 用户授权后 GitHub 回调到本站，附带授权码 code
//   ③ HandleCallbackAsync 用 code 向 GitHub 换取 access_token
//   ④ 用 access_token 获取用户信息（用户名、邮箱、头像）
//   ⑤ OAuth2Service 根据返回的用户信息创建/绑定本地账号
//
// 配置文件（appsettings.json）：
//   OAuth2:GitHub:ClientId     — GitHub OAuth App 的 Client ID
//   OAuth2:GitHub:ClientSecret — GitHub OAuth App 的 Client Secret
//   OAuth2:GitHub:Scope        — 权限范围（默认 user:email）
//
// 注册位置：SysModuleInstaller.cs（AddScoped<IOAuth2Provider, GitHubOAuth2Provider>）
// 使用位置：OAuth2Service.cs（通过 IEnumerable<IOAuth2Provider> 注入）
// ================================================================

public class GitHubOAuth2Provider : IOAuth2Provider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _scope;
    private readonly HttpClient _http;

    /// <summary>从配置读取 GitHub OAuth 参数，注入用于 HTTP 请求的客户端。</summary>
    public GitHubOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        var section = configuration.GetSection("OAuth2:GitHub");
        _clientId = section["ClientId"] ?? "";
        _clientSecret = section["ClientSecret"] ?? "";
        _scope = section["Scope"] ?? "user:email";
        _http = http;
    }

    /// <summary>提供者标识，与 OAuth2Service 中路由匹配使用</summary>
    public string Provider => "github";

    /// <summary>生成 GitHub OAuth 授权页 URL，用户浏览器跳转到此地址。</summary>
    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        return $"https://github.com/login/oauth/authorize?client_id={_clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(_scope)}&state={state}";
    }

    /// <summary>
    /// 处理 GitHub OAuth 回调。
    /// 用授权码换取 Token → 用 Token 获取用户资料 → 返回统一 OAuth2UserInfo。
    /// </summary>
    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        // 第 1 步：用授权码换取 access_token
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

        // 第 2 步：用 access_token 获取用户信息
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        _http.DefaultRequestHeaders.Add("User-Agent", "JeeSite.NET");

        var userResponse = await _http.GetAsync("https://api.github.com/user");
        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        // 第 3 步：获取用户的邮箱列表，找到主邮箱
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

    /// <summary>从 URL 查询字符串中解析指定 key 的值（如 access_token=xxx&scope=yyy）。</summary>
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
