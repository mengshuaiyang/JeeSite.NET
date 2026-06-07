using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class EmployeeDto
{
    public string EmpCode { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string EmpName { get; set; } = string.Empty;
    public string? EmpNameEn { get; set; }
    public string OfficeCode { get; set; } = string.Empty;
    public string OfficeName { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "0";
    public string? Remarks { get; set; }
    public List<string> PostCodes { get; set; } = new();
    public List<EmployeeOfficeDto> Offices { get; set; } = new();
}

public class EmployeeSaveDto
{
    public string? EmpCode { get; set; }
    public string? EmpNo { get; set; }
    public string EmpName { get; set; } = string.Empty;
    public string? EmpNameEn { get; set; }
    public string OfficeCode { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public List<string> PostCodes { get; set; } = new();
    public List<EmployeeOfficeSaveDto> Offices { get; set; } = new();
}

public class EmployeeOfficeDto
{
    public string Id { get; set; } = string.Empty;
    public string EmpCode { get; set; } = string.Empty;
    public string OfficeCode { get; set; } = string.Empty;
    public string OfficeName { get; set; } = string.Empty;
    public string? PostCode { get; set; }
    public string? PostName { get; set; }
}

public class EmployeeOfficeSaveDto
{
    public string? Id { get; set; }
    public string OfficeCode { get; set; } = string.Empty;
    public string? PostCode { get; set; }
}

public static class EmployeeMapping
{
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
