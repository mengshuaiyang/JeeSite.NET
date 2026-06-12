using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 文件上传安全工具类，提供扩展名白名单校验、文件签名（Magic Number）校验、
/// MIME 类型推断、文件名/路径清洗及路径遍历防御等安全能力。
/// </summary>
public static class FileSecurityUtil
{
    #region 扩展名白名单

    /// <summary>
    /// 安全的图片扩展名。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的文档扩展名。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeDocumentExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".md", ".csv", ".rtf",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的视频扩展名。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeVideoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".avi", ".mov", ".webm", ".mkv",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的音频扩展名。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeAudioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".mp3", ".wav", ".flac", ".ogg",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的压缩包扩展名。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeArchiveExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".zip", ".rar", ".7z", ".tar", ".gz",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 危险扩展名黑名单（脚本、可执行、配置文件等）。
    /// </summary>
    public static readonly ImmutableHashSet<string> DangerousExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".exe", ".dll", ".phtml", ".php", ".php5", ".pht", ".js", ".jsp", ".asp", ".aspx",
        ".sh", ".bat", ".cmd", ".ps1", ".psm1", ".vbs", ".jar", ".class", ".swf", ".fla",
        ".htm", ".html", ".svg", ".hta", ".cpl", ".msc", ".cer", ".csr", ".pem", ".key",
        ".ovpn", ".reg", ".ini", ".conf", ".yml", ".yaml", ".iso",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 默认允许的所有安全扩展名（图片 ∪ 文档 ∪ 视频 ∪ 音频 ∪ 压缩包）。
    /// </summary>
    public static readonly ImmutableHashSet<string> AllowedAll = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        .Concat(SafeImageExtensions)
        .Concat(SafeDocumentExtensions)
        .Concat(SafeVideoExtensions)
        .Concat(SafeAudioExtensions)
        .Concat(SafeArchiveExtensions)
        .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    #endregion

    #region 扩展名与基础校验

    /// <summary>
    /// 判断扩展名是否安全。扩展名应包含点号（如 <c>.jpg</c>）。
    /// 若 <paramref name="allowedExtensions"/> 为 null，则使用 <see cref="AllowedAll"/> 作为默认白名单。
    /// </summary>
    /// <param name="extension">文件扩展名（可带或不带前导点号）。</param>
    /// <param name="allowedExtensions">自定义允许的扩展名集合；为 null 时使用默认白名单。</param>
    /// <returns>扩展名安全返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool IsExtensionSafe(string extension, IEnumerable<string>? allowedExtensions = null)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return false;

        string normalized = extension.StartsWith(".", StringComparison.Ordinal) ? extension : "." + extension;

        if (DangerousExtensions.Contains(normalized))
            return false;

        if (allowedExtensions is null)
            return AllowedAll.Contains(normalized);

        var effectiveSet = new HashSet<string>(allowedExtensions, StringComparer.OrdinalIgnoreCase);
        return effectiveSet.Contains(normalized) && !DangerousExtensions.Contains(normalized);
    }

    /// <summary>
    /// 根据文件字节与扩展名判断文件内容是否安全。
    /// 将对已知文件类型执行 Magic Number（文件签名）校验，避免扩展名欺骗。
    /// </summary>
    /// <param name="fileBytes">完整文件字节（或至少前 16 字节即可，更多时也可）。</param>
    /// <param name="extension">文件扩展名（可带或不带前导点号）。</param>
    /// <returns>文件内容与扩展名一致或不触发危险特征时返回 <c>true</c>。</returns>
    public static bool IsFileContentSafe(byte[] fileBytes, string extension)
    {
        if (fileBytes is null || fileBytes.Length == 0)
            return false;

        string ext = NormalizeExtension(extension);

        if (SafeImageExtensions.Contains(ext))
        {
            return ValidateImageMagicNumber(fileBytes, ext);
        }

        if (SafeDocumentExtensions.Contains(ext))
        {
            return ValidateDocumentMagicNumber(fileBytes, ext);
        }

        if (SafeVideoExtensions.Contains(ext))
        {
            return ValidateVideoMagicNumber(fileBytes, ext);
        }

        if (SafeAudioExtensions.Contains(ext))
        {
            return ValidateAudioMagicNumber(fileBytes, ext);
        }

        if (SafeArchiveExtensions.Contains(ext))
        {
            return ValidateArchiveMagicNumber(fileBytes, ext);
        }

        return false;
    }

    /// <summary>
    /// 对文件执行完整的上传校验：扩展名 → 路径遍历 → 文件签名/Magic Number。
    /// </summary>
    /// <param name="fileBytes">文件字节。</param>
    /// <param name="fileName">原始文件名（含扩展名）。</param>
    /// <param name="allowedExtensions">允许的扩展名集合；为 null 时使用默认白名单。</param>
    /// <returns>
    /// 一个元组：<c>IsSafe</c> 指示是否通过校验；<c>Reason</c> 在校验失败时给出原因描述。
    /// </returns>
    public static (bool IsSafe, string Reason) ValidateUpload(byte[] fileBytes, string fileName, IEnumerable<string>? allowedExtensions = null)
    {
        if (fileBytes is null || fileBytes.Length == 0)
            return (false, "文件内容为空。");

        if (string.IsNullOrWhiteSpace(fileName))
            return (false, "文件名为空。");

        if (IsPathTraversalAttempt(fileName))
            return (false, "文件名包含路径遍历字符。");

        string extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension))
            return (false, "缺少文件扩展名。");

        if (!IsExtensionSafe(extension, allowedExtensions))
            return (false, $"扩展名 '{extension}' 不在白名单中。");

        if (!IsFileContentSafe(fileBytes, extension))
            return (false, "文件内容 Magic Number 与扩展名不匹配，疑似扩展名欺骗。");

        return (true, "OK");
    }

    #endregion

    #region Magic Number 校验

    /// <summary>
    /// 针对图片类型执行文件签名（Magic Number）校验。
    /// </summary>
    /// <param name="header">文件内容字节（取前 16 字节进行判定）。</param>
    /// <param name="extension">规范化的扩展名，须以点号开头。</param>
    /// <returns>签名匹配返回 <c>true</c>。</returns>
    public static bool ValidateImageMagicNumber(byte[] header, string extension)
    {
        if (header is null || header.Length < 2)
            return false;

        string ext = NormalizeExtension(extension);

        switch (ext)
        {
            case ".jpg":
            case ".jpeg":
                return StartsWithBytes(header, 0xFF, 0xD8, 0xFF);

            case ".png":
                return StartsWithBytes(header, 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A);

            case ".gif":
                return StartsWithBytes(header, 0x47, 0x49, 0x46, 0x38);

            case ".bmp":
                return StartsWithBytes(header, 0x42, 0x4D);

            case ".webp":
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x57, 0x45, 0x42, 0x50);

            case ".svg":
                string ascii = Encoding.UTF8.GetString(header, 0, Math.Min(header.Length, 512))
                    .TrimStart();
                return ascii.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase)
                    || ascii.StartsWith("<svg", StringComparison.OrdinalIgnoreCase);

            default:
                return false;
        }
    }

    private static bool ValidateDocumentMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".pdf":
                return StartsWithBytes(header, 0x25, 0x50, 0x44, 0x46);

            case ".doc":
            case ".ppt":
            case ".xls":
                return header.Length >= 8 && StartsWithBytes(header, 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1);

            case ".docx":
            case ".xlsx":
            case ".pptx":
                return IsZipSignature(header);

            case ".rtf":
                return StartsWithBytes(header, 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31);

            case ".txt":
            case ".md":
            case ".csv":
                return true;

            default:
                return false;
        }
    }

    private static bool ValidateVideoMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".mp4":
                if (header.Length < 12) return false;
                return BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70)
                    || BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D)
                    || BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32);

            case ".avi":
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x41, 0x56, 0x49, 0x20);

            case ".mov":
                if (header.Length < 12) return false;
                return BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74);

            case ".webm":
            case ".mkv":
                if (header.Length < 4) return false;
                return StartsWithBytes(header, 0x1A, 0x45, 0xDF, 0xA3);

            default:
                return false;
        }
    }

    private static bool ValidateAudioMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".mp3":
                if (header.Length < 3) return false;
                if (StartsWithBytes(header, 0x49, 0x44, 0x33)) return true;
                if (header[0] != 0xFF) return false;
                byte second = header[1];
                return second == 0xFB || second == 0xF3 || second == 0xF2 || (second & 0xE0) == 0xE0;

            case ".wav":
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x57, 0x41, 0x56, 0x45);

            case ".flac":
                return StartsWithBytes(header, 0x66, 0x4C, 0x61, 0x43);

            case ".ogg":
                return StartsWithBytes(header, 0x4F, 0x67, 0x67, 0x53);

            default:
                return false;
        }
    }

    private static bool ValidateArchiveMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".zip":
                return IsZipSignature(header);

            case ".rar":
                return StartsWithBytes(header, 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00);

            case ".7z":
                return StartsWithBytes(header, 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C);

            case ".tar":
                if (header.Length < 512) return false;
                byte[] magic = new byte[6];
                Array.Copy(header, 257, magic, 0, 6);
                return magic[0] == 'u' && magic[1] == 's' && magic[2] == 't' && magic[3] == 'a' && magic[4] == 'r';

            case ".gz":
                return StartsWithBytes(header, 0x1F, 0x8B);

            default:
                return false;
        }
    }

    private static bool IsZipSignature(byte[] header)
    {
        if (header.Length < 4) return false;
        if (StartsWithBytes(header, 0x50, 0x4B, 0x03, 0x04)) return true;
        if (StartsWithBytes(header, 0x50, 0x4B, 0x05, 0x06)) return true;
        if (StartsWithBytes(header, 0x50, 0x4B, 0x07, 0x08)) return true;
        return false;
    }

    private static bool StartsWithBytes(byte[] data, params byte[] signature)
    {
        if (data.Length < signature.Length) return false;
        for (int i = 0; i < signature.Length; i++)
        {
            if (data[i] != signature[i]) return false;
        }
        return true;
    }

    private static bool BytesAt(byte[] data, int offset, params byte[] signature)
    {
        if (data.Length < offset + signature.Length) return false;
        for (int i = 0; i < signature.Length; i++)
        {
            if (data[offset + i] != signature[i]) return false;
        }
        return true;
    }

    #endregion

    #region MIME 类型推断

    /// <summary>
    /// 基于扩展名与文件前导字节推断安全的 MIME 类型，不依赖浏览器上传的 Content-Type。
    /// </summary>
    /// <param name="extension">文件扩展名（可带或不带前导点号）。</param>
    /// <param name="fileHeader">文件前导字节（至少 16 字节更准确）。</param>
    /// <returns>推断出的 MIME 类型；无法识别时返回 <c>application/octet-stream</c>。</returns>
    public static string GetSafeMimeType(string extension, byte[] fileHeader)
    {
        string ext = NormalizeExtension(extension);

        bool headerOk = fileHeader != null && fileHeader.Length >= 2;

        if (headerOk && ValidateImageMagicNumber(fileHeader!, ext))
        {
            switch (ext)
            {
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".bmp": return "image/bmp";
                case ".webp": return "image/webp";
                case ".svg": return "image/svg+xml";
            }
        }

        if (headerOk && ValidateDocumentMagicNumber(fileHeader!, ext))
        {
            switch (ext)
            {
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".txt": return "text/plain";
                case ".md": return "text/markdown";
                case ".csv": return "text/csv";
                case ".rtf": return "application/rtf";
            }
        }

        if (headerOk && ValidateVideoMagicNumber(fileHeader!, ext))
        {
            switch (ext)
            {
                case ".mp4": return "video/mp4";
                case ".avi": return "video/x-msvideo";
                case ".mov": return "video/quicktime";
                case ".webm": return "video/webm";
                case ".mkv": return "video/x-matroska";
            }
        }

        if (headerOk && ValidateAudioMagicNumber(fileHeader!, ext))
        {
            switch (ext)
            {
                case ".mp3": return "audio/mpeg";
                case ".wav": return "audio/wav";
                case ".flac": return "audio/flac";
                case ".ogg": return "audio/ogg";
            }
        }

        if (headerOk && ValidateArchiveMagicNumber(fileHeader!, ext))
        {
            switch (ext)
            {
                case ".zip": return "application/zip";
                case ".rar": return "application/vnd.rar";
                case ".7z": return "application/x-7z-compressed";
                case ".tar": return "application/x-tar";
                case ".gz": return "application/gzip";
            }
        }

        return "application/octet-stream";
    }

    #endregion

    #region 文件名 / 路径清洗

    private static readonly char[] InvalidFileNameChars =
    {
        '\\', '/', ':', '*', '?', '"', '<', '>', '|',
        '\0', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07',
        '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F',
        '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17',
        '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F',
    };

    /// <summary>
    /// 清洗文件名，移除路径分隔符、控制字符、Unicode 控制字符及 <c>..</c> 等危险片段。
    /// </summary>
    /// <param name="fileName">原始文件名。</param>
    /// <returns>清洗后的文件名；若结果为空字符串，返回 <c>unnamed</c>。</returns>
    public static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "unnamed";

        StringBuilder sb = new StringBuilder(fileName.Length);
        foreach (char c in fileName)
        {
            if (IsDangerousFileNameChar(c)) continue;
            sb.Append(c);
        }

        string result = sb.ToString()
            .Replace("..", string.Empty, StringComparison.Ordinal);

        result = result.Trim();

        return string.IsNullOrEmpty(result) ? "unnamed" : result;
    }

    /// <summary>
    /// 清洗单个路径段（如子目录名），防止控制字符与路径遍历。
    /// </summary>
    /// <param name="pathPart">路径段。</param>
    /// <returns>清洗后的路径段。</returns>
    public static string SanitizePathComponent(string pathPart)
    {
        if (string.IsNullOrWhiteSpace(pathPart))
            return string.Empty;

        StringBuilder sb = new StringBuilder(pathPart.Length);
        foreach (char c in pathPart)
        {
            if (IsDangerousFileNameChar(c)) continue;
            sb.Append(c);
        }

        string result = sb.ToString()
            .Replace("..", string.Empty, StringComparison.Ordinal);

        return result.Trim();
    }

    private static bool IsDangerousFileNameChar(char c)
    {
        if (c < 0x20) return true;
        if (c == '\x7F') return true;
        if (Array.IndexOf(InvalidFileNameChars, c) >= 0) return true;
        UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
        return category == UnicodeCategory.Control
            || category == UnicodeCategory.Format
            || category == UnicodeCategory.PrivateUse
            || category == UnicodeCategory.Surrogate
            || category == UnicodeCategory.OtherNotAssigned;
    }

    #endregion

    #region 路径遍历防御

    /// <summary>
    /// 检测路径是否包含路径遍历尝试，包括 <c>../</c>、<c>..\\</c>、URL/Unicode 编码变体及重复点号等。
    /// </summary>
    /// <param name="path">要检查的路径或文件名。</param>
    /// <returns>若存在路径遍历迹象返回 <c>true</c>。</returns>
    public static bool IsPathTraversalAttempt(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        string normalized = path;

        if (normalized.Contains("..", StringComparison.Ordinal))
            return true;

        if (normalized.Contains("./", StringComparison.Ordinal) || normalized.Contains(".\\", StringComparison.Ordinal))
            return true;

        if (normalized.Contains("%2e", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("%2f", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("%5c", StringComparison.OrdinalIgnoreCase))
            return true;

        if (normalized.Contains("\u2024", StringComparison.Ordinal)
            || normalized.Contains("\u2025", StringComparison.Ordinal)
            || normalized.Contains("\u2026", StringComparison.Ordinal))
            return true;

        if (normalized.Contains(".../", StringComparison.Ordinal)
            || normalized.Contains("...\\", StringComparison.Ordinal)
            || normalized.Contains("....//", StringComparison.Ordinal)
            || normalized.Contains("....\\\\", StringComparison.Ordinal))
            return true;

        return false;
    }

    /// <summary>
    /// 规范化并校验路径，确保 <paramref name="relativePath"/> 不会突破 <paramref name="basePath"/>。
    /// 若检测到路径遍历或非法字符，将抛出 <see cref="ArgumentException"/>。
    /// </summary>
    /// <param name="basePath">基准目录（绝对路径）。</param>
    /// <param name="relativePath">相对于 basePath 的子路径。</param>
    /// <returns>绝对安全路径字符串。</returns>
    /// <exception cref="ArgumentException">当路径非法或试图突破 <paramref name="basePath"/> 时抛出。</exception>
    public static string NormalizeAndValidatePath(string basePath, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("基准路径不能为空。", nameof(basePath));
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("相对路径不能为空。", nameof(relativePath));

        if (IsPathTraversalAttempt(relativePath))
            throw new ArgumentException("相对路径包含路径遍历字符。", nameof(relativePath));

        string fullBase;
        try
        {
            fullBase = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("基准路径无效：" + ex.Message, nameof(basePath), ex);
        }

        string combined;
        try
        {
            combined = Path.Combine(fullBase, relativePath);
            combined = Path.GetFullPath(combined);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("相对路径无效：" + ex.Message, nameof(relativePath), ex);
        }

        string normalizedBase = fullBase + Path.DirectorySeparatorChar;
        if (!combined.StartsWith(normalizedBase, StringComparison.Ordinal)
            && !string.Equals(combined, fullBase, StringComparison.Ordinal))
        {
            throw new ArgumentException("路径试图突破基准目录。", nameof(relativePath));
        }

        return combined;
    }

    #endregion

    #region 内部辅助

    private static string NormalizeExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return string.Empty;
        return extension.StartsWith(".", StringComparison.Ordinal)
            ? extension.ToLowerInvariant()
            : "." + extension.ToLowerInvariant();
    }

    #endregion
}
