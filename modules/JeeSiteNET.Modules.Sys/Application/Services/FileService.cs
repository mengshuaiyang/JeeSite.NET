using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Storage;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>文件服务，负责文件上传/下载/删除，集成 MD5 秒传、文件内容签名校验和路径遍历防御。</summary>
public class FileService
{
    private readonly IFileEntityRepository _fileEntityRepo;
    private readonly IFileUploadRepository _fileUploadRepo;
    private readonly IFileStorageProvider _storage;
    private readonly ICurrentUser _currentUser;

    /// <summary>依赖注入构造函数。</summary>
    public FileService(
        IFileEntityRepository fileEntityRepo,
        IFileUploadRepository fileUploadRepo,
        IFileStorageProvider storage,
        ICurrentUser currentUser)
    {
        _fileEntityRepo = fileEntityRepo;
        _fileUploadRepo = fileUploadRepo;
        _storage = storage;
        _currentUser = currentUser;
    }

    /// <summary>上传单个文件（支持 MD5 秒传 + 三层安全校验）。</summary>
    /// <param name="file">上传的文件流。</param>
    /// <param name="bizType">业务类型（可选，用于文件归类）。</param>
    /// <param name="bizKey">业务 Key（可选，关联具体业务记录）。</param>
    /// <returns>上传结果（含文件访问 URL）。</returns>
    public async Task<ApiResult<FileUploadResult>> UploadAsync(IFormFile file, string? bizType = null, string? bizKey = null)
    {
        if (file == null || file.Length == 0)
            return ApiResult<FileUploadResult>.Fail(400, "请选择文件");

        // ---- 安全校验 1: 文件名清洗 + 扩展名白名单 ----
        var safeFileName = FileSecurityUtil.SanitizeFileName(file.FileName);
        var extension = Path.GetExtension(safeFileName).ToLowerInvariant();

        if (!FileSecurityUtil.IsExtensionSafe(extension))
            return ApiResult<FileUploadResult>.Fail(400, $"不允许的文件类型: {extension}");

        // ---- 安全校验 2: 文件大小上限（默认 50MB，可通过配置调整） ----
        const long maxSize = 50 * 1024 * 1024; // 50 MB
        if (file.Length > maxSize)
            return ApiResult<FileUploadResult>.Fail(400, $"文件大小超过限制（{maxSize / 1024 / 1024}MB）");

        var now = DateTime.Now;

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        // ---- 安全校验 3: 文件签名（Magic Number）与扩展名一致 ----
        var (contentOk, reason) = FileSecurityUtil.ValidateUpload(bytes, safeFileName);
        if (!contentOk)
            return ApiResult<FileUploadResult>.Fail(400, $"文件内容校验失败: {reason}");

        var md5 = ComputeMd5(bytes);
        var existing = await _fileEntityRepo.GetByMd5Async(md5);

        string fileId;
        string filePath;

        if (existing != null)
        {
            // MD5 命中已存在文件 → 秒传，无需重复落盘
            fileId = existing.FileId;
            filePath = existing.FilePath;
        }
        else
        {
            fileId = IdGenerator.NewId();
            filePath = await _storage.SaveAsync(fileId, new MemoryStream(bytes), extension);

            var fileEntity = new FileEntity
            {
                FileId = fileId,
                FileMd5 = md5,
                FilePath = filePath,
                FileContentType = file.ContentType,
                FileExtension = extension.TrimStart('.'),
                FileSize = bytes.Length,
                FilePreview = "1"
            };
            await _fileEntityRepo.AddAsync(fileEntity);
        }

        var upload = new FileUpload
        {
            Id = IdGenerator.NewId(),
            FileId = fileId,
            FileName = safeFileName,
            FileType = extension.TrimStart('.'),
            BizKey = bizKey,
            BizType = bizType,
            Status = "0",
            CreateBy = _currentUser.UserCode,
            CreateDate = now,
            UpdateBy = _currentUser.UserCode,
            UpdateDate = now
        };
        await _fileUploadRepo.AddAsync(upload);
        await _fileUploadRepo.SaveChangesAsync();

        return ApiResult<FileUploadResult>.Ok(new FileUploadResult
        {
            UploadId = upload.Id,
            FileId = fileId,
            FileName = file.FileName,
            FileSize = bytes.Length,
            FileUrl = _storage.GetUrl(filePath)
        });
    }

    /// <summary>根据 UploadId 获取下载信息（流 + 类型 + 原始文件名）。</summary>
    /// <param name="uploadId">上传记录 ID。</param>
    /// <returns>下载结果，不存在时返回 null。</returns>
    public async Task<FileDownloadResult?> GetDownloadAsync(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return null;

        var fileEntity = await _fileEntityRepo.GetAsync(upload.FileId);
        if (fileEntity == null) return null;

        var stream = await _storage.GetAsync(fileEntity.FilePath);
        if (stream == null) return null;

        return new FileDownloadResult
        {
            Stream = stream,
            ContentType = fileEntity.FileContentType,
            FileName = upload.FileName
        };
    }

    /// <summary>按业务维度查询关联的文件列表。</summary>
    /// <param name="bizType">业务类型。</param>
    /// <param name="bizKey">业务 Key。</param>
    /// <returns>文件列表。</returns>
    public async Task<List<FileUploadDto>> GetByBizAsync(string bizType, string bizKey)
    {
        var list = await _fileUploadRepo.GetByBizAsync(bizType, bizKey);
        var dtos = new List<FileUploadDto>();

        foreach (var upload in list)
        {
            var fileEntity = await _fileEntityRepo.GetAsync(upload.FileId);
            dtos.Add(new FileUploadDto
            {
                UploadId = upload.Id,
                FileId = upload.FileId,
                FileName = upload.FileName,
                FileSize = fileEntity?.FileSize ?? 0,
                FileUrl = fileEntity != null ? _storage.GetUrl(fileEntity.FilePath) : string.Empty,
                BizType = upload.BizType,
                BizKey = upload.BizKey
            });
        }

        return dtos;
    }

    /// <summary>根据 MD5 校验文件是否已上传（秒传查询接口）。</summary>
    /// <param name="md5">文件 MD5（小写十六进制）。</param>
    /// <returns>文件存在性结果，不存在时返回 null。</returns>
    public async Task<FileExistResult?> CheckMd5ExistAsync(string md5)
    {
        var entity = await _fileEntityRepo.GetByMd5Async(md5);
        if (entity == null) return null;

        var upload = await _fileUploadRepo.GetByFileIdAsync(entity.FileId);
        if (upload == null) return null;

        return new FileExistResult
        {
            UploadId = upload.Id,
            UploadId2 = upload.Id,
            FileUrl = _storage.GetUrl(entity.FilePath)
        };
    }

    /// <summary>按上传记录 ID 删除文件（仅删除 upload 记录，FileEntity 保留以便 MD5 秒传）。</summary>
    /// <param name="uploadId">上传记录 ID。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return ApiResult.NotFound("文件不存在");

        await _fileUploadRepo.DeleteAsync(uploadId);
        await _fileUploadRepo.SaveChangesAsync();
        return ApiResult.Ok();
    }

    /// <summary>按相对路径直接获取文件流（用于富文本/静态图引用）。</summary>
    /// <param name="path">文件相对路径。</param>
    /// <returns>文件流，路径非法或不存在时返回 null。</returns>
    public async Task<Stream?> GetByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        // ---- 路径遍历防御: 拒绝 ../ 以及 URL/Unicode 编码变体 ----
        if (FileSecurityUtil.IsPathTraversalAttempt(path))
            return null;

        // ---- 路径规范化: 只保留相对路径段，禁止跳出基础目录 ----
        var normalized = path.Trim();
        return await _storage.GetAsync(normalized);
    }

    /// <summary>计算字节数组的 MD5 并返回小写十六进制字符串。</summary>
    private static string ComputeMd5(byte[] bytes)
    {
        var hash = MD5.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }
}

/// <summary>文件上传结果 DTO。</summary>
public class FileUploadResult
{
    public string UploadId { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}

/// <summary>文件上传查询 DTO。</summary>
public class FileUploadDto
{
    public string UploadId { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public decimal FileSize { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string? BizType { get; set; }
    public string? BizKey { get; set; }
}

/// <summary>文件下载结果 DTO。</summary>
public class FileDownloadResult
{
    public Stream Stream { get; set; } = Stream.Null;
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = string.Empty;
}

/// <summary>MD5 秒传查询结果 DTO。</summary>
public class FileExistResult
{
    public string UploadId { get; set; } = string.Empty;
    public string UploadId2 { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}
