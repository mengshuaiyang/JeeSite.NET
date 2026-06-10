using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class EmpUser : BaseEntity
{
    public string EmpCode { get; set; } = string.Empty;
    public string UserCode { get; set; } = string.Empty;
    public string? EmpName { get; set; }
    public string? LoginCode { get; set; }
    public string? UserName { get; set; }
}
