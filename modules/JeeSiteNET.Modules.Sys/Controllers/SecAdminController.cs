    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
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
[Route("api/v1/sys/sec-admin")]
[Permission("sys:secAdmin:view")]
// 定义class SecAdminController
// 定义类：SecAdminController

public class SecAdminController : ControllerBase
{
    // 字段 _userRepo
    // 字段：_userRepo

    private readonly IUserRepository _userRepo;
    // 字段 _logRepo
    // 字段：_logRepo

    private readonly ILogRepository _logRepo;
    // 字段 _auditRepo
    // 字段：_auditRepo

    private readonly IAuditRepository _auditRepo;
    // 字段 _currentUser
    // 字段：_currentUser

    private readonly ICurrentUser _currentUser;

    // 构造函数 SecAdminController
    // 构造函数：SecAdminController

    public SecAdminController(
        IUserRepository userRepo,
        ILogRepository logRepo,
        IAuditRepository auditRepo,
        ICurrentUser currentUser)
    {
        _userRepo = userRepo;
        _logRepo = logRepo;
        _auditRepo = auditRepo;
        _currentUser = currentUser;
    }

    [HttpPost("user/{userCode}/lock")]
    [Permission("sys:secAdmin:lock")]
    // 方法 LockUser
    // 方法：LockUser

    public async Task<ApiResult> LockUser(string userCode)
    {
        // 缓存：获取值
        var user = await _userRepo.GetAsync(userCode);
        // if 条件判断
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.FreezeDate = DateTime.Now;
        user.FreezeCause = $"由安全管理员 {_currentUser.UserName} 锁定";
        // await 异步等待
        await _userRepo.UpdateAsync(user);
        // return 返回结果
        return ApiResult.Ok("用户已锁定");
    }

    [HttpPost("user/{userCode}/unlock")]
    [Permission("sys:secAdmin:lock")]
    // 方法 UnlockUser
    // 方法：UnlockUser

    public async Task<ApiResult> UnlockUser(string userCode)
    {
        // 缓存：获取值
        var user = await _userRepo.GetAsync(userCode);
        // if 条件判断
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.FreezeDate = null;
        user.FreezeCause = null;
        // await 异步等待
        await _userRepo.UpdateAsync(user);
        // return 返回结果
        return ApiResult.Ok("用户已解锁");
    }

    [HttpGet("audit-logs")]
    // 方法 GetAuditLogs
    // 方法：GetAuditLogs

    public async Task<ApiResult<List<Audit>>> GetAuditLogs([FromQuery] int days = 7)
    {
        // 声明并初始化变量：since
        var since = DateTime.Now.AddDays(-days);
        // 调用 Query
        var list = await _auditRepo.Query()
            // 数据库操作：条件过滤
            .Where(a => a.CreateDate >= since)
            // 数据库操作：降序排序
            .OrderByDescending(a => a.CreateDate)
            .Take(500)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<Audit>>.Ok(list);
    }

    [HttpGet("access-logs")]
    // 方法 GetAccessLogs
    // 方法：GetAccessLogs

    public async Task<ApiResult<List<Log>>> GetAccessLogs([FromQuery] int days = 7)
    {
        // 声明并初始化变量：since
        var since = DateTime.Now.AddDays(-days);
        // 调用 Query
        var list = await _logRepo.Query()
            // 数据库操作：条件过滤
            .Where(l => l.CreateDate >= since)
            // 数据库操作：降序排序
            .OrderByDescending(l => l.CreateDate)
            .Take(500)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<Log>>.Ok(list);
    }

    [HttpGet("frozen-users")]
    // 方法 GetFrozenUsers
    // 方法：GetFrozenUsers

    public async Task<ApiResult<List<User>>> GetFrozenUsers()
    {
        // 调用 Query
        var list = await _userRepo.Query()
            // 数据库操作：条件过滤
            .Where(u => u.FreezeDate != null)
            // 数据库操作：降序排序
            .OrderByDescending(u => u.FreezeDate)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<User>>.Ok(list);
    }
}
