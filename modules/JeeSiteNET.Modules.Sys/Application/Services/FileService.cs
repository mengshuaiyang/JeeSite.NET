using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Storage;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class FileService
{
    private readonly IFileEntityRepository _fileEntityRepo;
    private readonly IFileUploadRepository _fileUploadRepo;
    private readonly IFileStorageProvider _storage;
    private readonly ICurrentUser _currentUser;

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

    public async Task<ApiResult<FileUploadResult>> UploadAsync(IFormFile file, string? bizType = null, string? bizKey = null)
    {
        if (file == null || file.Length == 0)
            return ApiResult<FileUploadResult>.Fail(400, "请选择文件");

        var now = DateTime.Now;
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var md5 = ComputeMd5(bytes);
        var existing = await _fileEntityRepo.GetByMd5Async(md5);

        string fileId;
        string filePath;

        if (existing != null)
        {
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
            FileName = file.FileName,
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

    public async Task<ApiResult> DeleteAsync(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return ApiResult.NotFound("文件不存在");

        await _fileUploadRepo.DeleteAsync(uploadId);
        await _fileUploadRepo.SaveChangesAsync();
        return ApiResult.Ok();
    }

    public async Task<Stream?> GetByPathAsync(string path)
    {
        return await _storage.GetAsync(path);
    }

    private static string ComputeMd5(byte[] bytes)
    {
        var hash = MD5.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }
}

public class FileUploadResult
{
    public string UploadId { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}

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

public class FileDownloadResult
{
    public Stream Stream { get; set; } = Stream.Null;
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = string.Empty;
}

public class FileExistResult
{
    public string UploadId { get; set; } = string.Empty;
    public string UploadId2 { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}
