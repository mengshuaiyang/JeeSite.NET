    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/file")]
[Permission("sys:file")]
// 定义class FileController
// 定义类：FileController

public class FileController : ControllerBase
{
    // 字段 _fileService
    // 字段：_fileService

    private readonly FileService _fileService;
    // 字段 _chunkUploadService
    // 字段：_chunkUploadService

    private readonly ChunkUploadService _chunkUploadService;

    // 方法 FileController
    // 构造函数：FileController

    public FileController(FileService fileService, ChunkUploadService chunkUploadService)
    {
        _fileService = fileService;
        _chunkUploadService = chunkUploadService;
    }

    [HttpPost("upload")]
    // 方法：Upload

    public async Task<ApiResult<FileUploadResult>> Upload(
        IFormFile file,

        [FromQuery] string? bizType,
        [FromQuery] string? bizKey)
    {
        // return 返回结果
        return await _fileService.UploadAsync(file, bizType, bizKey);
    }

    [HttpGet("download/{uploadId}")]
    // 方法 Download
    // 方法：Download

    public async Task<IActionResult> Download(string uploadId)
    {
        var result = await _fileService.GetDownloadAsync(uploadId);
        // if 条件判断
        if (result == null) return NotFound();

        // return 返回结果
        return File(result.Stream, result.ContentType, result.FileName);
    }

    [HttpGet("biz")]
    // 方法：GetByBiz

    public async Task<ApiResult<List<FileUploadDto>>> GetByBiz(

        [FromQuery] string bizType,
        [FromQuery] string bizKey)
    {
        var list = await _fileService.GetByBizAsync(bizType, bizKey);
        // return 返回结果
        return ApiResult<List<FileUploadDto>>.Ok(list);
    }

    [HttpDelete("{uploadId}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string uploadId)
    {
        // return 返回结果
        return await _fileService.DeleteAsync(uploadId);
    }

    [HttpPost("chunk/init")]
    // 方法 ChunkInit
    // 方法：ChunkInit

    public async Task<ApiResult<ChunkInitResult>> ChunkInit([FromBody] ChunkInitRequest request)
    {
        var result = await _chunkUploadService.InitAsync(request);
        // return 返回结果
        return ApiResult<ChunkInitResult>.Ok(result);
    }

    [HttpPost("chunk/upload")]
    // 方法：ChunkUpload

    public async Task<ApiResult> ChunkUpload(

        [FromQuery] string uploadId,
        [FromQuery] int chunkIndex,
        IFormFile file)
    {
        // return 返回结果
        return await _chunkUploadService.UploadChunkAsync(uploadId, chunkIndex, file);
    }

    [HttpGet("chunk/status")]
    // 方法 ChunkStatus
    // 方法：ChunkStatus

    public ApiResult<ChunkStatusResult> ChunkStatus([FromQuery] string uploadId)
    {
        // return 返回结果
        return ApiResult<ChunkStatusResult>.Ok(_chunkUploadService.GetStatus(uploadId));
    }

    [HttpPost("chunk/merge")]
    // 方法：ChunkMerge

    public async Task<ApiResult<FileUploadResult>> ChunkMerge(

        [FromQuery] string uploadId,
        [FromQuery] string? bizType,
        [FromQuery] string? bizKey)
    {
        // return 返回结果
        return await _chunkUploadService.MergeAsync(uploadId, bizType, bizKey);
    }

    [HttpGet("download/by-path")]
    // 方法 DownloadByPath
    // 方法：DownloadByPath

    public async Task<IActionResult> DownloadByPath([FromQuery] string path)
    {
        var stream = await _fileService.GetByPathAsync(path);
        // if 条件判断
        if (stream == null) return NotFound();
        // return 返回结果
        return File(stream, "application/octet-stream");
    }
}
