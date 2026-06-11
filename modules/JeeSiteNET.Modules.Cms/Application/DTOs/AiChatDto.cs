namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class AiChatRequest
{
    public string Message { get; set; } = string.Empty;
    public List<AiChatMessage>? History { get; set; }
    public string? CategoryCode { get; set; }
    public bool EnableTools { get; set; }
}

public class AiChatMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<AiToolCall>? ToolCalls { get; set; }
    public string? ToolCallId { get; set; }
}

public class AiToolCall
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = "function";
    public AiToolCallFunction Function { get; set; } = new();
}

public class AiToolCallFunction
{
    public string Name { get; set; } = string.Empty;
    public string Arguments { get; set; } = string.Empty;
}

public class AiChatResponse
{
    public string Reply { get; set; } = string.Empty;
    public List<string>? SourceArticles { get; set; }
    public List<AiToolExecution>? ToolExecutions { get; set; }
}

public class AiToolExecution
{
    public string ToolName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Result { get; set; }
}
