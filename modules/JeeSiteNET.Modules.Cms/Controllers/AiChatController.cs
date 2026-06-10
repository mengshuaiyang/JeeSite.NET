using JeeSiteNET.Core;
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

    public AiChatController(AiChatService aiChatService)
    {
        _aiChatService = aiChatService;
    }

    [HttpPost("chat")]
    public async Task<ApiResult<AiChatResponse>> Chat([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return ApiResult<AiChatResponse>.Fail(400, "消息不能为空");

        var result = await _aiChatService.ChatAsync(request);
        return ApiResult<AiChatResponse>.Ok(result);
    }
}
