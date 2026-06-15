using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using JeeSiteNET.Core;
using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Modules.Cms.Controllers;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Modules.Cms.Application.Services;

// ================================================================
// AI 对话服务 —— 与 DeepSeek API 交互的核心业务逻辑
//
// 功能：
//   ① ChatAsync     — 普通对话（含 RAG 上下文检索 + 自动工具调用）
//   ② StreamAsync   — SSE 流式对话（逐 Token 返回）
//   ③ ChatJsonAsync — JSON 结构化输出（从回复中提取 JSON）
//   ④ ChatEntityAsync — 指定实体类型的结构化输出
//
// 调用链路：
//   AiChatController → AiChatService → DeepSeek API
//                                      → AiToolRegistry（函数调用）
//                                      → ArticleRepository（RAG 上下文）
//
// 工作流程（ChatAsync）：
//   ① 从文章库检索与问题相关的文章内容（RAG 上下文）
//   ② 构建包含 system prompt 和上下文的消息列表
//   ③ 调用 DeepSeek Chat Completion API
//   ④ 如果 AI 触发了工具调用（tool_calls），执行工具并返回结果
//   ⑤ 最多 3 轮工具调用循环，直到 AI 返回最终答案
//
// 配置项（appsettings.json AI 段）：
//   ApiUrl    — DeepSeek API 地址（默认 https://api.deepseek.com/v1/chat/completions）
//   ApiKey    — API 密钥
//   Model     — 模型名（默认 deepseek-chat）
//   MaxContextArticles — RAG 检索文章数（默认 5）
// ================================================================

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

    // ========== ① 普通对话 ==========

    /// <summary>
    /// 普通对话接口。
    /// 执行步骤：检索相关文章（RAG）→ 构建消息 → 调用 DeepSeek → 工具调用循环 → 返回回答
    /// </summary>
    /// <param name="request">包含消息、分类编码、历史记录等</param>
    /// <returns>AI 回复 + 来源文章 + 工具执行记录</returns>
    public async Task<AiChatResponse> ChatAsync(AiChatRequest request)
    {
        var section = _configuration.GetSection("AI");
        var apiUrl = section["ApiUrl"] ?? "https://api.deepseek.com/v1/chat/completions";
        var apiKey = section["ApiKey"] ?? "";
        var model = section["Model"] ?? "deepseek-chat";
        var maxContextArticles = int.Parse(section["MaxContextArticles"] ?? "5");

        // 从文章库检索与问题相关的内容，构建 RAG 上下文
        var context = await BuildContextAsync(request.Message, request.CategoryCode, maxContextArticles);

        // 构建消息列表：system prompt（含参考文章）+ 历史 + 当前提问
        var messages = new List<object>();
        messages.Add(new
        {
            role = "system",
            content = $"你是一个 CMS 内容助手，基于以下文章内容回答用户问题。如果问题与文章无关，请如实告知。\n\n参考文章：\n{context.Summary}"
        });

        if (request.History != null)
        {
            // 只取最近 10 轮对话，保持上下文在 Token 限制内
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

        // 启用工具时，将已注册的 AI 工具定义加入请求
        if (request.EnableTools)
        {
            var tools = BuildToolsDefinition();
            if (tools.Count > 0)
                bodyBuilder["tools"] = tools;
        }

        // 发送请求 + 多轮工具调用循环（最多 3 轮）
        var reply = await SendAndProcessAsync(apiUrl, apiKey, bodyBuilder, toolExecutions, 3);
        reply ??= "";

        return new AiChatResponse
        {
            Reply = reply,
            SourceArticles = context.Titles,
            ToolExecutions = toolExecutions.Count > 0 ? toolExecutions : null,
        };
    }

    // ========== ② 流式 SSE 对话 ==========

    /// <summary>
    /// 流式 SSE 对话。
    /// 以 IAsyncEnumerable<string> 逐块返回 AI 回复内容，
    /// 前端通过 EventSource 接收，实现打字机效果。
    /// </summary>
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
            stream = true  // SSE 模式
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
        httpRequest.Content = JsonContent.Create(body, options: JsonOptions);

        // 使用 ResponseHeadersRead 在收到头部后立即开始读取流
        using var response = await _http.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var reader = new StreamReader(stream);

        // 逐行解析 SSE 格式：data: {"choices":[{"delta":{"content":"..."}}]}
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

    // ========== ③ JSON 结构化输出 ==========

    /// <summary>
    /// 以 JSON 格式返回 AI 的回复。
    /// AI 的回答会被尝试解析为 JSON，支持 markdown ```json 代码块格式。
    /// </summary>
    public async Task<ApiResult<JsonElement?>> ChatJsonAsync(AiChatRequest request)
    {
        request.EnableTools = true;
        var result = await ChatAsync(request);

        if (string.IsNullOrWhiteSpace(result.Reply))
            return ApiResult<JsonElement?>.Ok(null);

        // 去除可能存在的 markdown 代码块标记 ```json ... ```
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

    // ========== ④ 实体抽取 ==========

    /// <summary>
    /// 指定实体类型的结构化输出。
    /// 在 prompt 中要求 AI 按 EntityType 输出 JSON，适合数据提取场景。
    /// </summary>
    public async Task<ApiResult<JsonElement?>> ChatEntityAsync(AiChatEntityRequest request)
    {
        var chatRequest = new AiChatRequest
        {
            Message = $"请以 JSON 格式回答以下问题（输出为 {request.EntityType} 类型的 JSON 对象，不要包含 markdown 代码块标记）：\n{request.Message}",
            CategoryCode = request.CategoryCode,
            EnableTools = true
        };

        return await ChatJsonAsync(chatRequest);
    }

    // ========== 内部方法：API 调用 + 工具循环 ==========

    /// <summary>
    /// 调用 DeepSeek API 并处理多轮工具调用。
    /// 当 AI 返回 tool_calls 时，执行对应工具并将结果反馈给 AI，
    /// 最多循环 maxRounds 次。
    /// </summary>
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

            // 如果没有工具调用，直接返回文本
            if (!message.TryGetProperty("tool_calls", out var toolCalls) || toolCalls.GetArrayLength() == 0)
                return content;

            // 有工具调用 → 将 assistant 消息加入对话历史
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

            // 执行每个工具调用，将结果返回给 AI
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

    /// <summary>通过 AiToolRegistry 执行指定的 AI 工具。</summary>
    private async Task<AiToolExecution> ExecuteToolAsync(string toolName, string argumentsJson)
    {
        try
        {
            var args = JsonSerializer.Deserialize<Dictionary<string, string>>(argumentsJson)
                ?? new Dictionary<string, string>();

            var context = new AiToolContext { Parameters = args };
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

    /// <summary>从 AiToolRegistry 获取所有工具定义，组装为 DeepSeek tools 格式。</summary>
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

    /// <summary>从文章库检索与问题相关的内容，构建 RAG 上下文。</summary>
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
            // 只截取前 500 字作为摘要，避免超出 Token 限制
            var snippet = content.Length > 500 ? content[..500] + "..." : content;
            summaryParts.Add($"【{article.Title}】\n{snippet}\n");
            titles.Add(article.Title);
        }

        return (string.Join("\n---\n", summaryParts), titles);
    }
}
