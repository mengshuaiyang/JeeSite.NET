namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 短信发送工具类（通过 HTTP API 调用第三方短信平台）
/// </summary>
public static class SmsUtil
{
    /// <summary>
    /// 异步发送短信
    /// </summary>
    /// <param name="apiUrl">第三方短信平台 API 地址</param>
    /// <param name="apiKey">短信平台 API 密钥</param>
    /// <param name="phoneNumber">接收短信的手机号码</param>
    /// <param name="message">短信内容</param>
    /// <returns>发送成功返回 true；发生异常返回 false</returns>
    public static async Task<bool> SendAsync(string apiUrl, string apiKey, string phoneNumber, string message)
    {
        try
        {
            var payload = new { phone = phoneNumber, content = message, key = apiKey };
            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            await HttpClientUtil.PostAsync(apiUrl, json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
