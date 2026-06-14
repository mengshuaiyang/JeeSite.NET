    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/userfiles")]
[Permission("sys:userfiles:view")]
// 定义class UserfilesController
// 定义类：UserfilesController

public class UserfilesController : ControllerBase
{
    // 字段 _fileUploadRepo
    // 字段：_fileUploadRepo

    private readonly IFileUploadRepository _fileUploadRepo;
    // 字段 _fileEntityRepo
    // 字段：_fileEntityRepo

    private readonly IFileEntityRepository _fileEntityRepo;
    // 字段 _currentUser
    // 字段：_currentUser

    private readonly ICurrentUser _currentUser;

    // 构造函数 UserfilesController
    // 构造函数：UserfilesController

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
    // 方法：List

    public async Task<ApiResult<List<FileUploadDto>>> List([FromQuery] string? bizType)
    {
        // 数据库操作：条件过滤
        var query = _fileUploadRepo.Query().Where(f => f.CreateBy == _currentUser.UserCode);
        // if 条件判断
        if (!string.IsNullOrEmpty(bizType))
            // 数据库操作：条件过滤
            query = query.Where(f => f.BizType == bizType);
        // 数据库操作：降序排序
        var uploads = await query.OrderByDescending(f => f.CreateDate).ToListAsync();

        // 创建 List实例并赋给 dtos
        var dtos = new List<FileUploadDto>();
        // foreach 遍历集合
        foreach (var u in uploads)
        {
            // 缓存：获取值
            var fe = await _fileEntityRepo.GetAsync(u.FileId);
            // 集合操作：添加元素
            dtos.Add(new FileUploadDto
            {
                UploadId = u.Id,
                FileId = u.FileId,
                FileName = u.FileName,
                // null 合并操作 ??（若为 null 则使用右侧值）
                FileSize = fe?.FileSize ?? 0,
                FileUrl = string.Empty,
                BizType = u.BizType,
                BizKey = u.BizKey,
            });
        }
        // return 返回结果
        return ApiResult<List<FileUploadDto>>.Ok(dtos);
    }

    [HttpGet("preview/{uploadId}")]
    // 方法 Preview
    // 方法：Preview

    public async Task<IActionResult> Preview(string uploadId)
    {
        // 缓存：获取值
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        // if 条件判断
        if (upload == null || upload.CreateBy != _currentUser.UserCode)
            // return 返回结果
            return NotFound();

        // 缓存：获取值
        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        // if 条件判断
        if (fe == null) return NotFound();

        // 声明并初始化变量：filePath
        var filePath = Path.Combine(StorageRoot, fe.FilePath);
        // if 条件判断
        if (!System.IO.File.Exists(filePath))
            // return 返回结果
            return NotFound();

        // return 返回结果
        return PhysicalFile(filePath, fe.FileContentType, upload.FileName);
    }

    [HttpDelete("{uploadId}")]
    [Permission("sys:userfiles:delete")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string uploadId)
    {
        // 缓存：获取值
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        // if 条件判断
        if (upload == null || upload.CreateBy != _currentUser.UserCode)
            // return 返回结果
            return ApiResult.NotFound("文件不存在或无权限");

        // await 异步等待
        await _fileUploadRepo.DeleteAsync(uploadId);
        // await 异步等待
        await _fileUploadRepo.SaveChangesAsync();
        // return 返回结果
        return ApiResult.Ok("已删除");
    }

    private static string StorageRoot =>
        Path.Combine(Directory.GetCurrentDirectory(), "uploads");
}
