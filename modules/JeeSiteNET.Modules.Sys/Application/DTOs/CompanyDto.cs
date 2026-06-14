using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 公司信息 DTO。
/// </summary>
public class CompanyDto
{
    /// <summary>
    /// 公司编码。
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 视图编码。
    /// </summary>
    public string? ViewCode { get; set; }

    /// <summary>
    /// 公司名称。
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司完整名称（含上级路径）。
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 所在区域编码。
    /// </summary>
    public string? AreaCode { get; set; }

    /// <summary>
    /// 所在区域名称。
    /// </summary>
    public string? AreaName { get; set; }

    /// <summary>
    /// 父级公司编码。
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// 父级完整路径编码。
    /// </summary>
    public string? ParentCodes { get; set; }

    /// <summary>
    /// 名称完整路径。
    /// </summary>
    public string? TreeNames { get; set; }

    /// <summary>
    /// 是否叶子节点（1 是 / 0 否）。
    /// </summary>
    public string? TreeLeaf { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string Status { get; set; } = "0";

    /// <summary>
    /// 备注说明。
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 公司关联的机构（办公室）编码列表。
    /// </summary>
    public List<string> OfficeCodes { get; set; } = new();
}

/// <summary>
/// 公司保存请求 DTO。
/// </summary>
public class CompanySaveDto
{
    /// <summary>
    /// 公司编码；空表示新建。
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// 视图编码。
    /// </summary>
    public string? ViewCode { get; set; }

    /// <summary>
    /// 公司名称。
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司完整名称。
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 区域编码。
    /// </summary>
    public string? AreaCode { get; set; }

    /// <summary>
    /// 父级公司编码。
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// 备注。
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 关联的机构编码列表。
    /// </summary>
    public List<string> OfficeCodes { get; set; } = new();
}

/// <summary>
/// 行政区域 DTO。
/// </summary>
public class AreaDto
{
    /// <summary>
    /// 区域编码（如行政区划代码）。
    /// </summary>
    public string AreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域名称。
    /// </summary>
    public string AreaName { get; set; } = string.Empty;

    /// <summary>
    /// 区域类型（省/市/县）。
    /// </summary>
    public string? AreaType { get; set; }

    /// <summary>
    /// 父级区域编码。
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// 父级区域完整路径。
    /// </summary>
    public string? ParentCodes { get; set; }

    /// <summary>
    /// 同级排序。
    /// </summary>
    public decimal? TreeSort { get; set; }

    /// <summary>
    /// 同级排序路径。
    /// </summary>
    public string? TreeSorts { get; set; }

    /// <summary>
    /// 名称完整路径。
    /// </summary>
    public string? TreeNames { get; set; }

    /// <summary>
    /// 是否叶子节点。
    /// </summary>
    public string? TreeLeaf { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string Status { get; set; } = "0";

    /// <summary>
    /// 子区域集合（树形渲染用）。
    /// </summary>
    public List<AreaDto> Children { get; set; } = new();
}

/// <summary>
/// 公司/区域相关 DTO 扩展方法集合。
/// </summary>
public static class CompanyMapping
{
    /// <summary>
    /// 将 <see cref="Company"/> 实体转换为 <see cref="CompanyDto"/>。
    /// </summary>
    /// <param name="c">公司实体。</param>
    /// <param name="officeCodes">关联的机构编码集合。</param>
    /// <returns>公司信息 DTO。</returns>
    public static CompanyDto ToDto(this Company c, List<string> officeCodes) => new()
    {
        CompanyCode = c.CompanyCode,
        ViewCode = c.ViewCode,
        CompanyName = c.CompanyName,
        FullName = c.FullName,
        AreaCode = c.AreaCode,
        AreaName = c.AreaName,
        ParentCode = c.ParentCode,
        ParentCodes = c.ParentCodes,
        TreeNames = c.TreeNames,
        TreeLeaf = c.TreeLeaf,
        Status = c.Status ?? "0",
        Remarks = c.Remarks,
        OfficeCodes = officeCodes
    };

    /// <summary>
    /// 将 <see cref="CompanySaveDto"/> 转换为 <see cref="Company"/> 实体（附带必要的默认值）。
    /// </summary>
    /// <param name="dto">保存请求 DTO。</param>
    /// <returns>公司实体。</returns>
    public static Company ToEntity(this CompanySaveDto dto)
    {
        var now = DateTime.Now;
        return new Company
        {
            CompanyCode = dto.CompanyCode ?? IdGenerator.NewId(),
            ViewCode = dto.ViewCode,
            CompanyName = dto.CompanyName,
            FullName = dto.FullName,
            AreaCode = dto.AreaCode,
            ParentCode = dto.ParentCode ?? "0",
            Remarks = dto.Remarks,
            Status = "0",
            TreeLeaf = "1",
            CreateDate = now,
            UpdateDate = now
        };
    }

    /// <summary>
    /// 将 <see cref="Area"/> 实体转换为 <see cref="AreaDto"/>。
    /// </summary>
    /// <param name="a">区域实体。</param>
    /// <returns>区域信息 DTO。</returns>
    public static AreaDto ToDto(this Area a) => new()
    {
        AreaCode = a.AreaCode,
        AreaName = a.AreaName,
        AreaType = a.AreaType,
        ParentCode = a.ParentCode,
        ParentCodes = a.ParentCodes,
        TreeSort = a.TreeSort,
        TreeSorts = a.TreeSorts,
        TreeNames = a.TreeNames,
        TreeLeaf = a.TreeLeaf,
        Status = a.Status ?? "0"
    };
}
