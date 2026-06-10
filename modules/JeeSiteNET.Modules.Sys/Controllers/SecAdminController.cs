using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/sec-admin")]
[Permission("sys:secAdmin:view")]
public class SecAdminController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly ILogRepository _logRepo;
    private readonly IAuditRepository _auditRepo;
    private readonly ICurrentUser _currentUser;

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
    public async Task<ApiResult> LockUser(string userCode)
    {
        var user = await _userRepo.GetAsync(userCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.FreezeDate = DateTime.Now;
        user.FreezeCause = $"由安全管理员 {_currentUser.UserName} 锁定";
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("用户已锁定");
    }

    [HttpPost("user/{userCode}/unlock")]
    [Permission("sys:secAdmin:lock")]
    public async Task<ApiResult> UnlockUser(string userCode)
    {
        var user = await _userRepo.GetAsync(userCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.FreezeDate = null;
        user.FreezeCause = null;
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("用户已解锁");
    }

    [HttpGet("audit-logs")]
    public async Task<ApiResult<List<Audit>>> GetAuditLogs([FromQuery] int days = 7)
    {
        var since = DateTime.Now.AddDays(-days);
        var list = await _auditRepo.Query()
            .Where(a => a.CreateDate >= since)
            .OrderByDescending(a => a.CreateDate)
            .Take(500)
            .ToListAsync();
        return ApiResult<List<Audit>>.Ok(list);
    }

    [HttpGet("access-logs")]
    public async Task<ApiResult<List<Log>>> GetAccessLogs([FromQuery] int days = 7)
    {
        var since = DateTime.Now.AddDays(-days);
        var list = await _logRepo.Query()
            .Where(l => l.CreateDate >= since)
            .OrderByDescending(l => l.CreateDate)
            .Take(500)
            .ToListAsync();
        return ApiResult<List<Log>>.Ok(list);
    }

    [HttpGet("frozen-users")]
    public async Task<ApiResult<List<User>>> GetFrozenUsers()
    {
        var list = await _userRepo.Query()
            .Where(u => u.FreezeDate != null)
            .OrderByDescending(u => u.FreezeDate)
            .ToListAsync();
        return ApiResult<List<User>>.Ok(list);
    }
}
