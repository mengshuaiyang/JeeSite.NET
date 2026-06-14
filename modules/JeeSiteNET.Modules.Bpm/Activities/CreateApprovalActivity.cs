    // 引入 Elsa.Workflows 命名空间
// 引入命名空间：Elsa.Workflows
using Elsa.Workflows;
    // 引入 Elsa.Workflows.Activities 命名空间
// 引入命名空间：Elsa.Workflows.Activities
using Elsa.Workflows.Activities;
    // 引入 Elsa.Workflows.Attributes 命名空间
// 引入命名空间：Elsa.Workflows.Attributes
using Elsa.Workflows.Attributes;
    // 引入 Elsa.Workflows.Models 命名空间
// 引入命名空间：Elsa.Workflows.Models
using Elsa.Workflows.Models;

// 定义 JeeSiteNET.Modules.Bpm.Activities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Activities
namespace JeeSiteNET.Modules.Bpm.Activities;

[Activity("JeeSite", "审批", "创建一条审批记录")]
// 定义class CreateApprovalActivity
// 定义类：CreateApprovalActivity
public class CreateApprovalActivity : CodeActivity
{
    [Input(Description = "业务主键")]
    // 属性 BusinessKey
    // 属性：BusinessKey
    public Input<string> BusinessKey { get; set; } = default!;

    [Input(Description = "业务类型")]
    // 属性 BusinessType
    // 属性：BusinessType
    public Input<string> BusinessType { get; set; } = default!;

    [Input(Description = "审批人")]
    // 属性 Assignee
    // 属性：Assignee
    public Input<string> Assignee { get; set; } = default!;

    [Input(Description = "审批人姓名")]
    // 属性 AssigneeName
    // 属性：AssigneeName
    public Input<string> AssigneeName { get; set; } = default!;

    [Input(Description = "活动名称")]
    // 属性 ActivityName
    // 属性：ActivityName
    public Input<string> ActivityName { get; set; } = default!;

    [Output(Description = "创建的记录ID")]
    // 属性：RecordId
    public Output<string?> RecordId { get; set; } = default!;

    // 方法 Execute
    // 方法：Execute
    protected override void Execute(ActivityExecutionContext context)
    {
        // 调用 ToString
        var recordId = Guid.NewGuid().ToString("N")[..20];
        // 调用 Set
        context.Set(RecordId, recordId);
    }
}
