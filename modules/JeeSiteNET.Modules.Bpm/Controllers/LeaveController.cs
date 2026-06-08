using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Bpm.Application.DTOs;
using JeeSiteNET.Modules.Bpm.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Bpm.Controllers;

[ApiController]
[Route("api/v1/bpm/leave")]
public class LeaveController : ControllerBase
{
    private readonly LeaveService _leaveService;
    private readonly BpmService _bpmService;
    public LeaveController(LeaveService leaveService, BpmService bpmService) { _leaveService = leaveService; _bpmService = bpmService; }

    [Permission("bpm:leave:list")]
    [HttpGet("my-leaves")]
    public async Task<ApiResult<List<LeaveRequestDto>>> MyLeaves([FromQuery] string applicant)
    {
        var leaves = await _leaveService.GetMyLeavesAsync(applicant);
        var dtos = leaves.Select(LeaveRequestDto.FromEntity).ToList();
        return ApiResult<List<LeaveRequestDto>>.Ok(dtos);
    }

    [Permission("bpm:leave:list")]
    [HttpGet("pending")]
    public async Task<ApiResult<List<LeaveRequestDto>>> Pending([FromQuery] string approver)
    {
        var leaves = await _leaveService.GetPendingApprovalsAsync(approver);
        var dtos = leaves.Select(LeaveRequestDto.FromEntity).ToList();
        return ApiResult<List<LeaveRequestDto>>.Ok(dtos);
    }

    [Permission("bpm:leave:submit")]
    [HttpPost("submit")]
    public async Task<ApiResult> Submit([FromBody] SubmitLeaveDto dto) => await _leaveService.SubmitAsync(dto);

    [Permission("bpm:leave:action")]
    [HttpPost("approve")]
    public async Task<ApiResult> Approve([FromBody] ApproveLeaveDto dto) => await _leaveService.ApproveAsync(dto);

    [Permission("bpm:leave:list")]
    [HttpGet("detail")]
    public async Task<ApiResult<object>> Detail([FromQuery] string leaveRequestId)
    {
        var leaves = await _leaveService.GetMyLeavesAsync("");
        var leave = leaves.FirstOrDefault(l => l.LeaveRequestId == leaveRequestId);
        if (leave == null) return ApiResult<object>.NotFound("请假申请不存在");

        var dto = LeaveRequestDto.FromEntity(leave);
        dto.History = await _bpmService.GetHistoryAsync(leaveRequestId);
        return ApiResult<object>.Ok(dto);
    }
}
