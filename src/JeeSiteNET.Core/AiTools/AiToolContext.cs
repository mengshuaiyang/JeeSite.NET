using System.Security.Claims;

namespace JeeSiteNET.Core.AiTools;

public class AiToolContext
{
    public string UserMessage { get; set; } = string.Empty;
    public Dictionary<string, string> Parameters { get; set; } = new();
    public ClaimsPrincipal? User { get; set; }
    public IServiceProvider? ServiceProvider { get; set; }
    public string? ConversationId { get; set; }

    public string? GetParameter(string key) =>
        Parameters.TryGetValue(key, out var val) ? val : null;
}
