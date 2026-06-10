using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/emp-user")]
[Permission("sys:empUser:view")]
public class EmpUserController : ControllerBase
{
    private readonly IEmpUserRepository _empUserRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICurrentUser _currentUser;

    public EmpUserController(
        IEmpUserRepository empUserRepo,
        IEmployeeRepository employeeRepo,
        IUserRepository userRepo,
        ICurrentUser currentUser)
    {
        _empUserRepo = empUserRepo;
        _employeeRepo = employeeRepo;
        _userRepo = userRepo;
        _currentUser = currentUser;
    }

    [HttpGet("{empCode}")]
    public async Task<ApiResult<List<EmpUser>>> GetByEmpCode(string empCode)
    {
        var list = await _empUserRepo.GetByEmpCodeAsync(empCode);
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [HttpGet("by-user/{userCode}")]
    public async Task<ApiResult<List<EmpUser>>> GetByUserCode(string userCode)
    {
        var list = await _empUserRepo.GetByUserCodeAsync(userCode);
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [HttpGet("list")]
    public async Task<ApiResult<List<EmpUser>>> List([FromQuery] string? empCode, [FromQuery] string? userCode)
    {
        var query = _empUserRepo.Query();
        if (!string.IsNullOrEmpty(empCode))
            query = query.Where(e => e.EmpCode == empCode);
        if (!string.IsNullOrEmpty(userCode))
            query = query.Where(e => e.UserCode == userCode);
        var list = await query.ToListAsync();
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] EmpUserSaveRequest request)
    {
        var emp = await _employeeRepo.GetAsync(request.EmpCode);
        if (emp == null) return ApiResult.NotFound("员工不存在");

        var user = await _userRepo.GetAsync(request.UserCode);
        if (user == null) return ApiResult.NotFound("用户不存在");

        var existing = await _empUserRepo.GetAsync(request.EmpCode, request.UserCode);
        if (existing != null) return ApiResult.Ok("关联已存在");

        var entity = new EmpUser
        {
            EmpCode = request.EmpCode,
            UserCode = request.UserCode,
            EmpName = emp.EmpName,
            LoginCode = user.LoginCode,
            UserName = user.UserName,
            CreateBy = _currentUser.UserCode,
            CreateDate = DateTime.Now,
        };
        await _empUserRepo.AddAsync(entity);
        return ApiResult.Ok("关联成功");
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] EmpUserDeleteRequest request)
    {
        await _empUserRepo.DeleteAsync(request.EmpCode, request.UserCode);
        return ApiResult.Ok("已解除关联");
    }

    [HttpGet("available-employees")]
    public async Task<ApiResult<List<Employee>>> AvailableEmployees()
    {
        var linkedEmpCodes = await _empUserRepo.Query().Select(e => e.EmpCode).Distinct().ToListAsync();
        var all = await _employeeRepo.Query().Where(e => !linkedEmpCodes.Contains(e.EmpCode)).ToListAsync();
        return ApiResult<List<Employee>>.Ok(all);
    }

    [HttpGet("available-users")]
    public async Task<ApiResult<List<User>>> AvailableUsers()
    {
        var linkedUserCodes = await _empUserRepo.Query().Select(e => e.UserCode).Distinct().ToListAsync();
        var all = await _userRepo.Query().Where(u => !linkedUserCodes.Contains(u.UserCode)).ToListAsync();
        return ApiResult<List<User>>.Ok(all);
    }
}

public class EmpUserSaveRequest
{
    public string EmpCode { get; set; } = string.Empty;
    public string UserCode { get; set; } = string.Empty;
}

public class EmpUserDeleteRequest
{
    public string EmpCode { get; set; } = string.Empty;
    public string UserCode { get; set; } = string.Empty;
}
