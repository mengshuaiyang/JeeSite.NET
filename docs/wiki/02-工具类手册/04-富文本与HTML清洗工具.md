# 工具类手册 · 富文本与 HTML 清洗工具

富文本（Rich Text）天然包含 HTML 内容，如果不加限制直接写入数据库并回显给用户，将导致**持久型 XSS**（Cross-Site Scripting）漏洞。本章介绍 `HtmlSanitizerUtil` 的用法、默认白名单策略以及常见攻击向量的防御方式。

---

## 一、HtmlSanitizerUtil

`HtmlSanitizerUtil`（`Utils/HtmlSanitizerUtil.cs`）是一个基于白名单的 HTML 清洗工具，核心策略是：

> 只保留显式声明"允许"的标签 / 属性 / CSS 属性 / URL 协议，其余一律移除或转义。

### 1.1 三种清洗强度

**（1）富文本内容清洗**
```csharp
public static string SanitizeRich(string html)
public static string SanitizeRich(string html, HtmlSanitizerOptions options)
```
保留图片、表格、格式化标签、常用 CSS 样式；移除所有脚本、事件处理器、危险协议。
适用场景：CMS 文章正文、文档编辑。

**（2）严格清洗**
```csharp
public static string SanitizeStrict(string html)
```
仅保留最基础的文本标签（`p`、`em`、`strong`、`br`），移除所有图片、链接、样式。
适用场景：文章摘要、搜索结果摘要、用户评论、表单输入。

**（3）预览模式清洗**
```csharp
public static string SanitizeForPreview(string html)
```
保留样式但移除脚本 / 外部资源（外部图片被替换为占位、iframe 被移除）。
适用场景：管理后台预览第三方内容。

### 1.2 HtmlSanitizerOptions

```csharp
public class HtmlSanitizerOptions
{
    // 允许的 HTML 标签（小写）
    public HashSet<string> AllowedTags { get; set; }

    // 允许的 HTML 属性（全标签通用 + 特殊标签专用）
    public HashSet<string> AllowedAttributes { get; set; }
    public Dictionary<string, HashSet<string>> AllowedAttributesByTag { get; set; }

    // 允许的 CSS 属性（style="..." 内部）
    public HashSet<string> AllowedCssProperties { get; set; }

    // 允许的 URL 协议（a:href / img:src）
    public HashSet<string> AllowedUriSchemes { get; set; }

    // 允许的图片域名白名单（img:src）
    public HashSet<string> AllowedImageDomains { get; set; }

    // 是否允许 data: URI 的图片（Base64）
    public bool AllowDataUriImages { get; set; } = false;

    // Base64 图片最大大小，超过则清除
    public int MaxDataUriImageSizeKb { get; set; } = 256;

    // 输出模式：Html / PlainText
    public OutputMode OutputMode { get; set; } = OutputMode.Html;
}
```

### 1.3 默认 Options 快速构造

```csharp
// 富文本（允许图片、表格、基本样式）
var richOptions = HtmlSanitizerOptions.DefaultRich();

// 严格模式（仅段落与基本文本标签）
var strictOptions = HtmlSanitizerOptions.DefaultStrict();

// 预览模式
var previewOptions = HtmlSanitizerOptions.DefaultPreview();
```

---

## 二、默认白名单配置

### 2.1 允许的 HTML 标签

| 类别 | 标签 |
|------|------|
| **文本** | `p`, `span`, `br`, `hr`, `strong`, `em`, `b`, `i`, `u`, `strike`, `sub`, `sup` |
| **标题** | `h1`, `h2`, `h3`, `h4`, `h5`, `h6` |
| **列表** | `ul`, `ol`, `li`, `dl`, `dt`, `dd` |
| **表格** | `table`, `thead`, `tbody`, `tfoot`, `tr`, `th`, `td`, `caption`, `colgroup`, `col` |
| **链接** | `a` |
| **图片** | `img` |
| **引用 / 代码** | `blockquote`, `pre`, `code` |
| **布局** | `div` |

> 不在白名单内的标签：要么被移除其标签名但保留内容（unwrapped），要么整个块被剥离——取决于配置。`script` / `iframe` / `object` / `embed` / `form` / `style` 默认采用"完全移除"策略。

### 2.2 允许的 HTML 属性

```text
通用属性  : title, class, style
图片属性  : src, alt, width, height
链接属性  : href, target, rel
表格属性  : colspan, rowspan, width, border, cellpadding, cellspacing
```

### 2.3 允许的 CSS 属性

```text
color, background-color, background, font-size, font-weight,
font-family, font-style, text-align, text-decoration,
width, height, margin, margin-left, margin-right, margin-top, margin-bottom,
padding, padding-left, padding-right, padding-top, padding-bottom,
border, border-left, border-right, border-top, border-bottom,
border-collapse, border-spacing, vertical-align, line-height,
display, float, clear, list-style, list-style-type
```

> 不在白名单内的 CSS 属性（如 `expression`、`behavior`、`url()`）一律移除。

### 2.4 允许的 URL 协议

```text
http, https, mailto
```

> `javascript:`、`vbscript:`、`livescript:`、`data:`（图片除外）、`file:` 均被拒绝。

### 2.5 强制过滤的危险内容

| 危险内容 | 处置 |
|---------|------|
| `<script> ... </script>` | 完全移除（包括标签内容） |
| `<style> ... </style>` | 完全移除 |
| `onclick / onerror / onload / onmouseover / on...` | 移除属性 |
| `href="javascript:alert(1)"` | 清空 href 属性 |
| `style="expression(alert(1))"` | 移除 expression 所在声明 |
| `style="url('javascript:')"` | 移除 url() 声明 |
| `<iframe> / <object> / <embed> / <form>` | 完全移除 |
| `<base href="...">` | 完全移除 |
| Base64 图片超大尺寸 | 默认禁止或限制大小 |

---

## 三、扩展白名单（按场景）

不同业务场景可以使用不同的 `HtmlSanitizerOptions` 实例。

### 3.1 示例：只允许纯文本 + 图片

```csharp
var options = new HtmlSanitizerOptions
{
    AllowedTags = new HashSet<string> { "p", "img", "br" },
    AllowedAttributes = new HashSet<string> { "src", "alt", "width", "height" },
    AllowedUriSchemes = new HashSet<string> { "http", "https" },
    AllowedImageDomains = new HashSet<string> { "your-cdn.com", "localhost" },
    AllowDataUriImages = true,
    MaxDataUriImageSizeKb = 512
};

string clean = HtmlSanitizerUtil.SanitizeRich(userInputHtml, options);
```

### 3.2 示例：完全去除 HTML（纯文本摘要）

```csharp
string plain = HtmlSanitizerUtil.SanitizeStrict(richHtml);
// 或者直接用:
string plain2 = richHtml.StripHtml();
```

### 3.3 示例：评论区（允许少量格式）

```csharp
var options = new HtmlSanitizerOptions
{
    AllowedTags = new HashSet<string> { "p", "strong", "em", "br" },
    AllowedAttributes = new HashSet<string>()
};
string safe = HtmlSanitizerUtil.SanitizeRich(comment, options);
```

---

## 四、在 ArticleService 中的调用链

`ArticleService`（`Modules/Cms/Application/Services/ArticleService.cs`）在保存文章前，会对相关字段执行统一清洗：

```csharp
public async Task<ArticleDto> SaveAsync(ArticleDto dto)
{
    // 1. 富文本正文：保留图片、表格、格式化标签
    dto.Content = HtmlSanitizerUtil.SanitizeRich(dto.Content);

    // 2. 摘要：严格模式，仅保留基础文本
    dto.Summary = HtmlSanitizerUtil.SanitizeStrict(dto.Summary);

    // 3. 标题：严格模式（防止标题注入 `<script>`）
    dto.Title   = HtmlSanitizerUtil.SanitizeStrict(dto.Title);

    // 4. 作者、来源等文本字段
    dto.Author  = HtmlSanitizerUtil.SanitizeStrict(dto.Author);

    // 5. 持久化
    var entity = await _repository.InsertOrUpdateAsync(dto);
    return _mapper.Map<ArticleDto>(entity);
}
```

> 关键点：清洗发生在**业务服务层**，所有入口（控制器 / API / 后台任务）只要通过 `ArticleService.SaveAsync` 保存，均被强制清洗。

---

## 五、在前端的再次清洗（深度防御）

XSS 防御是一个**多层防御**的过程：后端清洗是最后一道防线，但前端也应有输入过滤与输出过滤。

### 5.1 富文本编辑器（Vditor）

- Vditor 自带 `irateScript` 模式（类似 Markdown 即时渲染），在粘贴 HTML 时会执行内置过滤。
- 可进一步配置 `options.toolbarConfig.hintConfig` 中禁用的标签。

### 5.2 前端输出过滤（DOMPurify）

当使用 `v-html`、`innerHTML`、`dangerouslySetInnerHTML` 渲染用户提交的 HTML 时：

```js
import DOMPurify from 'dompurify';

// 后端已清洗 → 前端再次清洗（双保险）
const safeHtml = DOMPurify.sanitize(article.content, {
    FORBID_TAGS: ['script', 'style', 'iframe'],
    FORBID_ATTR: ['onerror', 'onload', 'onclick', 'onmouseover']
});
```

### 5.3 内容安全策略（CSP）

在 HTTP 响应头中设置 CSP 进一步限制：

```text
Content-Security-Policy:
  default-src 'self';
  img-src 'self' https://your-cdn.com data:;
  style-src 'self' 'unsafe-inline';
  script-src 'self';
  frame-ancestors 'none';
  base-uri 'self';
  form-action 'self';
```

> `'unsafe-inline'` 对 style 的放宽是富文本场景的常见妥协；但对 script 必须拒绝 inline。

### 5.4 四层防御总结

| 层级 | 措施 | 负责方 |
|------|------|--------|
| 1 | 浏览器自动转义（使用 `{{ }}` / `textContent` 而非 `innerHTML`） | 前端 |
| 2 | 编辑器粘贴过滤 + DOMPurify 二次清洗 | 前端 |
| 3 | 服务端 `HtmlSanitizerUtil` 白名单过滤 | 后端 |
| 4 | HTTP CSP 头禁止脚本 / 限制来源 | 后端（网关） |

---

## 六、常见 XSS 攻击向量与防御示例

| 攻击向量 | 原文 | 被清理后 | 说明 |
|---------|------|---------|------|
| **script 标签注入** | `<script>alert(1)</script>` | `（空）` | `<script>` 被完全移除 |
| **img onerror** | `<img src=x onerror="alert(1)">` | `<img>` | `onerror` 属性被移除 |
| **javascript: 链接** | `<a href="javascript:alert(1)">点我</a>` | `<a href="">点我</a>` | `href` 被清空为安全值 |
| **style expression** | `<p style="expression(alert(1))">hello</p>` | `<p style="">hello</p>` | expression 被移除 |
| **style url(javascript:)** | `<p style="background:url('javascript:alert(1)')">` | `<p style="">` | url(javascript:) 被拒绝 |
| **iframe 嵌入** | `<iframe src="evil.com/phishing.html"></iframe>` | `（空）` | `<iframe>` 被完全移除 |
| **onmouseover** | `<div onmouseover="alert(1)">悬停</div>` | `<div>悬停</div>` | `onmouseover` 属性被移除 |
| **Base64 图片超大** | `<img src="data:image/png;base64,... (10MB)">` | `<img>` 或被删除 | 超过 `MaxDataUriImageSizeKb` 被移除 |
| **SVG 脚本注入** | `<svg><script>alert(1)</script></svg>` | `（空）` | `<svg>` 默认不在白名单 |
| **事件处理属性大小写变种** | `<IMG SRC=X ONERROR=alert(1)>` | `<img>` | 大小写不敏感检测 |
| **CSS 注入窃取数据** | `<style>input[value^="a"]{background:url(...)}</style>` | `（空）` | `<style>` 被完全移除 |
| **HTML 实体编码绕过** | `&#x3C;script&#x3E;alert(1)&#x3C;/script&#x3E;` | 文本原样显示（不解析为标签） | 先解码再过滤 |

---

## 七、防御清单（开发自检）

1. [ ] 所有来自用户的 HTML 输入在保存前经过 `HtmlSanitizerUtil.SanitizeRich` / `SanitizeStrict`。
2. [ ] 标题、摘要、作者等非富文本字段使用 `SanitizeStrict`。
3. [ ] 允许的标签列表不包含 `script`、`iframe`、`object`、`embed`、`style`、`form`。
4. [ ] 允许的属性列表不包含任何 `on*` 事件处理器。
5. [ ] URL 协议仅限于 `http`、`https`、`mailto`；禁止 `javascript:` / `vbscript:`。
6. [ ] Base64 图片大小受限（或直接禁用，仅允许上传到 CDN）。
7. [ ] 外部图片域名使用白名单（避免图片请求泄露用户 Cookie / Referer）。
8. [ ] 前端 `v-html` 渲染前再次经过 DOMPurify。
9. [ ] HTTP 响应头包含合理 CSP。
10. [ ] 对 `<a>` 标签自动注入 `target="_blank"` + `rel="noopener noreferrer"`。

---

> **相关文件**
> - `src/JeeSiteNET.Core/Utils/HtmlSanitizerUtil.cs`
> - `src/JeeSiteNET.Modules.Cms/Application/Services/ArticleService.cs`
