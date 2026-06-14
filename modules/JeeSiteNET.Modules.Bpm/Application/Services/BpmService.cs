    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Bpm.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.DTOs
using JeeSiteNET.Modules.Bpm.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Bpm.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Application.Services
namespace JeeSiteNET.Modules.Bpm.Application.Services;

// 定义class BpmService
// 定义类：BpmService
public class BpmService
{
    // 字段 _approvalRecordRepository
    // 字段：_approvalRecordRepository
    private readonly IApprovalRecordRepository _approvalRecordRepository;
    // 字段 _workflowFormRepository
    // 字段：_workflowFormRepository
    private readonly IWorkflowFormRepository _workflowFormRepository;

    // 方法 BpmService
    // 构造函数：BpmService
    public BpmService(IApprovalRecordRepository approvalRecordRepository, IWorkflowFormRepository workflowFormRepository)
    {
        _approvalRecordRepository = approvalRecordRepository;
        _workflowFormRepository = workflowFormRepository;
    }

    // 方法 FindRecordsPageAsync
    // 方法：FindRecordsPageAsync
    public async Task<PageResult<ApprovalRecordDto>> FindRecordsPageAsync(PageRequest<ApprovalRecord> request)
    {
        // 调用 Query
        var query = _approvalRecordRepository.Query()
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.BusinessKey), r => r.BusinessKey == request.Entity!.BusinessKey)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.BusinessType), r => r.BusinessType == request.Entity!.BusinessType)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Assignee), r => r.Assignee == request.Entity!.Assignee)
            // 数据库操作：降序排序
            .OrderByDescending(r => r.CreateDate);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<ApprovalRecordDto> { List = list.Select(ApprovalRecordDto.FromEntity).ToList(), Total = total };
    }

    // 方法 SubmitApprovalAsync
    // 方法：SubmitApprovalAsync
    public async Task<ApiResult> SubmitApprovalAsync(ApprovalSubmitDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        var form = await _workflowFormRepository.FindByBusinessKeyAsync(dto.BusinessKey);
        // if 条件判断
        if (form != null)
            // return 返回结果
            return ApiResult.Fail(400, "该业务已经提交审批");

        // 创建 WorkflowForm实例并赋给 form
        form = new WorkflowForm
        {
            // 调用 ToString
            FormId = Guid.NewGuid().ToString("N")[..20],
            // 调用 ToString
            WorkflowInstanceId = Guid.NewGuid().ToString(),
            BusinessKey = dto.BusinessKey,
            BusinessType = dto.BusinessType,
            FormData = dto.FormData,
            CurrentActivityId = "start",
            FormStatus = "pending",
            CreateDate = now,
            UpdateDate = now
        };
        // await 异步等待
        await _workflowFormRepository.AddAsync(form);
        // return 返回结果
        return ApiResult.Ok(new { form.FormId, form.WorkflowInstanceId });
    }

    // 方法 ApproveAsync
    // 方法：ApproveAsync
    public async Task<ApiResult> ApproveAsync(ApprovalActionDto dto)
    {
        // 缓存：获取值
        var record = await _approvalRecordRepository.GetAsync(dto.RecordId);
        // if 条件判断
        if (record == null) return ApiResult.NotFound("审批记录不存在");
        record.Result = dto.Result;
        record.Comment = dto.Comment;
        record.CompletedDate = DateTime.Now;
        // await 异步等待
        await _approvalRecordRepository.UpdateAsync(record);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 GetHistoryAsync
    // 方法：GetHistoryAsync
    public async Task<List<ApprovalRecordDto>> GetHistoryAsync(string businessKey)
    {
        var records = await _approvalRecordRepository.FindByBusinessKeyAsync(businessKey);
        // return 返回结果
        return records.Select(ApprovalRecordDto.FromEntity).ToList();
    }
}
