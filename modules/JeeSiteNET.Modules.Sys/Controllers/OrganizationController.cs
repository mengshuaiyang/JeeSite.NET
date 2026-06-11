using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/org")]
public class OrganizationController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationController(OrganizationService organizationService) => _organizationService = organizationService;

    [Permission("sys:org:list")]
    [HttpGet("tree")]
    public async Task<ApiResult<List<OrganizationDto>>> Tree([FromQuery] string? orgType = null)
    {
        var tree = await _organizationService.FindTreeAsync(orgType);
        return ApiResult<List<OrganizationDto>>.Ok(tree);
    }

    [Permission("sys:org:list")]
    [HttpGet("get")]
    public async Task<ApiResult<OrganizationDto?>> Get([FromQuery] string orgCode)
    {
        var org = await _organizationService.GetAsync(orgCode);
        if (org == null) return ApiResult<OrganizationDto?>.NotFound("机构不存在");
        return ApiResult<OrganizationDto?>.Ok(org);
    }

    [Permission("sys:org:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] OrganizationSaveDto dto)
    {
        return await _organizationService.SaveAsync(dto);
    }

    [Permission("sys:org:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteOrgRequest request)
    {
        return await _organizationService.DeleteAsync(request.OrgCode);
    }

    [Permission("sys:org:list")]
    [HttpPost("export")]
    public async Task<IActionResult> ExportData()
    {
        var tree = await _organizationService.FindTreeAsync(null);
        var flat = FlattenTree(tree);
        var excel = new ExcelService();
        var bytes = excel.Export(flat, "机构数据");
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "机构数据.xlsx");
    }

    [Permission("sys:org:edit")]
    [HttpPost("import")]
    public async Task<ApiResult> ImportData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return ApiResult.Fail(400, "请选择上传文件");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var excel = new ExcelService();
        var records = excel.Import<OrgExportRow>(ms.ToArray(), "机构数据");
        return ApiResult.Ok(new { imported = records.Count });
    }

    [Permission("sys:org:edit")]
    [HttpGet("import-template")]
    public IActionResult ImportTemplate()
    {
        var excel = new ExcelService();
        var bytes = excel.ExportTemplate<OrgExportRow>("机构数据");
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "机构导入模板.xlsx");
    }

    private static List<OrgExportRow> FlattenTree(List<OrganizationDto> tree)
    {
        var result = new List<OrgExportRow>();
        void Walk(List<OrganizationDto> nodes, string parentPath)
        {
            foreach (var n in nodes)
            {
                var path = string.IsNullOrEmpty(parentPath) ? n.OrgName : $"{parentPath}/{n.OrgName}";
                if (n.OrgCode != "0")
                {
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
                if (n.Children?.Count > 0)
                    Walk(n.Children, path);
            }
        }
        Walk(tree, "");
        return result;
    }
}

public class DeleteOrgRequest
{
    public string OrgCode { get; set; } = string.Empty;
}

public class OrgExportRow
{
    [ExcelField("机构编码", ColumnWidth = 20)]
    public string? OrgCode { get; set; }
    [ExcelField("机构名称", ColumnWidth = 30)]
    public string? OrgName { get; set; }
    [ExcelField("机构代码", ColumnWidth = 15)]
    public string? ViewCode { get; set; }
    [ExcelField("全称", ColumnWidth = 40)]
    public string? FullName { get; set; }
    [ExcelField("机构类型", ColumnWidth = 15)]
    public string? OrgType { get; set; }
    [ExcelField("负责人", ColumnWidth = 15)]
    public string? Leader { get; set; }
    [ExcelField("电话", ColumnWidth = 20)]
    public string? Phone { get; set; }
    [ExcelField("上级路径", ColumnWidth = 30)]
    public string? ParentPath { get; set; }
    [ExcelField("树路径", ColumnWidth = 50)]
    public string? TreePath { get; set; }
}
