    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Domain.Entities
namespace JeeSiteNET.Modules.Test.Domain.Entities;

// 定义class TestData
// 定义类：TestData
public class TestData : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性：TestInput
    public string? TestInput { get; set; }
    // 属性：TestTextarea
    public string? TestTextarea { get; set; }
    // 属性：TestSelect
    public string? TestSelect { get; set; }
    // 属性：TestSelectMultiple
    public string? TestSelectMultiple { get; set; }
    // 属性：TestRadio
    public string? TestRadio { get; set; }
    // 属性：TestCheckbox
    public string? TestCheckbox { get; set; }
    // 属性：TestDate
    public DateTime? TestDate { get; set; }
    // 属性：TestDatetime
    public DateTime? TestDatetime { get; set; }
    // 属性：TestUserCode
    public string? TestUserCode { get; set; }
    // 属性：TestOfficeCode
    public string? TestOfficeCode { get; set; }
    // 属性：TestAreaCode
    public string? TestAreaCode { get; set; }
    // 属性：TestAreaName
    public string? TestAreaName { get; set; }
}
