    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
using JeeSiteNET.Modules.Bpm.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;

// 定义 JeeSiteNET.Modules.Bpm.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Application.Services
namespace JeeSiteNET.Modules.Bpm.Application.Services;

// 定义class LeaveService
// 定义类：LeaveService
public class LeaveService
{
    // 字段 _leaveRepository
    // 字段：_leaveRepository
    private readonly ILeaveRepository _leaveRepository;
    // 字段 _bpmService
    // 字段：_bpmService
    private readonly BpmService _bpmService;
    // 字段 _approvalRecordRepository
    // 字段：_approvalRecordRepository
    private readonly IApprovalRecordRepository _approvalRecordRepository;
    // 字段 _workflowFormRepository
    // 字段：_workflowFormRepository
    private readonly IWorkflowFormRepository _workflowFormRepository;

    // 方法 LeaveService
    // 构造函数：LeaveService
    public LeaveService(ILeaveRepository leaveRepository, BpmService bpmService, IApprovalRecordRepository approvalRecordRepository, IWorkflowFormRepository workflowFormRepository)
    {
        _leaveRepository = leaveRepository;
        _bpmService = bpmService;
        _approvalRecordRepository = approvalRecordRepository;
        _workflowFormRepository = workflowFormRepository;
    }

    // 方法 GetMyLeavesAsync
    // 方法：GetMyLeavesAsync
    public async Task<List<LeaveRequest>> GetMyLeavesAsync(string applicant)
        => await _leaveRepository.FindByApplicantAsync(applicant);

    // 方法 GetPendingApprovalsAsync
    // 方法：GetPendingApprovalsAsync
    public async Task<List<LeaveRequest>> GetPendingApprovalsAsync(string userCode)
        => await _leaveRepository.FindByApproverAsync(userCode);

    // 方法 SubmitAsync
    // 方法：SubmitAsync
    public async Task<ApiResult> SubmitAsync(SubmitLeaveDto dto)
    {
        // if 条件判断
        if (dto.StartDate >= dto.EndDate)
            // return 返回结果
            return ApiResult.Fail(400, "结束日期必须大于开始日期");

        // 创建 LeaveRequest实例并赋给 leave
        var leave = new LeaveRequest
        {
            LeaveRequestId = IdGenerator.NewId(),
            Applicant = dto.Applicant,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DurationDays = (dto.EndDate - dto.StartDate).TotalDays,
            Reason = dto.Reason,
            Status = "pending",
            ManagerApprover = dto.ManagerApprover,
            HrApprover = dto.HrApprover,
            SubmitDate = DateTime.Now,
            CreateBy = dto.Applicant,
            CreateDate = DateTime.Now,
            UpdateBy = dto.Applicant,
            UpdateDate = DateTime.Now
        };
        // await 异步等待
        await _leaveRepository.AddAsync(leave);

        var submitResult = await _bpmService.SubmitApprovalAsync(new ApprovalSubmitDto
        {
            BusinessKey = leave.LeaveRequestId,
            BusinessType = "leave",
            FormData = $"{{\"leaveType\":\"{dto.LeaveType}\",\"reason\":\"{dto.Reason}\",\"days\":{leave.DurationDays}}}"
        });

        // if 条件判断
        if (submitResult.Code != 200) return submitResult;

        var form = await _workflowFormRepository.FindByBusinessKeyAsync(leave.LeaveRequestId);
        // if 条件判断
        if (form == null) return ApiResult.Fail(500, "创建审批单失败");

        // await 异步等待
        await _approvalRecordRepository.AddAsync(new ApprovalRecord
        {
            RecordId = IdGenerator.NewId(),
            WorkflowInstanceId = form.WorkflowInstanceId,
            BusinessKey = leave.LeaveRequestId,
            BusinessType = "leave",
            ActivityId = "manager_approval",
            ActivityName = "经理审批",
            Assignee = dto.ManagerApprover,
            AssigneeName = dto.ManagerName,
            Result = "pending",
            CreateDate = DateTime.Now
        });

        // return 返回结果
        return ApiResult.Ok(new { leave.LeaveRequestId });
    }

    // 方法 ApproveAsync
    // 方法：ApproveAsync
    public async Task<ApiResult> ApproveAsync(ApproveLeaveDto dto)
    {
        // 缓存：获取值
        var leave = await _leaveRepository.GetAsync(dto.LeaveRequestId);
        // if 条件判断
        if (leave == null) return ApiResult.NotFound("请假申请不存在");

        // if 条件判断
        if (dto.Step == "manager")
        {
            // if 条件判断
            if (dto.Result == "approved")
            {
                leave.Status = "hr_pending";
                // await 异步等待
                await _approvalRecordRepository.AddAsync(new ApprovalRecord
                {
                    RecordId = IdGenerator.NewId(),
                    BusinessKey = leave.LeaveRequestId,
                    BusinessType = "leave",
                    ActivityId = "hr_confirmation",
                    ActivityName = "HR确认",
                    Assignee = leave.HrApprover,
                    AssigneeName = dto.NextAssigneeName,
                    Result = "pending",
                    CreateDate = DateTime.Now
                });
            }
            // else 否则分支
            else
            {
                leave.Status = "rejected";
                leave.CompleteDate = DateTime.Now;
            }
        }
        // else if 否则如果：附加条件判断
        else if (dto.Step == "hr")
        {
            leave.Status = dto.Result == "approved" ? "approved" : "rejected";
            leave.CompleteDate = DateTime.Now;
        }

        leave.UpdateBy = dto.Operator;
        leave.UpdateDate = DateTime.Now;
        // await 异步等待
        await _leaveRepository.UpdateAsync(leave);

        // if 条件判断
        if (!string.IsNullOrEmpty(dto.RecordId))
        {
            // await 异步等待
            await _bpmService.ApproveAsync(new ApprovalActionDto
            {
                RecordId = dto.RecordId,
                Result = dto.Result == "approved" ? "approved" : "rejected",
                Comment = dto.Comment
            });
        }

        // return 返回结果
        return ApiResult.Ok();
    }
}
