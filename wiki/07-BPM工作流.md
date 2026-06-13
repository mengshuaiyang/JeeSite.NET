<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 07 BPM工作流

> 基于 Elsa 工作流引擎的请假审批、报销审批等业务流程，支持可视化流程设计。
>
> **适用角色**：全栈开发人员
> **阅读时间**：约 10 分钟
> **相关文档**：[03-Sys系统管理](03-Sys系统管理) · [19-数据与字段权限](19-数据与字段权限)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、模块概述](#一、模块概述)
  - [1.1 模块定位](#11-模块定位)
  - [1.2 能力矩阵](#12-能力矩阵)
  - [1.3 与其他模块的依赖](#13-与其他模块的依赖)
- [二、核心实体](#二、核心实体)
  - [2.1 LeaveRequest（请假申请）](#21-leaverequest（请假申请）)
  - [2.2 ApprovalRecord（审批记录）](#22-approvalrecord（审批记录）)
  - [2.3 WorkflowForm（工作流表单定义）](#23-workflowform（工作流表单定义）)
- [三、核心服务](#三、核心服务)
  - [3.1 LeaveService（请假业务服务）](#31-leaveservice（请假业务服务）)
  - [3.2 BpmService（审批与流程服务）](#32-bpmservice（审批与流程服务）)
  - [3.3 自定义 Elsa Activity](#33-自定义-elsa-activity)
- [四、控制器与 API](#四、控制器与-api)
  - [4.1 LeaveController](#41-leavecontroller)
  - [4.2 ApprovalController](#42-approvalcontroller)
  - [4.3 请求体示例](#43-请求体示例)
- [五、前端页面](#五、前端页面)
- [六、请假审批流程图](#六、请假审批流程图)
- [七、扩展与自定义工作流](#七、扩展与自定义工作流)
  - [7.1 步骤概览](#71-步骤概览)
  - [7.2 代码定义一个极简工作流（示例）](#72-代码定义一个极简工作流（示例）)
  - [7.3 人工审批恢复约定](#73-人工审批恢复约定)
- [八、模块主要文件结构](#八、模块主要文件结构)
- [九、关键配置项（appsettings.json）](#九、关键配置项（appsettingsjson）)
  - [9.1 常见问题排查](#91-常见问题排查)

---


## 一、模块概述

### 1.1 模块定位

Bpm 模块以 **Elsa Workflows 3.x** 为工作流引擎，为 JeeSite.NET 提供可视化/可编程的工作流能力。模块同时预置了一套「请假审批」示例流程，覆盖多级审批、条件分支、通知发送、审批历史记录等典型场景，业务方可以此为模板快速扩展为采购、报销、合同审批等业务流程。

- 模块所在程序集：`JeeSiteNET.Modules.Bpm`
- API 根路径：`/api/v1/bpm/`
- 前端入口：`frontend/src/views/bpm/`

### 1.2 能力矩阵

- **工作流蓝图管理**（基于 Elsa 的 `IWorkflowBlueprint` / Workflow Definition）
- **多级审批**：主管审批 → HR 备案 → 结束，支持任意新增节点
- **条件分支**（申请人所在部门 / 请假时长 / 请假类型 …）
- **通知发送**（站内信 / 邮件 / 短信，由 `MsgService` 或 Email/Sms 服务驱动）
- **审批历史**：`ApprovalRecord` 记录每次审批人与意见
- **人工恢复**：审批人操作后通过 `ResumeWorkflowAsync` 让流程在该节点继续执行
- **与业务实体解耦**：工作流蓝图只接受输入/输出参数，业务实体由业务服务读写

### 1.3 与其他模块的依赖

- `JeeSiteNET.Modules.Sys`：用户、员工、字典、消息、权限
- `JeeSiteNET.Modules.Tasks`（可选）：定时触发流程

## 二、核心实体

### 2.1 LeaveRequest（请假申请）

命名空间：`JeeSiteNET.Modules.Bpm.Domain.Entities.LeaveRequest`

| 字段 | 类型 | 含义 |
|------|------|------|
| RequestId | long | 主键 |
| RequesterCode | string | 申请人 user_code |
| RequesterName | string | 申请人姓名（冗余，便于列表展示） |
| LeaveType | string | 类型：事假/病假/年假/调休/婚假/产假/其他（字典 `leave_type`） |
| StartTime | DateTime | 请假开始时间 |
| EndTime | DateTime | 请假结束时间 |
| DurationHours | decimal | 时长（小时），用于按天数计费或流程分支判断 |
| Reason | string | 事由 |
| Attachment | string | 附件 JSON（可选） |
| Status | string | Draft / Submitted / Approved / Rejected |
| CurrentStep | string | 当前步骤名称（便于展示当前节点） |
| WorkflowInstanceId | string | 关联的工作流实例 ID（Elsa Workflow Instance） |
| CreateBy | string | 创建人 |
| CreateDate | DateTime | 创建时间 |
| UpdateBy | string | 更新人 |
| UpdateDate | DateTime | 更新时间 |
| Remarks | string | 备注 |

### 2.2 ApprovalRecord（审批记录）

命名空间：`JeeSiteNET.Modules.Bpm.Domain.Entities.ApprovalRecord`

| 字段 | 类型 | 含义 |
|------|------|------|
| RecordId | long | 主键 |
| RequestId | long | 关联请假申请 |
| ApproverCode | string | 审批人 user_code |
| ApproverName | string | 审批人姓名（冗余） |
| StepName | string | 审批步骤名称（如「主管审批」「HR 备案」） |
| Decision | string | `Approved` / `Rejected` |
| Comment | string | 审批意见 |
| ApprovedAt | DateTime | 审批时间 |
| IsAutoStep | bool | 是否为自动步骤（非人工审批） |

### 2.3 WorkflowForm（工作流表单定义）

命名空间：`JeeSiteNET.Modules.Bpm.Domain.Entities.WorkflowForm`

用于将业务表单与 Elsa Workflow Blueprint 做映射，便于从业务视角管理「哪种单据用哪套流程」。

| 字段 | 类型 | 含义 |
|------|------|------|
| FormId | long | 主键 |
| FormName | string | 表单名称（如「请假申请」） |
| FormKey | string | 业务标识（如 `leave-request`） |
| WorkflowBlueprintId | string | 关联的工作流蓝图 ID（Elsa WorkflowDefinitionId） |
| JsonSchema | string | 表单 JSON Schema（用于前端表单自动渲染或校验） |
| UiSchema | string | 表单 UI Schema（可选，描述控件顺序） |
| Description | string | 描述 |
| Status | string | 状态（0 正常/1 停用） |
| CreateDate | DateTime | 创建时间 |

## 三、核心服务

### 3.1 LeaveService（请假业务服务）

命名空间：`JeeSiteNET.Modules.Bpm.Application.Services.LeaveService`

| 方法 | 说明 |
|------|------|
| CreateRequestAsync(LeaveCreateDto dto) | 新建请假申请（状态=Draft） |
| SubmitRequestAsync(long requestId) | 提交：状态=Submitted，启动工作流 |
| GetMyRequestsAsync(string userCode, pageNo, pageSize) | 我的申请列表 |
| GetRequestDetailAsync(long requestId) | 申请详情 + 审批记录列表 |
| UpdateStatusAsync(long requestId, string status) | 更新状态（流程回调时调用） |
| UpdateCurrentStepAsync(long requestId, string stepName) | 更新当前步骤（流程节点进入时调用） |
| DeleteDraftAsync(long requestId) | 删除草稿 |

### 3.2 BpmService（审批与流程服务）

命名空间：`JeeSiteNET.Modules.Bpm.Application.Services.BpmService`

| 方法 | 说明 |
|------|------|
| GetPendingApprovalsAsync(string approverCode, pageNo, pageSize) | 待审批列表 |
| GetApprovedAsync(string approverCode, pageNo, pageSize) | 已审批列表 |
| ApproveAsync(long requestId, string approverCode, string comment) | 同意：写入审批记录 → 恢复工作流 |
| RejectAsync(long requestId, string approverCode, string reason) | 驳回：写入审批记录 → 恢复工作流（流程分支判定为驳回） |
| GetApprovalHistoryAsync(long requestId) | 审批历史（按时间升序） |
| StartWorkflowAsync(string blueprintId, object input) | 启动工作流实例，返回 InstanceId |
| ResumeWorkflowAsync(string instanceId, string activityId, object input) | 在人工审批后恢复工作流 |
| NotifyApproverAsync(long requestId, string approverCode, string stepName) | 发送审批通知（站内信/邮件） |
| GetAvailableBlueprintsAsync(string formKey) | 获取指定业务表单可用的工作流蓝图 |

### 3.3 自定义 Elsa Activity

- `CreateApprovalActivity`：写入审批记录、更新当前步骤
- `SendNotificationActivity`：基于 `MsgService` 发送站内信/邮件
- `DetermineApproverActivity`：根据申请人部门、职级等规则查找审批人

## 四、控制器与 API

### 4.1 LeaveController

命名空间：`JeeSiteNET.Modules.Bpm.Controllers.LeaveController`

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| GetMyAsync | GET | `/api/v1/bpm/leave/my` | 我的请假列表 |
| GetAsync | GET | `/api/v1/bpm/leave/{id}` | 详情（包含审批记录） |
| CreateAsync | POST | `/api/v1/bpm/leave` | 创建请假（状态=Draft） |
| UpdateDraftAsync | PUT | `/api/v1/bpm/leave/{id}/update` | 编辑草稿 |
| SubmitAsync | POST | `/api/v1/bpm/leave/{id}/submit` | 提交审批（状态=Submitted，并启动工作流） |
| DeleteDraftAsync | DELETE | `/api/v1/bpm/leave/{id}` | 删除草稿 |

### 4.2 ApprovalController

命名空间：`JeeSiteNET.Modules.Bpm.Controllers.ApprovalController`

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| GetPendingAsync | GET | `/api/v1/bpm/approval/pending` | 待审批列表 |
| GetApprovedAsync | GET | `/api/v1/bpm/approval/approved` | 已审批列表 |
| ApproveAsync | POST | `/api/v1/bpm/approval/{requestId}/approve` | 同意 |
| RejectAsync | POST | `/api/v1/bpm/approval/{requestId}/reject` | 驳回 |
| GetHistoryAsync | GET | `/api/v1/bpm/approval/{requestId}/history` | 审批历史 |

### 4.3 请求体示例

**创建请假（POST /api/v1/bpm/leave）**：

```json
{
  "leaveType": "annual",
  "startTime": "2026-06-15T09:00:00+08:00",
  "endTime":   "2026-06-17T18:00:00+08:00",
  "reason": "家庭旅行"
}
```

**同意（POST /api/v1/bpm/approval/{requestId}/approve）**：

```json
{
  "comment": "同意，注意交接工作。"
}
```

## 五、前端页面

| 页面 | 文件 | 说明 |
|------|------|------|
| 我的请假 | `views/bpm/MyLeave.vue` | 列表 + 新建 + 编辑（Draft） + 提交 + 详情 |
| 待审批 | `views/bpm/PendingApproval.vue` | 列表 + 审批操作（同意/驳回 + 意见） + 详情 |
| 已审批 | `views/bpm/ApprovalHistory.vue`（或 PendingApproval 组件内切换） | 我已审批的记录 |
| 流程设计（可选） | `views/bpm/WorkflowDesigner.vue` | Elsa Studio 嵌入 |

前端 API 封装见 `frontend/src/api/leave.ts`。

## 六、请假审批流程图

```
                        ┌──────────────────────┐
                        │   员工提交申请        │
                        │  (草稿 → Submitted)   │
                        │  调用 LeaveService    │
                        │   .SubmitRequestAsync │
                        └─────────┬────────────┘
                                  │
                                  ▼
                ┌────────────────────────────────────┐
                │   BpmService.StartWorkflowAsync     │
                │   (LeaveApproval Blueprint)         │
                │   写入 WorkflowInstanceId           │
                └─────────────────┬──────────────────┘
                                  │
                                  ▼
                  ┌────────────────────────────────┐
                  │  DetermineApproverActivity     │
                  │  (查询申请人的主管/HR，写入     │
                  │   CurrentStep = 主管审批)       │
                  └─────────────┬──────────────────┘
                                │
                                ▼
                ┌─────────────────────────────────────┐
                │   人工节点（阻塞等待审批）            │
                │   PendingApproval 列表可看到该申请   │
                └──────────┬──────────────────────────┘
                           │
        ┌──────────────────┼─────────────────────────┐
        ▼                  ▼                          │
  ┌────────────┐   ┌───────────────┐                 │
  │  同意       │   │  驳回         │                 │
  │  ApproveAsync│ │  RejectAsync   │                 │
  └────┬───────┘   └───────┬───────┘                 │
       │                   │                          │
       ▼                   ▼                          │
  ┌─────────────┐   ┌──────────────────┐              │
  │ 写入审批记录 │   │ 写入审批记录      │              │
  │ Decision =  │   │ Decision = Rejected│             │
  │ Approved    │   └────────┬─────────┘              │
  └────┬───────┘            │                        │
       │                    ▼                        │
       │            ┌─────────────────────┐          │
       │            │ UpdateStatus =      │          │
       │            │ Rejected            │          │
       │            │ 发送通知给员工        │          │
       │            │ 流程结束             │          │
       │            └─────────────────────┘          │
       ▼                                             │
  ┌─────────────────────┐                            │
  │ 进入下一节点          │                            │
  │ (HR 备案步骤)         │                            │
  │ CurrentStep = HR备案 │                            │
  └───────────┬──────────┘                            │
              │                                       │
     ┌────────┴────────────┐                          │
     ▼                     ▼                          │
  ┌────────┐          ┌─────────┐                     │
  │ HR同意 │          │ HR驳回 │                      │
  └───┬────┘          └────┬────┘                     │
      │                    │                          │
      ▼                    ▼                          │
  ┌───────────────┐  ┌─────────────────┐              │
  │ 状态=Approved  │  │ 状态=Rejected   │              │
  │ 通知员工        │  │ 通知员工         │             │
  │ 流程结束        │  │ 流程结束         │             │
  └────────────────┘  └─────────────────┘              │
                                                        │
                                                        ▼
                                                 （流程结束）
```

## 七、扩展与自定义工作流

### 7.1 步骤概览

目标：为新业务（如 `PurchaseRequest` 采购申请）添加工作流。

1. **创建业务实体与 DTO**
   - `PurchaseRequest`（与 `LeaveRequest` 字段结构类似）
   - `PurchaseRequestDto` / `ApprovalDto`
2. **创建仓储**：`IPurchaseRequestRepository` + `PurchaseRequestRepository`
3. **创建业务服务**：`PurchaseRequestService`（CRUD + Submit）
4. **创建或配置工作流蓝图**
   - 方式 A：在 Elsa Studio 中可视化拖拽并保存为 `PurchaseApproval`
   - 方式 B：以代码形式通过 `IWorkflow` 接口编写 `PurchaseApprovalWorkflow`
5. **自定义 Activity（如有需要）**
   - `DeterminePurchaseApproverActivity`：根据金额找对应审批人
   - `SendNotificationActivity`（可复用现有）
6. **创建 Controller 暴露 API**：`PurchaseRequestController`
7. **创建 Vue 页面**：
   - `views/purchase/MyPurchase.vue`
   - `views/purchase/PendingApproval.vue`（或复用 Bpm 的通用审批组件）
8. **在数据库中登记**
   - `WorkflowForm`：`{ FormName: "采购申请", FormKey: "purchase-request", WorkflowBlueprintId: "<blueprintId>" }`
   - `sys_menu`：菜单记录（`menu_href = views/purchase/MyPurchase`, `permission = bpm:purchase:list`）
   - `sys_dict_data`（若业务需要字典值）
9. **编译并验证**
   - `dotnet build`
   - `pnpm dev`
   - 登录：新建采购单 → 提交 → 切到审批人账号 → 审批 → 回到申请人查看状态

### 7.2 代码定义一个极简工作流（示例）

```csharp
using Elsa.Extensions;
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Models;
using JeeSiteNET.Modules.Bpm.Activities;

namespace JeeSiteNET.Modules.Bpm.Workflows
{
    public class LeaveApprovalWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder.Root = new Sequence
            {
                Activities =
                {
                    new DetermineApproverActivity
                    {
                        RequesterCode = new Input<string>(ctx => ctx.GetVariable<string>("RequesterCode")),
                        StepName = new Input<string>("主管审批")
                    },
                    new SendNotificationActivity
                    {
                        ToUserCode = new Input<string>(ctx => ctx.GetVariable<string>("ApproverCode")),
                        Title = new Input<string>("请假审批待处理")
                    },
                    // 人工审批节点：等待外部调用 ResumeWorkflowAsync
                    new Event("Approval:Submitted"),
                    // 再次查找 HR 审批人
                    new DetermineApproverActivity
                    {
                        StepName = new Input<string>("HR备案")
                    },
                    new SendNotificationActivity(),
                    new Event("Approval:HRSubmitted"),
                    // 结束更新状态
                    new SetVariable("Status", "Approved")
                }
            };
        }
    }
}
```

> 实际实现中会结合业务 Activity 与 Elsa 内置 `Event`/`If`/`Sequence` 等节点组织流程。

### 7.3 人工审批恢复约定

- 每个需要人工审批的节点命名一个 `Event`（如 `Approval:Submitted-{requestId}`）
- 前端点击「同意/驳回」调用 `ApprovalController.ApproveAsync` / `RejectAsync`
- 后端调用 `BpmService.ApproveAsync(requestId, approverCode, comment)`，内部：
  1. 写入 `ApprovalRecord`
  2. 通过 `IWorkflowRuntime` 触发 `ResumeWorkflowAsync(instanceId, activityId, input: new { Decision, Comment })`
  3. 工作流继续执行直到下一个人工节点或结束

## 八、模块主要文件结构

```
modules/JeeSiteNET.Modules.Bpm/
├─ Activities/
│   ├─ CreateApprovalActivity.cs
│   ├─ SendNotificationActivity.cs
│   └─ DetermineApproverActivity.cs
├─ Application/
│   ├─ DTOs/
│   │   ├─ LeaveDtos.cs
│   │   └─ ApprovalRecordDto.cs
│   └─ Services/
│       ├─ LeaveService.cs
│       └─ BpmService.cs
├─ Controllers/
│   ├─ LeaveController.cs
│   └─ ApprovalController.cs
├─ Domain/
│   ├─ Entities/
│   │   ├─ LeaveRequest.cs
│   │   ├─ ApprovalRecord.cs
│   │   └─ WorkflowForm.cs
│   └─ Interfaces/
│       ├─ ILeaveRepository.cs
│       ├─ IApprovalRecordRepository.cs
│       └─ IWorkflowFormRepository.cs
├─ Infrastructure/
│   ├─ EntityConfigurations/
│   │   ├─ LeaveRequestConfiguration.cs
│   │   ├─ ApprovalRecordConfiguration.cs
│   │   └─ WorkflowFormConfiguration.cs
│   └─ Repositories/
│       ├─ LeaveRepository.cs
│       ├─ ApprovalRecordRepository.cs
│       └─ WorkflowFormRepository.cs
├─ Workflows/
│   └─ LeaveApprovalWorkflow.cs
├─ BpmModuleInstaller.cs
└─ JeeSiteNET.Modules.Bpm.csproj

frontend/src/
├─ api/
│   └─ leave.ts
└─ views/bpm/
    ├─ MyLeave.vue
    └─ PendingApproval.vue
```

## 九、关键配置项（`appsettings.json`）

```json
{
  "Elsa": {
    "Server": {
      "BaseUrl": "https://localhost:5001/elsa/api"
    },
    "Workflows": {
      "LeaveApprovalBlueprintId": "leave-approval-v1"
    }
  }
}
```

### 9.1 常见问题排查

- **提交后工作流未启动**：检查 `WorkflowBlueprintId` 是否与 `WorkflowForm.FormKey` 正确映射；检查 Elsa Feature 是否正确注册。
- **人工审批后流程卡死**：确认 `ResumeWorkflowAsync` 传入的 `activityId` / 事件名称与工作流中定义一致；查看 Elsa Workflow Instance 的执行历史。
- **审批人查不到**：确认申请人有直属上级（`Employee.ParentCode` 或部门主管配置）；调试 `DetermineApproverActivity`。
- **通知未送达**：检查 `MsgService` 与 `EmailService` 配置（SMTP/短信网关等）；查看 `sys_msg_inner` 是否已写入。
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---
## 💡 快速参考
### 核心类与接口表

| 类型 | 名称 | 命名空间 | 说明 |
|------|------|---------|------|
| Service | `LeaveService` | `JeeSiteNET.Modules.Bpm.Application.Services` | 请假审批服务 |
| Service | `BpmService` | `JeeSiteNET.Modules.Bpm.Application.Services` | 流程管理服务 |
| Controller | `LeaveController` | `JeeSiteNET.Modules.Bpm.Controllers` | 请假 API |
| Controller | `ApprovalController` | `JeeSiteNET.Modules.Bpm.Controllers` | 审批 API |
| Entity | `LeaveRequest` | `JeeSiteNET.Modules.Bpm.Domain.Entities` | 请假申请实体 |
| Entity | `ApprovalRecord` | `JeeSiteNET.Modules.Bpm.Domain.Entities` | 审批记录实体 |
| Entity | `WorkflowForm` | `JeeSiteNET.Modules.Bpm.Domain.Entities` | 工作流表单实体 |

### 常用 API 速查表

| API | 说明 |
|-----|------|
| `POST /api/v1/bpm/leave` | 发起请假申请 |
| `GET /api/v1/bpm/leave/my` | 我的请假列表 |
| `GET /api/v1/bpm/leave/pending` | 待我审批的列表 |
| `POST /api/v1/bpm/leave/{id}/approve` | 审批通过 |
| `POST /api/v1/bpm/leave/{id}/reject` | 审批驳回 |

### 最小工作示例

```csharp
// ===== 发起请假申请 =====
[HttpPost]
public async Task<IActionResult> CreateLeave([FromBody] LeaveRequestDto dto)
{
    var leave = _mapper.Map<LeaveRequest>(dto);
    leave.ApplicantId = CurrentUserId;
    leave.Status = LeaveStatus.Pending;
    var result = await _leaveService.CreateAsync(leave);

    // 触发工作流：找到审批人并推送任务
    await _bpmService.StartWorkflowAsync("leave_approval", result.Id);
    return Ok(result);
}

// ===== 审批处理（Service 层）=====
public async Task ApproveAsync(string leaveId, string approverId, string comment)
{
    var leave = await _leaveRepository.GetByIdAsync(leaveId);
    // 记录审批动作
    await _approvalRecordRepository.CreateAsync(new ApprovalRecord
    {
        LeaveRequestId = leaveId,
        ApproverId = approverId,
        Action = ApprovalAction.Approved,
        Comment = comment,
        CreateTime = DateTime.Now
    });
    // 判断是否所有审批完成，更新状态
    leave.Status = LeaveStatus.Approved;
    await _leaveRepository.UpdateAsync(leave);
}
```

---
## ❓ 常见问题

1. **问：审批人休假期间如何转交？**
答：实现审批代理功能，在用户设置中配置代理人，工作流自动将待办任务同时推送给代理人。
2. **问：如何支持并行审批（多人同时审批）？**
答：配置并行网关节点，在所有并行分支全部完成后进入下一节点。

---
## 📚 相关文档

- **上一篇**：[06-Tasks任务调度](06-Tasks任务调度)
- **同系列**：[03-Sys系统管理](03-Sys系统管理) · [04-CMS内容管理](04-CMS内容管理) · [05-CodeGen代码生成](05-CodeGen代码生成) · [08-App移动端](08-App移动端)
- **下一篇**：[08-App移动端](08-App移动端)

---
## 🚀 下一步

- 结合 [06-Tasks任务调度](06-Tasks任务调度)，实现超时未审批自动提醒或跳转到下一级审批。
- 阅读 [19-数据与字段权限](19-数据与字段权限)，确保审批过程中用户只能看到自己有权限的申请单。
