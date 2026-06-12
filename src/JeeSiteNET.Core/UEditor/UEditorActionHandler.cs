using JeeSiteNET.Core.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 协议处理入口 — 处理 config/uploadimage/uploadscrawl/uploadvideo/uploadfile/catchimage/listimage/listfile 所有 action。
/// 
/// 使用示例（在 Controller 中）:
/// <code>
/// var result = await _handler.HandleAsync(HttpContext);
/// return Content(result, "application/json; charset=utf-8");
/// </code>
/// </summary>
public sealed class UEditorActionHandler
{
    private readonly UEditorOptions _opts;
    private readonly IUEditorUploadStore _store;
    private readonly IHttpClientFactory? _httpFactory;
    private static readonly ConcurrentDictionary<string, DateTimeOffset> _rateLimitBucket = new();

    public UEditorActionHandler(UEditorOptions options, IUEditorUploadStore store, IHttpClientFactory? httpFactory = null)
    {
        _opts = options ?? throw new ArgumentNullException(nameof(options));
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _httpFactory = httpFactory;
    }

    /// <summary>
    /// 根据 HttpContext.Request.Query["action"] 分派处理器。
    /// 返回 UEditor 要求的 JSON 字符串。
    /// </summary>
    public async Task<string> HandleAsync(HttpContext context)
    {
        var action = UEditorActionMap.Parse(context.Request.Query["action"]);
        if (action == null) return JsonError("不支持的 action");

        try
        {
            return action switch
            {
                UEditorAction.Config => HandleConfig(context),
                UEditorAction.UploadImage => await HandleUploadImage(context),
                UEditorAction.UploadScrawl => await HandleUploadScrawl(context),
                UEditorAction.UploadVideo => await HandleUploadVideo(context),
                UEditorAction.UploadFile => await HandleUploadFile(context),
                UEditorAction.CatchImage => await HandleCatchImage(context),
                UEditorAction.ListImage => await HandleListImage(context),
                UEditorAction.ListFile => await HandleListFile(context),
                _ => JsonError("未知 action")
            };
        }
        catch (Exception ex)
        {
            return JsonError("服务器错误: " + ex.Message);
        }
    }

    /* ============== 核心处理器 ============== */

    private string HandleConfig(HttpContext context)
    {
        // 返回 UEditor 要求的 config.json 内容
        var config = new
        {
            imageActionName = _opts.ImageActionName,
            imageFieldName = _opts.ImageFieldName,
            imageMaxSize = _opts.ImageMaxSize,
            imageAllowFiles = _opts.ImageAllowFiles,
            imageCompressEnable = _opts.ImageCompressEnable,
            imageCompressBorder = _opts.ImageCompressBorder,
            imageInsertAlign = _opts.ImageInsertAlign,
            imagePathFormat = _opts.ImagePathFormat,
            imageUrlPrefix = _opts.ImageUrlPrefix,

            scrawlActionName = _opts.ScrawlActionName,
            scrawlFieldName = _opts.ScrawlFieldName,
            scrawlPathFormat = _opts.ScrawlPathFormat,
            scrawlMaxSize = _opts.ScrawlMaxSize,
            scrawlAllowFiles = _opts.ScrawlAllowFiles,
            scrawlUrlPrefix = _opts.ScrawlUrlPrefix,
            scrawlInsertAlign = _opts.ScrawlInsertAlign,

            videoActionName = _opts.VideoActionName,
            videoFieldName = _opts.VideoFieldName,
            videoPathFormat = _opts.VideoPathFormat,
            videoMaxSize = _opts.VideoMaxSize,
            videoAllowFiles = _opts.VideoAllowFiles,
            videoUrlPrefix = _opts.VideoUrlPrefix,

            fileActionName = _opts.FileActionName,
            fileFieldName = _opts.FileFieldName,
            filePathFormat = _opts.FilePathFormat,
            fileMaxSize = _opts.FileMaxSize,
            fileAllowFiles = _opts.FileAllowFiles,
            fileUrlPrefix = _opts.FileUrlPrefix,

            catcherActionName = _opts.CatcherActionName,
            catcherFieldName = _opts.CatcherFieldName,
            catcherUrlPrefix = _opts.CatcherUrlPrefix,
            catcherPathFormat = _opts.CatcherPathFormat,
            catcherMaxSize = _opts.CatcherMaxSize,
            catcherAllowFiles = _opts.CatcherAllowFiles,

            imageManagerActionName = _opts.ImageManagerActionName,
            imageManagerListPath = _opts.ImageManagerListPath,
            imageManagerListSize = _opts.ImageManagerListSize,
            imageManagerUrlPrefix = _opts.ImageManagerUrlPrefix,
            imageManagerInsertAlign = _opts.ImageManagerInsertAlign,
            imageManagerAllowFiles = _opts.ImageManagerAllowFiles,

            fileManagerActionName = _opts.FileManagerActionName,
            fileManagerListPath = _opts.FileManagerListPath,
            fileManagerListSize = _opts.FileManagerListSize,
            fileManagerUrlPrefix = _opts.FileManagerUrlPrefix,
            fileManagerAllowFiles = _opts.FileManagerAllowFiles,
        };
        return JsonSerializer.Serialize(config);
    }

    private async Task<string> HandleUploadImage(HttpContext context)
    {
        var file = context.Request.Form.Files.GetFile(_opts.ImageFieldName);
        if (file == null) return JsonError("请选择文件");
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_opts.ImageAllowFiles.Contains(extension))
            return JsonError("不允许的图片扩展名");
        if (file.Length > _opts.ImageMaxSize) return JsonError("图片大小超出限制");

        // ---- 安全校验: 文件名清洗 + 文件签名 ----
        var safeFileName = FileSecurityUtil.SanitizeFileName(file.FileName);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();
        if (!FileSecurityUtil.IsFileContentSafe(bytes, extension))
            return JsonError("文件内容与扩展名不匹配");

        var path = UEditorPathFormatter.WithExtension(
            UEditorPathFormatter.Format(_opts.ImagePathFormat),
            extension);
        using var storeStream = new MemoryStream(bytes);
        var result = await _store.StoreAsync(path, storeStream, file.ContentType, bytes.Length);

        return JsonSerializer.Serialize(new UEditorUploadResult
        {
            State = "SUCCESS",
            Url = result.Url,
            Title = Path.GetFileName(path),
            Original = safeFileName,
            Type = extension,
            Size = bytes.Length
        });
    }

    private async Task<string> HandleUploadScrawl(HttpContext context)
    {
        // scrawl 是经过 base64 编码的 PNG 字符串
        var form = context.Request.Form;
        var base64 = form[_opts.ScrawlFieldName].ToString();
        if (string.IsNullOrWhiteSpace(base64)) return JsonError("请粘贴涂鸦内容");

        byte[] bytes;
        try { bytes = Convert.FromBase64String(base64); }
        catch { return JsonError("涂鸦内容格式错误"); }

        if (bytes.Length > _opts.ScrawlMaxSize) return JsonError("涂鸦大小超出限制");

        // ---- 安全校验: PNG magic number (89 50 4E 47 0D 0A 1A 0A) ----
        if (!FileSecurityUtil.IsFileContentSafe(bytes, ".png"))
            return JsonError("涂鸦内容不是合法的图片");

        var path = UEditorPathFormatter.WithExtension(
            UEditorPathFormatter.Format(_opts.ScrawlPathFormat), ".png");
        using var ms = new MemoryStream(bytes);
        var result = await _store.StoreAsync(path, ms, "image/png", bytes.Length);

        return JsonSerializer.Serialize(new UEditorUploadResult
        {
            State = "SUCCESS",
            Url = result.Url,
            Title = Path.GetFileName(path),
            Original = Path.GetFileName(path),
            Type = ".png",
            Size = bytes.Length
        });
    }

    private async Task<string> HandleUploadVideo(HttpContext context)
    {
        var file = context.Request.Form.Files.GetFile(_opts.VideoFieldName);
        if (file == null) return JsonError("请选择文件");
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_opts.VideoAllowFiles.Contains(extension))
            return JsonError("不允许的视频扩展名");
        if (file.Length > _opts.VideoMaxSize) return JsonError("视频大小超出限制");

        var safeFileName = FileSecurityUtil.SanitizeFileName(file.FileName);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();
        if (!FileSecurityUtil.IsFileContentSafe(bytes, extension))
            return JsonError("文件内容与扩展名不匹配");

        var path = UEditorPathFormatter.WithExtension(
            UEditorPathFormatter.Format(_opts.VideoPathFormat),
            extension);
        using var storeStream = new MemoryStream(bytes);
        var result = await _store.StoreAsync(path, storeStream, file.ContentType, bytes.Length);

        return JsonSerializer.Serialize(new UEditorUploadResult
        {
            State = "SUCCESS",
            Url = result.Url,
            Title = Path.GetFileName(path),
            Original = safeFileName,
            Type = extension,
            Size = bytes.Length
        });
    }

    private async Task<string> HandleUploadFile(HttpContext context)
    {
        var file = context.Request.Form.Files.GetFile(_opts.FileFieldName);
        if (file == null) return JsonError("请选择文件");
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_opts.FileAllowFiles.Contains(extension))
            return JsonError("不允许的文件扩展名");
        if (file.Length > _opts.FileMaxSize) return JsonError("文件大小超出限制");

        var safeFileName = FileSecurityUtil.SanitizeFileName(file.FileName);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();
        if (!FileSecurityUtil.IsFileContentSafe(bytes, extension))
            return JsonError("文件内容与扩展名不匹配");

        var path = UEditorPathFormatter.WithExtension(
            UEditorPathFormatter.Format(_opts.FilePathFormat),
            extension);
        using var storeStream = new MemoryStream(bytes);
        var result = await _store.StoreAsync(path, storeStream, file.ContentType, bytes.Length);

        return JsonSerializer.Serialize(new UEditorUploadResult
        {
            State = "SUCCESS",
            Url = result.Url,
            Title = Path.GetFileName(path),
            Original = safeFileName,
            Type = extension,
            Size = bytes.Length
        });
    }

    private async Task<string> HandleCatchImage(HttpContext context)
    {
        var sources = context.Request.Form[_opts.CatcherFieldName].ToList();
        if (sources.Count == 0) return JsonError("缺少 source 参数");

        var httpClient = _httpFactory != null ? _httpFactory.CreateClient() : new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(15);

        var list = new List<UEditorCatcherEntry>();
        foreach (var src in sources)
        {
            if (string.IsNullOrWhiteSpace(src)) continue;
            try
            {
                // ---- SSRF 防护: 只允许 http/https, 拒绝内网 IP, file:// 等 ----
                if (!Uri.TryCreate(src, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "不支持的协议" });
                    continue;
                }
                var host = uri.Host;
                if (host == "localhost" || host.StartsWith("127.") || host.StartsWith("192.168.") ||
                    host.StartsWith("10.") || host.StartsWith("172.") ||
                    host.Equals("::1", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "禁止访问内网地址" });
                    continue;
                }

                var bytes = await httpClient.GetByteArrayAsync(src);
                if (bytes.Length > _opts.CatcherMaxSize) { list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "大小超出限制" }); continue; }
                var ext = GuessImageExtension(bytes, src);
                if (!_opts.CatcherAllowFiles.Contains(ext)) { list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "不允许的扩展名" }); continue; }

                if (!FileSecurityUtil.IsFileContentSafe(bytes, ext))
                {
                    list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "文件内容与扩展名不匹配" });
                    continue;
                }

                var path = UEditorPathFormatter.WithExtension(
                    UEditorPathFormatter.Format(_opts.CatcherPathFormat), ext);
                using var ms = new MemoryStream(bytes);
                var stored = await _store.StoreAsync(path, ms, "image/" + ext.TrimStart('.'), bytes.Length);

                list.Add(new UEditorCatcherEntry { Url = stored.Url, Source = src, State = "SUCCESS" });
            }
            catch (Exception ex)
            {
                list.Add(new UEditorCatcherEntry { Url = src, Source = src, State = "抓取失败: " + ex.Message });
            }
        }

        return JsonSerializer.Serialize(new UEditorCatcherResult { State = "SUCCESS", List = list });
    }

    private async Task<string> HandleListImage(HttpContext context)
    {
        int start = int.TryParse(context.Request.Query["start"], out var s) ? s : 0;
        int size = int.TryParse(context.Request.Query["size"], out var sz) ? sz : _opts.ImageManagerListSize;

        var entries = await _store.ListAsync(_opts.ImageManagerListPath, start, size, _opts.ImageManagerAllowFiles);
        return JsonSerializer.Serialize(new UEditorListResult
        {
            State = "SUCCESS",
            Start = start,
            Total = entries.Count,
            List = entries
        });
    }

    private async Task<string> HandleListFile(HttpContext context)
    {
        int start = int.TryParse(context.Request.Query["start"], out var s) ? s : 0;
        int size = int.TryParse(context.Request.Query["size"], out var sz) ? sz : _opts.FileManagerListSize;

        var entries = await _store.ListAsync(_opts.FileManagerListPath, start, size, _opts.FileManagerAllowFiles);
        return JsonSerializer.Serialize(new UEditorListResult
        {
            State = "SUCCESS",
            Start = start,
            Total = entries.Count,
            List = entries
        });
    }

    /* ============== 辅助 ============== */

    private static string JsonError(string message) =>
        JsonSerializer.Serialize(new UEditorResult { State = message });

    private static string GuessImageExtension(byte[] header, string url)
    {
        // 先根据 magic number 判断
        if (header.Length >= 4)
        {
            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47) return ".png";
            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF) return ".jpg";
            if (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46) return ".gif";
            if (header[0] == 0x42 && header[1] == 0x4D) return ".bmp";
            if (header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F' &&
                header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P') return ".webp";
        }
        // fallback: 按 URL 判断
        var ext = Path.GetExtension(url).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext)) ext = ".png";
        return ext;
    }
}
