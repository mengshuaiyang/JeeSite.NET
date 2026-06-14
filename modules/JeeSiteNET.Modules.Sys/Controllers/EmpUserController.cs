    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;
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
[Route("api/v1/sys/emp-user")]
[Permission("sys:empUser:view")]
// 定义class EmpUserController
// 定义类：EmpUserController

public class EmpUserController : ControllerBase
{
    // 字段 _empUserRepo
    // 字段：_empUserRepo

    private readonly IEmpUserRepository _empUserRepo;
    // 字段 _employeeRepo
    // 字段：_employeeRepo

    private readonly IEmployeeRepository _employeeRepo;
    // 字段 _userRepo
    // 字段：_userRepo

    private readonly IUserRepository _userRepo;
    // 字段 _currentUser
    // 字段：_currentUser

    private readonly ICurrentUser _currentUser;

    // 构造函数 EmpUserController
    // 构造函数：EmpUserController

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
    // 方法 GetByEmpCode
    // 方法：GetByEmpCode

    public async Task<ApiResult<List<EmpUser>>> GetByEmpCode(string empCode)
    {
        var list = await _empUserRepo.GetByEmpCodeAsync(empCode);
        // return 返回结果
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [HttpGet("by-user/{userCode}")]
    // 方法 GetByUserCode
    // 方法：GetByUserCode

    public async Task<ApiResult<List<EmpUser>>> GetByUserCode(string userCode)
    {
        var list = await _empUserRepo.GetByUserCodeAsync(userCode);
        // return 返回结果
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [HttpGet("list")]
    // 方法：List

    public async Task<ApiResult<List<EmpUser>>> List([FromQuery] string? empCode, [FromQuery] string? userCode)
    {
        // 调用 Query
        var query = _empUserRepo.Query();
        // if 条件判断
        if (!string.IsNullOrEmpty(empCode))
            // 数据库操作：条件过滤
            query = query.Where(e => e.EmpCode == empCode);
        // if 条件判断
        if (!string.IsNullOrEmpty(userCode))
            // 数据库操作：条件过滤
            query = query.Where(e => e.UserCode == userCode);
        // 数据库操作：异步查询为列表
        var list = await query.ToListAsync();
        // return 返回结果
        return ApiResult<List<EmpUser>>.Ok(list);
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] EmpUserSaveRequest request)
    {
        // 缓存：获取值
        var emp = await _employeeRepo.GetAsync(request.EmpCode);
        // if 条件判断
        if (emp == null) return ApiResult.NotFound("员工不存在");

        // 缓存：获取值
        var user = await _userRepo.GetAsync(request.UserCode);
        // if 条件判断
        if (user == null) return ApiResult.NotFound("用户不存在");

        // 缓存：获取值
        var existing = await _empUserRepo.GetAsync(request.EmpCode, request.UserCode);
        // if 条件判断
        if (existing != null) return ApiResult.Ok("关联已存在");

        // 创建 EmpUser实例并赋给 entity
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
        // await 异步等待
        await _empUserRepo.AddAsync(entity);
        // return 返回结果
        return ApiResult.Ok("关联成功");
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("delete")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] EmpUserDeleteRequest request)
    {
        // await 异步等待
        await _empUserRepo.DeleteAsync(request.EmpCode, request.UserCode);
        // return 返回结果
        return ApiResult.Ok("已解除关联");
    }

    [HttpGet("available-employees")]
    // 方法 AvailableEmployees
    // 方法：AvailableEmployees

    public async Task<ApiResult<List<Employee>>> AvailableEmployees()
    {
        // 数据库操作：投影选择
        var linkedEmpCodes = await _empUserRepo.Query().Select(e => e.EmpCode).Distinct().ToListAsync();
        // 集合操作：检查是否包含
        var all = await _employeeRepo.Query().Where(e => !linkedEmpCodes.Contains(e.EmpCode)).ToListAsync();
        // return 返回结果
        return ApiResult<List<Employee>>.Ok(all);
    }

    [HttpGet("available-users")]
    // 方法 AvailableUsers
    // 方法：AvailableUsers

    public async Task<ApiResult<List<User>>> AvailableUsers()
    {
        // 数据库操作：投影选择
        var linkedUserCodes = await _empUserRepo.Query().Select(e => e.UserCode).Distinct().ToListAsync();
        // 集合操作：检查是否包含
        var all = await _userRepo.Query().Where(u => !linkedUserCodes.Contains(u.UserCode)).ToListAsync();
        // return 返回结果
        return ApiResult<List<User>>.Ok(all);
    }

    [Permission("sys:empUser:list")]
    [HttpPost("export")]
    // 方法 ExportData
    // 方法：ExportData

    public async Task<IActionResult> ExportData()
    {
        // 数据库操作：异步查询为列表
        var empUsers = await _empUserRepo.Query().ToListAsync();
        // 数据库操作：投影选择
        var rows = empUsers.Select(e => new EmpUserExportRow
        {
            EmpCode = e.EmpCode,
            UserCode = e.UserCode,
            EmpName = e.EmpName,
            LoginCode = e.LoginCode,
            UserName = e.UserName
        // 数据库操作：查询为列表
        }).ToList();

        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 声明并初始化变量：bytes
        var bytes = excel.Export(rows, "员工用户");
        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "员工用户.xlsx");
    }

    [Permission("sys:empUser:edit")]
    [HttpPost("import")]
    // 方法 ImportData
    // 方法：ImportData

    public async Task<ApiResult> ImportData(IFormFile file)
    {
        // if 条件判断
        if (file == null || file.Length == 0)
            // return 返回结果
            return ApiResult.Fail(400, "请选择上传文件");

    // 引入 var ms 命名空间
        using var ms = new MemoryStream();
        // await 异步等待
        await file.CopyToAsync(ms);
        ms.Position = 0;

        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 调用 ToArray
        var records = excel.Import<EmpUserExportRow>(ms.ToArray(), "员工用户");
        // return 返回结果
        return ApiResult.Ok(new { imported = records.Count });
    }

    [Permission("sys:empUser:edit")]
    [HttpGet("import-template")]
    // 方法 ImportTemplate
    // 方法：ImportTemplate

    public IActionResult ImportTemplate()
    {
        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 声明并初始化变量：bytes
        var bytes = excel.ExportTemplate<EmpUserExportRow>("员工用户");
        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "员工用户导入模板.xlsx");
    }
}

// 定义class EmpUserExportRow
// 定义类：EmpUserExportRow

public class EmpUserExportRow
{
    [ExcelField("员工编码", ColumnWidth = 20)]
    // 属性：EmpCode

    public string? EmpCode { get; set; }

    [ExcelField("用户编码", ColumnWidth = 20)]
    // 属性：UserCode

    public string? UserCode { get; set; }

    [ExcelField("员工姓名", ColumnWidth = 15)]
    // 属性：EmpName

    public string? EmpName { get; set; }

    [ExcelField("登录名", ColumnWidth = 20)]
    // 属性：LoginCode

    public string? LoginCode { get; set; }

    [ExcelField("用户名", ColumnWidth = 20)]
    // 属性：UserName

    public string? UserName { get; set; }
}

// 定义class EmpUserSaveRequest
// 定义类：EmpUserSaveRequest

public class EmpUserSaveRequest
{
    // 属性 EmpCode
    // 属性：EmpCode

    public string EmpCode { get; set; } = string.Empty;
    // 属性 UserCode
    // 属性：UserCode

    public string UserCode { get; set; } = string.Empty;
}

// 定义class EmpUserDeleteRequest
// 定义类：EmpUserDeleteRequest

public class EmpUserDeleteRequest
{
    // 属性 EmpCode
    // 属性：EmpCode

    public string EmpCode { get; set; } = string.Empty;
    // 属性 UserCode
    // 属性：UserCode

    public string UserCode { get; set; } = string.Empty;
}
