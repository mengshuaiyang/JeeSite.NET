using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class WordUtil
{
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
