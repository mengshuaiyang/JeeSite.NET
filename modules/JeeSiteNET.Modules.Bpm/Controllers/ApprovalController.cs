using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Application.DTOs;
using JeeSiteNET.Modules.Bpm.Application.Services;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Bpm.Controllers;

[ApiController]
[Route("api/v1/bpm/approval")]
public class ApprovalController : ControllerBase
{
    private readonly BpmService _bpmService;
    public ApprovalController(BpmService bpmService) => _bpmService = bpmService;

    [Permission("bpm:approval:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<ApprovalRecordDto>>> List([FromBody] PageRequest<ApprovalRecord> request)
        => ApiResult<PageResult<ApprovalRecordDto>>.Ok(await _bpmService.FindRecordsPageAsync(request));

    [Permission("bpm:approval:submit")]
    [HttpPost("submit")]
    public async Task<ApiResult> Submit([FromBody] ApprovalSubmitDto dto) => await _bpmService.SubmitApprovalAsync(dto);

    [Permission("bpm:approval:action")]
    [HttpPost("action")]
    public async Task<ApiResult> Action([FromBody] ApprovalActionDto dto) => await _bpmService.ApproveAsync(dto);

    [Permission("bpm:approval:list")]
    [HttpGet("history")]
    public async Task<ApiResult<List<ApprovalRecordDto>>> History([FromQuery] string businessKey)
        => ApiResult<List<ApprovalRecordDto>>.Ok(await _bpmService.GetHistoryAsync(businessKey));
}
