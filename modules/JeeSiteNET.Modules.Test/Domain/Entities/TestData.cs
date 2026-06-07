using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Test.Domain.Entities;

public class TestData : DataEntity
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
}
