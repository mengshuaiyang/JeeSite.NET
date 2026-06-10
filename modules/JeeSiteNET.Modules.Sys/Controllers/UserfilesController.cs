using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/userfiles")]
[Permission("sys:userfiles:view")]
public class UserfilesController : ControllerBase
{
    private readonly IFileUploadRepository _fileUploadRepo;
    private readonly IFileEntityRepository _fileEntityRepo;
    private readonly ICurrentUser _currentUser;

    public UserfilesController(
        IFileUploadRepository fileUploadRepo,
        IFileEntityRepository fileEntityRepo,
        ICurrentUser currentUser)
    {
        _fileUploadRepo = fileUploadRepo;
        _fileEntityRepo = fileEntityRepo;
        _currentUser = currentUser;
    }

    [HttpGet("list")]
    public async Task<ApiResult<List<FileUploadDto>>> List([FromQuery] string? bizType)
    {
        var query = _fileUploadRepo.Query().Where(f => f.CreateBy == _currentUser.UserCode);
        if (!string.IsNullOrEmpty(bizType))
            query = query.Where(f => f.BizType == bizType);
        var uploads = await query.OrderByDescending(f => f.CreateDate).ToListAsync();

        var dtos = new List<FileUploadDto>();
        foreach (var u in uploads)
        {
            var fe = await _fileEntityRepo.GetAsync(u.FileId);
            dtos.Add(new FileUploadDto
            {
                UploadId = u.Id,
                FileId = u.FileId,
                FileName = u.FileName,
                FileSize = fe?.FileSize ?? 0,
                FileUrl = string.Empty,
                BizType = u.BizType,
                BizKey = u.BizKey,
            });
        }
        return ApiResult<List<FileUploadDto>>.Ok(dtos);
    }

    [HttpGet("preview/{uploadId}")]
    public async Task<IActionResult> Preview(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null || upload.CreateBy != _currentUser.UserCode)
            return NotFound();

        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        if (fe == null) return NotFound();

        var filePath = Path.Combine(StorageRoot, fe.FilePath);
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        return PhysicalFile(filePath, fe.FileContentType, upload.FileName);
    }

    [HttpDelete("{uploadId}")]
    [Permission("sys:userfiles:delete")]
    public async Task<ApiResult> Delete(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null || upload.CreateBy != _currentUser.UserCode)
            return ApiResult.NotFound("文件不存在或无权限");

        await _fileUploadRepo.DeleteAsync(uploadId);
        await _fileUploadRepo.SaveChangesAsync();
        return ApiResult.Ok("已删除");
    }

    private static string StorageRoot =>
        Path.Combine(Directory.GetCurrentDirectory(), "uploads");
}
