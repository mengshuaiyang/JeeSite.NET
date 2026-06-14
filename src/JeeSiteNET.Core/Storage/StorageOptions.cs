namespace JeeSiteNET.Core.Storage;

/// <summary>
/// 文件存储配置选项：通过 IConfiguration 绑定 appsettings 中的节点，
/// 指定当前使用的存储提供者及其参数
/// </summary>
public class StorageOptions
{
    /// <summary>
    /// 当前使用的存储提供者（如 "local" / "s3" / "aliyun"），默认值 "local"
    /// </summary>
    public string Provider { get; set; } = "local";

    /// <summary>
    /// S3 兼容存储（AWS S3 / MinIO）配置
    /// </summary>
    public S3Options? S3 { get; set; }

    /// <summary>
    /// 阿里云 OSS 配置
    /// </summary>
    public AliyunOssOptions? Aliyun { get; set; }
}

/// <summary>
/// S3 兼容存储连接配置（MinIO / AWS S3 / 腾讯 COS 等）
/// </summary>
public class S3Options
{
    /// <summary>
    /// S3 服务端点（如 "localhost:9000" 或 "s3.amazonaws.com"），默认值 "localhost:9000"
    /// </summary>
    public string Endpoint { get; set; } = "localhost:9000";

    /// <summary>
    /// 访问密钥 AccessKey
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>
    /// 访问密钥 SecretKey
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// 存储桶名称，默认值 "files"
    /// </summary>
    public string Bucket { get; set; } = "files";

    /// <summary>
    /// 是否启用 SSL（false 即 http），默认值 false
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// 区域（Region），默认值 "us-east-1"
    /// </summary>
    public string Region { get; set; } = "us-east-1";
}

/// <summary>
/// 阿里云对象存储 OSS 连接配置
/// </summary>
public class AliyunOssOptions
{
    /// <summary>
    /// OSS 服务端点（如 "oss-cn-hangzhou.aliyuncs.com"），默认值 "oss-cn-hangzhou.aliyuncs.com"
    /// </summary>
    public string Endpoint { get; set; } = "oss-cn-hangzhou.aliyuncs.com";

    /// <summary>
    /// 阿里云 AccessKeyId
    /// </summary>
    public string AccessKeyId { get; set; } = string.Empty;

    /// <summary>
    /// 阿里云 AccessKeySecret
    /// </summary>
    public string AccessKeySecret { get; set; } = string.Empty;

    /// <summary>
    /// Bucket 名称，默认值 "jeesite"
    /// </summary>
    public string Bucket { get; set; } = "jeesite";

    /// <summary>
    /// 区域（Region），默认值 "cn-hangzhou"
    /// </summary>
    public string Region { get; set; } = "cn-hangzhou";
}
