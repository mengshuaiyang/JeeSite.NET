using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Application.DTOs;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Bpm.Application.Services;

public class BpmService
{
    private readonly IApprovalRecordRepository _approvalRecordRepository;
    private readonly IWorkflowFormRepository _workflowFormRepository;

    public BpmService(IApprovalRecordRepository approvalRecordRepository, IWorkflowFormRepository workflowFormRepository)
    {
        _approvalRecordRepository = approvalRecordRepository;
        _workflowFormRepository = workflowFormRepository;
    }

    public async Task<PageResult<ApprovalRecordDto>> FindRecordsPageAsync(PageRequest<ApprovalRecord> request)
    {
        var query = _approvalRecordRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.BusinessKey), r => r.BusinessKey == request.Entity!.BusinessKey)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.BusinessType), r => r.BusinessType == request.Entity!.BusinessType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Assignee), r => r.Assignee == request.Entity!.Assignee)
            .OrderByDescending(r => r.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<ApprovalRecordDto> { List = list.Select(ApprovalRecordDto.FromEntity).ToList(), Total = total };
    }

    public async Task<ApiResult> SubmitApprovalAsync(ApprovalSubmitDto dto)
    {
        var now = DateTime.Now;
        var form = await _workflowFormRepository.FindByBusinessKeyAsync(dto.BusinessKey);
        if (form != null)
            return ApiResult.Fail(400, "该业务已经提交审批");

        form = new WorkflowForm
        {
            FormId = Guid.NewGuid().ToString("N")[..20],
            WorkflowInstanceId = Guid.NewGuid().ToString(),
            BusinessKey = dto.BusinessKey,
            BusinessType = dto.BusinessType,
            FormData = dto.FormData,
            CurrentActivityId = "start",
            FormStatus = "pending",
            CreateDate = now,
            UpdateDate = now
        };
        await _workflowFormRepository.AddAsync(form);
        return ApiResult.Ok(new { form.FormId, form.WorkflowInstanceId });
    }

    public async Task<ApiResult> ApproveAsync(ApprovalActionDto dto)
    {
        var record = await _approvalRecordRepository.GetAsync(dto.RecordId);
        if (record == null) return ApiResult.NotFound("审批记录不存在");
        record.Result = dto.Result;
        record.Comment = dto.Comment;
        record.CompletedDate = DateTime.Now;
        await _approvalRecordRepository.UpdateAsync(record);
        return ApiResult.Ok();
    }

    public async Task<List<ApprovalRecordDto>> GetHistoryAsync(string businessKey)
    {
        var records = await _approvalRecordRepository.FindByBusinessKeyAsync(businessKey);
        return records.Select(ApprovalRecordDto.FromEntity).ToList();
    }
}
