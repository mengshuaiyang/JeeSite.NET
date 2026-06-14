namespace JeeSiteNET.Core.Storage;

/// <summary>
/// 文件存储提供者接口：抽象不同的存储后端（本地、S3/MinIO、阿里云 OSS 等），
/// 应用层仅依赖此接口即可完成文件的增删查（下载）与 URL 生成
/// </summary>
public interface IFileStorageProvider
{
    /// <summary>
    /// 存储提供者名称（如 "local" / "s3" / "aliyun"，用于区分与路由）
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 保存文件内容到存储后端
    /// </summary>
    /// <param name="fileId">文件唯一标识（如上传时生成的 id）</param>
    /// <param name="content">文件内容流</param>
    /// <param name="extension">文件扩展名（不带前导点，如 "png"）</param>
    /// <returns>存储后端返回的文件路径/Key，供后续下载或访问使用</returns>
    Task<string> SaveAsync(string fileId, Stream content, string extension);

    /// <summary>
    /// 从存储后端读取文件内容
    /// </summary>
    /// <param name="filePath">保存时返回的文件路径/Key</param>
    /// <returns>文件流（文件不存在返回 null）</returns>
    Task<Stream?> GetAsync(string filePath);

    /// <summary>
    /// 从存储后端删除指定文件
    /// </summary>
    /// <param name="filePath">保存时返回的文件路径/Key</param>
    /// <returns>Task</returns>
    Task DeleteAsync(string filePath);

    /// <summary>
    /// 获取可访问的公开/预签名 URL（返回给前端用于下载或展示）
    /// </summary>
    /// <param name="filePath">保存时返回的文件路径/Key</param>
    /// <returns>可访问 URL</returns>
    string GetUrl(string filePath);
}
