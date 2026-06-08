using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/file")]
[Permission("sys:file")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly ChunkUploadService _chunkUploadService;

    public FileController(FileService fileService, ChunkUploadService chunkUploadService)
    {
        _fileService = fileService;
        _chunkUploadService = chunkUploadService;
    }

    [HttpPost("upload")]
    public async Task<ApiResult<FileUploadResult>> Upload(
        IFormFile file,
        [FromQuery] string? bizType,
        [FromQuery] string? bizKey)
    {
        return await _fileService.UploadAsync(file, bizType, bizKey);
    }

    [HttpGet("download/{uploadId}")]
    public async Task<IActionResult> Download(string uploadId)
    {
        var result = await _fileService.GetDownloadAsync(uploadId);
        if (result == null) return NotFound();

        return File(result.Stream, result.ContentType, result.FileName);
    }

    [HttpGet("biz")]
    public async Task<ApiResult<List<FileUploadDto>>> GetByBiz(
        [FromQuery] string bizType,
        [FromQuery] string bizKey)
    {
        var list = await _fileService.GetByBizAsync(bizType, bizKey);
        return ApiResult<List<FileUploadDto>>.Ok(list);
    }

    [HttpDelete("{uploadId}")]
    public async Task<ApiResult> Delete(string uploadId)
    {
        return await _fileService.DeleteAsync(uploadId);
    }

    [HttpPost("chunk/init")]
    public async Task<ApiResult<ChunkInitResult>> ChunkInit([FromBody] ChunkInitRequest request)
    {
        var result = await _chunkUploadService.InitAsync(request);
        return ApiResult<ChunkInitResult>.Ok(result);
    }

    [HttpPost("chunk/upload")]
    public async Task<ApiResult> ChunkUpload(
        [FromQuery] string uploadId,
        [FromQuery] int chunkIndex,
        IFormFile file)
    {
        return await _chunkUploadService.UploadChunkAsync(uploadId, chunkIndex, file);
    }

    [HttpGet("chunk/status")]
    public ApiResult<ChunkStatusResult> ChunkStatus([FromQuery] string uploadId)
    {
        return ApiResult<ChunkStatusResult>.Ok(_chunkUploadService.GetStatus(uploadId));
    }

    [HttpPost("chunk/merge")]
    public async Task<ApiResult<FileUploadResult>> ChunkMerge(
        [FromQuery] string uploadId,
        [FromQuery] string? bizType,
        [FromQuery] string? bizKey)
    {
        return await _chunkUploadService.MergeAsync(uploadId, bizType, bizKey);
    }
}
