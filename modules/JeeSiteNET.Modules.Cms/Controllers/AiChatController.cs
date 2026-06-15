using System.Text.Json;
using JeeSiteNET.Core;
using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

// ================================================================
// AI 对话控制器 —— 前端与 AI 模型（DeepSeek）交互的入口
//
// 调用链路：
//   前端提问 → POST /api/v1/cms/ai/chat
//            → AiChatService.ChatAsync
//            → 调用 DeepSeek API（含 RAG 上下文 + 工具调用）
//            → 返回回复文本 + 来源文章 + 工具执行记录
//
// 支持的交互方式：
//   ① POST /chat      — 普通对话（含 RAG + 工具自动调用）
//   ② GET /chat/stream — SSE 流式对话（打字机效果）
//   ③ POST /chat/json  — JSON 结构化输出（适合数据抽取场景）
//   ④ POST /chat/entity — 指定实体类型的结构化输出
//   ⑤ GET /tools       — 查询已注册的 AI 工具列表
// ================================================================

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

    /// <summary>
    /// HTTP POST - AI 对话。
    /// 发送消息和可选的分类编码，AI 会基于 CMS 文章内容（RAG 上下文）回答，
    /// 并自动调用已注册的工具（如天气/搜索/时间查询）。
    /// </summary>
    [HttpPost("chat")]
    public async Task<ApiResult<AiChatResponse>> Chat([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return ApiResult<AiChatResponse>.Fail(400, "消息不能为空");
        var result = await _aiChatService.ChatAsync(request);
        return ApiResult<AiChatResponse>.Ok(result);
    }

    /// <summary>
    /// HTTP GET - 流式 SSE 对话。
    /// 以 Server-Sent Events（text/event-stream）格式逐块返回 AI 回复，
    /// 前端用 EventSource API 接收，实现"打字机"效果。
    /// 数据格式：
    ///   data: {"content":"第一段"}\n\n
    ///   data: {"content":"第二段"}\n\n
    ///   data: [DONE]\n\n
    /// </summary>
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

    /// <summary>
    /// HTTP POST - JSON 结构化输出。
    /// AI 的回复将被解析为 JSON 对象返回，适合数据抽取、报表生成等场景。
    /// 内部先调用 ChatAsync，再从回复中提取 JSON（支持 ```json 代码块）。
    /// </summary>
    [HttpPost("chat/json")]
    public async Task<ApiResult<JsonElement?>> ChatJson([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return ApiResult<JsonElement?>.Fail(400, "消息不能为空");
        request.EnableTools = true;
        return await _aiChatService.ChatJsonAsync(request);
    }

    /// <summary>
    /// HTTP POST - 实体抽取对话。
    /// 指定目标实体类型（如 "Person" / "Order"），AI 按该结构输出 JSON。
    /// 适用于从非结构化文本中提取结构化数据。
    /// </summary>
    [HttpPost("chat/entity")]
    public async Task<ApiResult<JsonElement?>> ChatEntity([FromBody] AiChatEntityRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return ApiResult<JsonElement?>.Fail(400, "消息不能为空");
        return await _aiChatService.ChatEntityAsync(request);
    }

    /// <summary>
    /// HTTP GET - 查询已注册的 AI 工具列表。
    /// 返回所有通过 [AiTool] 属性注册的工具名称、描述和分类，
    /// 供前端展示或调试使用。
    /// </summary>
    [HttpGet("tools")]
    public ApiResult<object> GetTools()
    {
        var tools = _toolRegistry.GetToolDefinitions()
            .Select(t => new { t.Name, t.Description, t.Category })
            .ToList();
        return ApiResult<object>.Ok(new { tools, total = tools.Count });
    }
}

/// <summary>AI 实体抽取请求 DTO。指定消息和目标实体类型。</summary>
public class AiChatEntityRequest
{
    public string Message { get; set; } = string.Empty;
    public string EntityType { get; set; } = "object";
    public string? CategoryCode { get; set; }
}
