using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
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

    [Permission("sys:empUser:list")]
    [HttpPost("export")]
    public async Task<IActionResult> ExportData()
    {
        var empUsers = await _empUserRepo.Query().ToListAsync();
        var rows = empUsers.Select(e => new EmpUserExportRow
        {
            EmpCode = e.EmpCode,
            UserCode = e.UserCode,
            EmpName = e.EmpName,
            LoginCode = e.LoginCode,
            UserName = e.UserName
        }).ToList();

        var excel = new ExcelService();
        var bytes = excel.Export(rows, "员工用户");
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "员工用户.xlsx");
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("import")]
    public async Task<ApiResult> ImportData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return ApiResult.Fail(400, "请选择上传文件");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var excel = new ExcelService();
        var records = excel.Import<EmpUserExportRow>(ms.ToArray(), "员工用户");
        return ApiResult.Ok(new { imported = records.Count });
    }

    [Permission("sys:empUser:edit")]
    [HttpGet("import-template")]
    public IActionResult ImportTemplate()
    {
        var excel = new ExcelService();
        var bytes = excel.ExportTemplate<EmpUserExportRow>("员工用户");
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "员工用户导入模板.xlsx");
    }
}

public class EmpUserExportRow
{
    [ExcelField("员工编码", ColumnWidth = 20)]
    public string? EmpCode { get; set; }
    [ExcelField("用户编码", ColumnWidth = 20)]
    public string? UserCode { get; set; }
    [ExcelField("员工姓名", ColumnWidth = 15)]
    public string? EmpName { get; set; }
    [ExcelField("登录名", ColumnWidth = 20)]
    public string? LoginCode { get; set; }
    [ExcelField("用户名", ColumnWidth = 20)]
    public string? UserName { get; set; }
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
