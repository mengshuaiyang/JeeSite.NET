using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Organization : TreeEntity
{
    public string OrgCode { get; set; } = string.Empty;
    public string OrgName { get; set; } = string.Empty;
    public string? OrgType { get; set; }
    public string? OrgTypeName { get; set; }

    public override string GetName() => OrgName;
}
