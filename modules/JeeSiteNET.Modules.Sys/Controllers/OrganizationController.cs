    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/org")]
// 定义class OrganizationController
// 定义类：OrganizationController

public class OrganizationController : ControllerBase
{
    // 字段 _organizationService
    // 字段：_organizationService

    private readonly OrganizationService _organizationService;

    // 构造函数 OrganizationController
    // 构造函数：OrganizationController

    public OrganizationController(OrganizationService organizationService) => _organizationService = organizationService;

    [Permission("sys:org:list")]
    [HttpGet("tree")]
    // 方法 Tree
    // 方法：Tree

    public async Task<ApiResult<List<OrganizationDto>>> Tree([FromQuery] string? orgType = null)
    {
        var tree = await _organizationService.FindTreeAsync(orgType);
        // return 返回结果
        return ApiResult<List<OrganizationDto>>.Ok(tree);
    }

    [Permission("sys:org:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<OrganizationDto?>> Get([FromQuery] string orgCode)
    {
        // 缓存：获取值
        var org = await _organizationService.GetAsync(orgCode);
        // if 条件判断
        if (org == null) return ApiResult<OrganizationDto?>.NotFound("机构不存在");
        // return 返回结果
        return ApiResult<OrganizationDto?>.Ok(org);
    }

    [Permission("sys:org:edit")]
    [HttpPost("save")]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] OrganizationSaveDto dto)
    {
        // return 返回结果
        return await _organizationService.SaveAsync(dto);
    }

    [Permission("sys:org:delete")]
    [HttpPost("delete")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteOrgRequest request)
    {
        // return 返回结果
        return await _organizationService.DeleteAsync(request.OrgCode);
    }

    [Permission("sys:org:list")]
    [HttpPost("export")]
    // 方法 ExportData
    // 方法：ExportData

    public async Task<IActionResult> ExportData()
    {
        var tree = await _organizationService.FindTreeAsync(null);
        // 声明并初始化变量：flat
        var flat = FlattenTree(tree);
        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 声明并初始化变量：bytes
        var bytes = excel.Export(flat, "机构数据");
        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "机构数据.xlsx");
    }

    [Permission("sys:org:edit")]
    [HttpPost("import")]
    // 方法 ImportData
    // 方法：ImportData

    public async Task<ApiResult> ImportData(IFormFile file)
    {
        // if 条件判断
        if (file == null || file.Length == 0)
            // return 返回结果
            return ApiResult.Fail(400, "请选择上传文件");

    // 引入 var ms 命名空间
        using var ms = new MemoryStream();
        // await 异步等待
        await file.CopyToAsync(ms);
        ms.Position = 0;

        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 调用 ToArray
        var records = excel.Import<OrgExportRow>(ms.ToArray(), "机构数据");
        // return 返回结果
        return ApiResult.Ok(new { imported = records.Count });
    }

    [Permission("sys:org:edit")]
    [HttpGet("import-template")]
    // 方法 ImportTemplate
    // 方法：ImportTemplate

    public IActionResult ImportTemplate()
    {
        // 创建 ExcelService实例并赋给 excel
        var excel = new ExcelService();
        // 声明并初始化变量：bytes
        var bytes = excel.ExportTemplate<OrgExportRow>("机构数据");
        // return 返回结果
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "机构导入模板.xlsx");
    }

    // 方法 FlattenTree
    // 方法：FlattenTree

    private static List<OrgExportRow> FlattenTree(List<OrganizationDto> tree)
    {
        // 创建 List实例并赋给 result
        var result = new List<OrgExportRow>();

        void Walk(List<OrganizationDto> nodes, string parentPath)
        {
            // foreach 遍历集合
            foreach (var n in nodes)
            {
                // 调用 IsNullOrEmpty
                var path = string.IsNullOrEmpty(parentPath) ? n.OrgName : $"{parentPath}/{n.OrgName}";
                // if 条件判断
                if (n.OrgCode != "0")
                {
                    // 集合操作：添加元素
                    result.Add(new OrgExportRow
                    {
                        OrgCode = n.OrgCode,
                        OrgName = n.OrgName,
                        ViewCode = n.ViewCode,
                        FullName = n.FullName,
                        OrgType = n.OrgType,
                        Leader = n.Leader,
                        Phone = n.Phone,
                        ParentPath = parentPath,
                        TreePath = path
                    });
                }
                // if 条件判断
                if (n.Children?.Count > 0)
                    Walk(n.Children, path);
            }
        }
        Walk(tree, "");
        // return 返回结果
        return result;
    }
}

// 定义class DeleteOrgRequest
// 定义类：DeleteOrgRequest

public class DeleteOrgRequest
{
    // 属性 OrgCode
    // 属性：OrgCode

    public string OrgCode { get; set; } = string.Empty;
}

// 定义class OrgExportRow
// 定义类：OrgExportRow

public class OrgExportRow
{
    [ExcelField("机构编码", ColumnWidth = 20)]
    // 属性：OrgCode

    public string? OrgCode { get; set; }

    [ExcelField("机构名称", ColumnWidth = 30)]
    // 属性：OrgName

    public string? OrgName { get; set; }

    [ExcelField("机构代码", ColumnWidth = 15)]
    // 属性：ViewCode

    public string? ViewCode { get; set; }

    [ExcelField("全称", ColumnWidth = 40)]
    // 属性：FullName

    public string? FullName { get; set; }

    [ExcelField("机构类型", ColumnWidth = 15)]
    // 属性：OrgType

    public string? OrgType { get; set; }

    [ExcelField("负责人", ColumnWidth = 15)]
    // 属性：Leader

    public string? Leader { get; set; }

    [ExcelField("电话", ColumnWidth = 20)]
    // 属性：Phone

    public string? Phone { get; set; }

    [ExcelField("上级路径", ColumnWidth = 30)]
    // 属性：ParentPath

    public string? ParentPath { get; set; }

    [ExcelField("树路径", ColumnWidth = 50)]
    // 属性：TreePath

    public string? TreePath { get; set; }
}
