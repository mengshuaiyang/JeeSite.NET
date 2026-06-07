using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Test.Domain.Entities;

public class TestTree : TreeEntity
{
    public string TreeCode { get; set; } = string.Empty;
    public string TreeName { get; set; } = string.Empty;

    public override string GetName() => TreeName;
}
