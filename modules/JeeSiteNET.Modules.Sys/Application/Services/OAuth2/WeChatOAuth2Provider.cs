using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

public class WeChatOAuth2Provider : IOAuth2Provider
{
    private readonly string _appId;
    private readonly string _appSecret;
    private readonly HttpClient _http;

    public WeChatOAuth2Provider(IConfiguration configuration, HttpClient http)
    {
        var section = configuration.GetSection("OAuth2:WeChat");
        _appId = section["AppId"] ?? "";
        _appSecret = section["AppSecret"] ?? "";
        _http = http;
    }

    public string Provider => "wechat";

    public string GetAuthorizationUrl(string redirectUri, string state)
    {
        return $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={_appId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=snsapi_userinfo&state={state}#wechat_redirect";
    }

    public async Task<OAuth2UserInfo?> HandleCallbackAsync(string code, string redirectUri)
    {
        var tokenResponse = await _http.GetAsync(
            $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={_appId}&secret={_appSecret}&code={code}&grant_type=authorization_code");

        tokenResponse.EnsureSuccessStatusCode();
        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
        if (tokenJson.TryGetProperty("errcode", out var err) && err.GetInt32() != 0)
            return null;

        var accessToken = tokenJson.GetProperty("access_token").GetString()!;
        var openId = tokenJson.GetProperty("openid").GetString()!;

        var userResponse = await _http.GetAsync(
            $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN");

        userResponse.EnsureSuccessStatusCode();
        var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();

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
