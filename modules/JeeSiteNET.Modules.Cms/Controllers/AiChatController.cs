using JeeSiteNET.Core;
using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

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

    [HttpPost("chat")]
    public async Task<ApiResult<AiChatResponse>> Chat([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return ApiResult<AiChatResponse>.Fail(400, "消息不能为空");

        var result = await _aiChatService.ChatAsync(request);
        return ApiResult<AiChatResponse>.Ok(result);
    }

    [HttpGet("tools")]
    public ApiResult<object> GetTools()
    {
        var tools = _toolRegistry.GetToolDefinitions()
            .Select(t => new { t.Name, t.Description, t.Category })
            .ToList();

        return ApiResult<object>.Ok(new { tools, total = tools.Count });
    }
}
