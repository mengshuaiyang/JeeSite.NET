using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 员工信息 DTO。
/// </summary>
public class EmployeeDto
{
    /// <summary>
    /// 员工编码（主键）。
    /// </summary>
    public string EmpCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工工号。
    /// </summary>
    public string? EmpNo { get; set; }

    /// <summary>
    /// 员工姓名。
    /// </summary>
    public string EmpName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名/拼音名。
    /// </summary>
    public string? EmpNameEn { get; set; }

    /// <summary>
    /// 所属办公室/部门编码。
    /// </summary>
    public string OfficeCode { get; set; } = string.Empty;

    /// <summary>
    /// 所属办公室/部门名称。
    /// </summary>
    public string OfficeName { get; set; } = string.Empty;

    /// <summary>
    /// 所属公司编码。
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 所属公司名称。
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string Status { get; set; } = "0";

    /// <summary>
    /// 备注说明。
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 员工的岗位编码列表。
    /// </summary>
    public List<string> PostCodes { get; set; } = new();

    /// <summary>
    /// 员工的多机构信息（跨部门兼职）。
    /// </summary>
    public List<EmployeeOfficeDto> Offices { get; set; } = new();
}

/// <summary>
/// 员工保存请求 DTO。
/// </summary>
public class EmployeeSaveDto
{
    /// <summary>
    /// 员工编码；空表示新建。
    /// </summary>
    public string? EmpCode { get; set; }

    /// <summary>
    /// 员工工号。
    /// </summary>
    public string? EmpNo { get; set; }

    /// <summary>
    /// 员工姓名。
    /// </summary>
    public string EmpName { get; set; } = string.Empty;

    /// <summary>
    /// 员工英文名。
    /// </summary>
    public string? EmpNameEn { get; set; }

    /// <summary>
    /// 所属机构编码。
    /// </summary>
    public string OfficeCode { get; set; } = string.Empty;

    /// <summary>
    /// 所属公司编码。
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 备注。
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 岗位编码列表。
    /// </summary>
    public List<string> PostCodes { get; set; } = new();

    /// <summary>
    /// 多机构（兼职）信息。
    /// </summary>
    public List<EmployeeOfficeSaveDto> Offices { get; set; } = new();
}

/// <summary>
/// 员工-机构（办公室）关联 DTO。
/// </summary>
public class EmployeeOfficeDto
{
    /// <summary>
    /// 主键标识。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 员工编码。
    /// </summary>
    public string EmpCode { get; set; } = string.Empty;

    /// <summary>
    /// 机构（办公室）编码。
    /// </summary>
    public string OfficeCode { get; set; } = string.Empty;

    /// <summary>
    /// 机构名称。
    /// </summary>
    public string OfficeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联岗位编码。
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    /// 关联岗位名称。
    /// </summary>
    public string? PostName { get; set; }
}

/// <summary>
/// 员工-机构（办公室）保存 DTO。
/// </summary>
public class EmployeeOfficeSaveDto
{
    /// <summary>
    /// 主键标识；空表示新建。
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 机构编码。
    /// </summary>
    public string OfficeCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码。
    /// </summary>
    public string? PostCode { get; set; }
}

/// <summary>
/// 员工相关 DTO 转换扩展方法。
/// </summary>
public static class EmployeeMapping
{
    /// <summary>
    /// 将 <see cref="Employee"/> 实体转换为 <see cref="EmployeeDto"/>。
    /// </summary>
    /// <param name="e">员工实体。</param>
    /// <param name="postCodes">员工对应的岗位编码列表。</param>
    /// <param name="offices">员工的多机构（办公室）信息。</param>
    /// <returns>员工信息 DTO。</returns>
    public static EmployeeDto ToDto(this Employee e, List<string> postCodes, List<EmployeeOfficeDto> offices)
        => new()
        {
            EmpCode = e.EmpCode,
            EmpNo = e.EmpNo,
            EmpName = e.EmpName,
            EmpNameEn = e.EmpNameEn,
            OfficeCode = e.OfficeCode,
            OfficeName = e.OfficeName,
            CompanyCode = e.CompanyCode,
            CompanyName = e.CompanyName,
            Status = e.Status ?? "0",
            Remarks = e.Remarks,
            PostCodes = postCodes,
            Offices = offices
        };

    /// <summary>
    /// 将 <see cref="EmployeeSaveDto"/> 转换为 <see cref="Employee"/> 实体（附带必要的默认值）。
    /// </summary>
    /// <param name="dto">保存请求 DTO。</param>
    /// <returns>员工实体。</returns>
    public static Employee ToEntity(this EmployeeSaveDto dto)
    {
        var now = DateTime.Now;
        return new Employee
        {
            EmpCode = dto.EmpCode ?? IdGenerator.NewId(),
            EmpNo = dto.EmpNo,
            EmpName = dto.EmpName,
            EmpNameEn = dto.EmpNameEn,
            OfficeCode = dto.OfficeCode,
            CompanyCode = dto.CompanyCode,
            Remarks = dto.Remarks,
            Status = "0",
            CreateDate = now,
            UpdateDate = now
        };
    }
}
