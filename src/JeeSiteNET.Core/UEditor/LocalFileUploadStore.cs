using System.Text;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// 本地文件上传存储的默认实现：将文件写入 web-root 下相对路径的位置；
/// URL 由 baseUrl + 相对路径拼接，便于反向代理/CDN 切换。
/// </summary>
public sealed class LocalFileUploadStore : IUEditorUploadStore
{
    /// <summary>
    /// 物理存储根目录（通常为 wwwroot 绝对路径）
    /// </summary>
    private readonly string _webRoot;

    /// <summary>
    /// 访问 URL 前缀（如 "https://cdn.example.com/"），为空时使用相对路径
    /// </summary>
    private readonly string _baseUrl;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="webRoot">物理存储根目录绝对路径</param>
    /// <param name="baseUrl">可选的 URL 前缀，便于反向代理/CDN</param>
    public LocalFileUploadStore(string webRoot, string baseUrl = "")
    {
        _webRoot = webRoot ?? throw new ArgumentNullException(nameof(webRoot));
        _baseUrl = baseUrl ?? string.Empty;
    }

    /// <summary>
    /// 将文件流保存到 webRoot 下相对路径位置，并返回可公开访问的 URL
    /// </summary>
    /// <param name="relativePath">相对路径</param>
    /// <param name="stream">输入流</param>
    /// <param name="contentType">Content-Type（当前实现未校验，由上游校验）</param>
    /// <param name="size">文件大小（字节）</param>
    /// <returns>保存结果（URL + 相对路径）</returns>
    public async Task<UEditorStoreResult> StoreAsync(string relativePath, Stream stream, string contentType, long size)
    {
        var fullPath = Path.Combine(_webRoot, relativePath.TrimStart('/', '\\'));
        var dir = Path.GetDirectoryName(fullPath)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fs);
        await fs.FlushAsync();

        var url = _baseUrl + "/" + relativePath.Replace('\\', '/').TrimStart('/');
        return new UEditorStoreResult { Url = url, RelativePath = relativePath };
    }

    /// <summary>
    /// 从本地目录枚举指定扩展名的文件，按修改时间降序分页返回
    /// </summary>
    /// <param name="basePath">目录相对路径</param>
    /// <param name="start">跳过条目数</param>
    /// <param name="size">返回条目数</param>
    /// <param name="allowedExtensions">允许的扩展名集合</param>
    /// <returns>文件条目列表</returns>
    public Task<List<UEditorListEntry>> ListAsync(string basePath, int start, int size, string[] allowedExtensions)
    {
        var dir = Path.Combine(_webRoot, basePath.TrimStart('/', '\\'));
        var result = new List<UEditorListEntry>();
        if (!Directory.Exists(dir)) return Task.FromResult(result);

        var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
            .Select(f => new { Path = f, Time = new FileInfo(f).LastWriteTime.ToUnixTimeSeconds() })
            .OrderByDescending(f => f.Time)
            .Skip(start)
            .Take(size)
            .ToList();

        foreach (var f in files)
        {
            var rel = f.Path[(_webRoot.Length + 1)..].Replace('\\', '/');
            result.Add(new UEditorListEntry { Url = _baseUrl + "/" + rel, Mtime = f.Time });
        }

        return Task.FromResult(result);
    }
}

/// <summary>
/// 本地日期时间扩展：将 DateTime 转为 Unix 时间戳（秒），供 UEditor list 操作的 mtime 字段使用
/// </summary>
internal static class DateTimeExtensions
{
    /// <summary>
    /// 将本地时间转换为 Unix 时间戳（秒）
    /// </summary>
    /// <param name="dt">DateTime</param>
    /// <returns>Unix 时间戳（秒）</returns>
    public static long ToUnixTimeSeconds(this DateTime dt) =>
        (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
}
