namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class PostDto
{
    public string PostCode { get; set; } = string.Empty;
    public string PostName { get; set; } = string.Empty;
    public string? OrgCode { get; set; }
    public decimal? Sort { get; set; }
    public string? Status { get; set; }
}

public class PostSaveDto
{
    public string? PostCode { get; set; }
    public string PostName { get; set; } = string.Empty;
    public string? OrgCode { get; set; }
    public decimal? Sort { get; set; }
}
