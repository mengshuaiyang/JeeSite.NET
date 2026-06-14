using Aliyun.OSS;
using JeeSiteNET.Core.Storage;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Infrastructure.FileStorage;

/// <summary>
/// 阿里云 OSS 存储提供者。
/// 使用阿里云官方 SDK 完成对象上传、下载、删除以及预签名 URL 访问。
/// </summary>
public class AliyunOssStorageProvider : IFileStorageProvider
{
    private readonly OssClient _client;
    private readonly string _bucket;
    private readonly ILogger<AliyunOssStorageProvider> _logger;

    /// <summary>
    /// 初始化 <see cref="AliyunOssStorageProvider"/> 的新实例。
    /// </summary>
    /// <param name="options">阿里云 OSS 配置（Endpoint、AccessKey、Bucket）。</param>
    /// <param name="logger">日志记录器。</param>
    public AliyunOssStorageProvider(AliyunOssOptions options, ILogger<AliyunOssStorageProvider> logger)
    {
        _bucket = options.Bucket;
        _logger = logger;
        _client = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
    }

    /// <summary>
    /// 获取提供者名称标识。
    /// </summary>
    public string Name => "aliyun";

    /// <summary>
    /// 上传流到阿里云 OSS，按日期分目录（yyyy/MM/dd/fileId.ext）。
    /// </summary>
    /// <param name="fileId">文件唯一标识。</param>
    /// <param name="content">文件流。</param>
    /// <param name="extension">扩展名（带点）。</param>
    /// <returns>对象键。</returns>
    public Task<string> SaveAsync(string fileId, Stream content, string extension)
    {
        var datePath = DateTime.Now.ToString("yyyy/MM/dd");
        var objectName = $"{datePath}/{fileId}{extension}";
        content.Position = 0;

        var result = _client.PutObject(_bucket, objectName, content);
        _logger.LogInformation("Uploaded {Object} to Aliyun OSS bucket {Bucket}, ETag: {ETag}",
            objectName, _bucket, result.ETag);
        return Task.FromResult(objectName);
    }

    /// <summary>
    /// 从阿里云 OSS 下载对象到内存流；读取失败时返回 null。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>可读取的内存流（已复位到开头）。</returns>
    public async Task<Stream?> GetAsync(string filePath)
    {
        try
        {
            var obj = _client.GetObject(_bucket, filePath);
            var ms = new MemoryStream();
            await obj.Content.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get {Object} from Aliyun OSS bucket {Bucket}", filePath, _bucket);
            return null;
        }
    }

    /// <summary>
    /// 删除指定对象键的阿里云 OSS 对象。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>表示异步操作的任务。</returns>
    public Task DeleteAsync(string filePath)
    {
        _client.DeleteObject(_bucket, filePath);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 生成有效期 1 小时的预签名 GET URL。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>预签名 URL 字符串。</returns>
    public string GetUrl(string filePath)
    {
        var req = new GeneratePresignedUriRequest(_bucket, filePath, SignHttpMethod.Get)
        {
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        return _client.GeneratePresignedUri(req).ToString();
    }
}
