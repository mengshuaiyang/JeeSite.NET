using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Post : DataEntity, ICorpEntity
{
    public string PostCode { get; set; } = string.Empty;
    public string PostName { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string? PostType { get; set; }
    public decimal? PostSort { get; set; }
    public string? OrgCode { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
