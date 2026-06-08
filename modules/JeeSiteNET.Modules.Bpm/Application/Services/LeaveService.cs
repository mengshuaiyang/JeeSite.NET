using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Bpm.Application.DTOs;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;

namespace JeeSiteNET.Modules.Bpm.Application.Services;

public class LeaveService
{
    private readonly ILeaveRepository _leaveRepository;
    private readonly BpmService _bpmService;
    private readonly IApprovalRecordRepository _approvalRecordRepository;
    private readonly IWorkflowFormRepository _workflowFormRepository;

    public LeaveService(ILeaveRepository leaveRepository, BpmService bpmService, IApprovalRecordRepository approvalRecordRepository, IWorkflowFormRepository workflowFormRepository)
    {
        _leaveRepository = leaveRepository;
        _bpmService = bpmService;
        _approvalRecordRepository = approvalRecordRepository;
        _workflowFormRepository = workflowFormRepository;
    }

    public async Task<List<LeaveRequest>> GetMyLeavesAsync(string applicant)
        => await _leaveRepository.FindByApplicantAsync(applicant);

    public async Task<List<LeaveRequest>> GetPendingApprovalsAsync(string userCode)
        => await _leaveRepository.FindByApproverAsync(userCode);

    public async Task<ApiResult> SubmitAsync(SubmitLeaveDto dto)
    {
        if (dto.StartDate >= dto.EndDate)
            return ApiResult.Fail(400, "结束日期必须大于开始日期");

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
        await _leaveRepository.AddAsync(leave);

        var submitResult = await _bpmService.SubmitApprovalAsync(new ApprovalSubmitDto
        {
            BusinessKey = leave.LeaveRequestId,
            BusinessType = "leave",
            FormData = $"{{\"leaveType\":\"{dto.LeaveType}\",\"reason\":\"{dto.Reason}\",\"days\":{leave.DurationDays}}}"
        });

        if (submitResult.Code != 200) return submitResult;

        var form = await _workflowFormRepository.FindByBusinessKeyAsync(leave.LeaveRequestId);
        if (form == null) return ApiResult.Fail(500, "创建审批单失败");

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

        return ApiResult.Ok(new { leave.LeaveRequestId });
    }

    public async Task<ApiResult> ApproveAsync(ApproveLeaveDto dto)
    {
        var leave = await _leaveRepository.GetAsync(dto.LeaveRequestId);
        if (leave == null) return ApiResult.NotFound("请假申请不存在");

        if (dto.Step == "manager")
        {
            if (dto.Result == "approved")
            {
                leave.Status = "hr_pending";
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
            else
            {
                leave.Status = "rejected";
                leave.CompleteDate = DateTime.Now;
            }
        }
        else if (dto.Step == "hr")
        {
            leave.Status = dto.Result == "approved" ? "approved" : "rejected";
            leave.CompleteDate = DateTime.Now;
        }

        leave.UpdateBy = dto.Operator;
        leave.UpdateDate = DateTime.Now;
        await _leaveRepository.UpdateAsync(leave);

        if (!string.IsNullOrEmpty(dto.RecordId))
        {
            await _bpmService.ApproveAsync(new ApprovalActionDto
            {
                RecordId = dto.RecordId,
                Result = dto.Result == "approved" ? "approved" : "rejected",
                Comment = dto.Comment
            });
        }

        return ApiResult.Ok();
    }
}
