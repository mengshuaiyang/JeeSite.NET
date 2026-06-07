using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class BizCategory : TreeEntity, ICorpEntity
{
    public string CategoryCode { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }

    public override string GetName() => CategoryName;
}
