using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Modules.Bpm.Activities;

[Activity("JeeSite", "通知", "发送审批通知")]
public class SendNotificationActivity : CodeActivity
{
    [Input(Description = "接收人")]
    public Input<string> Recipient { get; set; } = default!;

    [Input(Description = "消息标题")]
    public Input<string> Subject { get; set; } = default!;

    [Input(Description = "消息内容")]
    public Input<string> Body { get; set; } = default!;

    protected override void Execute(ActivityExecutionContext context)
    {
        var logger = context.GetRequiredService<ILogger<SendNotificationActivity>>();
        var subject = context.Get(Subject);
        var body = context.Get(Body);
        logger.LogInformation("通知: {Subject} - {Body}", subject, body);
    }
}
