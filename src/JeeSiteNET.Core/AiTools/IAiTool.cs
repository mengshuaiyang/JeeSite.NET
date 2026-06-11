namespace JeeSiteNET.Core.AiTools;

public interface IAiTool
{
    Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default);

    string Name { get; }
    string Description { get; }
}
