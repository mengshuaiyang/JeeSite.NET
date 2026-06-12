<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# CMS内容管理

> 站点、栏目、文章、标签、评论、留言、举报、访问统计等内容管理功能。
>
> **适用角色**：内容运营、开发人员
> **阅读时间**：约 12 分钟
> **相关文档**：[25-Vditor编辑器](25-Vditor编辑器) · [22-Elasticsearch](22-Elasticsearch)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、模块概述](#一、模块概述)
  - [1.1 模块定位](#11-模块定位)
  - [1.2 与其他模块的依赖关系](#12-与其他模块的依赖关系)
- [二、实体类清单](#二、实体类清单)
- [三、服务层清单](#三、服务层清单)
  - [3.1 文章核心服务](#31-文章核心服务)
  - [3.2 权限服务](#32-权限服务)
  - [3.3 搜索与向量库](#33-搜索与向量库)
  - [3.4 栏目与站点](#34-栏目与站点)
  - [3.5 互动、访问日志与文件模板](#35-互动、访问日志与文件模板)
  - [3.6 AI 智能问答](#36-ai-智能问答)
- [四、控制器与 API](#四、控制器与-api)
  - [4.1 管理端核心端点](#41-管理端核心端点)
  - [4.2 前台端点](#42-前台端点)
- [五、前端页面](#五、前端页面)
  - [5.1 管理端（frontend/src/views/cms）](#51-管理端（frontend-src-views-cms）)
  - [5.2 前台（frontend/src/views/cms-front）](#52-前台（frontend-src-views-cms-front）)
- [六、内容安全与 XSS 防护](#六、内容安全与-xss-防护)
  - [6.1 HTML 清洗调用链](#61-html-清洗调用链)
  - [6.2 文件上传安全](#62-文件上传安全)
  - [6.3 评论反垃圾](#63-评论反垃圾)
- [七、AI 智能问答与向量库](#七、ai-智能问答与向量库)
  - [7.1 流程（RAG 两阶段）](#71-流程（rag-两阶段）)
  - [7.2 关键服务职责](#72-关键服务职责)
  - [7.3 工具（Tool）扩展](#73-工具（tool）扩展)
  - [7.4 会话与会话缓存](#74-会话与会话缓存)
- [八、扩展与二次开发](#八、扩展与二次开发)
  - [8.1 新增栏目类型/模板](#81-新增栏目类型-模板)
  - [8.2 新增文章自定义字段](#82-新增文章自定义字段)
  - [8.3 接入真实向量库](#83-接入真实向量库)
  - [8.4 接入外部大模型](#84-接入外部大模型)
  - [8.5 扩展文章权限（付费阅读）](#85-扩展文章权限（付费阅读）)
  - [8.6 自定义推荐位](#86-自定义推荐位)
  - [8.7 自定义搜索引擎](#87-自定义搜索引擎)

---


## 一、模块概述

### 1.1 模块定位

Cms 模块是 JeeSite.NET 的内容管理子系统，提供"文章—栏目—站点"的三级内容结构以及评论、留言、标签、举报、访问日志、文件模板、AI 智能问答与向量库等能力。它基于 Sys 模块的用户体系与权限模型构建，面向内容编辑、运营人员以及终端读者。

核心能力：

- **内容生产**：文章（`Article`）+ 正文数据（`ArticleData`）+ 推荐位（`ArticlePosId`）+ 标签（`ArticleTag`）。
- **组织架构**：栏目（`Category`）树 + 站点（`Site`），支持多站点与自定义模板。
- **互动与风控**：评论（`Comment`）、留言（`Guestbook`）、举报（`Report`）、访问日志（`VisitLog`）。
- **内容安全**：HTML 白名单清洗（`HtmlSanitizerUtil`）、文件上传安全、评论反垃圾。
- **AI 智能问答**：基于文章内容的检索-生成（RAG）两阶段问答，核心由 `ArticleIndexService` / `ArticleVectorStore` / `AiChatService` 组成，并提供 `DateTimeTool`、`WeatherTool`、`CmsSearchTool` 等插件式工具。

### 1.2 与其他模块的依赖关系

```
JeeSiteNET.Modules.Cms
  ├─ 依赖 JeeSiteNET.Modules.Sys（用户/角色/日志/文件/消息/权限）
  └─ 依赖 JeeSiteNET.Core（仓储、实体基类、IdGenerator、ApiResult 等）
```

对外暴露：
- 管理端 API：`api/v1/cms/article`、`category`、`site`、`cms`（评论/留言/标签/访问日志/举报）、`file-template`、`ai`。
- 前台 API：`api/v1/cms/front/*`（文章列表、详情、搜索、站点信息、标签云）。
- 前端页面：`views/cms/*`（管理端）+ `views/cms-front/*`（前台）。

## 二、实体类清单

所有实体位于 `JeeSiteNET.Modules.Cms.Domain.Entities`。每个实体列出主键 + 3-5 个核心字段。

| 实体 | 主键 | 核心字段 |
|------|------|----------|
| **Article** | `ArticleCode` | `CategoryCode`（所属栏目）、`Title`（标题）、`Summary`、`IsTop`（是否置顶）、`IsRecommend`、`ClickCount`、`PublishDate`、`CorpCode` |
| **ArticleData** | 与 `ArticleCode` 1:1 | `Content`（富文本正文）、`Relation`（相关文章）、`IsCanComment`、`ExtendS1~4` |
| **ArticlePosId** | 无独立主键 | `ArticleCode` + `PosId`（推荐位编号，用于首页轮播/焦点图等） |
| **ArticleTag** | 无独立主键 | `ArticleCode` + `TagName`（文章-标签多对多） |
| **Category** | `CategoryCode` | `CategoryName`、`CategoryType`、`ParentCode`（树）、`SiteCode`、`IsCanComment`、`InMenu`、`CustomListView`、`CustomContentView`、`CorpCode` |
| **Site** | `SiteCode` | `SiteName`、`Domain`、`Theme`、`Keywords`、`Description`、`Copyright`、`CustomIndexView`、`CorpCode` |
| **Tag** | `TagName`（本身即主键） | `ClickNum`（标签点击量，用于标签云排序） |
| **Comment** | `CommentCode` | `CategoryCode`、`ArticleCode`、`ParentCode`、`ArticleTitle`、`Content`、`Name`、`Ip`、`AuditUserCode`、`AuditDate`、`HitsPlus/HitsMinus`、`CorpCode` |
| **Guestbook** | `GbCode` | `GbType`、`Content`、`Name`、`Email`、`Phone`、`WorkUnit`、`Ip`、`ReUserCode`、`ReDate`、`ReContent`、`CorpCode` |
| **Report** | `ReportCode` | `ArticleCode`、`ArticleTitle`、`ReportType`、`Content`、`Ip`、`DealUserCode`、`DealDate`、`DealResult`、`CorpCode` |
| **VisitLog** | `VisitId` | `RequestUrl`、`SourceReferer`、`SourceType`、`SearchEngine`、`SearchWord`、`RemoteAddr`、`UserAgent`、`UserOsName`、`UserBrowser`、`UniqueVisitId`、`VisitDate`、`IsNewVisit`、`SiteCode`、`CategoryCode`、`ContentId`、`VisitUserCode`、`CorpCode` |
| **FileTemplate** | `FileTemplateCode`（若使用，见 FileTemplateController） | `TemplateName`、`TemplatePath`、`Content`、`ContentType`（HTML 模板，用于动态渲染栏目/文章页） |

> **主键策略**：与 Sys 模块一致，使用 `string` 型业务编码（`ArticleCode`/`CategoryCode`/`SiteCode`/`CommentCode` 等），由 `IdGenerator.NewId()` 生成。关系表 `ArticlePosId`、`ArticleTag` 使用组合外键。

## 三、服务层清单

所有服务位于 `JeeSiteNET.Modules.Cms.Application.Services`（AI 工具位于 `Application/AiTools`）。

### 3.1 文章核心服务

| 服务 | 方法签名 | 作用 |
|------|---------|------|
| **ArticleService** | `GetAsync(string articleCode) -> ArticleDto?` | 按 ArticleCode 读取文章详情（含 `ArticleData.Content`） |
| | `FindPageAsync(PageRequest<Article> request) -> PageResult<ArticleDto>` | 分页查询（栏目/站点/关键词过滤） |
| | `SaveAsync(ArticleSaveDto dto) -> ApiResult` | 新增/更新文章；自动清洗 HTML 内容、维护推荐位与标签、刷新缓存 |
| | `DeleteAsync(string articleCode) -> ApiResult` | 级联删除文章、正文、推荐位、标签 |
| | `RecordClickAsync(string articleCode) -> ApiResult` | 记录点击（用于 `ClickCount` 统计） |
| | `GetTagCloudAsync() -> List<TagDto>` | 返回全局标签云（按 `ClickNum` 排序） |

### 3.2 权限服务

| 服务 | 方法签名 | 作用 |
|------|---------|------|
| **ArticleAuthService**<br>（`IArticleAuthService`） | `CanViewAsync(string articleCode, string? userCode) -> bool` | 判断是否有权限阅读（默认 `true`，可替换为会员/付费逻辑） |
| | `CanEditAsync(string articleCode, string? userCode) -> bool` | 判断是否可编辑（默认 `false`，需编辑员/管理员角色） |

### 3.3 搜索与向量库

| 服务 | 方法签名 | 作用 |
|------|---------|------|
| **ArticleIndexService**<br>（`IArticleIndexService`） | `IndexAsync(Article article) -> Task` | 新增/更新文章时将元数据写入搜索引擎索引（默认空实现，可接 Elasticsearch/Lucene） |
| | `RemoveAsync(string articleCode) -> Task` | 删除索引中的文章 |
| **ArticleVectorStore**<br>（`IArticleVectorStore`） | `GetVectorAsync(string articleCode) -> ReadOnlyMemory<float>?` | 根据文章 ID 返回向量嵌入，供 RAG 召回阶段使用 |

### 3.4 栏目与站点

| 服务 | 方法签名 | 作用 |
|------|---------|------|
| **CategoryService** | `GetBySiteAsync(string siteCode) -> List<CategoryDto>` | 读取某站点所有栏目 |
| | `GetAllAsync() -> List<CategoryDto>` | 读取所有栏目（用于管理端） |
| | `FindTreeAsync() -> List<CategoryDto>` | 构造完整栏目树 |
| | `GetAsync(string categoryCode) -> CategoryDto?` | 栏目详情 |
| | `SaveAsync(CategorySaveDto dto) -> ApiResult` | 新增/更新栏目（维护 `ParentCode` 与树结构） |
| | `DeleteAsync(string categoryCode) -> ApiResult` | 删除栏目（递归校验无子文章） |
| **SiteService** | `GetAllAsync() -> List<SiteDto>` | 全部站点列表 |
| | `GetAsync(string siteCode) -> SiteDto?` | 站点详情（含主题、域名、SEO 信息） |
| | `SaveAsync(SiteSaveDto dto) -> ApiResult` | 新增/更新站点 |
| | `DeleteAsync(string siteCode) -> ApiResult` | 删除站点 |

### 3.5 互动、访问日志与文件模板

| 服务 | 方法签名 | 作用 |
|------|---------|------|
| **CmsService** | `FindCommentPageAsync(PageRequest<Comment>) -> PageResult<CommentDto>` | 评论分页（支持按文章/栏目过滤） |
| | `SaveCommentAsync(CommentSaveDto dto) -> ApiResult` | 发布评论（反垃圾 + HTML 清洗） |
| | `AuditCommentAsync(string commentCode, string status, string? auditComment) -> ApiResult` | 审核评论 |
| | `DeleteCommentAsync(string commentCode) -> ApiResult` | 删除评论 |
| | `FindGuestbookPageAsync(PageRequest<Guestbook>) -> PageResult<GuestbookDto>` | 留言分页 |
| | `SaveGuestbookAsync(GuestbookSaveDto dto) -> ApiResult` | 提交留言 |
| | `ReplyGuestbookAsync(string gbCode, string reContent) -> ApiResult` | 回复留言 |
| | `DeleteGuestbookAsync(string gbCode) -> ApiResult` | 删除留言 |
| | `GetAllTagsAsync() -> List<TagDto>` | 读取全部标签 |
| | `SaveTagAsync(string tagName) -> ApiResult` | 新增标签 |
| | `DeleteTagAsync(string tagName) -> ApiResult` | 删除标签 |
| | `AddVisitLogAsync(VisitLog log) -> ApiResult` | 写入访问日志（客户端指纹 + 搜索引擎解析） |
| | `FindVisitLogPageAsync(PageRequest<VisitLog>) -> PageResult<VisitLogDto>` | 访问日志分页 |
| | `FindReportPageAsync(PageRequest<Report>) -> PageResult<ReportDto>` | 举报分页 |
| | `SaveReportAsync(ReportSaveDto dto) -> ApiResult` | 提交举报 |
| | `DealReportAsync(string reportCode, string dealResult) -> ApiResult` | 处理举报 |
| | `DeleteReportAsync(string reportCode) -> ApiResult` | 删除举报 |
| | `GetTodayVisitCountAsync() -> long` | 今日访问量（快速统计） |
| **PageCacheService** | `ClearArticleCacheAsync(Article article) -> Task` | 清除单篇文章缓存键 |
| | `ClearCategoryCacheAsync(Category category) -> Task` | 清除栏目页缓存 |
| | `ClearSiteCacheAsync(Site site) -> Task` | 清除站点级缓存 |
| | `ClearAllAsync() -> Task` | 清理全站页面缓存 |

### 3.6 AI 智能问答

| 服务/工具 | 方法签名 | 作用 |
|----------|---------|------|
| **AiChatService** | `ChatAsync(AiChatRequest request) -> AiChatResponse` | 核心对话入口：问题理解 → 工具调用/检索 → 生成回答 |
| | `ChatJsonAsync(AiChatRequest request) -> ApiResult<JsonElement?>` | 返回 JSON 结构的工具调用结果 |
| | `ChatEntityAsync(AiChatEntityRequest request) -> ApiResult<JsonElement?>` | 按实体类型执行结构化问答 |
| | `BuildContextAsync(string query, string? categoryCode, int maxArticles) -> (Summary, List<string> Titles)` | 内部方法：召回并拼装 RAG 上下文 |
| | `ExecuteToolAsync(string toolName, string argumentsJson) -> AiToolExecution` | 调用注册工具（DateTime/Weather/Search） |
| **CmsSearchTool** | `InvokeAsync(string query, int limit) -> string` | 在文章内容中做关键词检索（轻量版向量库替代） |
| **DateTimeTool** | `InvokeAsync(string format) -> string` | 返回当前时间/日期字符串，支持格式化 |
| **WeatherTool** | `InvokeAsync(string city) -> string` | 调用外部天气 API，用于回答"明天北京是否下雨"等 |

## 四、控制器与 API

所有控制器位于 `JeeSiteNET.Modules.Cms.Controllers`。

### 4.1 管理端核心端点

| 控制器 | Route | 核心 HTTP 方法 |
|--------|-------|---------------|
| **ArticleController** | `api/v1/cms/article` | `POST /list`（分页，支持关键词/栏目/站点过滤）、`GET /get?articleCode=`（含正文）、`POST /save`（新增/更新）、`POST /delete`、`GET /click?articleCode=`（点击计数） |
| **CategoryController** | `api/v1/cms/category` | `GET /list`、`GET /tree`、`GET /get?categoryCode=`、`POST /save`、`POST /delete` |
| **SiteController** | `api/v1/cms/site` | `POST /list`、`GET /get?siteCode=`、`POST /save`、`POST /delete` |
| **CmsController** | `api/v1/cms` | `POST /comment/list`、`POST /comment/save`、`POST /comment/audit?commentCode=&status=`、`POST /comment/delete`、`POST /guestbook/list`、`POST /guestbook/save`、`POST /guestbook/reply?gbCode=&reContent=`、`POST /guestbook/delete`、`GET /tag/list`、`POST /tag/save?tagName=`、`POST /tag/delete`、`POST /visit-log/add`、`POST /visit-log/list`、`GET /today-visits`、`POST /report/save`、`POST /report/list`、`POST /report/deal?reportCode=&dealResult=`、`POST /report/delete` |
| **FileTemplateController** | `api/v1/cms/file-template` | `GET /list`、`GET /get?code=`、`POST /save`、`POST /delete`（维护 HTML 页面模板） |
| **AiChatController** | `api/v1/cms/ai` | `POST /chat`、`GET /stream?message=&categoryCode=`、`POST /json`（工具调用）、`POST /entity`（实体问答） |

### 4.2 前台端点

| 控制器 | Route | 核心 HTTP 方法 |
|--------|-------|---------------|
| **CmsFrontController** | `api/v1/cms/front` | `GET /sites`（站点列表）、`GET /categories?siteCode=`（某站点栏目）、`POST /article/list`（文章分页列表）、`GET /article/get?articleCode=`（文章详情 + 阅读计数）、`POST /article/search?keyword=`（全文检索）、`GET /tag-cloud`（标签云） |

> 所有前台端点默认匿名访问，可在中间件中按 `SiteCode`/`CategoryCode` 做权限控制；如需"付费阅读"，可在 `ArticleAuthService` 中替换默认实现。

## 五、前端页面

### 5.1 管理端（`frontend/src/views/cms`）

| 页面文件 | 说明 |
|---------|------|
| `ArticleList.vue` | 文章管理（表格 + 筛选 + 批量操作）；点击"编辑"进入 `ArticleEdit.vue` |
| `ArticleEdit.vue` | 富文本编辑器 + 元信息（标题/作者/栏目/标签/置顶/推荐/发布日期），保存调用 `POST /api/v1/cms/article/save` |
| `CategoryList.vue` | 栏目管理（树形结构，支持拖拽与增删改） |
| `CommentList.vue` | 评论管理（按文章/状态筛选，支持审核与删除） |
| `GuestbookList.vue` | 留言管理（回复/删除/导出） |
| `FileTemplateList.vue` | 文件模板管理（编辑 HTML/Markdown 模板片段，供前台动态渲染使用） |

### 5.2 前台（`frontend/src/views/cms-front`）

| 页面文件 | 说明 |
|---------|------|
| `CmsSite.vue` | 站点首页（按 `Site.CustomIndexView` 与栏目树加载；展示最新文章/推荐文章） |
| `CmsArticleList.vue` | 栏目文章列表（按 `CategoryCode` 分页，调用 `POST /api/v1/cms/front/article/list`） |
| `CmsArticleDetail.vue` | 文章详情页（渲染 `ArticleData.Content`、展示标签/相关文章、评论框与点赞按钮） |
| `CmsSearch.vue` | 站内搜索（关键词过滤 + 标签云辅助入口，调用 `POST /api/v1/cms/front/article/search`） |

> 前台页面通常部署为独立路由子树，通过 `SiteCode` 切换多站点。

## 六、内容安全与 XSS 防护

### 6.1 HTML 清洗调用链

文章保存时会对 `ArticleData.Content` 做**白名单式 HTML 清洗**，核心调用链：

```
ArticleController.save(ArticleSaveDto dto)
  └─ ArticleService.SaveAsync(dto)
        ├─ if (!string.IsNullOrEmpty(dto.Content))
        │     dto.Content = HtmlSanitizerUtil.Sanitize(dto.Content);
        ├─ 保存 Article 基础信息
        ├─ 保存 ArticleData（清洗后的 Content）
        ├─ 维护 ArticlePosId / ArticleTag
        └─ PageCacheService.ClearArticleCacheAsync(article)
```

其中 `HtmlSanitizerUtil` 的白名单配置：

- **允许的 HTML 标签**：`p, div, span, strong, em, b, i, u, br, hr, ul, ol, li, blockquote, pre, code, h1~h6, a, img, table, thead, tbody, tfoot, tr, th, td, figure, figcaption, video, audio`
- **允许的属性**：`href, src, alt, title, target, style, class, width, height`（对 `href`/`src` 限制为 `http/https/data/相对路径`，阻止 `javascript:` 协议）
- **删除的危险元素**：`script, iframe, object, embed, form, input, button, style, link, meta, base`
- **内联样式白名单**：`color, background-color, font-size, font-weight, text-align, margin, padding, border, width, height`

### 6.2 文件上传安全

文件上传统一走 Sys 模块的 `FileController`：

- 扩展名白名单：图片（`jpg/jpeg/png/gif/webp/svg`）、音视频（`mp4/webm/mp3/wav`）、文档（`pdf/doc/docx/ppt/pptx/xls/xlsx/txt/md/zip/rar`）。
- MIME 类型与扩展名一致性校验。
- 最大单文件大小：`50MB`（可由配置 `FileUpload:MaxSizeBytes` 覆盖）。
- 防执行：上传目录不映射为可执行路径；图片/文档通过 `PreviewController` 以"预览"模式提供访问，避免浏览器执行。
- 上传后记录 `bizType` / `bizKey`，便于追溯。

### 6.3 评论反垃圾

发布评论时调用 `CmsService.SaveCommentAsync`，依次触发以下检查：

1. **频率限制**：同一 `Ip` 在 60 秒内最多提交 3 条评论，超限时返回 `ApiResult.Fail(429, "评论过于频繁")`。
2. **敏感词过滤**：使用 `SensitiveWordUtil.Match(content)` 检测命中词并自动打标，命中 ≥ 2 条自动设置 `AuditStatus = "pending"`，进入人工审核。
3. **URL 数量限制**：单条评论中外部链接数量超过 3 条视为广告，同样进入待审核队列。
4. **内容最小/最大长度**：`2 ≤ content.Length ≤ 1000`。
5. **清洗**：保存前使用 `HtmlSanitizerUtil.Sanitize(content)` 去除富文本，仅保留纯文本与极少的安全标签。
6. **HTML 编码呈现**：在 `CommentList.vue` 中对评论内容使用 `v-html` + `DOMPurify` 二次清洗。

## 七、AI 智能问答与向量库

Cms 模块内置一套轻量 RAG 流程，支持"按文章内容问答"与"按栏目过滤问答"两种模式。

### 7.1 流程（RAG 两阶段）

```
用户问题 (AiChatRequest.message)
  │
  ▼
AiChatService.ChatAsync
  │
  ├─ [阶段一：检索 Retrieve]
  │     ├─ 1) 调用 ArticleIndexService 做关键词检索（默认 EF Core IArticleRepository 过滤）
  │     ├─ 2) 调用 ArticleVectorStore.GetVectorAsync / 相似度 Top-K 召回（若接入向量库）
  │     └─ 3) 调用 CmsSearchTool.InvokeAsync(query, limit) 做补充关键词检索
  │
  ├─ [构建上下文] BuildContextAsync(query, categoryCode, maxArticles)
  │     └─ 对召回的 ArticleDto.Title/摘要按得分排序并拼接成 Prompt 文本
  │
  └─ [阶段二：生成 Generate]
        └─ 以"系统 Prompt + 检索上下文 + 用户问题"调用 LLM，返回纯文本 / JSON
             （默认实现为本地规则式生成，可替换为 SemanticKernel / 本地大模型）
```

### 7.2 关键服务职责

- **ArticleIndexService**：文章索引抽象层；默认空实现，便于替换为 Elasticsearch、Lunr.js 服务端或 EF Core 的 `Where(...).ToListAsync`。
- **ArticleVectorStore**：向量读取；默认返回 `null`，可实现为 Qdrant/Weaviate/Milvus 或 `Microsoft.ML.OnnxRuntime` 的本地嵌入层。
- **AiChatService**：核心编排器：
  - 路由用户问题到不同工具（通过 `request.Tools` 指定或基于关键词自动选择）；
  - 支持 `categoryCode` 过滤（仅在某栏目内问答）；
  - 内置简单对话状态（`AiChatRequest.SessionId` → 缓存最近 10 条）。

### 7.3 工具（Tool）扩展

在 `Application/AiTools/` 目录下实现 `ITool` 接口（统一 `InvokeAsync(string argsJson) -> string`）：

- **DateTimeTool**：回答时间/日期相关问题。
- **WeatherTool**：回答天气相关问题（需配置天气 API Key）。
- **CmsSearchTool**：对站内文章做关键词/标签联合检索。

新增工具的 3 步标准路径：

```csharp
// 1) 在 Application/AiTools 下新建类并实现 ITool 接口
public class YourTool : ITool
{
    public string Name => "your_tool";
    public Task<string> InvokeAsync(string argsJson) { ... }
}

// 2) 在 CmsModuleInstaller / Program.cs 注册
services.AddScoped<ITool, YourTool>();

// 3) 在 AiChatService 的工具路由表中追加
//    var tool = serviceProvider.GetServices<ITool>().FirstOrDefault(t => t.Name == toolName);
```

### 7.4 会话与会话缓存

- `AiChatRequest.SessionId` 唯一标识一次会话；
- 使用 `IFusionCache`（来自 `JeeSiteNET.Core`）缓存最近 10 轮 `user/assistant` 消息，TTL 默认 30 分钟；
- 超过 TTL 的会话自动创建新上下文，保证上下文长度可控。

## 八、扩展与二次开发

### 8.1 新增栏目类型/模板

- 在 `DictData` 字典类型 `cms_category_type` 中新增一条字典项（如 `video = "视频栏目"`）。
- 在 `Category.CustomContentView` 中配置前台渲染模板路径；
- 在 `views/cms-front/` 下新增对应的 `VideoCategory.vue`，通过 `CmsFrontController` 的 `categoryCode` 动态匹配路由。

### 8.2 新增文章自定义字段

- `ArticleData` 自带 `ExtendS1~4`、`ExtendI1~4`、`ExtendF1~4`、`ExtendD1~4`、`ExtendJson` 等扩展字段；
- 在 `ArticleSaveDto` / `ArticleDto` 中新增对应字段映射；
- 在 `ArticleEdit.vue` 增加表单控件，并在 `CmsArticleDetail.vue` 中按模板展示。

### 8.3 接入真实向量库

- 将 `DefaultArticleVectorStore` 替换为自己的实现（Qdrant / Milvus / Redis-Search）；
- 在 `ArticleService.SaveAsync` 中追加 `_articleVectorStore.UpsertAsync(article)`（建议新增接口，避免与默认只读签名冲突）；
- 删除文章时调用 `_articleVectorStore.RemoveAsync(articleCode)` 清除。

### 8.4 接入外部大模型

- 在 `AiChatService.SendAndProcessAsync`（私有方法）中替换默认"本地规则生成"，改为调用 `HttpClient` 访问 OpenAI/阿里通义/本地 Ollama：
  ```csharp
  var body = new { model, messages = new[] { ... } };
  var response = await _httpClient.PostAsJsonAsync(url, body);
  ```
- 在 `appsettings.json` 增加 `Cms:Ai:Endpoint`、`Cms:Ai:ApiKey`、`Cms:Ai:ModelName` 等配置项。

### 8.5 扩展文章权限（付费阅读）

- 替换 `DefaultArticleAuthService` 为自己的实现：读取当前 `userCode` 的会员状态与文章付费标记；
- 在 `CmsFrontController.ArticleGet` 中调用 `await _auth.CanViewAsync(articleCode, userCode)` 控制是否返回完整 `Content` 或仅摘要。

### 8.6 自定义推荐位

- `ArticlePosId` 为 `ArticleCode`+`PosId` 组合主键，可作为首页不同位置（轮播、焦点、专题）的内容来源；
- 在 `ArticleService.SaveAsync` 已内置对 `dto.PosIds` 的 upsert/delete，前端在 `ArticleEdit.vue` 增加多选框即可。

### 8.7 自定义搜索引擎

- `SearchController`（Sys 模块）提供 `POST /api/v1/sys/search`，可在 `SearchService` 中注入 `IArticleRepository` 作为 CMS 内容的检索源；
- 同时 `AiChatService.CmsSearchTool` 可独立作为 AI 问答的检索插件被调用。
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---

## 💡 快速参考

| 项目 | 关键信息 |
|------|---------|
| **模块名称** | CMS内容管理 |
| **最后更新** | 2026-06-13 |
| **相关文档** | [25-Vditor编辑器](25-Vditor编辑器) · [22-Elasticsearch](22-Elasticsearch) |

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>