using System.Text.Json;
using JeeSiteNET.Core;
using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

/// <summary>AI 对话接口控制器，封装 DeepSeek 对话、流式对话、JSON 结构化输出、实体抽取以及工具集查询接口。</summary>
[Route("api/v1/cms/ai")]
[ApiController]
[Authorize]
public class AiChatController : ControllerBase

{
    private readonly AiChatService _aiChatService;

    private readonly AiToolRegistry _toolRegistry;

    public AiChatController(AiChatService aiChatService, AiToolRegistry toolRegistry)
    {
        _aiChatService = aiChatService;

        _toolRegistry = toolRegistry;
    }

    /// <summary>HTTP POST - 普通文本对话，返回 AI 回复与工具调用结果。</summary>
    [HttpPost("chat")]
    public async Task<ApiResult<AiChatResponse>> Chat([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))

            return ApiResult<AiChatResponse>.Fail(400, "消息不能为空");

        var result = await _aiChatService.ChatAsync(request);

        return ApiResult<AiChatResponse>.Ok(result);
    }

    /// <summary>HTTP GET - 流式对话接口，以 Server-Sent Events 形式持续返回内容。</summary>
    [HttpGet("chat/stream")]
    public async Task Stream([FromQuery] string message, [FromQuery] string? categoryCode = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Response.StatusCode = 400;

            await Response.WriteAsync("消息不能为空");

            return;
        }

        Response.Headers.ContentType = "text/event-stream";

        Response.Headers.CacheControl = "no-cache";

        Response.Headers.Connection = "keep-alive";

        await foreach (var chunk in _aiChatService.StreamAsync(new AiChatRequest

        {
            Message = message,

            CategoryCode = categoryCode,

            EnableTools = true

        }))
        {
            var json = JsonSerializer.Serialize(new { content = chunk });

            await Response.WriteAsync($"data: {json}\n\n");

            await Response.Body.FlushAsync();
        }

        await Response.WriteAsync("data: [DONE]\n\n");

        await Response.Body.FlushAsync();
    }

    /// <summary>HTTP POST - 以 JSON 结构返回 AI 回答，用于结构化数据抽取。</summary>
    [HttpPost("chat/json")]
    public async Task<ApiResult<JsonElement?>> ChatJson([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))

            return ApiResult<JsonElement?>.Fail(400, "消息不能为空");

        request.EnableTools = true;

        var result = await _aiChatService.ChatJsonAsync(request);

        return result;
    }

    /// <summary>HTTP POST - 实体抽取对话接口，按指定 EntityType 输出 JSON。</summary>
    [HttpPost("chat/entity")]
    public async Task<ApiResult<JsonElement?>> ChatEntity([FromBody] AiChatEntityRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))

            return ApiResult<JsonElement?>.Fail(400, "消息不能为空");

        var result = await _aiChatService.ChatEntityAsync(request);

        return result;
    }

    /// <summary>HTTP GET - 获取当前已注册的 AI 工具定义列表。</summary>
    [HttpGet("tools")]
    public ApiResult<object> GetTools()
    {
        var tools = _toolRegistry.GetToolDefinitions()

            .Select(t => new { t.Name, t.Description, t.Category })

            .ToList();

        return ApiResult<object>.Ok(new { tools, total = tools.Count });
    }
}

/// <summary>AI 实体抽取请求 DTO。</summary>

public class AiChatEntityRequest

{
    public string Message { get; set; } = string.Empty;

    public string EntityType { get; set; } = "object";

    public string? CategoryCode { get; set; }
}
