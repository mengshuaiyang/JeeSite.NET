namespace JeeSiteNET.Core.Modules;

public interface IModuleDescriptor
{
    string Code { get; }
    string Name { get; }
    string Version { get; }
    string[] Dependencies { get; }
    int SortOrder { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class ModuleDescriptionAttribute : Attribute, IModuleDescriptor
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0.0";
    public string[] Dependencies { get; set; } = [];
    public int SortOrder { get; set; } = 100;
}
