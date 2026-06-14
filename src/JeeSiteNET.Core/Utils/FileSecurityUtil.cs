using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 文件上传安全工具类。提供扩展名白名单校验、文件签名（Magic Number）校验、
/// MIME 类型推断、文件名/路径清洗及路径遍历防御等能力。
/// 设计原则：拒绝优先（deny-by-default），仅允许显式白名单通过。
/// </summary>
public static class FileSecurityUtil
{
    #region 扩展名白名单

    /// <summary>
    /// 安全的图片扩展名集合（含 SVG，仍建议图片服务做扫描）。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的文档扩展名集合。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeDocumentExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".md", ".csv", ".rtf",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的视频扩展名集合。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeVideoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".avi", ".mov", ".webm", ".mkv",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的音频扩展名集合。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeAudioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".mp3", ".wav", ".flac", ".ogg",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 安全的压缩包扩展名集合。
    /// </summary>
    public static readonly ImmutableHashSet<string> SafeArchiveExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".zip", ".rar", ".7z", ".tar", ".gz",
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 危险扩展名黑名单（脚本、可执行文件、配置文件等）。
    /// 即使在用户提供的自定义白名单中出现，也会被拒绝。
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
    /// 判断扩展名是否安全。扩展名可以带或不带前导点号（如 ".jpg" 或 "jpg"）。
    /// 若 <paramref name="allowedExtensions"/> 为 null，则使用 <see cref="AllowedAll"/> 作为默认白名单。
    /// 同时会排除 <see cref="DangerousExtensions"/> 中的任何扩展名。
    /// </summary>
    /// <param name="extension">文件扩展名。</param>
    /// <param name="allowedExtensions">自定义允许的扩展名集合；为 null 时使用默认白名单。</param>
    /// <returns>扩展名安全返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool IsExtensionSafe(string extension, IEnumerable<string>? allowedExtensions = null)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return false;

        // 统一为「小写 + 点号」形式，避免大小写与前缀差异绕过
        string normalized = extension.StartsWith(".", StringComparison.Ordinal)
            ? extension.ToLowerInvariant()
            : "." + extension.ToLowerInvariant();

        if (DangerousExtensions.Contains(normalized))
            return false;

        if (allowedExtensions is null)
            return AllowedAll.Contains(normalized);

        // 自定义白名单：先规范化再比对；同时必须「不在危险黑名单中」
        var effectiveSet = new HashSet<string>(allowedExtensions, StringComparer.OrdinalIgnoreCase);
        return effectiveSet.Contains(normalized) && !DangerousExtensions.Contains(normalized);
    }

    /// <summary>
    /// 根据文件字节与扩展名判断内容是否安全。
    /// 对已知文件类型执行 Magic Number（文件签名）校验，防止扩展名欺骗。
    /// </summary>
    /// <param name="fileBytes">文件完整字节（或至少前 16 字节）。</param>
    /// <param name="extension">文件扩展名（可带或不带前导点号）。</param>
    /// <returns>文件内容与扩展名一致返回 <c>true</c>。</returns>
    public static bool IsFileContentSafe(byte[] fileBytes, string extension)
    {
        if (fileBytes is null || fileBytes.Length == 0)
            return false;

        string ext = NormalizeExtension(extension);

        if (SafeImageExtensions.Contains(ext))
            return ValidateImageMagicNumber(fileBytes, ext);

        if (SafeDocumentExtensions.Contains(ext))
            return ValidateDocumentMagicNumber(fileBytes, ext);

        if (SafeVideoExtensions.Contains(ext))
            return ValidateVideoMagicNumber(fileBytes, ext);

        if (SafeAudioExtensions.Contains(ext))
            return ValidateAudioMagicNumber(fileBytes, ext);

        if (SafeArchiveExtensions.Contains(ext))
            return ValidateArchiveMagicNumber(fileBytes, ext);

        return false;
    }

    /// <summary>
    /// 对上传文件执行完整校验流程：扩展名 → 路径遍历 → 文件签名 Magic Number。
    /// </summary>
    /// <param name="fileBytes">文件字节。</param>
    /// <param name="fileName">原始文件名（含扩展名）。</param>
    /// <param name="allowedExtensions">允许的扩展名集合；为 null 时使用默认白名单。</param>
    /// <returns>
    /// (<c>IsSafe</c>=是否通过校验, <c>Reason</c>=失败时给出可读原因描述)。
    /// </returns>
    public static (bool IsSafe, string Reason) ValidateUpload(byte[] fileBytes, string fileName, IEnumerable<string>? allowedExtensions = null)
    {
        if (fileBytes is null || fileBytes.Length == 0)
            return (false, "文件内容为空。");

        if (string.IsNullOrWhiteSpace(fileName))
            return (false, "文件名为空。");

        // 文件名中包含路径遍历字符（../ / URL 编码等）→ 拒绝
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

        // 依据扩展名匹配对应的 Magic Number 签名
        switch (ext)
        {
            case ".jpg":
            case ".jpeg":
                return StartsWithBytes(header, 0xFF, 0xD8, 0xFF); // JPEG: FF D8 FF

            case ".png":
                return StartsWithBytes(header, 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A); // PNG: 89 50 4E 47 0D 0A 1A 0A

            case ".gif":
                return StartsWithBytes(header, 0x47, 0x49, 0x46, 0x38); // GIF: "GIF8"

            case ".bmp":
                return StartsWithBytes(header, 0x42, 0x4D); // BMP: "BM"

            case ".webp":
                // WebP: RIFF ... WEBP
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x57, 0x45, 0x42, 0x50);

            case ".svg":
                // SVG: 文本格式，检查是否以 <?xml 或 <svg 开头
                string ascii = Encoding.UTF8.GetString(header, 0, Math.Min(header.Length, 512)).TrimStart();
                return ascii.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase)
                    || ascii.StartsWith("<svg", StringComparison.OrdinalIgnoreCase);

            default:
                return false;
        }
    }

    /// <summary>
    /// 针对文档类型执行 Magic Number 校验。
    /// </summary>
    private static bool ValidateDocumentMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".pdf":
                return StartsWithBytes(header, 0x25, 0x50, 0x44, 0x46); // %PDF

            case ".doc":
            case ".ppt":
            case ".xls":
                // OLE2 复合文档：D0 CF 11 E0 A1 B1 1A E1
                return header.Length >= 8 && StartsWithBytes(header, 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1);

            case ".docx":
            case ".xlsx":
            case ".pptx":
                // Office Open XML 格式：实为 ZIP 容器
                return IsZipSignature(header);

            case ".rtf":
                return StartsWithBytes(header, 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31); // {\rtf1

            case ".txt":
            case ".md":
            case ".csv":
                // 纯文本无法可靠检测 Magic Number，放行（后续依赖扩展名与内容扫描）
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// 针对视频类型执行 Magic Number 校验。
    /// </summary>
    private static bool ValidateVideoMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".mp4":
                // 典型 MP4：前 4 字节为 box size，offset 4 处为 "ftyp"（后续变体：ftypisom、ftypmp42、ftypMSNV 等）
                if (header.Length < 12) return false;
                return BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70)          // ftyp
                    || BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D) // ftypisom
                    || BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32); // ftypmp42

            case ".avi":
                // RIFF...AVI
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x41, 0x56, 0x49, 0x20);

            case ".mov":
                // QuickTime: ...ftypqt
                if (header.Length < 12) return false;
                return BytesAt(header, 4, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74);

            case ".webm":
            case ".mkv":
                // Matroska / WebM EBML：1A 45 DF A3
                if (header.Length < 4) return false;
                return StartsWithBytes(header, 0x1A, 0x45, 0xDF, 0xA3);

            default:
                return false;
        }
    }

    /// <summary>
    /// 针对音频类型执行 Magic Number 校验。
    /// </summary>
    private static bool ValidateAudioMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".mp3":
                // ID3v2：49 44 33；否则以 0xFF FB / 0xFF F3 / 0xFF F2 / 0xFF E* 开头
                if (header.Length < 3) return false;
                if (StartsWithBytes(header, 0x49, 0x44, 0x33)) return true;
                if (header[0] != 0xFF) return false;
                byte second = header[1];
                return second == 0xFB || second == 0xF3 || second == 0xF2 || (second & 0xE0) == 0xE0;

            case ".wav":
                // RIFF...WAVE
                if (header.Length < 12) return false;
                if (!StartsWithBytes(header, 0x52, 0x49, 0x46, 0x46)) return false;
                return BytesAt(header, 8, 0x57, 0x41, 0x56, 0x45);

            case ".flac":
                return StartsWithBytes(header, 0x66, 0x4C, 0x61, 0x43); // fLaC

            case ".ogg":
                return StartsWithBytes(header, 0x4F, 0x67, 0x67, 0x53); // OggS

            default:
                return false;
        }
    }

    /// <summary>
    /// 针对压缩包类型执行 Magic Number 校验。
    /// </summary>
    private static bool ValidateArchiveMagicNumber(byte[] header, string extension)
    {
        switch (extension)
        {
            case ".zip":
                return IsZipSignature(header);

            case ".rar":
                return StartsWithBytes(header, 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00); // Rar!...

            case ".7z":
                return StartsWithBytes(header, 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C); // 7z 签名

            case ".tar":
                // TAR 头部 offset 257 处为 "ustar"
                if (header.Length < 512) return false;
                byte[] magic = new byte[6];
                Array.Copy(header, 257, magic, 0, 6);
                return magic[0] == 'u' && magic[1] == 's' && magic[2] == 't' && magic[3] == 'a' && magic[4] == 'r';

            case ".gz":
                return StartsWithBytes(header, 0x1F, 0x8B); // gzip

            default:
                return false;
        }
    }

    /// <summary>
    /// 判断字节流是否符合 ZIP 系列签名（Local File Header、Central Directory、End of Central Directory）。
    /// </summary>
    private static bool IsZipSignature(byte[] header)
    {
        if (header.Length < 4) return false;
        if (StartsWithBytes(header, 0x50, 0x4B, 0x03, 0x04)) return true; // PK.. local file
        if (StartsWithBytes(header, 0x50, 0x4B, 0x05, 0x06)) return true; // PK.. empty archive
        if (StartsWithBytes(header, 0x50, 0x4B, 0x07, 0x08)) return true; // PK.. spanned
        return false;
    }

    /// <summary>
    /// 检查字节数组是否以指定签名字节起始。
    /// </summary>
    private static bool StartsWithBytes(byte[] data, params byte[] signature)
    {
        if (data.Length < signature.Length) return false;
        for (int i = 0; i < signature.Length; i++)
        {
            if (data[i] != signature[i]) return false;
        }
        return true;
    }

    /// <summary>
    /// 检查字节数组在指定偏移处是否匹配指定签名字节。
    /// </summary>
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
    /// 基于扩展名与文件前导字节推断安全的 MIME 类型。
    /// 不依赖浏览器上传的 Content-Type（易被伪造），而是在内部二次确认后返回。
    /// </summary>
    /// <param name="extension">文件扩展名。</param>
    /// <param name="fileHeader">文件前导字节（至少 16 字节更准确）。</param>
    /// <returns>推断出的 MIME 类型；无法识别时返回 <c>application/octet-stream</c>。</returns>
    public static string GetSafeMimeType(string extension, byte[] fileHeader)
    {
        string ext = NormalizeExtension(extension);
        bool headerOk = fileHeader != null && fileHeader.Length >= 2;

        // 图片 MIME
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

        // 文档 MIME
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

        // 视频 MIME
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

        // 音频 MIME
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

        // 压缩包 MIME
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

    /// <summary>
    /// 不合法文件名字符（路径分隔符、通配符、ASCII 控制字符）。
    /// </summary>
    private static readonly char[] InvalidFileNameChars =
    {
        '\\', '/', ':', '*', '?', '"', '<', '>', '|',
        '\0', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07',
        '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F',
        '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17',
        '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F',
    };

    /// <summary>
    /// 清洗文件名：移除路径分隔符、控制字符、Unicode 控制字符与 <c>..</c> 等危险片段。
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

        // 移除连续 ".." 以防止路径遍历（尽管前面已过滤分隔符，仍是一道防御）
        string result = sb.ToString().Replace("..", string.Empty, StringComparison.Ordinal);
        result = result.Trim();

        return string.IsNullOrEmpty(result) ? "unnamed" : result;
    }

    /// <summary>
    /// 清洗单个路径段（如子目录名），防止控制字符与路径遍历。
    /// </summary>
    /// <param name="pathPart">路径段字符串。</param>
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

        string result = sb.ToString().Replace("..", string.Empty, StringComparison.Ordinal);
        return result.Trim();
    }

    /// <summary>
    /// 判断单个字符是否属于危险的文件名字符（控制字符、Unicode 私有使用区等）。
    /// </summary>
    private static bool IsDangerousFileNameChar(char c)
    {
        // 小于 0x20 的 ASCII 控制字符（换行、制表符等）
        if (c < 0x20) return true;
        // DEL 字符
        if (c == '\x7F') return true;
        if (Array.IndexOf(InvalidFileNameChars, c) >= 0) return true;

        // Unicode 类别：控制字符、格式符、私有使用区、代理项、未分配字符
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
    /// 检测路径或文件名是否包含路径遍历迹象。包括 <c>../</c>、<c>..\\</c>、
    /// URL/Unicode 编码变体（%2e、%2f、%5c、U+2024/2025/2026 等）与重复点号。
    /// </summary>
    /// <param name="path">要检查的路径或文件名。</param>
    /// <returns>若存在路径遍历迹象返回 <c>true</c>。</returns>
    public static bool IsPathTraversalAttempt(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        string normalized = path;

        // 直接包含 ".." → 判定为路径遍历
        if (normalized.Contains("..", StringComparison.Ordinal))
            return true;

        // ./ 或 .\
        if (normalized.Contains("./", StringComparison.Ordinal) || normalized.Contains(".\\", StringComparison.Ordinal))
            return true;

        // URL 编码形式：%2e (%2e%2e)、%2f (/)、%5c (\)
        if (normalized.Contains("%2e", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("%2f", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("%5c", StringComparison.OrdinalIgnoreCase))
            return true;

        // Unicode 一个点 / 两个点 / 水平省略号（可被绕过工具利用）
        if (normalized.Contains("\u2024", StringComparison.Ordinal)
            || normalized.Contains("\u2025", StringComparison.Ordinal)
            || normalized.Contains("\u2026", StringComparison.Ordinal))
            return true;

        // .../ 与 ....// 等混淆形式
        if (normalized.Contains(".../", StringComparison.Ordinal)
            || normalized.Contains("...\\", StringComparison.Ordinal)
            || normalized.Contains("....//", StringComparison.Ordinal)
            || normalized.Contains("....\\\\", StringComparison.Ordinal))
            return true;

        return false;
    }

    /// <summary>
    /// 规范化并校验路径：将 <paramref name="relativePath"/> 与 <paramref name="basePath"/> 结合后，
    /// 验证其未突破 <paramref name="basePath"/>。检测到路径遍历或非法字符将抛出异常。
    /// </summary>
    /// <param name="basePath">基准目录（绝对路径）。</param>
    /// <param name="relativePath">相对于 basePath 的子路径。</param>
    /// <returns>规范化后的绝对路径字符串。</returns>
    /// <exception cref="ArgumentException">当路径非法或试图突破 <paramref name="basePath"/> 时抛出。</exception>
    public static string NormalizeAndValidatePath(string basePath, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("基准路径不能为空。", nameof(basePath));
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("相对路径不能为空。", nameof(relativePath));

        if (IsPathTraversalAttempt(relativePath))
            throw new ArgumentException("相对路径包含路径遍历字符。", nameof(relativePath));

        // 获取基础路径的绝对形式，并移除末尾分隔符以便后续拼接
        string fullBase;
        try
        {
            fullBase = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("基准路径无效：" + ex.Message, nameof(basePath), ex);
        }

        // 组合后再次规范化，用于比较
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

        // 关键防御：组合后的路径必须仍以 basePath 起始
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

    /// <summary>
    /// 将扩展名规范化为以点号开头的小写形式（".jpg"）。
    /// </summary>
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
