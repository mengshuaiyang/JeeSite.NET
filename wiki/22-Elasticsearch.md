<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# Elasticsearch

> 基于 Elasticsearch 的全文搜索方案，IK 中文分词、向量相似度、聚合分析、零停机索引重建。
>
> **适用角色**：全栈开发人员
> **阅读时间**：约 10 分钟
> **相关文档**：[20-AI智能问答](20-AI智能问答) · [23-FusionCache缓存](23-FusionCache缓存)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、概述](#一、概述)
  - [为什么需要 Elasticsearch](#为什么需要-elasticsearch)
  - [JeeSite.NET 中 ES 的使用场景](#jeesitenet-中-es-的使用场景)
- [二、Elasticsearch 配置（appsettings.json）](#二、elasticsearch-配置（appsettingsjson）)
- [三、索引结构示例](#三、索引结构示例)
  - [索引 Mapping](#索引-mapping)
  - [字段设计说明](#字段设计说明)
- [四、核心服务：SearchService](#四、核心服务：searchservice)
  - [SearchAsync 详细参数](#searchasync-详细参数)
  - [返回结构](#返回结构)
- [五、索引管理策略](#五、索引管理策略)
  - [5.1 实时索引（主路径）](#51-实时索引（主路径）)
  - [5.2 定时增量索引](#52-定时增量索引)
  - [5.3 全量重建（管理员手动触发）](#53-全量重建（管理员手动触发）)
  - [5.4 冷热分离](#54-冷热分离)
- [六、IK 中文分词](#六、ik-中文分词)
  - [两种分词器对比](#两种分词器对比)
  - [自定义词库](#自定义词库)
  - [同义词](#同义词)
- [七、CMS 前端搜索集成](#七、cms-前端搜索集成)
  - [页面：frontend/src/views/cms-front/Search.vue](#页面：frontend-src-views-cms-front-searchvue)
  - [接口：GET /api/v1/cms/article/search](#接口：get-api-v1-cms-article-search)
  - [热门搜索词（Redis ZSET）](#热门搜索词（redis-zset）)
- [八、索引监控与优化](#八、索引监控与优化)
  - [8.1 Kibana Dashboard](#81-kibana-dashboard)
  - [8.2 索引模板](#82-索引模板)
  - [8.3 Shards / Replicas 建议](#83-shards-replicas-建议)
  - [8.4 常见问题排查](#84-常见问题排查)
- [九、Elasticsearch 启动（Docker）](#九、elasticsearch-启动（docker）)
  - [触发全量重建](#触发全量重建)

---


> 当 CMS 文章量超过 10 万篇时，数据库 `LIKE` 查询效率不足。JeeSite.NET 采用 **Elasticsearch 8.x** 作为全文搜索引擎，结合 **IK 中文分词**提供快速、相关的搜索能力。

---

## 一、概述

### 为什么需要 Elasticsearch

| 方案 | 10 万篇查询耗时 | 100 万篇查询耗时 | 中文分词 | 相关度评分 | 聚合分析 |
|------|-----------------|-------------------|---------|-----------|---------|
| 数据库 `LIKE '%key%'` | 200 ~ 800 ms | 2 ~ 5 s | ❌ | ❌ | ❌ |
| PostgreSQL `to_tsvector` | 50 ~ 200 ms | 300 ~ 800 ms | ⚠️（需额外插件） | ✅ | ⚠️ |
| **Elasticsearch** | **5 ~ 30 ms** | **10 ~ 80 ms** | ✅（IK） | ✅（BM25） | ✅（aggs） |

### JeeSite.NET 中 ES 的使用场景

1. **CMS 文章搜索**：用户在前台输入关键词 → Elasticsearch 全文检索。
2. **后台全局搜索**：管理员搜索用户、角色、菜单、文章。
3. **搜索建议**：输入框下拉 suggest 提示。
4. **热点统计**：按搜索词聚合出热门搜索榜。
5. **日志分析**：（可选）将 `sys_log` 写入 ES，做日志可视化。

---

## 二、Elasticsearch 配置（appsettings.json）

```json
{
  "Elasticsearch": {
    "Enabled": true,
    "Nodes": ["http://localhost:9200"],
    "DefaultIndex": "jeesite-articles",
    "Username": "elastic",
    "Password": "changeme",
    "EnableApiVersioningHeader": true,
    "DisableDirectStreaming": false,
    "RequestTimeoutSeconds": 30,
    "SniffOnStartup": false,
    "RefreshInterval": "30s"
  }
}
```

| 配置项 | 说明 |
|--------|------|
| `Enabled` | 是否启用 Elasticsearch。禁用时搜索回退到数据库 `LIKE` 查询 |
| `Nodes` | ES 集群节点列表，可配置多个做负载均衡 |
| `DefaultIndex` | CMS 文章默认索引名 |
| `Username` / `Password` | 集群凭据（ES 8.x 默认启用 basic auth + https） |
| `RefreshInterval` | 索引刷新间隔。生产设 `30s` 提升写入性能，开发可设 `1s` |
| `RequestTimeoutSeconds` | 单次请求超时时间 |

---

## 三、索引结构示例

索引名：`jeesite-articles`（别名 `jeesite-articles-read` / `jeesite-articles-write`，用于零停机重建）

### 索引 Mapping

```json
{
  "mappings": {
    "properties": {
      "articleId":    { "type": "keyword" },
      "title":        { "type": "text", "analyzer": "ik_max_word", "search_analyzer": "ik_smart", "fields": { "raw": { "type": "keyword" } } },
      "summary":      { "type": "text", "analyzer": "ik_max_word" },
      "content":      { "type": "text", "analyzer": "ik_max_word" },
      "categoryCode": { "type": "keyword" },
      "categoryPath": { "type": "keyword" },
      "tags":         { "type": "keyword" },
      "author":       { "type": "keyword" },
      "isPublished":  { "type": "boolean" },
      "publishDate":  { "type": "date", "format": "yyyy-MM-dd HH:mm:ss||epoch_millis" },
      "viewCount":    { "type": "integer" },
      "corpCode":     { "type": "keyword" }
    }
  },
  "settings": {
    "number_of_shards": 3,
    "number_of_replicas": 1,
    "refresh_interval": "30s",
    "analysis": {
      "analyzer": {
        "ik_max_word": { "type": "custom", "tokenizer": "ik_max_word" },
        "ik_smart":    { "type": "custom", "tokenizer": "ik_smart" }
      }
    }
  }
}
```

### 字段设计说明

- **`articleId` / `categoryCode` / `tags` / `author` / `corpCode`**：`keyword` 类型，精确匹配，用于过滤与聚合。
- **`title` / `summary` / `content`**：`text` 类型，`ik_max_word` 索引分词、`ik_smart` 搜索分词；`title.raw` 额外提供 keyword 精确匹配。
- **`publishDate`**：`date` 类型，支持范围查询（最近 7 天 / 最近一月）。
- **`viewCount`**：`integer` 类型，可用于按浏览量排序或做"热门文章"。
- **`isPublished`**：`boolean`，搜索时默认仅返回已发布。

---

## 四、核心服务：SearchService

位置：`modules/JeeSiteNET.Modules.Cms/Services/SearchService.cs`

| 方法 | 签名 | 说明 |
|------|------|------|
| `IndexAsync(article)` | `Task IndexAsync(Article article)` | 单篇文章写入/更新索引 |
| `IndexBatchAsync(articles)` | `Task IndexBatchAsync(List<Article> articles)` | 批量索引（Bulk API） |
| `DeleteIndexAsync(articleId)` | `Task DeleteIndexAsync(string articleId)` | 根据 ID 删除文档 |
| `SearchAsync(query, pageIndex, pageSize, filters)` | `Task<SearchResult<ArticleSearchItem>> SearchAsync(...)` | 全文搜索 |
| `RebuildIndexAsync()` | `Task RebuildIndexAsync()` | 清空并重建整个索引 |
| `GetSuggestionsAsync(prefix)` | `Task<List<string>> GetSuggestionsAsync(string prefix)` | 搜索建议（Suggester） |

### SearchAsync 详细参数

```csharp
public class ArticleSearchQuery
{
    public string Keyword   { get; set; }   // 搜索词
    public string Category  { get; set; }   // 栏目代码（可选）
    public string Tag       { get; set; }   // 标签（可选）
    public string Author    { get; set; }   // 作者（可选）
    public DateTime? DateFrom { get; set; } // 发布起始日期
    public DateTime? DateTo   { get; set; } // 发布结束日期
    public int  PageIndex   { get; set; } = 1;
    public int  PageSize    { get; set; } = 10;
    public string SortBy    { get; set; } = "relevance";   // relevance | date | views
    public bool EnableHighlight { get; set; } = true;
}
```

### 返回结构

```csharp
public class SearchResult<T>
{
    public long Total { get; set; }
    public List<T> Items { get; set; }
    public Dictionary<string, long> Aggregations { get; set; }   // 按栏目/标签聚合
    public List<string> Suggestions { get; set; }                // 相关搜索词
    public double LatencyMs { get; set; }                        // ES 内部耗时
}
```

---

## 五、索引管理策略

### 5.1 实时索引（主路径）

- 文章新增 / 修改时，`ArticleService` 通过 `SearchService.IndexAsync(article)` 同步写入 ES。
- 文章删除时，调用 `SearchService.DeleteIndexAsync(articleId)`。
- **注意**：ES 文档写入到可搜索之间存在 `refresh_interval`（默认 30s），用户搜索时可能短暂滞后。

### 5.2 定时增量索引

- 每日 **02:00** 执行 `SearchService.IndexBatchAsync(last24HoursArticles)`。
- 扫描最近 24 小时修改过、但 `update_date` 晚于 `last_sync_marker` 的文章。
- 用于补偿实时索引失败或数据库直接修改的场景。

### 5.3 全量重建（管理员手动触发）

流程：

```
1. 创建新索引 jeesite-articles-20260612-01（带时间戳后缀）
2. 批量写入所有已发布文章（Bulk，每批 1000 篇）
3. 将别名 jeesite-articles 指向新索引
4. 删除旧索引
```

> 整个过程对外无感知，搜索请求始终命中别名。

### 5.4 冷热分离

| 层级 | 条件 | 存储 | 副本 |
|------|------|------|------|
| 热数据 | 发布 90 天内 | SSD，1 副本 | 1 |
| 冷数据 | 发布超过 90 天 | 普通磁盘，0 副本 | 只读 |
| 归档 | 发布超过 1 年 | 仅数据库保留 | 不索引 |

通过 `_reindex` + ILM（Index Lifecycle Management）策略自动维护。

---

## 六、IK 中文分词

### 两种分词器对比

| 分词器 | 作用时机 | 粒度 | 典型切分结果"南京市长江大桥" |
|--------|---------|------|---------------------------|
| `ik_max_word` | **索引时** | 最细粒度，召回率高 | `南京市 / 南京 / 市 / 长江大桥 / 长江 / 大桥` |
| `ik_smart` | **搜索时** | 粗粒度，精确率高 | `南京市 / 长江大桥` |

### 自定义词库

在 ES 配置目录 `elasticsearch/config/analysis-ik/custom/` 下维护：

| 文件 | 说明 |
|------|------|
| `mydict.dic` | 自定义正向词库，一行一个词（如 `JeeSite.NET`、`低代码平台`） |
| `stopword.dic` | 停用词（如 `的`、`了`、`在`、`是`、`我`、`有`、`和`） |

词库热更新（7.x+ IK 支持远程词库）：

```
# IKAnalyzer.cfg.xml
<entry key="remote_ext_dict">http://your-app/ik/words.txt</entry>
```

在应用内暴露 `/ik/words.txt` 文本端点，由 IK 插件定期（默认 60s）拉取。

### 同义词

在 ES 中配置 `synonym` filter：

```
"filter": {
  "my_synonym": {
    "type": "synonym",
    "synonyms": [
      "笔记本,笔记本电脑",
      "ES,Elasticsearch"
    ]
  }
}
```

---

## 七、CMS 前端搜索集成

### 页面：`frontend/src/views/cms-front/Search.vue`

典型 UI：

```
┌──────────────────────────────────────────────────┐
│  🔍  [请输入关键词...]         [搜索]           │
├──────────────────────────────────────────────────┤
│  找到 1,234 条结果 (耗时 23 ms)                  │
│                                                  │
│  📂 按栏目:  全部 / 产品(210) / 博客(580) ...    │
│  🏷️  按标签:  .NET / Vue / JeeSite ...           │
│  📅 按时间:  今日 / 本周 / 本月 / 不限           │
│  ↕️  排序:  相关度 / 最新 / 最热                  │
├──────────────────────────────────────────────────┤
│  1. **<em>JeeSite</em>.NET 初体验**              │   ← 高亮
│     这是一篇介绍 <em>JeeSite</em>.NET 的文章...  │
│     2026-06-10  ·  博客  ·  浏览 1200            │
│                                                  │
│  2. ...                                          │
└──────────────────────────────────────────────────┘
```

### 接口：`GET /api/v1/cms/article/search`

Query 参数：

| 参数 | 类型 | 说明 |
|------|------|------|
| `q` | string | 搜索关键词 |
| `category` | string | 栏目代码 |
| `tag` | string | 标签 |
| `from` | date | 起始日期 `yyyy-MM-dd` |
| `to` | date | 结束日期 |
| `sort` | string | `relevance` / `date` / `views` |
| `page` | int | 页码，从 1 开始 |
| `size` | int | 每页数量 |

响应示例：

```json
{
  "code": 200,
  "message": "success",
  "data": {
    "total": 1234,
    "latencyMs": 23,
    "items": [
      {
        "articleId": "A001",
        "title": "<em>JeeSite</em>.NET 初体验",
        "summary": "介绍 <em>JeeSite</em>.NET 的开发体验...",
        "category": "博客",
        "publishDate": "2026-06-10 08:00:00",
        "viewCount": 1200
      }
    ],
    "aggregations": {
      "by_category": { "博客": 580, "产品": 210 },
      "by_tag":      { ".NET": 800, "Vue": 320 }
    },
    "suggestions": ["jeesite 教程", "jeesite 开发"]
  }
}
```

### 热门搜索词（Redis ZSET）

每次用户搜索时：

```
ZINCRBY hot-search 1 "关键词"
```

前端加载搜索页时：

```
ZREVRANGE hot-search 0 9
→ 返回 Top 10 热门搜索词
```

ZSET 由定时任务每日滚动重置。

---

## 八、索引监控与优化

### 8.1 Kibana Dashboard

建议在 Kibana 中建立以下可视化面板：

| 面板 | 指标 |
|------|------|
| 搜索 QPS | `GET /api/v1/cms/article/search` 每分钟调用量 |
| 搜索耗时分布 | P50 / P95 / P99 响应时间 |
| 索引大小 | `jeesite-articles` 的 `store.size` |
| 文档数量 | `count` 随时间变化曲线 |
| 慢查询 TOP | 超过 500ms 的查询 |
| 空结果词 TOP | 无结果返回的搜索词排名（用于补充词库） |

### 8.2 索引模板

按日 / 月滚动的索引建议使用 **Index Template**：

```
PUT _index_template/jeesite-articles-template
{
  "index_patterns": ["jeesite-articles-*"],
  "template": { "settings": {...}, "mappings": {...} },
  "priority": 100
}
```

### 8.3 Shards / Replicas 建议

| 阶段 | 文档数量 | shards | replicas |
|------|---------|--------|----------|
| 开发 | < 10 万 | 1 | 0 |
| 生产初 | < 100 万 | 3 | 1 |
| 生产大 | > 100 万 | 5 | 1~2 |

每个 shard 建议大小 **10~50 GB**，避免过小或过大。

### 8.4 常见问题排查

| 现象 | 可能原因 | 解决办法 |
|------|---------|---------|
| 搜索到"关键词"但正文没有 | 同义词配置错误 | 检查 synonym filter |
| 中文单字被拆成乱码 | IK 插件未正确安装 | 重新安装 IK，版本需与 ES 严格一致 |
| 新增文章搜不到 | refresh_interval 太大 | 等 30s，或重建索引 |
| 搜索超时 504 | 查询未加 `size` 限制 / 超时过短 | 默认 size≤100；确认 `RequestTimeoutSeconds` 足够 |
| 磁盘告警 | 索引未做冷热分离 | 启用 ILM，超期数据迁移到冷节点 |

---

## 九、Elasticsearch 启动（Docker）

仓库根目录的 `docker-compose.yml` 已包含 ES 与 Kibana：

```bash
cd d:\Projects\jeesite.net
docker compose up -d elasticsearch kibana

# 首次启动可能需要调大 vm.max_map_count
# wsl -d docker-desktop
# sysctl -w vm.max_map_count=262144

# 等待启动完成后验证
curl http://localhost:9200/_cluster/health
# 返回 {"status":"green","number_of_nodes":1,...}

# 查看索引
curl http://localhost:9200/_cat/indices?v

# 访问 Kibana
# 浏览器打开 http://localhost:5601
# 默认账号：elastic / changeme（生产环境请立即修改）
```

### 触发全量重建

在 JeeSite.NET 管理后台进入 **系统管理 → 高级 → 索引管理**：

- 选择 `jeesite-articles`
- 点击 **重建索引**
- 系统按第五节 5.3 的零停机流程完成

---

*文档最后更新：2026-06-12*
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---

## 💡 快速参考

| 项目 | 关键信息 |
|------|---------|
| **文档** | Elasticsearch |
| **最后更新** | 2026-06-13 |
| **相关文档** | [20-AI智能问答](20-AI智能问答) · [23-FusionCache缓存](23-FusionCache缓存) |

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>