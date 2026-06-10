namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class AiChatRequest
{
    public string Message { get; set; } = string.Empty;
    public List<AiChatMessage>? History { get; set; }
    public string? CategoryCode { get; set; }
}

public class AiChatMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class AiChatResponse
{
    public string Reply { get; set; } = string.Empty;
    public List<string>? SourceArticles { get; set; }
}
