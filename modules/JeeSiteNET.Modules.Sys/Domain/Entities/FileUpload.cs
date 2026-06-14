using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 文件上传记录实体，描述业务对象与文件的关联关系。一条上传记录对应一个 FileEntity（物理文件）。
/// </summary>
public class FileUpload : DataEntity
{
    /// <summary>上传记录 ID。</summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>关联物理文件 ID（引用 FileEntity.FileId）。</summary>
    public string FileId { get; set; } = string.Empty;
    /// <summary>上传时的原始文件名。</summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>文件分类/业务类型：image（图片）、document（文档）、video（视频）、audio（音频）。</summary>
    public string FileType { get; set; } = string.Empty;
    /// <summary>同一业务对象下的文件排序。</summary>
    public decimal? FileSort { get; set; }
    /// <summary>关联业务对象主键。</summary>
    public string? BizKey { get; set; }
    /// <summary>关联业务类型编码。</summary>
    public string? BizType { get; set; }
}
