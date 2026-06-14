using System.ComponentModel.DataAnnotations;

namespace JeeSiteNET.Core;

/// <summary>
/// 基础实体接口：定义创建人/创建时间/更新人/更新时间/备注等审计字段
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// 创建人编码
    /// </summary>
    string? CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime? CreateDate { get; set; }

    /// <summary>
    /// 最后更新人编码
    /// </summary>
    string? UpdateBy { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    DateTime? UpdateDate { get; set; }

    /// <summary>
    /// 备注信息
    /// </summary>
    string? Remarks { get; set; }
}

/// <summary>
/// 公司实体接口：用于隔离不同公司（Corp）的数据
/// </summary>
public interface ICorpEntity
{
    /// <summary>
    /// 公司编码
    /// </summary>
    string? CorpCode { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    string? CorpName { get; set; }
}

/// <summary>
/// 数据实体接口：在基础实体之上扩展状态字段（启用/禁用）
/// </summary>
public interface IDataEntity : IBaseEntity
{
    /// <summary>
    /// 状态（0=正常，1=禁用）
    /// </summary>
    string? Status { get; set; }
}

/// <summary>
/// 树形实体接口：定义树形结构所需字段（父级、层级、排序等）
/// </summary>
public interface ITreeEntity
{
    /// <summary>
    /// 父级编码（顶层节点为 "0"）
    /// </summary>
    string ParentCode { get; set; }

    /// <summary>
    /// 所有父级编码路径，逗号分隔（如 "0,A,A1"）
    /// </summary>
    string ParentCodes { get; set; }

    /// <summary>
    /// 当前节点排序值（数值越小越靠前）
    /// </summary>
    decimal TreeSort { get; set; }

    /// <summary>
    /// 所有父级排序值路径，逗号分隔
    /// </summary>
    string TreeSorts { get; set; }

    /// <summary>
    /// 是否叶子节点（1=叶子，0=非叶子）
    /// </summary>
    string TreeLeaf { get; set; }

    /// <summary>
    /// 节点层级（0=根，1=一级，依次递增）
    /// </summary>
    decimal TreeLevel { get; set; }

    /// <summary>
    /// 所有父级名称路径，斜线分隔（如 "总公司/分公司/部门"）
    /// </summary>
    string TreeNames { get; set; }

    /// <summary>
    /// 获取当前节点的显示名称
    /// </summary>
    /// <returns>节点名称</returns>
    string GetName();
}

/// <summary>
/// 基础实体抽象类：实现 IBaseEntity，提供通用审计字段
/// </summary>
public abstract class BaseEntity : IBaseEntity
{
    /// <summary>
    /// 创建人编码
    /// </summary>
    [StringLength(100)]
    public string? CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// 最后更新人编码
    /// </summary>
    [StringLength(100)]
    public string? UpdateBy { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// 备注信息
    /// </summary>
    [StringLength(500)]
    public string? Remarks { get; set; }
}

/// <summary>
/// 数据实体抽象类：继承基础实体并扩展状态字段
/// </summary>
public abstract class DataEntity : BaseEntity, IDataEntity
{
    /// <summary>
    /// 状态（0=正常，1=禁用），默认值 "0"
    /// </summary>
    [StringLength(1)]
    public string? Status { get; set; } = "0";
}
