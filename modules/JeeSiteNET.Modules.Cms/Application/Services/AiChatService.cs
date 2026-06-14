using System.Net.Http;

using System.Net.Http.Json;

using System.Runtime.CompilerServices;

using System.Text.Json;

using JeeSiteNET.Core;

using JeeSiteNET.Core.AiTools;

using JeeSiteNET.Modules.Cms.Application.DTOs;

using JeeSiteNET.Modules.Cms.Controllers;

using JeeSiteNET.Modules.Cms.Domain.Interfaces;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Modules.Cms.Application.Services;
/// <summary>AI 对话领域服务，负责构建上下文、调用 DeepSeek 接口、多轮工具调用（Tool-calling）以及结构化输出。</summary>
public class AiChatService

{

    private readonly IArticleRepository _articleRepository;

    private readonly IArticleDataRepository _articleDataRepository;

    private readonly HttpClient _http;

    private readonly IConfiguration _configuration;

    private readonly ILogger<AiChatService> _logger;

    private readonly AiToolRegistry _toolRegistry;

    private static readonly JsonSerializerOptions JsonOptions = new()

    {

        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,

        WriteIndented = false

    };

    private static readonly JsonSerializerOptions PrettyJsonOptions = new()

    {

        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,

        WriteIndented = true

    };

    public AiChatService(

        IArticleRepository articleRepository,

        IArticleDataRepository articleDataRepository,

        HttpClient http,

        IConfiguration configuration,

        ILogger<AiChatService> logger,

        AiToolRegistry toolRegistry)

    {

        _articleRepository = articleRepository;

        _articleDataRepository = articleDataRepository;

        _http = http;

        _configuration = configuration;

        _logger = logger;

        _toolRegistry = toolRegistry;

    }

    public async Task<AiChatResponse> ChatAsync(AiChatRequest request)

    {

        var section = _configuration.GetSection("AI");

        var apiUrl = section["ApiUrl"] ?? "https://api.deepseek.com/v1/chat/completions";

        var apiKey = section["ApiKey"] ?? "";

        var model = section["Model"] ?? "deepseek-chat";

        var maxContextArticles = int.Parse(section["MaxContextArticles"] ?? "5");

        var context = await BuildContextAsync(request.Message, request.CategoryCode, maxContextArticles);

        var messages = new List<object>();

        messages.Add(new

        {

            role = "system",

            content = $"你是一个 CMS 内容助手，基于以下文章内容回答用户问题。如果问题与文章无关，请如实告知。\n\n参考文章：\n{context.Summary}"

        });

        if (request.History != null)

        {

            foreach (var msg in request.History.TakeLast(10))

                messages.Add(new { role = msg.Role, content = msg.Content });

        }

        messages.Add(new { role = "user", content = request.Message });

        var toolExecutions = new List<AiToolExecution>();

        var bodyBuilder = new Dictionary<string, object?>

        {

            ["model"] = model,

            ["messages"] = messages,

            ["temperature"] = 0.7,

            ["max_tokens"] = 4096,

        };

        if (request.EnableTools)

        {

            var tools = BuildToolsDefinition();

            if (tools.Count > 0)

                bodyBuilder["tools"] = tools;

        }

        var reply = await SendAndProcessAsync(apiUrl, apiKey, bodyBuilder, toolExecutions, 3);

        reply ??= "";

        return new AiChatResponse

        {

            Reply = reply,

            SourceArticles = context.Titles,

            ToolExecutions = toolExecutions.Count > 0 ? toolExecutions : null,

        };

    }

    public async IAsyncEnumerable<string> StreamAsync(AiChatRequest request, [EnumeratorCancellation] CancellationToken ct = default)

    {

        var section = _configuration.GetSection("AI");

        var apiUrl = section["ApiUrl"] ?? "https://api.deepseek.com/v1/chat/completions";

        var apiKey = section["ApiKey"] ?? "";

        var model = section["Model"] ?? "deepseek-chat";

        var maxContextArticles = int.Parse(section["MaxContextArticles"] ?? "5");

        var context = await BuildContextAsync(request.Message, request.CategoryCode, maxContextArticles);

        var messages = new List<object>();

        messages.Add(new

        {

            role = "system",

            content = $"你是一个 CMS 内容助手，基于以下文章内容回答用户问题。如果问题与文章无关，请如实告知。\n\n参考文章：\n{context.Summary}"

        });

        if (request.History != null)

        {

            foreach (var msg in request.History.TakeLast(10))

                messages.Add(new { role = msg.Role, content = msg.Content });

        }

        messages.Add(new { role = "user", content = request.Message });

        var body = new

        {

            model,

            messages,

            temperature = 0.7,

            max_tokens = 4096,

            stream = true

        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);

        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

        httpRequest.Content = JsonContent.Create(body, options: JsonOptions);

        using var response = await _http.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, ct);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(ct);

        using var reader = new StreamReader(stream);

        while (!ct.IsCancellationRequested)

        {

            var line = await reader.ReadLineAsync(ct);

            if (line == null) yield break;

            if (string.IsNullOrEmpty(line)) continue;

            if (line == "data: [DONE]") yield break;

            if (line.StartsWith("data: "))

            {

                var json = line[6..];

                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)

                {

                    var delta = choices[0].GetProperty("delta");

                    if (delta.TryGetProperty("content", out var content))

                        yield return content.GetString() ?? "";

                }

            }

        }

    }

    public async Task<ApiResult<JsonElement?>> ChatJsonAsync(AiChatRequest request)

    {

        request.EnableTools = true;

        var result = await ChatAsync(request);

        if (string.IsNullOrWhiteSpace(result.Reply)) return ApiResult<JsonElement?>.Ok(null);

        var cleaned = result.Reply.Trim();

        if (cleaned.StartsWith("```json"))

        {

            var start = cleaned.IndexOf('\n') + 1;

            var end = cleaned.LastIndexOf("```");

            cleaned = end > start ? cleaned[start..end] : cleaned[start..];

        }

        else if (cleaned.StartsWith("```"))

        {

            var start = cleaned.IndexOf('\n') + 1;

            var end = cleaned.LastIndexOf("```");

            cleaned = end > start ? cleaned[start..end] : cleaned[start..];

        }

        try

        {

            var json = JsonSerializer.Deserialize<JsonElement>(cleaned);

            return ApiResult<JsonElement?>.Ok(json);

        }

        catch

        {

            return ApiResult<JsonElement?>.Ok(null);

        }

    }

    public async Task<ApiResult<JsonElement?>> ChatEntityAsync(AiChatEntityRequest request)

    {

        var chatRequest = new AiChatRequest

        {

            Message = $"请以 JSON 格式回答以下问题（输出为 {request.EntityType} 类型的 JSON 对象，不要包含 markdown 代码块标记）：\n{request.Message}",

            CategoryCode = request.CategoryCode,

            EnableTools = true

        };

        var jsonResult = await ChatJsonAsync(chatRequest);

        return jsonResult;

    }

    private async Task<string?> SendAndProcessAsync(

        string apiUrl, string apiKey,

        Dictionary<string, object?> bodyBuilder,

        List<AiToolExecution> toolExecutions,

        int maxRounds)

    {

        for (int round = 0; round < maxRounds; round++)

        {

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

            httpRequest.Content = JsonContent.Create(bodyBuilder, options: JsonOptions);

            var response = await _http.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (!result.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)

                return null;

            var choice = choices[0];

            var message = choice.GetProperty("message");

            var content = message.TryGetProperty("content", out var contentEl)

                ? contentEl.GetString()

                : "";

            if (!message.TryGetProperty("tool_calls", out var toolCalls) || toolCalls.GetArrayLength() == 0)

                return content;

            var replyMsg = new Dictionary<string, object?>

            {

                ["role"] = "assistant",

                ["content"] = content

            };

            var tcList = new List<object>();

            foreach (var tc in toolCalls.EnumerateArray())

            {

                var tcObj = new Dictionary<string, object?>

                {

                    ["id"] = tc.GetProperty("id").GetString(),

                    ["type"] = "function",

                    ["function"] = new Dictionary<string, object?>

                    {

                        ["name"] = tc.GetProperty("function").GetProperty("name").GetString(),

                        ["arguments"] = tc.GetProperty("function").GetProperty("arguments").GetString()

                    }

                };

                tcList.Add(tcObj);

            }

            replyMsg["tool_calls"] = tcList;

            var messages = (List<object>)bodyBuilder["messages"]!;

            messages.Add(replyMsg);

            foreach (var tc in toolCalls.EnumerateArray())

            {

                var func = tc.GetProperty("function");

                var toolName = func.GetProperty("name").GetString() ?? "";

                var argsStr = func.GetProperty("arguments").GetString() ?? "{}";

                var toolResult = await ExecuteToolAsync(toolName, argsStr);

                toolExecutions.Add(toolResult);

                messages.Add(new Dictionary<string, object?>

                {

                    ["role"] = "tool",

                    ["tool_call_id"] = tc.GetProperty("id").GetString(),

                    ["content"] = toolResult.Success

                        ? (toolResult.Result ?? "执行成功")

                        : $"错误: {toolResult.Result ?? "执行失败"}"

                });

            }

            bodyBuilder["messages"] = messages;

        }

        return null;

    }

    /// <summary>解析 AI 返回的工具参数并调用指定的 AI 工具，返回执行结果。</summary>

    private async Task<AiToolExecution> ExecuteToolAsync(string toolName, string argumentsJson)

    {

        try

        {

            var args = JsonSerializer.Deserialize<Dictionary<string, string>>(argumentsJson)

                ?? new Dictionary<string, string>();

            var context = new AiToolContext

            {

                Parameters = args

            };

            var result = await _toolRegistry.ExecuteToolAsync(toolName, context);

            return new AiToolExecution

            {

                ToolName = toolName,

                Success = result.Success,

                Result = result.Data

            };

        }

        catch (Exception ex)

        {

            return new AiToolExecution

            {

                ToolName = toolName,

                Success = false,

                Result = $"异常: {ex.Message}"

            };

        }

    }

    /// <summary>从注册的 AI 工具构建工具调用 JSON 描述。</summary>

    private List<object> BuildToolsDefinition()

    {

        var tools = new List<object>();

        foreach (var tool in _toolRegistry.GetAllTools())

        {

            var attr = tool.GetType().GetCustomAttributes(typeof(AiToolAttribute), false)

                .Cast<AiToolAttribute>().FirstOrDefault();

            tools.Add(new

            {

                type = "function",

                function = new

                {

                    name = tool.Name,

                    description = attr?.Description ?? tool.Description,

                    parameters = new

                    {

                        type = "object",

                        properties = new Dictionary<string, object>(),

                    }

                }

            });

        }

        return tools;

    }

    private async Task<(string Summary, List<string> Titles)> BuildContextAsync(string query, string? categoryCode, int maxArticles)

    {

        var pageRequest = new Core.PageRequest<Domain.Entities.Article>

        {

            PageSize = maxArticles,

            PageNo = 1,

            Entity = !string.IsNullOrEmpty(categoryCode) ? new Domain.Entities.Article { CategoryCode = categoryCode } : null,

        };

        var pageResult = await _articleRepository.FindPageAsync(pageRequest);

        var summaryParts = new List<string>();

        var titles = new List<string>();

        foreach (var article in pageResult.List.Take(maxArticles))

        {

            var data = await _articleDataRepository.GetAsync(article.ArticleCode);

            var content = data?.Content ?? "";

            var snippet = content.Length > 500 ? content[..500] + "..." : content;

            summaryParts.Add($"【{article.Title}】\n{snippet}\n");

            titles.Add(article.Title);

        }

        return (string.Join("\n---\n", summaryParts), titles);

    }

}
