using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class EmployeeOffice : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string EmpCode { get; set; } = string.Empty;
    public string OfficeCode { get; set; } = string.Empty;
    public string? PostCode { get; set; }
}
