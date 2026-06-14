using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// HTML 富文本清理工具类。基于白名单策略（标签/属性/CSS/协议），
/// 使用轻量级逐字符状态机扫描实现，不依赖第三方库，不使用正则表达式，避免 ReDoS。
/// 核心能力：移除 script/iframe 等危险标签、剥离事件处理程序、
/// 规范化 href/src 伪协议、允许/禁止按等级过滤标签与属性、
/// 多重 HTML 实体解码循环防御（针对双层编码绕过攻击）。
/// </summary>
public static class HtmlSanitizerUtil
{
    /// <summary>
    /// HTML 清理等级，决定标签白名单范围：
    /// <list type="bullet">
    /// <item><term>Strict</term><description>仅保留换行标签（&lt;br&gt; &lt;p&gt;），其余作为纯文本。</description></item>
    /// <item><term>Balanced</term><description>默认等级，适用于 CMS 文章/评论。</description></item>
    /// <item><term>Relaxed</term><description>保留更丰富的标签（iframe、video、audio 等），仅用于后台编辑。</description></item>
    /// </list>
    /// </summary>
    public enum SanitizerLevel
    {
        /// <summary>严格模式。</summary>
        Strict,
        /// <summary>均衡模式（默认）。</summary>
        Balanced,
        /// <summary>宽松模式。</summary>
        Relaxed
    }

    // ========== 白名单集合：按等级/标签决定允许的内容 ==========

    /// <summary>
    /// 均衡模式允许的标签集合。包含常见排版、表格、列表、图片、超链接。
    /// </summary>
    private static readonly HashSet<string> BalancedTagWhitelist = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "b", "blockquote", "br", "caption", "code", "col", "colgroup", "del", "dd",
        "div", "dl", "dt", "em", "figcaption", "figure", "font", "h1", "h2", "h3", "h4",
        "h5", "h6", "hr", "i", "img", "ins", "li", "ol", "p", "pre", "q", "small",
        "span", "strong", "sub", "sup", "table", "tbody", "td", "tfoot", "th", "thead",
        "tr", "u", "ul"
    };

    /// <summary>
    /// 严格模式允许的标签集合（仅换行/段落）。
    /// </summary>
    private static readonly HashSet<string> StrictTagWhitelist = new(StringComparer.OrdinalIgnoreCase)
    {
        "br", "p"
    };

    /// <summary>
    /// 宽松模式额外允许的标签集合（多媒体容器）。
    /// </summary>
    private static readonly HashSet<string> RelaxedExtraTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "iframe", "video", "audio", "source", "track"
    };

    /// <summary>通用属性白名单（title、class、style、alt、width、height）。</summary>
    private static readonly HashSet<string> GeneralAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "title", "class", "style", "alt", "width", "height"
    };

    /// <summary>&lt;a&gt; 的属性白名单。</summary>
    private static readonly HashSet<string> AnchorAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "href", "title", "class", "style", "target", "rel"
    };

    /// <summary>&lt;img&gt; 的属性白名单。</summary>
    private static readonly HashSet<string> ImageAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "alt", "title", "width", "height", "class", "style"
    };

    /// <summary>&lt;iframe&gt; 的属性白名单。</summary>
    private static readonly HashSet<string> IframeAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "width", "height", "frameborder", "allow", "class", "style", "title"
    };

    /// <summary>&lt;video&gt; / &lt;audio&gt; / &lt;source&gt; / &lt;track&gt; 共用属性白名单。</summary>
    private static readonly HashSet<string> MediaAllowedAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "src", "width", "height", "controls", "autoplay", "loop", "muted", "preload", "class", "style", "title"
    };

    /// <summary>
    /// style 属性中允许的 CSS 键（仅保留无执行能力的视觉样式，禁止 expression、url 等）。
    /// </summary>
    private static readonly HashSet<string> StyleAllowedProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        "color", "background-color", "font-size", "font-weight",
        "font-style", "text-align", "text-decoration"
    };

    /// <summary>
    /// 显式危险属性集合（含能形成脚本回调、XBL 绑定等能力的属性名）。
    /// </summary>
    private static readonly HashSet<string> DangerousAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "xmlns", "xlink:href", "formaction", "form", "formmethod",
        "formtarget", "fscommand", "seeksegmenttime", "ping"
    };

    // ========== 公共入口方法 ==========

    /// <summary>
    /// 使用指定等级清理 HTML 文本。
    /// 流程：多重实体解码 → 移除危险标签 → 状态机白名单过滤 → 伪协议归一化。
    /// </summary>
    /// <param name="html">原始 HTML 字符串（可能含恶意内容）。</param>
    /// <param name="level">清理等级，默认 <see cref="SanitizerLevel.Balanced"/>。</param>
    /// <returns>清理后的安全 HTML 字符串。</returns>
    public static string Sanitize(string html, SanitizerLevel level = SanitizerLevel.Balanced)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;

        var working = html;

        // 步骤 1：多重实体解码循环（最多 5 次），防御双层编码绕过（如 &amp;lt;script&amp;gt;）
        working = DecodeLoop(working);

        // 步骤 2：整体移除 script/iframe/form 等危险标签及其内容
        working = RemoveScriptTagsInternal(working);

        // 步骤 3：基于白名单进行标签/属性级状态机扫描与重写
        working = StateMachineSanitize(working, level);

        // 步骤 4：再次扫描 href/src，对残留的 javascript:/vbscript: 做兜底归一
        working = NormalizeProtocolInternal(working);

        return working;
    }

    /// <summary>严格模式清理（别名）。</summary>
    public static string SanitizeStrict(string html) => Sanitize(html, SanitizerLevel.Strict);

    /// <summary>均衡模式清理（别名）。</summary>
    public static string SanitizeRich(string html) => Sanitize(html, SanitizerLevel.Balanced);

    /// <summary>
    /// 整体移除 script/iframe/object/style 等危险标签及其内容（作为独立公开方法调用）。
    /// </summary>
    /// <param name="html">原始 HTML。</param>
    /// <returns>移除了危险标签的文本。</returns>
    public static string RemoveScriptTags(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return RemoveScriptTagsInternal(html);
    }

    /// <summary>
    /// 仅移除 onclick/onload/onmouseover 等事件处理程序，保留其它标签与属性。
    /// </summary>
    /// <param name="html">原始 HTML。</param>
    /// <returns>移除了事件处理程序的 HTML。</returns>
    public static string RemoveEventHandlers(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return StateMachineSanitize(html, SanitizerLevel.Relaxed, onlyRemoveHandlers: true);
    }

    /// <summary>
    /// 归一化伪协议：将 href/src 中的 javascript:/vbscript:/data: 等值替换为 #。
    /// </summary>
    /// <param name="html">原始 HTML。</param>
    /// <returns>协议安全后的 HTML。</returns>
    public static string NormalizeProtocol(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html ?? string.Empty;
        return NormalizeProtocolInternal(html);
    }

    // ========== 核心：逐字符状态机扫描 ==========

    /// <summary>
    /// 逐字符扫描并重建标签树，按白名单保留/丢弃标签、属性与 style。
    /// </summary>
    /// <param name="html">输入 HTML。</param>
    /// <param name="level">清理等级。</param>
    /// <param name="onlyRemoveHandlers">true=仅移除事件处理程序，保留全部标签。</param>
    /// <returns>清理后的 HTML。</returns>
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
                // 进入标签解析：寻找起始/结束与标签名
                int j = i + 1;
                while (j < n && char.IsWhiteSpace(html[j])) j++;
                if (j >= n)
                {
                    // 孤立 '<'：直接编码为文本实体
                    sb.Append("&lt;");
                    i++;
                    continue;
                }

                bool isClosing = false;
                if (html[j] == '/') { isClosing = true; j++; }

                int nameStart = j;
                while (j < n && !char.IsWhiteSpace(html[j]) && html[j] != '>' && html[j] != '/')
                    j++;

                if (j == nameStart)
                {
                    sb.Append("&lt;");
                    i++;
                    continue;
                }

                string tagName = html.Substring(nameStart, j - nameStart).Trim();
                // 标签名仅允许字母/数字/'-'/':'，抑制任意自定义标签攻击
                if (string.IsNullOrEmpty(tagName) || !IsValidTagName(tagName))
                {
                    sb.Append("&lt;");
                    i++;
                    continue;
                }

                // 寻找标签结束位置 '>'
                bool selfClosing = false;
                while (j < n && html[j] != '>')
                {
                    if (html[j] == '<') break; // 异常嵌套，放弃此标签
                    if (html[j] == '/' && j + 1 < n && html[j + 1] == '>')
                    {
                        selfClosing = true;
                        break;
                    }
                    j++;
                }

                if (j >= n || html[j] != '>')
                {
                    // 未闭合的标签，整个 '<' 编码为文本
                    sb.Append("&lt;");
                    i++;
                    continue;
                }

                int tagEnd = j;

                bool keepTag = onlyRemoveHandlers || IsTagAllowed(tagName, level);

                if (keepTag && !onlyRemoveHandlers)
                {
                    // 在白名单内：重写属性，同时对 <a> 强制附加 target/rel
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

                    // 链接强制 target=_blank 且 rel=noopener noreferrer
                    // 避免在新窗口中通过 window.opener 访问原窗口
                    if (string.Equals(tagName, "a", StringComparison.OrdinalIgnoreCase) && !isClosing)
                    {
                        bool hasTarget = ContainsAttribute(html, attrStart, attrEnd, "target");
                        bool hasRel = ContainsAttribute(html, attrStart, attrEnd, "rel");
                        if (!hasTarget)
                        {
                            sb.Append(" target=\"_blank\"");
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
                    // 仅事件处理程序剥离模式：保留标签本身，重写属性
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
                    // 不在白名单：将 '<' 编码为文本，其内容作为普通文字继续
                    sb.Append("&lt;");
                    i++;
                    continue;
                }

                i = tagEnd + 1;
            }
            else if (c == '&')
            {
                // HTML 实体原样输出（解码由 DecodeLoop 完成）
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

    /// <summary>
    /// 判断标签名在指定等级下是否被允许。
    /// </summary>
    private static bool IsTagAllowed(string tagName, SanitizerLevel level)
    {
        return level switch
        {
            SanitizerLevel.Strict => StrictTagWhitelist.Contains(tagName),
            SanitizerLevel.Balanced => BalancedTagWhitelist.Contains(tagName),
            SanitizerLevel.Relaxed => BalancedTagWhitelist.Contains(tagName) || RelaxedExtraTags.Contains(tagName),
            _ => false
        };
    }

    /// <summary>
    /// 校验标签名合法性：仅允许字母/数字/'-'/':'（防止脚本、SVG 命名空间滥用等攻击）。
    /// </summary>
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

    // ========== 属性解析与规则 ==========

    /// <summary>
    /// 扫描 [attrStart, attrEnd) 区间内的属性串，按标签白名单规则重建安全属性串。
    /// </summary>
    private static string ProcessAttributes(string tagName, string html, int attrStart, int attrEnd, SanitizerLevel level, bool onlyRemoveHandlers = false)
    {
        if (attrEnd <= attrStart)
            return string.Empty;

        var sb = new StringBuilder();
        int i = attrStart;
        int n = Math.Min(attrEnd, html.Length);

        // 跳过标签名后面的空白
        while (i < n && char.IsWhiteSpace(html[i])) i++;

        while (i < n)
        {
            // 跳过自闭合符号 '/' 与连续空白
            if (html[i] == '/') { i++; continue; }
            if (char.IsWhiteSpace(html[i])) { i++; continue; }

            // 读取属性名：直到 =、空白、/ 或区间结束
            int nameStart = i;
            while (i < n && html[i] != '=' && !char.IsWhiteSpace(html[i]) && html[i] != '/')
                i++;

            if (i == nameStart)
            {
                if (i < n) i++;
                continue;
            }

            string attrName = html.Substring(nameStart, i - nameStart).Trim();
            if (string.IsNullOrEmpty(attrName))
                continue;

            // 读取 '=' 及其值（支持双引号、单引号、无引号三种书写形式）
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

            // 属性白名单判定
            if (!IsAttributeAllowed(tagName, attrName, attrValue, level, onlyRemoveHandlers, out string? finalValue))
                continue;

            // 追加安全属性
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

    /// <summary>
    /// 判断单个属性是否允许：事件处理程序一律禁止、显式危险属性禁止、
    /// 按标签区分属性白名单、href/src 只允许 http(s)/相对路径、
    /// style 只允许白名单内 CSS 键，并对值中的伪协议再做一轮防御。
    /// </summary>
    private static bool IsAttributeAllowed(string tagName, string attrName, string? rawValue, SanitizerLevel level, bool onlyRemoveHandlers, out string? finalValue)
    {
        finalValue = rawValue;

        // 1) on* 事件处理程序：永远禁止（onclick、onload、onmouseover 等）
        if (attrName.StartsWith("on", StringComparison.OrdinalIgnoreCase))
            return false;

        // 2) 显式危险属性黑名单（xlink:href、formaction、ping 等）
        if (DangerousAttributes.Contains(attrName))
            return false;

        // 3) 仅移除事件处理程序模式：此处即可放行（但仍然对伪协议防御）
        if (onlyRemoveHandlers)
        {
            if (rawValue != null && ContainsDangerousProtocol(rawValue))
                return false;
            return true;
        }

        // 4) 按标签选择属性白名单
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

        // 5) href/src 只允许 http(s) 绝对 URL 或相对路径，禁止伪协议
        if (string.Equals(attrName, "href", StringComparison.OrdinalIgnoreCase)
            || string.Equals(attrName, "src", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(rawValue))
                return false;

            string value = rawValue.Trim();
            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("/", StringComparison.Ordinal))
            {
                finalValue = value;
                return true;
            }
            return false;
        }

        // 6) target 属性：仅允许 _blank
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

        // 8) style 属性：按 CSS 键白名单过滤，禁止 expression/url/javascript 等
        if (string.Equals(attrName, "style", StringComparison.OrdinalIgnoreCase))
        {
            var cleaned = FilterStyleValue(rawValue);
            if (string.IsNullOrWhiteSpace(cleaned))
                return false;
            finalValue = cleaned;
            return true;
        }

        // 9) 值中包含 javascript:/vbscript: 的一律禁止（兜底防御）
        if (rawValue != null && ContainsDangerousProtocol(rawValue))
            return false;

        finalValue = rawValue;
        return true;
    }

    /// <summary>
    /// 过滤 style 属性值：按 CSS 键白名单保留安全的视觉声明，并对值中的危险模式过滤。
    /// </summary>
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
            // 禁止包含 expression、url、javascript、data:、moz-binding、behavior 等执行能力
            if (ContainsStyleDangerousValue(value))
                continue;

            if (sb.Length > 0) sb.Append(';');
            sb.Append(key.ToLowerInvariant());
            sb.Append(':');
            sb.Append(value);
        }

        return sb.Length == 0 ? null : sb.ToString();
    }

    /// <summary>
    /// 判断 style 值中是否存在危险关键字（expression、url、javascript 等）。
    /// </summary>
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

    /// <summary>
    /// 判断字符串中是否包含 javascript:/vbscript:/data: 等危险伪协议。
    /// 去除空格与制表符以防御 "java script:" 这种插入空白的绕过。
    /// </summary>
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

    // ========== 辅助工具 ==========

    /// <summary>
    /// 判断原属性区间中是否包含指定属性名（用于 <a> 的 target/rel 强制补充决策）。
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

    /// <summary>
    /// 判断已处理后的属性串中是否已包含 rel="noopener noreferrer"。
    /// </summary>
    private static bool ContainsRelNoopener(string attrsText)
    {
        if (string.IsNullOrWhiteSpace(attrsText)) return false;
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

    /// <summary>
    /// 对属性值进行 HTML 编码：转义双引号、与号、尖括号，保证属性解析安全。
    /// </summary>
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

    // ========== 脚本标签与伪协议整体清理 ==========

    /// <summary>
    /// 整体移除 script/iframe/object/form/input 等危险标签及其内部内容，
    /// 不进入后续白名单扫描（用于双重防御）。
    /// </summary>
    private static string RemoveScriptTagsInternal(string html)
    {
        // 危险标签集合：含脚本、外嵌资源、文档结构与表单元素
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
                while (j < n && html[j] != '>') j++;
                if (j >= n)
                {
                    sb.Append(c);
                    i++;
                    continue;
                }

                if (!string.IsNullOrEmpty(tagName) && dangerous.Contains(tagName))
                {
                    // 开标签：跳到对应闭合标签
                    if (!isClosing)
                    {
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
                            // 未找到闭合：跳到文档末尾（保守策略）
                            i = html.Length;
                        }
                    }
                    else
                    {
                        i = j + 1;
                    }
                    continue;
                }

                // 非危险标签：原样追加标签区间
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

    /// <summary>
    /// 伪协议归一化：针对 state machine 之后可能残留的属性再次扫描，
    /// 将所有 href/src 中含 javascript:/vbscript:/data: 的值替换为 #。
    /// </summary>
    private static string NormalizeProtocolInternal(string html)
    {
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
                while (j < n && !char.IsWhiteSpace(html[j]) && html[j] != '>' && html[j] != '/') j++;
                while (j < n && html[j] != '>') j++;
                if (j >= n) { sb.Append(c); i++; continue; }

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

    /// <summary>
    /// 在单个标签区间内扫描 href/src 属性并替换危险协议值。
    /// </summary>
    private static string SanitizeProtocolInTagSegment(string segment)
    {
        var patterns = new[] { "href", "src" };
        string result = segment;
        foreach (var p in patterns)
        {
            result = ReplaceProtocol(result, p);
        }
        return result;
    }

    /// <summary>
    /// 将指定属性名（href/src）下的值中如含危险协议，则值整体替换为 #。
    /// </summary>
    private static string ReplaceProtocol(string text, string attrName)
    {
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
            int eq = text.IndexOf('=', i);
            if (eq < 0)
            {
                sb.Append(text.AsSpan(i));
                break;
            }
            sb.Append(text.AsSpan(i, eq - i + 1));
            i = eq + 1;
            // 跳过 = 后的空白
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

    /// <summary>
    /// 从文本中从 startIndex 起寻找属性名「attrName」的起始下标（大小写不敏感，支持属性名与 '=' 之间存在空白）。
    /// </summary>
    private static int IndexOfAttrName(string text, string attrName, int startIndex)
    {
        for (int i = startIndex; i + attrName.Length <= text.Length; i++)
        {
            if (string.Compare(text, i, attrName, 0, attrName.Length, StringComparison.OrdinalIgnoreCase) == 0)
            {
                int after = i + attrName.Length;
                if (after < text.Length && text[after] == '=')
                    return i;
                // 允许属性名与 = 之间存在空白（兼容常见的宽松书写）
                int k = after;
                while (k < text.Length && char.IsWhiteSpace(text[k])) k++;
                if (k < text.Length && text[k] == '=')
                    return i;
            }
        }
        return -1;
    }

    // ========== 多重 HTML 实体解码循环防御 ==========

    /// <summary>
    /// 对输入进行多次 HTML 实体解码（最多 5 次），直到结果不再变化，
    /// 以防御 "&amp;lt;script&amp;gt;" 这样的双层编码绕过攻击。
    /// </summary>
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

    /// <summary>
    /// 手写简化版 HTML 实体解码：处理常见命名实体（amp/lt/gt/quot/apos/nbsp）
    /// 以及数字实体 &#x / &#，避免引入 System.Web 依赖。
    /// </summary>
    private static string WebUtilityHtmlDecode(string value)
    {
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
                // 限制实体长度（最多 12 字符），避免误匹配长文本
                if (end > 0 && end - i <= 12)
                {
                    string entity = value.Substring(i + 1, end - i - 1);
                    if (TryDecodeEntity(entity, out char decoded))
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

    /// <summary>
    /// 尝试解码单个 HTML 实体字符串（不含前后的 & 与 ;）。
    /// 支持常见命名实体及数字实体（&#123; / &#x1F;）。
    /// </summary>
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
        // &#xHHHH; 十六进制
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
        // &#123; 十进制
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
