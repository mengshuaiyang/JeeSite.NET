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

// 定义class DingTalkOAuth2Provider
// 定义类：DingTalkOAuth2Provider
public class DingTalkOAuth2Provider : IOAuth2Provider
{
    // 字段 _clientId
    // 字段：_clientId
    private readonly string _clientId;
    // 字段 _clientSecret
    // 字段：_clientSecret
    private readonly string _clientSecret;
    // 字段 _http
    // 字段：_http
    private readonly HttpClient _http;

    // 方法 DingTalkOAuth2Provider
    // 构造函数：DingTalkOAuth2Provider
    public DingTalkOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        // 声明并初始化变量：section
        var section = configuration.GetSection("OAuth2:DingTalk");
        // null 合并操作 ??（若为 null 则使用右侧值）
        _clientId = section["ClientId"] ?? "";
        // null 合并操作 ??（若为 null 则使用右侧值）
        _clientSecret = section["ClientSecret"] ?? "";
        _http = http;
    }

    public string Provider => "dingtalk";

    // 方法 GetAuthorizationUrl
    // 方法：GetAuthorizationUrl
    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        // return 返回结果
        return $"https://login.dingtalk.com/oauth2/auth?client_id={_clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope=openid&state={state}&prompt=consent";
    }

    // 方法 HandleCallbackAsync
    // 方法：HandleCallbackAsync
    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        var tokenResponse = await _http.PostAsync(
            "https://api.dingtalk.com/v1.0/oauth2/accessToken",
            // JSON 序列化
            new StringContent(JsonSerializer.Serialize(new
            {
                appKey = _clientId,
                appSecret = _clientSecret,
            }), System.Text.Encoding.UTF8, "application/json"));

        tokenResponse.EnsureSuccessStatusCode();
        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
        // 声明并初始化变量：accessToken
        var accessToken = tokenJson.GetProperty("accessToken").GetString();
        // if 条件判断
        if (string.IsNullOrEmpty(accessToken))
            // return 返回结果
            return null;

        // 集合操作：清空集合
        _http.DefaultRequestHeaders.Clear();
        // 集合操作：添加元素
        _http.DefaultRequestHeaders.Add("x-acs-dingtalk-access-token", accessToken);

        var userResponse = await _http.PostAsync(
            "https://api.dingtalk.com/v1.0/oauth2/userinfo",
            // JSON 序列化
            new StringContent(JsonSerializer.Serialize(new { }), System.Text.Encoding.UTF8, "application/json"));

        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        // return 返回结果
        return new OAuth2UserInfo
        {
            Provider = "dingtalk",
            // null 合并操作 ??（若为 null 则使用右侧值）
            ProviderUserId = userJson.GetProperty("openId").GetString() ?? userJson.GetProperty("unionId").GetString() ?? "",
            // null 合并操作 ??（若为 null 则使用右侧值）
            UserName = userJson.GetProperty("nick").GetString() ?? "",
            Avatar = userJson.TryGetProperty("avatarUrl", out var av) ? av.GetString() : null,
        };
    }
}
