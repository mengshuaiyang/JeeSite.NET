namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class RoleMenuSaveDto
{
    public string RoleCode { get; set; } = string.Empty;
    public List<string> MenuCodes { get; set; } = [];
}
