namespace JeeSiteNET.Modules.CodeGen.Application.DTOs;

public class GenConfigDto
{
    public string ModuleCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string FunctionName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string? TplCategory { get; set; } = "crud";
    public bool GenEntity { get; set; } = true;
    public bool GenDto { get; set; } = true;
    public bool GenRepository { get; set; } = true;
    public bool GenService { get; set; } = true;
    public bool GenController { get; set; } = true;
    public bool GenVue { get; set; } = true;
}

public class GenPreviewItem
{
    public string FileName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class ImportTableRequest
{
    public List<string> TableNames { get; set; } = [];
}
