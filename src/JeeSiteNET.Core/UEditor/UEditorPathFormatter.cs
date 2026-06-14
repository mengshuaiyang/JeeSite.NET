using System.Text;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 路径格式化工具：将 {yyyy}{mm}{dd}{hh}{ii}{ss}{time}{rand:N} 等占位符替换为实际值，
/// 与官方 PHP/Java 版路径格式化规则保持一致。
/// </summary>
public static class UEditorPathFormatter
{
    /// <summary>
    /// 伪随机数生成器（静态共享，配合锁使用）
    /// </summary>
    private static readonly Random _rng = new();

    /// <summary>
    /// _rng 的同步锁
    /// </summary>
    private static readonly object _rngLock = new();

    /// <summary>
    /// 格式化路径模板：依次替换常见占位符并处理随机数
    /// </summary>
    /// <param name="template">路径模板，如 "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}"</param>
    /// <returns>格式化后的相对路径字符串</returns>
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

        // {rand:N} 替换为 N 位随机十进制数字（N 取值 1-10，按位限制范围避免首位为 0）
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

    /// <summary>
    /// 在格式化路径后追加扩展名（确保小写且以 '.' 开头）
    /// </summary>
    /// <param name="formattedPath">经 Format 处理的路径</param>
    /// <param name="extension">原始扩展名字符串（如 ".png" 或 "png"）</param>
    /// <returns>拼接扩展名后的完整相对路径</returns>
    public static string WithExtension(string formattedPath, string extension)
    {
        if (!extension.StartsWith('.')) extension = "." + extension;
        return formattedPath + extension.ToLowerInvariant();
    }
}
