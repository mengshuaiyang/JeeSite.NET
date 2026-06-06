using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace JeeSiteNET.Modules.Bpm.Activities;

[Activity("JeeSite", "审批", "创建一条审批记录")]
public class CreateApprovalActivity : CodeActivity
{
    [Input(Description = "业务主键")]
    public Input<string> BusinessKey { get; set; } = default!;

    [Input(Description = "业务类型")]
    public Input<string> BusinessType { get; set; } = default!;

    [Input(Description = "审批人")]
    public Input<string> Assignee { get; set; } = default!;

    [Input(Description = "审批人姓名")]
    public Input<string> AssigneeName { get; set; } = default!;

    [Input(Description = "活动名称")]
    public Input<string> ActivityName { get; set; } = default!;

    [Output(Description = "创建的记录ID")]
    public Output<string?> RecordId { get; set; } = default!;

    protected override void Execute(ActivityExecutionContext context)
    {
        var recordId = Guid.NewGuid().ToString("N")[..20];
        context.Set(RecordId, recordId);
    }
}
