    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;

// 定义 JeeSiteNET.Modules.Test.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Application.DTOs
namespace JeeSiteNET.Modules.Test.Application.DTOs;

// 定义class TestDataDto
// 定义类：TestDataDto
public class TestDataDto
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
    // 属性：Status
    public string? Status { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static TestDataDto FromEntity(TestData e) => new()
    {
        Id = e.Id, TestInput = e.TestInput, TestTextarea = e.TestTextarea,
        TestSelect = e.TestSelect, TestSelectMultiple = e.TestSelectMultiple,
        TestRadio = e.TestRadio, TestCheckbox = e.TestCheckbox,
        TestDate = e.TestDate, TestDatetime = e.TestDatetime,
        TestUserCode = e.TestUserCode, TestOfficeCode = e.TestOfficeCode,
        TestAreaCode = e.TestAreaCode, TestAreaName = e.TestAreaName,
        Status = e.Status
    };
}

// 定义class TestTreeDto
// 定义类：TestTreeDto
public class TestTreeDto
{
    // 属性 TreeCode
    // 属性：TreeCode
    public string TreeCode { get; set; } = string.Empty;
    // 属性 TreeName
    // 属性：TreeName
    public string TreeName { get; set; } = string.Empty;
    // 属性：ParentCode
    public string? ParentCode { get; set; }
    // 属性：ParentCodes
    public string? ParentCodes { get; set; }
    // 属性 TreeSort
    // 属性：TreeSort
    public decimal TreeSort { get; set; }
    // 属性：TreeSorts
    public string? TreeSorts { get; set; }
    // 属性：TreeLeaf
    public string? TreeLeaf { get; set; }
    // 属性 TreeLevel
    // 属性：TreeLevel
    public decimal TreeLevel { get; set; }
    // 属性：TreeNames
    public string? TreeNames { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：Children
    public List<TestTreeDto>? Children { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static TestTreeDto FromEntity(TestTree e) => new()
    {
        TreeCode = e.TreeCode, TreeName = e.TreeName,
        ParentCode = e.ParentCode, ParentCodes = e.ParentCodes,
        TreeSort = e.TreeSort, TreeSorts = e.TreeSorts,
        TreeLeaf = e.TreeLeaf, TreeLevel = e.TreeLevel,
        TreeNames = e.TreeNames, Status = e.Status
    };
}
