namespace JeeSiteNET.Core.AiTools;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AiToolAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public string? Category { get; }

    public AiToolAttribute(string name, string description, string? category = null)
    {
        Name = name;
        Description = description;
        Category = category;
    }
}
