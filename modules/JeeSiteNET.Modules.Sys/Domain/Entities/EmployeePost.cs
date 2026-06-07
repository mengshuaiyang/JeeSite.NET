using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class EmployeePost : DataEntity
{
    public string EmpCode { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
}
