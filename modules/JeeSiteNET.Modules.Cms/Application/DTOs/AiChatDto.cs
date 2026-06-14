// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class AiChatRequest
// 定义类：AiChatRequest
public class AiChatRequest
{
    // 属性 Message
    // 属性：Message
    public string Message { get; set; } = string.Empty;
    // 属性：History
    public List<AiChatMessage>? History { get; set; }
    // 属性：CategoryCode
    public string? CategoryCode { get; set; }
    // 属性 EnableTools
    // 属性：EnableTools
    public bool EnableTools { get; set; }
}

// 定义class AiChatMessage
// 定义类：AiChatMessage
public class AiChatMessage
{
    // 属性 Role
    // 属性：Role
    public string Role { get; set; } = string.Empty;
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
    // 属性：ToolCalls
    public List<AiToolCall>? ToolCalls { get; set; }
    // 属性：ToolCallId
    public string? ToolCallId { get; set; }
}

// 定义class AiToolCall
// 定义类：AiToolCall
public class AiToolCall
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 Type
    // 属性：Type
    public string Type { get; set; } = "function";
    // 属性 Function
    // 属性：Function
    public AiToolCallFunction Function { get; set; } = new();
}

// 定义class AiToolCallFunction
// 定义类：AiToolCallFunction
public class AiToolCallFunction
{
    // 属性 Name
    // 属性：Name
    public string Name { get; set; } = string.Empty;
    // 属性 Arguments
    // 属性：Arguments
    public string Arguments { get; set; } = string.Empty;
}

// 定义class AiChatResponse
// 定义类：AiChatResponse
public class AiChatResponse
{
    // 属性 Reply
    // 属性：Reply
    public string Reply { get; set; } = string.Empty;
    // 属性：SourceArticles
    public List<string>? SourceArticles { get; set; }
    // 属性：ToolExecutions
    public List<AiToolExecution>? ToolExecutions { get; set; }
}

// 定义class AiToolExecution
// 定义类：AiToolExecution
public class AiToolExecution
{
    // 属性 ToolName
    // 属性：ToolName
    public string ToolName { get; set; } = string.Empty;
    // 属性 Success
    // 属性：Success
    public bool Success { get; set; }
    // 属性：Result
    public string? Result { get; set; }
}
