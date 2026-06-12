using System.Text;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// 路径格式化器 — 解析 {yyyy}{mm}{dd}{time}{rand:6} 等占位符
/// 参考 UEditor PHP/Java 版中的 PathFormat.parse()
/// </summary>
public static class UEditorPathFormatter
{
    private static readonly Random _rng = new();
    private static readonly object _rngLock = new();

    /// <summary>
    /// 格式化路径模板。示例：
    ///   "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}"
    ///   => "upload/image/20260612/1718181234567_123456"
    /// </summary>
    public static string Format(string template)
    {
        if (string.IsNullOrEmpty(template)) return "";
        var now = DateTime.Now;
        var sb = new StringBuilder(template);

        sb.Replace("{yyyy}", now.Year.ToString("0000"));
        sb.Replace("{yy}", (now.Year % 100).ToString("00"));
        sb.Replace("{mm}", now.Month.ToString("00"));
        sb.Replace("{dd}", now.Day.ToString("00"));
        sb.Replace("{hh}", now.Hour.ToString("00"));
        sb.Replace("{ii}", now.Minute.ToString("00"));
        sb.Replace("{ss}", now.Second.ToString("00"));
        sb.Replace("{time}", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        // {rand:N} → N 位随机数字
        int i = 0;
        while (i < sb.Length)
        {
            if (sb[i] == '{')
            {
                int end = sb.ToString().IndexOf('}', i);
                if (end < 0) break;
                var token = sb.ToString(i, end - i + 1);
                if (token.StartsWith("{rand:") && token.EndsWith('}'))
                {
                    if (int.TryParse(token.AsSpan(6, token.Length - 7), out var n) && n > 0)
                    {
                        int digits = Math.Clamp(n, 1, 10);
                        int r;
                        lock (_rngLock) { r = _rng.Next((int)Math.Pow(10, digits - 1), (int)Math.Pow(10, digits)); }
                        sb.Remove(i, token.Length);
                        sb.Insert(i, r.ToString());
                        i += digits;
                        continue;
                    }
                }
            }
            i++;
        }

        return sb.ToString();
    }

    public static string WithExtension(string formattedPath, string extension)
    {
        if (!extension.StartsWith('.')) extension = "." + extension;
        return formattedPath + extension.ToLowerInvariant();
    }
}
