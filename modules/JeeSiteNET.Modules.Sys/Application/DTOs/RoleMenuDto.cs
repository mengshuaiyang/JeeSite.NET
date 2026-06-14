// 定义 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
namespace JeeSiteNET.Modules.Sys.Application.DTOs;

// 定义class RoleMenuSaveDto
// 定义类：RoleMenuSaveDto
public class RoleMenuSaveDto
{
    // 属性 RoleCode
    // 属性：RoleCode
    public string RoleCode { get; set; } = string.Empty;
    // 属性 MenuCodes
    // 属性：MenuCodes
    public List<string> MenuCodes { get; set; } = [];
}
