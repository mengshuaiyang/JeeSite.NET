using System.Text.Json;

namespace JeeSiteNET.Core.Utils;

public static class HttpClientUtil
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(30) };

    public static async Task<string> GetAsync(string url)
    {
        var res = await Client.GetAsync(url);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }

    public static async Task<T?> GetJsonAsync<T>(string url)
    {
        var json = await GetAsync(url);
        return JsonSerializer.Deserialize<T>(json);
    }

    public static async Task<string> PostAsync(string url, string jsonContent, string contentType = "application/json")
    {
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, contentType);
        var res = await Client.PostAsync(url, content);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }
}
