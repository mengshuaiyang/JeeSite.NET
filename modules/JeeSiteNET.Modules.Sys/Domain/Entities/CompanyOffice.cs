using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class CompanyOffice : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string OfficeCode { get; set; } = string.Empty;
}
