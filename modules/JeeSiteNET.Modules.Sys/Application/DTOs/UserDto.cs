namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class UserDto
{
    public string UserCode { get; set; } = string.Empty;
    public string LoginCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserType { get; set; } = "employee";
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OrgCode { get; set; }
    public string? OrgName { get; set; }
    public string? Status { get; set; }
    public DateTime? LoginDate { get; set; }
    public DateTime? CreateDate { get; set; }
}

public class UserSaveDto
{
    public string? UserCode { get; set; }
    public string LoginCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserType { get; set; } = "employee";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OrgCode { get; set; }
    public List<string>? RoleCodes { get; set; }
}
