using System.Text.Json;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 共享 HttpClient 的 HTTP 请求工具类（静态单例模式，避免 Socket 资源耗尽）
/// </summary>
public static class HttpClientUtil
{
    /// <summary>
    /// 内部共享的 HttpClient 实例；默认请求超时 30 秒
    /// </summary>
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(30) };

    /// <summary>
    /// 发送 HTTP GET 请求，以字符串形式返回响应内容
    /// </summary>
    /// <param name="url">请求 URL</param>
    /// <returns>响应体字符串；非 2xx 状态时抛出 HttpRequestException</returns>
    public static async Task<string> GetAsync(string url)
    {
        var res = await Client.GetAsync(url);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发送 HTTP GET 请求，并将 JSON 响应反序列化为 T 类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="url">请求 URL</param>
    /// <returns>反序列化后的对象；响应体为空或无法解析时返回 default(T)</returns>
    public static async Task<T?> GetJsonAsync<T>(string url)
    {
        var json = await GetAsync(url);
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// 发送 HTTP POST 请求，以字符串形式提交请求体
    /// </summary>
    /// <param name="url">请求 URL</param>
    /// <param name="jsonContent">请求体字符串（通常为 JSON）</param>
    /// <param name="contentType">Content-Type，默认 "application/json"</param>
    /// <returns>响应体字符串；非 2xx 状态时抛出 HttpRequestException</returns>
    public static async Task<string> PostAsync(string url, string jsonContent, string contentType = "application/json")
    {
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, contentType);
        var res = await Client.PostAsync(url, content);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }
}
