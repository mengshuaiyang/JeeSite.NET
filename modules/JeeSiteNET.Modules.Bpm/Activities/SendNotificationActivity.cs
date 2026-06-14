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
    // 引入 Microsoft.Extensions.Logging 命名空间
// 引入命名空间：Microsoft.Extensions.Logging
using Microsoft.Extensions.Logging;

// 定义 JeeSiteNET.Modules.Bpm.Activities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Activities
namespace JeeSiteNET.Modules.Bpm.Activities;

[Activity("JeeSite", "通知", "发送审批通知")]
// 定义class SendNotificationActivity
// 定义类：SendNotificationActivity
public class SendNotificationActivity : CodeActivity
{
    [Input(Description = "接收人")]
    // 属性 Recipient
    // 属性：Recipient
    public Input<string> Recipient { get; set; } = default!;

    [Input(Description = "消息标题")]
    // 属性 Subject
    // 属性：Subject
    public Input<string> Subject { get; set; } = default!;

    [Input(Description = "消息内容")]
    // 属性 Body
    // 属性：Body
    public Input<string> Body { get; set; } = default!;

    // 方法 Execute
    // 方法：Execute
    protected override void Execute(ActivityExecutionContext context)
    {
        // 声明并初始化变量：logger
        var logger = context.GetRequiredService<ILogger<SendNotificationActivity>>();
        // 调用 Get
        var subject = context.Get(Subject);
        // 调用 Get
        var body = context.Get(Body);
        // 日志：记录信息
        logger.LogInformation("通知: {Subject} - {Body}", subject, body);
    }
}
