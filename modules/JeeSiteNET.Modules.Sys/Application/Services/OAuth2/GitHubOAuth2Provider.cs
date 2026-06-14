    // 引入 System.Net.Http.Json 命名空间
// 引入命名空间：System.Net.Http.Json
using System.Net.Http.Json;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;

// 定义 JeeSiteNET.Modules.Sys.Application.Services.OAuth2 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services.OAuth2
namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

// 定义class GitHubOAuth2Provider
// 定义类：GitHubOAuth2Provider
public class GitHubOAuth2Provider : IOAuth2Provider
{
    // 字段 _clientId
    // 字段：_clientId
    private readonly string _clientId;
    // 字段 _clientSecret
    // 字段：_clientSecret
    private readonly string _clientSecret;
    // 字段 _scope
    // 字段：_scope
    private readonly string _scope;
    // 字段 _http
    // 字段：_http
    private readonly HttpClient _http;

    // 方法 GitHubOAuth2Provider
    // 构造函数：GitHubOAuth2Provider
    public GitHubOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        // 声明并初始化变量：section
        var section = configuration.GetSection("OAuth2:GitHub");
        // null 合并操作 ??（若为 null 则使用右侧值）
        _clientId = section["ClientId"] ?? "";
        // null 合并操作 ??（若为 null 则使用右侧值）
        _clientSecret = section["ClientSecret"] ?? "";
        // null 合并操作 ??（若为 null 则使用右侧值）
        _scope = section["Scope"] ?? "user:email";
        _http = http;
    }

    public string Provider => "github";

    // 方法 GetAuthorizationUrl
    // 方法：GetAuthorizationUrl
    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        // return 返回结果
        return $"https://github.com/login/oauth/authorize?client_id={_clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(_scope)}&state={state}";
    }

    // 方法 HandleCallbackAsync
    // 方法：HandleCallbackAsync
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
        // 声明并初始化变量：accessToken
        var accessToken = ParseQueryString(tokenBody, "access_token");
        // if 条件判断
        if (string.IsNullOrEmpty(accessToken))
            // return 返回结果
            return null;

        // 集合操作：清空集合
        _http.DefaultRequestHeaders.Clear();
        // 集合操作：添加元素
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        // 集合操作：添加元素
        _http.DefaultRequestHeaders.Add("User-Agent", "JeeSite.NET");

        // 缓存：获取值
        var userResponse = await _http.GetAsync("https://api.github.com/user");
        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        // 缓存：获取值
        var emailsResponse = await _http.GetAsync("https://api.github.com/user/emails");
        var emailsJson = await emailsResponse.Content.ReadFromJsonAsync<JsonElement[]>();
        // 数据库操作：取首条或默认值
        var primaryEmail = emailsJson?.FirstOrDefault(e =>
            // 数据库操作：取首条或默认值
            e.GetProperty("primary").GetBoolean()) ?? emailsJson?.FirstOrDefault();

        // return 返回结果
        return new OAuth2UserInfo
        {
            Provider = "github",
            // 调用 ToString
            ProviderUserId = userJson.GetProperty("id").GetInt64().ToString(),
            LoginCode = userJson.GetProperty("login").GetString(),
            // null 合并操作 ??（若为 null 则使用右侧值）
            UserName = userJson.GetProperty("name").GetString() ?? userJson.GetProperty("login").GetString(),
            // null 条件运算符?.（不为 null 时才访问成员）
            Email = primaryEmail?.GetProperty("email").GetString(),
            Avatar = userJson.GetProperty("avatar_url").GetString(),
        };
    }

    // 方法 ParseQueryString
    // 方法：ParseQueryString
    private static string? ParseQueryString(string query, string key)
    {
        // foreach 遍历集合
        foreach (var part in query.Split('&'))
        {
            // 声明并初始化变量：eq
            var eq = part.IndexOf('=');
            // if 条件判断
            if (eq > 0 && part[..eq] == key)
                // return 返回结果
                return Uri.UnescapeDataString(part[(eq + 1)..]);
        }
        // return 返回结果
        return null;
    }
}
