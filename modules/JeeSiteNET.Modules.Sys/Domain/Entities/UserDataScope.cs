using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class UserDataScope : DataEntity
{
    public string UserCode { get; set; } = string.Empty;
    public string CtrlType { get; set; } = string.Empty;
    public string? CtrlData { get; set; }
    public string? CtrlPermi { get; set; }
}
