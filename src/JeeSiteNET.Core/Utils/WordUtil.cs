using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// Word 文档生成工具类（将 HTML 片段包装为 Word 可直接打开的 HTML 文档）
/// </summary>
public static class WordUtil
{
    /// <summary>
    /// 将 HTML 片段包装为完整的 HTML/Word 文档并以 UTF-8 编码输出字节数组
    /// </summary>
    /// <param name="html">HTML 内容片段（不含 html/body 外层标签）</param>
    /// <returns>可写入 .doc 文件的 UTF-8 字节数组</returns>
    public static byte[] GenerateHtmlToWord(string html)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset=\"UTF-8\">");
        sb.AppendLine("<style>table{border-collapse:collapse}td,th{border:1px solid #000;padding:4px}</style>");
        sb.AppendLine("</head><body>");
        sb.AppendLine(html);
        sb.AppendLine("</body></html>");
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
