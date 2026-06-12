using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// HTML 富文本清洗工具类，基于白名单策略，使用轻量级状态机扫描实现，不依赖第三方库。
/// </summary>
public static class HtmlSanitizerUtil
{
    /// <summary>
    /// 清洗等级。
    /// </summary>
    public enum SanitizerLevel
    {
        /// <summary>
        /// 严格模式，仅保留纯文本及少量换行标签 (&lt;br&gt; &lt;p&gt;)。
        /// </summary>
        Strict,

        /// <summary>
        /// 均衡模式，标准内容管理（CMS 文章、评论默认使用此级别）。
        /// </summary>
        Balanced,

        /// <summary>
        /// 宽松模式，保留更多标签（iframe、video 等，用于后台管理富文本编辑）。
        /// </summary>
        Relaxed
    }

    // ============= 白名单集合 =============

    private static readonly HashSet<string> BalancedTagWhitelist = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "b", "blockquote", "br", "caption", "code", "col", "colgroup", "del", "dd",
        "div", "dl", "dt", "em", "figcaption", "figure", "font", "h1", "h2", "h3", "h4",
        "h5", "h6", "hr", "i", "img", "ins", "li", "ol", "p", "pre", "q", "small",
        "span", "strong", "sub", "sup", "table", "tbody", "td", "tfoot", "th", "thead",
        "tr", "u", "ul"
    };

    private static readonly HashSet<string> StrictTagWhitelist = new(StringComparer.OrdinalIgnoreCase)
    {
        "br", "p"
    };

    private static readonly HashSet<string> RelaxedExtraTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "iframe", "video", "audio", "source", "track"
    };

    private static readonly HashSet<string> GeneralAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "title", "class", "style", "alt", "width", "height"
    };

    private static readonly HashSet<string> AnchorAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "href", "title", "class", "style", "target", "rel"
    };

    private static readonly HashSet<string> ImageAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "alt", "title", "width", "height", "class", "style"
    };

    private static readonly HashSet<string> IframeAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "width", "height", "frameborder", "allow", "class", "style", "title"
    };

    private static readonly HashSet<string> MediaAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "width", "height", "controls", "autoplay", "loop", "muted", "preload", "class", "style", "title"
    };

    private static readonly HashSet<string> StyleAllowedProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        "color", "background-color", "font-size", "font-weight",
        "font-style", "text-align", "text-decoration"
    };

    private static readonly HashSet<string> DangerousAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "xmlns", "xlink:href", "formaction", "form", "formmethod",
        "formtarget", "fscommand", "seeksegmenttime", "ping"
    };

    // ============= 公共入口 =============

    /// <summary>
    /// 使用指定等级清洗 HTML 文本。
    /// </summary>
    /// <param name="html">原始 HTML 字符串。</param>
    /// <param name="level">清洗等级，默认 <see cref="SanitizerLevel.Balanced"/>。</param>
    /// <returns>清洗后的安全 HTML。</returns>
    public static string Sanitize(string html, SanitizerLevel level = SanitizerLevel.Balanced)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;

        var working = html;

        // Step 1: 多重编码/解码循环防御 &lt;script&gt; 等绕过
        working = DecodeLoop(working);

        // Step 2: 移除 script / iframe 等危险标签 & 内容（先于白名单扫描运行，对内容也做一次兜底）
        working = RemoveScriptTagsInternal(working);

        // Step 3: 状态机扫描：基于白名单处理标签与属性
        working = StateMachineSanitize(working, level);

        // Step 4: 再次对残留的 javascript: / vbscript: 伪协议进行归一化
        working = NormalizeProtocolInternal(working);

        return working;
    }

    /// <summary>
    /// 严格模式：仅保留纯文本（保留 &lt;br&gt; 与 &lt;p&gt;）。
    /// </summary>
    public static string SanitizeStrict(string html)
    {
        return Sanitize(html, SanitizerLevel.Strict);
    }

    /// <summary>
    /// 富文本模式：保留常用格式化标签。
    /// </summary>
    public static string SanitizeRich(string html)
    {
        return Sanitize(html, SanitizerLevel.Balanced);
    }

    /// <summary>
    /// 独立方法：移除 script / iframe / object / embed / style / link / meta 等危险标签及其内容。
    /// </summary>
    public static string RemoveScriptTags(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return RemoveScriptTagsInternal(html);
    }

    /// <summary>
    /// 独立方法：移除 onclick / onload / onmouseover 等事件处理属性。
    /// </summary>
    public static string RemoveEventHandlers(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return StateMachineSanitize(html, SanitizerLevel.Relaxed, onlyRemoveHandlers: true);
    }

    /// <summary>
    /// 独立方法：归一化伪协议，将 javascript: / vbscript: 等危险 URL 替换为 #。
    /// </summary>
    public static string NormalizeProtocol(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return NormalizeProtocolInternal(html);
    }

    // ============= 核心：状态机扫描 =============

    /// <summary>
    /// 逐字符扫描的 HTML 标签解析器，非正则，避免 ReDoS。
    /// </summary>
    private static string StateMachineSanitize(string html, SanitizerLevel level, bool onlyRemoveHandlers = false)
    {
        var sb = new StringBuilder(html.Length);
        int i = 0;
        int n = html.Length;

        while (i < n)
        {
            char c = html[i];

            if (c == '<')
            {
                // 尝试解析标签
                int tagStart = i;
                int j = i + 1;

                // 跳过可选空白
                while (j < n && char.IsWhiteSpace(html[j])) j++;
                if (j >= n)
                {
                    // 不是完整标签，直接追加 '<' 并继续
                    sb.Append('&');
                    sb.Append("lt;");
                    i++;
                    continue;
                }

                bool isClosing = false;
                if (html[j] == '/') { isClosing = true; j++; }

                // 读取标签名：直到空白、> 或 /
                int nameStart = j;
                while (j < n && !char.IsWhiteSpace(html[j]) && html[j] != '>' && html[j] != '/')
                    j++;

                if (j == nameStart)
                {
                    // 空标签名：编码为文本
                    sb.Append('&');
                    sb.Append("lt;");
                    i++;
                    continue;
                }

                string tagName = html.Substring(nameStart, j - nameStart).Trim();
                if (string.IsNullOrEmpty(tagName) || !IsValidTagName(tagName))
                {
                    sb.Append('&');
                    sb.Append("lt;");
                    i++;
                    continue;
                }

                // 寻找标签结束
                bool selfClosing = false;
                while (j < n && html[j] != '>')
                {
                    if (html[j] == '<')
                    {
                        // 异常的嵌套 '<'，放弃此标签
                        break;
                    }
                    if (html[j] == '/' && j + 1 < n && html[j + 1] == '>')
                    {
                        selfClosing = true;
                        break;
                    }
                    j++;
                }

                if (j >= n || html[j] != '>')
                {
                    // 未闭合的标签，编码为文本
                    sb.Append('&');
                    sb.Append("lt;");
                    i++;
                    continue;
                }

                int tagEnd = j; // index of '>'

                // 决定是否保留此标签
                bool keepTag = onlyRemoveHandlers || IsTagAllowed(tagName, level);

                if (keepTag && !onlyRemoveHandlers)
                {
                    int attrStart = nameStart + tagName.Length;
                    int attrEnd = selfClosing ? tagEnd - 1 : tagEnd;

                    var processedAttrs = ProcessAttributes(tagName, html, attrStart, attrEnd, level);

                    sb.Append('<');
                    if (isClosing) sb.Append('/');
                    sb.Append(tagName.ToLowerInvariant());

                    if (processedAttrs.Length > 0)
                    {
                        sb.Append(' ');
                        sb.Append(processedAttrs);
                    }

                    // 为 <a> 强制补充 rel / target
                    if (string.Equals(tagName, "a", StringComparison.OrdinalIgnoreCase) && !isClosing)
                    {
                        bool hasTarget = ContainsAttribute(html, attrStart, attrEnd, "target");
                        bool hasRel = ContainsAttribute(html, attrStart, attrEnd, "rel");
                        if (!hasTarget)
                        {
                            sb.Append(" target=\"_blank\"");
                        }
                        else if (processedAttrs.Length == 0 || !processedAttrs.Contains("target="))
                        {
                            // processedAttrs 已经过滤出合法 target，这里只是保护
                        }
                        if (!hasRel || !ContainsRelNoopener(processedAttrs))
                        {
                            sb.Append(" rel=\"noopener noreferrer\"");
                        }
                    }

                    if (selfClosing)
                        sb.Append(" /");
                    sb.Append('>');
                }
                else if (onlyRemoveHandlers)
                {
                    // 仅移除事件处理：保留标签，但重写属性
                    int attrStart = nameStart + tagName.Length;
                    int attrEnd = selfClosing ? tagEnd - 1 : tagEnd;
                    var processedAttrs = ProcessAttributes(tagName, html, attrStart, attrEnd, level, onlyRemoveHandlers: true);

                    sb.Append('<');
                    if (isClosing) sb.Append('/');
                    sb.Append(tagName.ToLowerInvariant());
                    if (processedAttrs.Length > 0)
                    {
                        sb.Append(' ');
                        sb.Append(processedAttrs);
                    }
                    if (selfClosing)
                        sb.Append(" /");
                    sb.Append('>');
                }
                else
                {
                    // 不在白名单：编码为文本
                    sb.Append('&');
                    sb.Append("lt;");
                    // 保留原内容作为纯文本的一部分
                    // （将 '<' 编码，后续字符作为普通文本追加）
                    i++;
                    continue;
                }

                i = tagEnd + 1;
            }
            else if (c == '&')
            {
                // 保留 HTML 实体原样输出
                sb.Append(c);
                i++;
            }
            else
            {
                sb.Append(c);
                i++;
            }
        }

        return sb.ToString();
    }

    private static bool IsTagAllowed(string tagName, SanitizerLevel level)
    {
        switch (level)
        {
            case SanitizerLevel.Strict:
                return StrictTagWhitelist.Contains(tagName);
            case SanitizerLevel.Balanced:
                return BalancedTagWhitelist.Contains(tagName);
            case SanitizerLevel.Relaxed:
                return BalancedTagWhitelist.Contains(tagName) || RelaxedExtraTags.Contains(tagName);
            default:
                return false;
        }
    }

    private static bool IsValidTagName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if (!char.IsLetterOrDigit(c) && c != '-' && c != ':')
                return false;
        }
        return true;
    }

    // ============= 属性解析 =============

    /// <summary>
    /// 扫描属性区间，按标签白名单规则重新生成安全的属性串。
    /// </summary>
    private static string ProcessAttributes(string tagName, string html, int attrStart, int attrEnd, SanitizerLevel level, bool onlyRemoveHandlers = false)
    {
        if (attrEnd <= attrStart)
            return string.Empty;

        var sb = new StringBuilder();
        int i = attrStart;
        int n = Math.Min(attrEnd, html.Length);

        // 跳过标签名后面的空白，直接进入属性扫描
        while (i < n && char.IsWhiteSpace(html[i])) i++;

        while (i < n)
        {
            // 跳过 '/' 自闭合符号
            if (html[i] == '/') { i++; continue; }
            if (char.IsWhiteSpace(html[i])) { i++; continue; }

            // 读取属性名：直到 =、空白、/、或区间结束
            int nameStart = i;
            while (i < n && html[i] != '=' && !char.IsWhiteSpace(html[i]) && html[i] != '/')
                i++;

            if (i == nameStart)
            {
                // 空属性名，跳过
                if (i < n) i++;
                continue;
            }

            string attrName = html.Substring(nameStart, i - nameStart).Trim();
            if (string.IsNullOrEmpty(attrName))
                continue;

            // 跳过 '=' 与空白
            bool hasValue = false;
            string? attrValue = null;
            while (i < n && char.IsWhiteSpace(html[i])) i++;
            if (i < n && html[i] == '=')
            {
                hasValue = true;
                i++;
                while (i < n && char.IsWhiteSpace(html[i])) i++;

                if (i < n && (html[i] == '"' || html[i] == '\''))
                {
                    char quote = html[i];
                    i++;
                    int valStart = i;
                    while (i < n && html[i] != quote) i++;
                    if (i < n)
                    {
                        attrValue = html.Substring(valStart, i - valStart);
                        i++; // 跳过闭合引号
                    }
                    else
                    {
                        attrValue = html.Substring(valStart, n - valStart);
                    }
                }
                else
                {
                    int valStart = i;
                    while (i < n && !char.IsWhiteSpace(html[i]) && html[i] != '>' && html[i] != '/')
                        i++;
                    attrValue = html.Substring(valStart, i - valStart);
                }
            }

            // --- 属性白名单判断 ---
            if (!IsAttributeAllowed(tagName, attrName, attrValue, level, onlyRemoveHandlers, out string? finalValue))
                continue;

            // 追加
            if (sb.Length > 0)
                sb.Append(' ');
            sb.Append(attrName.ToLowerInvariant());
            if (hasValue && finalValue != null)
            {
                sb.Append('=');
                sb.Append('"');
                sb.Append(HtmlEncodeAttributeValue(finalValue));
                sb.Append('"');
            }
        }

        return sb.ToString();
    }

    private static bool IsAttributeAllowed(string tagName, string attrName, string? rawValue, SanitizerLevel level, bool onlyRemoveHandlers, out string? finalValue)
    {
        finalValue = rawValue;

        // 1) on* 事件处理程序：永远禁止
        if (attrName.StartsWith("on", StringComparison.OrdinalIgnoreCase))
            return false;

        // 2) 显式危险属性
        if (DangerousAttributes.Contains(attrName))
            return false;

        // 3) 若是仅移除事件处理模式：到这里就通过
        if (onlyRemoveHandlers)
        {
            // 仍然防御：如果值是 javascript: 协议则拒绝
            if (rawValue != null && ContainsDangerousProtocol(rawValue))
                return false;
            return true;
        }

        // 4) 按标签决定属性是否许可
        HashSet<string>? allowedAttrs = null;
        if (string.Equals(tagName, "a", StringComparison.OrdinalIgnoreCase))
            allowedAttrs = AnchorAllowedAttributes;
        else if (string.Equals(tagName, "img", StringComparison.OrdinalIgnoreCase))
            allowedAttrs = ImageAllowedAttributes;
        else if (string.Equals(tagName, "iframe", StringComparison.OrdinalIgnoreCase))
            allowedAttrs = IframeAllowedAttributes;
        else if (string.Equals(tagName, "video", StringComparison.OrdinalIgnoreCase)
              || string.Equals(tagName, "audio", StringComparison.OrdinalIgnoreCase)
              || string.Equals(tagName, "source", StringComparison.OrdinalIgnoreCase)
              || string.Equals(tagName, "track", StringComparison.OrdinalIgnoreCase))
            allowedAttrs = MediaAllowedAttributes;
        else
            allowedAttrs = GeneralAllowedAttributes;

        if (!allowedAttrs.Contains(attrName))
            return false;

        // 5) href / src 值安全检查
        if (string.Equals(attrName, "href", StringComparison.OrdinalIgnoreCase)
            || string.Equals(attrName, "src", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(rawValue))
                return false;

            string value = rawValue.Trim();
            // 允许 http:// / https:// / / (相对路径)
            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("/", StringComparison.Ordinal))
            {
                finalValue = value;
                return true;
            }
            return false;
        }

        // 6) target 属性：仅允许 _blank（对 <a>）
        if (string.Equals(attrName, "target", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(rawValue, "_blank", StringComparison.OrdinalIgnoreCase))
            {
                finalValue = "_blank";
                return true;
            }
            return false;
        }

        // 7) rel 属性：规范化为 noopener noreferrer
        if (string.Equals(attrName, "rel", StringComparison.OrdinalIgnoreCase))
        {
            finalValue = "noopener noreferrer";
            return true;
        }

        // 8) style 属性：仅允许特定 CSS 键
        if (string.Equals(attrName, "style", StringComparison.OrdinalIgnoreCase))
        {
            var cleaned = FilterStyleValue(rawValue);
            if (string.IsNullOrWhiteSpace(cleaned))
                return false;
            finalValue = cleaned;
            return true;
        }

        // 9) 值中包含 javascript: 的一律禁止（防御性）
        if (rawValue != null && ContainsDangerousProtocol(rawValue))
            return false;

        finalValue = rawValue;
        return true;
    }

    private static string? FilterStyleValue(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
            return null;

        var sb = new StringBuilder();
        var declarations = rawValue.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var decl in declarations)
        {
            var pair = decl.AsSpan();
            int colon = pair.IndexOf(':');
            if (colon <= 0) continue;

            var key = pair.Slice(0, colon).Trim().ToString();
            var value = pair.Slice(colon + 1).Trim().ToString();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                continue;
            if (!StyleAllowedProperties.Contains(key))
                continue;

            // 防御：值中不能包含 expression、url、javascript 等
            if (ContainsStyleDangerousValue(value))
                continue;

            if (sb.Length > 0) sb.Append(';');
            sb.Append(key.ToLowerInvariant());
            sb.Append(':');
            sb.Append(value);
        }

        return sb.Length == 0 ? null : sb.ToString();
    }

    private static bool ContainsStyleDangerousValue(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        string v = value.ToLowerInvariant();
        return v.Contains("expression")
            || v.Contains("url(")
            || v.Contains("javascript")
            || v.Contains("vbscript")
            || v.Contains("data:")
            || v.Contains("moz-binding")
            || v.Contains("behavior");
    }

    private static bool ContainsDangerousProtocol(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        string v = value.Trim().ToLowerInvariant().Replace(" ", "").Replace("\t", "");
        return v.StartsWith("javascript:")
            || v.StartsWith("vbscript:")
            || v.StartsWith("data:")
            || v.Contains("javascript:")
            || v.Contains("vbscript:");
    }

    // ============= 辅助 =============

    /// <summary>
    /// 识别 raw 属性文本中是否包含指定属性（仅用于 <a> 的 target/rel 判断）。
    /// </summary>
    private static bool ContainsAttribute(string html, int start, int end, string attrName)
    {
        int i = start;
        int n = Math.Min(end, html.Length);
        while (i < n)
        {
            while (i < n && char.IsWhiteSpace(html[i])) i++;
            int nameStart = i;
            while (i < n && html[i] != '=' && !char.IsWhiteSpace(html[i]) && html[i] != '/')
                i++;
            if (i > nameStart)
            {
                string n2 = html.Substring(nameStart, i - nameStart).Trim();
                if (string.Equals(n2, attrName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            // 跳过 = 与值
            while (i < n && char.IsWhiteSpace(html[i])) i++;
            if (i < n && html[i] == '=')
            {
                i++;
                while (i < n && char.IsWhiteSpace(html[i])) i++;
                if (i < n && (html[i] == '"' || html[i] == '\''))
                {
                    char q = html[i++];
                    while (i < n && html[i] != q) i++;
                    if (i < n) i++;
                }
                else
                {
                    while (i < n && !char.IsWhiteSpace(html[i]) && html[i] != '>' && html[i] != '/') i++;
                }
            }
        }
        return false;
    }

    private static bool ContainsRelNoopener(string attrsText)
    {
        if (string.IsNullOrWhiteSpace(attrsText)) return false;
        // 简单扫描：寻找 rel="..." 中是否包含 noopener + noreferrer
        int idx = attrsText.IndexOf("rel", StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return false;
        int eq = attrsText.IndexOf('=', idx);
        if (eq < 0) return false;
        int q1 = attrsText.IndexOf('"', eq);
        int q2 = attrsText.IndexOf('\'', eq);
        char quote = q1 > 0 && (q2 < 0 || q1 < q2) ? '"' : '\'';
        int qs = quote == '"' ? q1 : q2;
        if (qs < 0) return false;
        int qe = attrsText.IndexOf(quote, qs + 1);
        if (qe < 0) return false;
        string v = attrsText.Substring(qs + 1, qe - qs - 1);
        return v.Contains("noopener", StringComparison.OrdinalIgnoreCase)
            && v.Contains("noreferrer", StringComparison.OrdinalIgnoreCase);
    }

    private static string HtmlEncodeAttributeValue(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        var sb = new StringBuilder(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            switch (c)
            {
                case '"': sb.Append("&quot;"); break;
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                default: sb.Append(c); break;
            }
        }
        return sb.ToString();
    }

    // ============= 脚本标签与伪协议清理 =============

    private static string RemoveScriptTagsInternal(string html)
    {
        // 按标签名完整移除（含内容）的危险标签集合
        var dangerous = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "script", "noscript", "style", "link", "meta", "base",
            "iframe", "object", "embed", "form", "input", "textarea",
            "button", "select", "option", "frameset", "frame", "html",
            "head", "body", "comment", "video", "audio"
        };

        var sb = new StringBuilder(html.Length);
        int i = 0;
        int n = html.Length;

        while (i < n)
        {
            char c = html[i];
            if (c == '<')
            {
                int j = i + 1;
                while (j < n && char.IsWhiteSpace(html[j])) j++;
                bool isClosing = false;
                if (j < n && html[j] == '/') { isClosing = true; j++; }
                int nameStart = j;
                while (j < n && !char.IsWhiteSpace(html[j]) && html[j] != '>' && html[j] != '/') j++;
                if (j == nameStart)
                {
                    sb.Append(c);
                    i++;
                    continue;
                }
                string tagName = html.Substring(nameStart, j - nameStart).Trim();
                // 找 '>'
                while (j < n && html[j] != '>') j++;
                if (j >= n)
                {
                    sb.Append(c);
                    i++;
                    continue;
                }

                if (!string.IsNullOrEmpty(tagName) && dangerous.Contains(tagName))
                {
                    // 若是开标签，跳到对应闭合标签
                    if (!isClosing)
                    {
                        // 跳到内容结尾（下一个 </tagName>）
                        string closeTag = "</" + tagName;
                        int closeIdx = html.IndexOf(closeTag, j + 1, StringComparison.OrdinalIgnoreCase);
                        if (closeIdx > 0)
                        {
                            int closeEnd = html.IndexOf('>', closeIdx);
                            if (closeEnd > 0)
                                i = closeEnd + 1;
                            else
                                i = html.Length;
                        }
                        else
                        {
                            // 未找到闭合：跳到文档末尾
                            i = html.Length;
                        }
                    }
                    else
                    {
                        i = j + 1;
                    }
                    continue;
                }

                // 非危险标签：原样追加
                sb.Append(html.AsSpan(i, j - i + 1));
                i = j + 1;
            }
            else
            {
                sb.Append(c);
                i++;
            }
        }

        return sb.ToString();
    }

    private static string NormalizeProtocolInternal(string html)
    {
        // 扫描所有 href / src，将危险协议替换为 #（针对 state machine 遗漏的双写情况做兜底）
        var sb = new StringBuilder(html.Length);
        int i = 0;
        int n = html.Length;
        while (i < n)
        {
            char c = html[i];
            if (c == '<')
            {
                int tagStart = i;
                int j = i + 1;
                while (j < n && char.IsWhiteSpace(html[j])) j++;
                if (j < n && html[j] == '/') j++;
                int nameStart = j;
                while (j < n && !char.IsWhiteSpace(html[j]) && html[j] != '>' && html[j] != '/') j++;
                while (j < n && html[j] != '>') j++;
                if (j >= n) { sb.Append(c); i++; continue; }

                // 处理标签区间：扫描属性中的 href / src 值
                string segment = html.Substring(tagStart, j - tagStart + 1);
                segment = SanitizeProtocolInTagSegment(segment);
                sb.Append(segment);
                i = j + 1;
            }
            else
            {
                sb.Append(c);
                i++;
            }
        }
        return sb.ToString();
    }

    private static string SanitizeProtocolInTagSegment(string segment)
    {
        // 简单的属性内协议替换：查找 href="..." src="..." href='...' src='...' 并替换危险协议值
        var patterns = new[] { "href", "src" };
        string result = segment;
        foreach (var p in patterns)
        {
            result = ReplaceProtocol(result, p);
        }
        return result;
    }

    private static string ReplaceProtocol(string text, string attrName)
    {
        // 寻找 attrName="..." 或 attrName='...'
        var sb = new StringBuilder(text.Length);
        int i = 0;
        int n = text.Length;
        while (i < n)
        {
            int idx = IndexOfAttrName(text, attrName, i);
            if (idx < 0)
            {
                sb.Append(text.AsSpan(i));
                break;
            }
            sb.Append(text.AsSpan(i, idx - i));
            i = idx;
            // 找 '='
            int eq = text.IndexOf('=', i);
            if (eq < 0)
            {
                sb.Append(text.AsSpan(i));
                break;
            }
            sb.Append(text.AsSpan(i, eq - i + 1));
            i = eq + 1;
            // 跳过空白
            while (i < n && char.IsWhiteSpace(text[i])) { sb.Append(text[i]); i++; }
            if (i < n && (text[i] == '"' || text[i] == '\''))
            {
                char q = text[i];
                sb.Append(q);
                i++;
                int valStart = i;
                while (i < n && text[i] != q) i++;
                string value = text.Substring(valStart, i - valStart);
                if (ContainsDangerousProtocol(value))
                    sb.Append("#");
                else
                    sb.Append(value);
                if (i < n) { sb.Append(text[i]); i++; }
            }
        }
        return sb.ToString();
    }

    private static int IndexOfAttrName(string text, string attrName, int startIndex)
    {
        for (int i = startIndex; i + attrName.Length <= text.Length; i++)
        {
            if (string.Compare(text, i, attrName, 0, attrName.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                int after = i + attrName.Length;
                if (after < text.Length && text[after] == '=')
                    return i;
                // 或空白后 =
                int k = after;
                while (k < text.Length && char.IsWhiteSpace(text[k])) k++;
                if (k < text.Length && text[k] == '=')
                    return i;
            }
        }
        return -1;
    }

    // ============= 多重编码/解码循环防御 =============

    private static string DecodeLoop(string html)
    {
        string current = html;
        string prev;
        int guard = 0;
        do
        {
            prev = current;
            current = WebUtilityHtmlDecode(current);
            guard++;
        } while (current != prev && guard < 5);
        return current;
    }

    private static string WebUtilityHtmlDecode(string value)
    {
        // 手写简单 HTML 实体解码，避免引入 System.Web；
        // 仅处理常见命名实体与数字实体 &#x / &#
        if (string.IsNullOrEmpty(value))
            return value;

        var sb = new StringBuilder(value.Length);
        int i = 0;
        int n = value.Length;
        while (i < n)
        {
            char c = value[i];
            if (c == '&')
            {
                int end = value.IndexOf(';', i);
                if (end > 0 && end - i <= 12)
                {
                    string entity = value.Substring(i + 1, end - i - 1);
                    char decoded;
                    if (TryDecodeEntity(entity, out decoded))
                    {
                        sb.Append(decoded);
                        i = end + 1;
                        continue;
                    }
                }
                sb.Append(c);
                i++;
            }
            else
            {
                sb.Append(c);
                i++;
            }
        }
        return sb.ToString();
    }

    private static bool TryDecodeEntity(string entity, out char value)
    {
        value = '\0';
        if (string.IsNullOrEmpty(entity)) return false;
        switch (entity.ToLowerInvariant())
        {
            case "amp": value = '&'; return true;
            case "lt": value = '<'; return true;
            case "gt": value = '>'; return true;
            case "quot": value = '"'; return true;
            case "apos": value = '\''; return true;
            case "nbsp": value = '\u00A0'; return true;
        }
        if (entity.StartsWith("#x", StringComparison.OrdinalIgnoreCase))
        {
            string hex = entity.Substring(2);
            if (hex.Length > 0 && hex.Length <= 6
                && int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int code)
                && code >= 0 && code <= 0xFFFF)
            {
                value = (char)code;
                return true;
            }
        }
        else if (entity.StartsWith("#", StringComparison.Ordinal))
        {
            string num = entity.Substring(1);
            if (num.Length > 0 && num.Length <= 6
                && int.TryParse(num, NumberStyles.Integer, CultureInfo.InvariantCulture, out int code)
                && code >= 0 && code <= 0xFFFF)
            {
                value = (char)code;
                return true;
            }
        }
        return false;
    }
}
