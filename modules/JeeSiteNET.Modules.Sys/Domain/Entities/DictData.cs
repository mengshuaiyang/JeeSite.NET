using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class DictData : DataEntity
{
    public string DictCode { get; set; } = string.Empty;
    public string DictType { get; set; } = string.Empty;
    public string DictLabel { get; set; } = string.Empty;
    public string DictValue { get; set; } = string.Empty;
    public decimal? Sort { get; set; }
}
