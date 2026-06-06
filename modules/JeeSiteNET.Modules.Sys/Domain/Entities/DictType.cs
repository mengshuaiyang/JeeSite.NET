using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class DictType : DataEntity
{
    public string DictTypeCode { get; set; } = string.Empty;
    public string DictName { get; set; } = string.Empty;
    public string? IsSys { get; set; } = "0";
    public decimal? Sort { get; set; }
}
