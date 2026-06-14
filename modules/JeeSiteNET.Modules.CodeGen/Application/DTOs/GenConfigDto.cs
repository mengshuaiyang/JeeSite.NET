// 定义 JeeSiteNET.Modules.CodeGen.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Application.DTOs
namespace JeeSiteNET.Modules.CodeGen.Application.DTOs;

// 定义class GenConfigDto
// 定义类：GenConfigDto
public class GenConfigDto
{
    // 属性 ModuleCode
    // 属性：ModuleCode
    public string ModuleCode { get; set; } = string.Empty;
    // 属性 ClassName
    // 属性：ClassName
    public string ClassName { get; set; } = string.Empty;
    // 属性 FunctionName
    // 属性：FunctionName
    public string FunctionName { get; set; } = string.Empty;
    // 属性 BusinessName
    // 属性：BusinessName
    public string BusinessName { get; set; } = string.Empty;
    // 属性：TplCategory
    public string? TplCategory { get; set; } = "crud";
    // 属性 GenEntity
    // 属性：GenEntity
    public bool GenEntity { get; set; } = true;
    // 属性 GenDto
    // 属性：GenDto
    public bool GenDto { get; set; } = true;
    // 属性 GenRepository
    // 属性：GenRepository
    public bool GenRepository { get; set; } = true;
    // 属性 GenService
    // 属性：GenService
    public bool GenService { get; set; } = true;
    // 属性 GenController
    // 属性：GenController
    public bool GenController { get; set; } = true;
    // 属性 GenVue
    // 属性：GenVue
    public bool GenVue { get; set; } = true;
}

// 定义class GenPreviewItem
// 定义类：GenPreviewItem
public class GenPreviewItem
{
    // 属性 FileName
    // 属性：FileName
    public string FileName { get; set; } = string.Empty;
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
}

// 定义class ImportTableRequest
// 定义类：ImportTableRequest
public class ImportTableRequest
{
    // 属性 TableNames
    // 属性：TableNames
    public List<string> TableNames { get; set; } = [];
}
