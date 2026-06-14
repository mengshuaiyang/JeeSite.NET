using JeeSiteNET.Core.Storage;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace JeeSiteNET.Infrastructure.FileStorage;

/// <summary>
/// 基于 MinIO SDK 的 S3 兼容对象存储提供者。
/// 支持文件上传、下载、删除与预签名访问，Bucket 不存在时自动创建。
/// </summary>
public class S3StorageProvider : IFileStorageProvider
{
    private readonly IMinioClient _client;
    private readonly string _bucket;
    private readonly ILogger<S3StorageProvider> _logger;

    /// <summary>
    /// 初始化 <see cref="S3StorageProvider"/> 的新实例。
    /// </summary>
    /// <param name="options">S3 配置（端点、密钥、Bucket、区域、SSL 开关）。</param>
    /// <param name="logger">日志记录器。</param>
    public S3StorageProvider(S3Options options, ILogger<S3StorageProvider> logger)
    {
        _bucket = options.Bucket;
        _logger = logger;

        _client = new MinioClient()
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(options.UseSsl)
            .WithRegion(options.Region)
            .Build();
    }

    /// <summary>
    /// 获取提供者名称标识。
    /// </summary>
    public string Name => "s3";

    /// <summary>
    /// 将流保存为 S3 对象，按日期分目录（yyyy/MM/dd/fileId.ext）。
    /// </summary>
    /// <param name="fileId">文件唯一标识。</param>
    /// <param name="content">文件流。</param>
    /// <param name="extension">扩展名（带点）。</param>
    /// <returns>保存后的对象键。</returns>
    public async Task<string> SaveAsync(string fileId, Stream content, string extension)
    {
        var bucketExists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket));
        if (!bucketExists)
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket));

        var datePath = DateTime.Now.ToString("yyyy/MM/dd");
        var objectName = $"{datePath}/{fileId}{extension}";

        content.Position = 0;
        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(content.Length));

        _logger.LogInformation("Uploaded {Object} to bucket {Bucket}", objectName, _bucket);
        return objectName;
    }

    /// <summary>
    /// 根据对象键读取文件流；对象不存在或读取失败时返回 null。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>可读取的内存流（已复位到开头）。</returns>
    public async Task<Stream?> GetAsync(string filePath)
    {
        var ms = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_bucket)
            .WithObject(filePath)
            .WithCallbackStream(s => s.CopyTo(ms));

        try
        {
            await _client.GetObjectAsync(args);
            ms.Position = 0;
            return ms;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get {Object} from bucket {Bucket}", filePath, _bucket);
            return null;
        }
    }

    /// <summary>
    /// 根据对象键删除 S3 对象。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task DeleteAsync(string filePath)
    {
        await _client.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject(filePath));
    }

    /// <summary>
    /// 获取对象的访问 URL；优先生成预签名 URL，失败则回退到 API 下载路径。
    /// </summary>
    /// <param name="filePath">对象键。</param>
    /// <returns>可访问 URL。</returns>
    public string GetUrl(string filePath)
    {
        try
        {
            var task = _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucket)
                .WithObject(filePath)
                .WithExpiry(3600));
            return task.GetAwaiter().GetResult();
        }
        catch
        {
            return $"/api/v1/sys/file/download/by-path?path={Uri.EscapeDataString(filePath)}";
        }
    }
}
