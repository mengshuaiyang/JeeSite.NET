using JeeSiteNET.Modules.Test.Domain.Entities;

namespace JeeSiteNET.Modules.Test.Application.DTOs;

public class TestDataDto
{
    public string Id { get; set; } = string.Empty;
    public string? TestInput { get; set; }
    public string? TestTextarea { get; set; }
    public string? TestSelect { get; set; }
    public string? TestSelectMultiple { get; set; }
    public string? TestRadio { get; set; }
    public string? TestCheckbox { get; set; }
    public DateTime? TestDate { get; set; }
    public DateTime? TestDatetime { get; set; }
    public string? TestUserCode { get; set; }
    public string? TestOfficeCode { get; set; }
    public string? TestAreaCode { get; set; }
    public string? TestAreaName { get; set; }
    public string? Status { get; set; }

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

public class TestTreeDto
{
    public string TreeCode { get; set; } = string.Empty;
    public string TreeName { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public string? ParentCodes { get; set; }
    public decimal TreeSort { get; set; }
    public string? TreeSorts { get; set; }
    public string? TreeLeaf { get; set; }
    public decimal TreeLevel { get; set; }
    public string? TreeNames { get; set; }
    public string? Status { get; set; }
    public List<TestTreeDto>? Children { get; set; }

    public static TestTreeDto FromEntity(TestTree e) => new()
    {
        TreeCode = e.TreeCode, TreeName = e.TreeName,
        ParentCode = e.ParentCode, ParentCodes = e.ParentCodes,
        TreeSort = e.TreeSort, TreeSorts = e.TreeSorts,
        TreeLeaf = e.TreeLeaf, TreeLevel = e.TreeLevel,
        TreeNames = e.TreeNames, Status = e.Status
    };
}
