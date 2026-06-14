using System.ComponentModel.DataAnnotations;

namespace JeeSiteNET.Core;

/// <summary>
/// 树形实体抽象基类：继承数据实体并实现树形结构所需的全部字段
/// </summary>
public abstract class TreeEntity : DataEntity, ITreeEntity
{
    /// <summary>
    /// 父级编码（顶层节点为 "0"）
    /// </summary>
    [StringLength(100)]
    public string ParentCode { get; set; } = "0";

    /// <summary>
    /// 所有父级编码路径，逗号分隔（如 "0,A,A1"）
    /// </summary>
    [StringLength(2000)]
    public string ParentCodes { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点排序值（数值越小越靠前），默认值 1000
    /// </summary>
    public decimal TreeSort { get; set; } = 1000;

    /// <summary>
    /// 所有父级排序值路径，逗号分隔
    /// </summary>
    [StringLength(2000)]
    public string TreeSorts { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（1=叶子，0=非叶子），默认值 "1"
    /// </summary>
    [StringLength(1)]
    public string TreeLeaf { get; set; } = "1";

    /// <summary>
    /// 节点层级（0=根，1=一级，依次递增），默认值 0
    /// </summary>
    public decimal TreeLevel { get; set; } = 0;

    /// <summary>
    /// 所有父级名称路径，斜线分隔（如 "总公司/分公司/部门"）
    /// </summary>
    [StringLength(2000)]
    public string TreeNames { get; set; } = string.Empty;

    /// <summary>
    /// 获取当前节点的显示名称（由子类实现）
    /// </summary>
    /// <returns>节点名称</returns>
    public abstract string GetName();
}
