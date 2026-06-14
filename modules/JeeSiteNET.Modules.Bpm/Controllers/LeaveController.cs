    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
using JeeSiteNET.Modules.Bpm.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Bpm.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.Services
using JeeSiteNET.Modules.Bpm.Application.Services;
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
[Route("api/v1/bpm/leave")]
// 定义class LeaveController
// 定义类：LeaveController

public class LeaveController : ControllerBase
{
    // 字段 _leaveService
    // 字段：_leaveService

    private readonly LeaveService _leaveService;
    // 字段 _bpmService
    // 字段：_bpmService

    private readonly BpmService _bpmService;
    // 方法 LeaveController
    // 构造函数：LeaveController

    public LeaveController(LeaveService leaveService, BpmService bpmService) { _leaveService = leaveService; _bpmService = bpmService; }

    [Permission("bpm:leave:list")]
    [HttpGet("my-leaves")]
    // 方法 MyLeaves
    // 方法：MyLeaves

    public async Task<ApiResult<List<LeaveRequestDto>>> MyLeaves([FromQuery] string applicant)
    {
        var leaves = await _leaveService.GetMyLeavesAsync(applicant);
        // 数据库操作：投影选择
        var dtos = leaves.Select(LeaveRequestDto.FromEntity).ToList();
        // return 返回结果
        return ApiResult<List<LeaveRequestDto>>.Ok(dtos);
    }

    [Permission("bpm:leave:list")]
    [HttpGet("pending")]
    // 方法 Pending
    // 方法：Pending

    public async Task<ApiResult<List<LeaveRequestDto>>> Pending([FromQuery] string approver)
    {
        var leaves = await _leaveService.GetPendingApprovalsAsync(approver);
        // 数据库操作：投影选择
        var dtos = leaves.Select(LeaveRequestDto.FromEntity).ToList();
        // return 返回结果
        return ApiResult<List<LeaveRequestDto>>.Ok(dtos);
    }

    [Permission("bpm:leave:submit")]
    [HttpPost("submit")]
    // 方法：Submit

    public async Task<ApiResult> Submit([FromBody] SubmitLeaveDto dto) => await _leaveService.SubmitAsync(dto);

    [Permission("bpm:leave:action")]
    [HttpPost("approve")]
    // 方法：Approve

    public async Task<ApiResult> Approve([FromBody] ApproveLeaveDto dto) => await _leaveService.ApproveAsync(dto);

    [Permission("bpm:leave:list")]
    [HttpGet("detail")]
    // 方法 Detail
    // 方法：Detail

    public async Task<ApiResult<object>> Detail([FromQuery] string leaveRequestId)
    {
        var leaves = await _leaveService.GetMyLeavesAsync("");
        // 数据库操作：取首条或默认值
        var leave = leaves.FirstOrDefault(l => l.LeaveRequestId == leaveRequestId);
        // if 条件判断
        if (leave == null) return ApiResult<object>.NotFound("请假申请不存在");

        // 声明并初始化变量：dto
        var dto = LeaveRequestDto.FromEntity(leave);
        dto.History = await _bpmService.GetHistoryAsync(leaveRequestId);
        // return 返回结果
        return ApiResult<object>.Ok(dto);
    }
}
