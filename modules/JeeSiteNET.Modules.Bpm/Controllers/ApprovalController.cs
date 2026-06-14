    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
using JeeSiteNET.Modules.Bpm.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Bpm.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.Services
using JeeSiteNET.Modules.Bpm.Application.Services;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Bpm.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Controllers
namespace JeeSiteNET.Modules.Bpm.Controllers;

[ApiController]
[Route("api/v1/bpm/approval")]
// 定义class ApprovalController
// 定义类：ApprovalController

public class ApprovalController : ControllerBase
{
    // 字段 _bpmService
    // 字段：_bpmService

    private readonly BpmService _bpmService;
    // 构造函数 ApprovalController
    // 构造函数：ApprovalController

    public ApprovalController(BpmService bpmService) => _bpmService = bpmService;

    [Permission("bpm:approval:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<ApprovalRecordDto>>> List([FromBody] PageRequest<ApprovalRecord> request)
        => ApiResult<PageResult<ApprovalRecordDto>>.Ok(await _bpmService.FindRecordsPageAsync(request));

    [Permission("bpm:approval:submit")]
    [HttpPost("submit")]
    // 方法：Submit

    public async Task<ApiResult> Submit([FromBody] ApprovalSubmitDto dto) => await _bpmService.SubmitApprovalAsync(dto);

    [Permission("bpm:approval:action")]
    [HttpPost("action")]
    // 方法：Action

    public async Task<ApiResult> Action([FromBody] ApprovalActionDto dto) => await _bpmService.ApproveAsync(dto);

    [Permission("bpm:approval:list")]
    [HttpGet("history")]
    // 方法 History
    // 方法：History

    public async Task<ApiResult<List<ApprovalRecordDto>>> History([FromQuery] string businessKey)
        => ApiResult<List<ApprovalRecordDto>>.Ok(await _bpmService.GetHistoryAsync(businessKey));
}
