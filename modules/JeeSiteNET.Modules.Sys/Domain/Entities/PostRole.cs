using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class PostRole : DataEntity
{
    public string PostCode { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
}
