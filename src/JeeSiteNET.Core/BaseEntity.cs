using System.ComponentModel.DataAnnotations;

namespace JeeSiteNET.Core;

public interface IBaseEntity
{
    string? CreateBy { get; set; }
    DateTime? CreateDate { get; set; }
    string? UpdateBy { get; set; }
    DateTime? UpdateDate { get; set; }
    string? Remarks { get; set; }
}

public interface IDataEntity : IBaseEntity
{
    string? Status { get; set; }
}

public interface ITreeEntity
{
    string ParentCode { get; set; }
    string ParentCodes { get; set; }
    decimal TreeSort { get; set; }
    string TreeSorts { get; set; }
    string TreeLeaf { get; set; }
    decimal TreeLevel { get; set; }
    string TreeNames { get; set; }
    string GetName();
}

public abstract class BaseEntity : IBaseEntity
{
    [StringLength(100)]
    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    [StringLength(100)]
    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }
}

public abstract class DataEntity : BaseEntity, IDataEntity
{
    [StringLength(1)]
    public string? Status { get; set; } = "0";
}
