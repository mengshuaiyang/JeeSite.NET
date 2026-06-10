namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class MsgInnerDto
{
    public string Id { get; set; } = string.Empty;
    public string MsgTitle { get; set; } = string.Empty;
    public string? ContentLevel { get; set; }
    public string? ContentType { get; set; }
    public string? MsgContent { get; set; }
    public string? ReceiveType { get; set; }
    public string? SendUserCode { get; set; }
    public string? SendUserName { get; set; }
    public DateTime? SendDate { get; set; }
    public string? Status { get; set; }
    public string? NotifyTypes { get; set; }
}

public class MsgInnerSaveDto
{
    public string? Id { get; set; }
    public string MsgTitle { get; set; } = string.Empty;
    public string? ContentLevel { get; set; }
    public string? ContentType { get; set; }
    public string MsgContent { get; set; } = string.Empty;
    public string? ReceiveType { get; set; }
    public string? ReceiveCodes { get; set; }
    public string? NotifyTypes { get; set; }
    public string? IsAttac { get; set; }
}

public class MsgInnerRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string MsgInnerId { get; set; } = string.Empty;
    public string ReceiveUserCode { get; set; } = string.Empty;
    public string ReceiveUserName { get; set; } = string.Empty;
    public string ReadStatus { get; set; } = "0";
    public DateTime? ReadDate { get; set; }
    public string? IsStar { get; set; }
}

public class MsgPushDto
{
    public string Id { get; set; } = string.Empty;
    public string? MsgType { get; set; }
    public string? MsgTitle { get; set; }
    public string? MsgContent { get; set; }
    public string? BizKey { get; set; }
    public string? BizType { get; set; }
    public string? ReceiveUserCode { get; set; }
    public string? ReceiveUserName { get; set; }
    public string? SendUserCode { get; set; }
    public string? SendUserName { get; set; }
    public DateTime? SendDate { get; set; }
    public string? PushStatus { get; set; }
    public string? ReadStatus { get; set; }
}

public class MsgTemplateDto
{
    public string Id { get; set; } = string.Empty;
    public string? ModuleCode { get; set; }
    public string TplKey { get; set; } = string.Empty;
    public string TplName { get; set; } = string.Empty;
    public string TplType { get; set; } = string.Empty;
    public string TplContent { get; set; } = string.Empty;
    public string? Status { get; set; }
}

public class MsgPushSaveDto
{
    public string? Id { get; set; }
    public string MsgType { get; set; } = "pc";
    public string MsgTitle { get; set; } = string.Empty;
    public string MsgContent { get; set; } = string.Empty;
    public string? BizKey { get; set; }
    public string? BizType { get; set; }
    public string? ReceiveCode { get; set; }
    public string? ReceiveUserCode { get; set; }
    public string? ReceiveUserName { get; set; }
    public DateTime? PlanPushDate { get; set; }
}

public class MsgTemplateSaveDto
{
    public string? Id { get; set; }
    public string? ModuleCode { get; set; }
    public string TplKey { get; set; } = string.Empty;
    public string TplName { get; set; } = string.Empty;
    public string TplType { get; set; } = string.Empty;
    public string TplContent { get; set; } = string.Empty;
}
