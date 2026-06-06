using System.ComponentModel.DataAnnotations;

namespace JeeSiteNET.Core;

public abstract class TreeEntity : DataEntity, ITreeEntity
{
    [StringLength(100)]
    public string ParentCode { get; set; } = "0";

    [StringLength(2000)]
    public string ParentCodes { get; set; } = string.Empty;

    public decimal TreeSort { get; set; } = 1000;

    [StringLength(2000)]
    public string TreeSorts { get; set; } = string.Empty;

    [StringLength(1)]
    public string TreeLeaf { get; set; } = "1";

    public decimal TreeLevel { get; set; } = 0;

    [StringLength(2000)]
    public string TreeNames { get; set; } = string.Empty;

    public abstract string GetName();
}
