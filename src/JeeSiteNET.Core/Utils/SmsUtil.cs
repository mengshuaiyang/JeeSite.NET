namespace JeeSiteNET.Core.Utils;

public static class SmsUtil
{
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
