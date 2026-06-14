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

// 定义class WeChatOAuth2Provider
// 定义类：WeChatOAuth2Provider
public class WeChatOAuth2Provider : IOAuth2Provider
{
    // 字段 _appId
    // 字段：_appId
    private readonly string _appId;
    // 字段 _appSecret
    // 字段：_appSecret
    private readonly string _appSecret;
    // 字段 _http
    // 字段：_http
    private readonly HttpClient _http;

    // 方法 WeChatOAuth2Provider
    // 构造函数：WeChatOAuth2Provider
    public WeChatOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        // 声明并初始化变量：section
        var section = configuration.GetSection("OAuth2:WeChat");
        // null 合并操作 ??（若为 null 则使用右侧值）
        _appId = section["AppId"] ?? "";
        // null 合并操作 ??（若为 null 则使用右侧值）
        _appSecret = section["AppSecret"] ?? "";
        _http = http;
    }

    public string Provider => "wechat";

    // 方法 GetAuthorizationUrl
    // 方法：GetAuthorizationUrl
    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        // return 返回结果
        return $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={_appId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=snsapi_userinfo&state={state}#wechat_redirect";
    }

    // 方法 HandleCallbackAsync
    // 方法：HandleCallbackAsync
    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        // 缓存：获取值
        var tokenResponse = await _http.GetAsync(
            $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={_appId}&secret={_appSecret}&code={code}&grant_type=authorization_code");

        tokenResponse.EnsureSuccessStatusCode();
        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
        // if 条件判断
        if (tokenJson.TryGetProperty("errcode", out var err) && err.GetInt32() != 0)
            // return 返回结果
            return null;

        // 声明并初始化变量：accessToken
        var accessToken = tokenJson.GetProperty("access_token").GetString()!;
        // 声明并初始化变量：openId
        var openId = tokenJson.GetProperty("openid").GetString()!;

        // 缓存：获取值
        var userResponse = await _http.GetAsync(
            $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN");

        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

        // return 返回结果
        return new OAuth2UserInfo
        {
            Provider = "wechat",
            ProviderUserId = openId,
            LoginCode = $"wx_{openId[..8]}",
            UserName = userJson.GetProperty("nickname").GetString(),
            Avatar = userJson.GetProperty("headimgurl").GetString(),
        };
    }
}
