<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# FusionCache缓存

> 基于 FusionCache 的双层缓存架构：L1 内存 + L2 Redis + Pub/Sub 失效广播，雪崩防护。
>
> **适用角色**：架构师、后端开发
> **阅读时间**：约 10 分钟
> **相关文档**：[22-Elasticsearch](22-Elasticsearch) · [33-深入架构剖析](33-深入架构剖析)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、缓存架构](#一、缓存架构)
  - [分层示意](#分层示意)
  - [读写路径](#读写路径)
  - [失效广播（多节点同步](#失效广播（多节点同步)
- [二、缓存类型与 Key 规范](#二、缓存类型与-key-规范)
  - [Key 命名约定](#key-命名约定)
- [三、核心服务：CacheService](#三、核心服务：cacheservice)
  - [CacheStats 示例返回](#cachestats-示例返回)
- [四、缓存失效策略](#四、缓存失效策略)
  - [4.1 主动失效（最高优先级）](#41-主动失效（最高优先级）)
  - [4.2 TTL 自动过期（兜底）](#42-ttl-自动过期（兜底）)
  - [4.3 滑动过期](#43-滑动过期)
  - [4.4 事件广播（Pub/Sub）](#44-事件广播（pub-sub）)
- [五、缓存穿透 / 击穿 / 雪崩防护](#五、缓存穿透-击穿-雪崩防护)
  - [5.1 穿透：null 值占位](#51-穿透：null-值占位)
  - [5.2 击穿：FusionCache 的锁机制](#52-击穿：fusioncache-的锁机制)
  - [5.3 雪崩：抖动 + 预热 + 断路器](#53-雪崩：抖动-预热-断路器)
- [六、缓存预热](#六、缓存预热)
  - [启动时预加载](#启动时预加载)
  - [每日 04:00 定时刷新](#每日-0400-定时刷新)
- [七、监控与管理](#七、监控与管理)
  - [缓存管理页面](#缓存管理页面)
  - [管理接口](#管理接口)
  - [告警规则](#告警规则)
- [八、Redis 配置（appsettings.json）](#八、redis-配置（appsettingsjson）)

---


> FusionCache 采用 **双层缓存（Memory + Redis）+ 失效广播机制，提供 fail-safe、击穿防护、滑动过期等高级特性。

---

## 一、缓存架构

### 分层示意

```
┌──────────────────────────────────────────────────────┐
│ 应用服务器内存 (Memory Cache / L1)               │
│                                                      │
│  • 极快: < 1 ms                                     │
│  • 容量受限: 通常 1 ~ 2 GB                         │
│  • 单节点可见                                        │
│  • key: "auth:user:admin                         │
├──────────────────────────────────────────────────────┤
│ 分布式缓存 (Redis / L2)                              │
│                                                      │
│  • 较快: 1 ~ 5 ms                                   │
│  • 容量大: 几十 GB                                  │
│  • 多节点共享                                         │
│  • Pub/Sub 通知机制（失效广播）                   │
│  • key: "jeesite:auth:user:admin"                │
└──────────────────────────────────────────────────────┘
```

### 读写路径

```
            GET(key)
              │
       ┌──────▼──────┐
       │ 命中 L1？  │ → 直接返回（< 1ms）
       └──────┬──────┘
              否
              ▼
       ┌──────▼──────┐
       │ 命中 L2？  │ → 返回，同步回写 L1
       └──────┬──────┘
              否
              ▼
       ┌──────▼──────┐
       │ 执行 factory │ → 从数据库读取
       │  (DB Query)   │
       └──────┬──────┘
              ▼
        同时写入 L1 + L2
```

### 失效广播（多节点同步

- 任一节点执行 `Set / Remove` 操作
- 通过 Redis Pub/Sub 通道（`jeesite-fc`）广播 `Invalidate(key)`
- 其他节点收到后清除本地 L1 中对应 key
- 后续请求将从 L2 / 源头重新加载

---

## 二、缓存类型与 Key 规范

| 缓存类型 | Key 格式 | 内容示例 | 默认过期时间 |
|---------|---------|---------|-----------|
| 用户权限 | `auth:perm:{userCode}` | 用户权限标识集合（`List<string>`） | 2 小时 / 登出主动失效 |
| 用户信息 | `auth:user:{userCode}` | 用户基本信息（名称/所属机构） | 2 小时 |
| 菜单树 | `menu:tree:{userCode}:{sysCode}` | 用户可访问的菜单 JSON 树 | 2 小时 |
| 字典数据 | `dict:{dictType}` | 字典数据列表 | 1 天 |
| 配置项 | `config:{configKey}` | 配置值（string / number） | 1 小时 |
| 机构树 | `org:tree:{corpCode}` | 机构树 JSON | 4 小时 |
| 公司树 | `company:tree:{corpCode}` | 公司树 JSON | 4 小时 |
| CMS 文章详情 | `cms:article:{articleId}` | 文章详情对象 | 1 小时 |
| CMS 文章列表 | `cms:list:{category}:{page}` | 分页列表（包含摘要） | 15 分钟 |
| Token 黑名单 | `jwt:revoked:{jti}` | 被吊销的 JWT ID（string placeholder） | Token 剩余有效期 |
| 业务缓存 | `{module}:{key}:{tenant}` | 各模块自定义业务缓存 | 按需配置 |

### Key 命名约定

- 总前缀由 `DistributedCacheKeyPrefix`（默认 `jeesite:`）统一控制
- 层级使用 `:` 分隔，便于 Redis 客户端按前缀扫描与清仓
- `{tenant}` 部分在多租户场景下自动注入，默认 `default`

---

## 三、核心服务：CacheService

位置：`src/JeeSiteNET.Core/Services/CacheService.cs`

| 方法 | 签名 | 说明 |
|------|------|------|
| `GetAsync<T>` | `Task<T> GetAsync<T>(string key)` | 读取缓存（L1 → L2），不存在返回 `default` |
| `SetAsync<T>` | `Task SetAsync<T>(string key, T value, TimeSpan? duration)` | 写入缓存（同时写 L1 + L2，广播失效） |
| `RemoveAsync` | `Task RemoveAsync(string key)` | 移除 key，广播所有节点 |
| `RemoveByPrefixAsync` | `Task RemoveByPrefixAsync(string prefix)` | 按前缀批量移除（依赖 Redis SCAN） |
| `GetOrCreateAsync<T>` | `Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? duration)` | 经典 GetOrSet，**FusionCache 提供 fail-safe 保护** |
| `RefreshAsync` | `Task RefreshAsync(string key)` | 立即刷新 key 的 TTL |
| `GetStatsAsync` | `Task<CacheStats> GetStatsAsync()` | 统计命中率 / L1 内存占用 / key 数量 |

### CacheStats 示例返回

```csharp
public class CacheStats
{
    public long TotalKeys { get; set; }      // L1 中的 key 数量
    public long L1HitCount { get; set; }
    public long L2HitCount { get; set; }
    public long MissCount { get; set; }
    public double HitRate { get; set; }     // 命中率, e.g. 0.923
    public long EstimatedMemoryBytes { get; set; }
}
```

---

## 四、缓存失效策略

### 4.1 主动失效（最高优先级）

数据变更时立即调用 `RemoveAsync(key)`。

典型场景：

| 操作 | 失效的 key 示例 |
|------|----------------|
| 用户修改权限 | `auth:perm:{userCode}`、`menu:tree:{userCode}:*` |
| 修改字典 | `dict:*` |
| 修改配置 | `config:*` |
| 文章发布 / 修改 | `cms:article:*`、`cms:list:*` |

### 4.2 TTL 自动过期（兜底）

每个缓存项都有绝对过期时间，避免"永久缓存"的脏数据。

### 4.3 滑动过期

对访问频次高的 key 自动延长 TTL。通过 FusionCache 的 `options.Duration` + `Jitter` 组合实现。

### 4.4 事件广播（Pub/Sub）

```
节点 A 修改数据 → RemoveAsync(key)
                 │
                 ├── 本地 L1 立即清除
                 │
                 └── Redis Pub/Sub 广播 Invalidate(key)
                          │
                          ▼
                    节点 B / C / D ...
                       清除本地 L1[key]
```

---

## 五、缓存穿透 / 击穿 / 雪崩防护

| 问题 | 现象 | 防护方案 |
|------|------|---------|
| **穿透** | 恶意请求一个不存在的 key，每次都打到数据库 | 对不存在的 key 写入 null 值占位，5 分钟 TTL |
| **击穿** | 热点 key 过期瞬间，大量请求同时涌入 DB | FusionCache 内置锁机制，同一 key 只有一个 factory 执行，其他请求等待结果或使用 fail-safe 值 |
| **雪崩** | 大量 key 同时过期，DB 瞬间压力暴增 | 1. 随机化过期时间（±5 分钟抖动） 2. 热点数据预加载 3. 断路器（Circuit Breaker）模式 |

### 5.1 穿透：null 值占位

```csharp
// CacheService.GetOrCreateAsync 内部伪代码
var value = await db.Query(...);
if (value == null) {
    // 写入 null 占位，5 分钟过期
    await cache.SetAsync(key, null, TimeSpan.FromMinutes(5));
}
return value;
```

### 5.2 击穿：FusionCache 的锁机制

FusionCache 对同一 key 的并发请求采用以下流程：

```
请求 1 获得锁 → 执行 factory → 写入缓存 → 释放锁 → 返回
请求 2 等待... → 从缓存取
请求 3 等待... → 从缓存取
```

### 5.3 雪崩：抖动 + 预热 + 断路器

- **抖动**：`options.JitterMaxDuration = TimeSpan.FromMinutes(5)`
- **预热**：启动时加载字典、配置、机构树（见第六节）
- **断路器**：当 DB 响应时长超过阈值（如 5s），自动降级返回 fail-safe 值

---

## 六、缓存预热

### 启动时预加载

应用启动后（`Program.cs` 的 `HostedService` 中）依次加载：

```csharp
await cacheService.GetOrCreateAsync("dict:sys_common", async () => {
    return await dbContext.DictTypes
        .Where(d => d.Status == "0")
        .ToListAsync();
}, TimeSpan.FromDays(1));

// 类似地：系统配置、机构/公司树、热门栏目文章...
```

### 每日 04:00 定时刷新

由 `Tasks` 模块的定时任务执行：

```
0 4 * * *  →  调用 CacheService.RefreshHotKeys()
                扫描热门 key 并刷新
```

---

## 七、监控与管理

### 缓存管理页面

**系统管理 → 系统监控 → 缓存管理**

```
┌──────────────────────────────────────────────────────┐
│  模块    │ 命中率 │  key 数 │ 内存 (MB) │ 操作  │
├──────────────────────────────────────────────────────┤
│  auth    │  94.2% │   2,103 │    12.3 │ [清] │
│  dict    │  99.1% │      87 │     0.8 │ [清] │
│  cms     │  88.6% │  18,292 │    85.1 │ [清] │
│  config  │  97.0% │     120 │     0.3 │ [清] │
│  全部    │  92.1% │  38,512 │   134.6 │ [清] │
└──────────────────────────────────────────────────────┘
```

### 管理接口

| HTTP | 路径 | 说明 |
|------|------|------|
| GET | `/api/v1/sys/cache/stats` | 各模块命中率统计 |
| GET | `/api/v1/sys/cache/list?prefix=auth` | 按前缀列出 key |
| POST | `/api/v1/sys/cache/remove` | 移除指定 key（body: `{ "key": "auth:user:admin" }`） |
| POST | `/api/v1/sys/cache/clear` | 清除整个缓存（⚠️ 需二次确认） |

### 告警规则

| 规则 | 触发条件 | 建议动作 |
|------|---------|---------|
| 命中率低于 70% | `HitRate < 0.7` | 检查缓存 key 设计或 TTL 配置 |
| L1 内存占用 > 1.5 GB | `EstimatedMemoryBytes > 1.5GB` | 检查 L1 size 限制，考虑增大 Redis |
| Redis 连接失败 | Redis 连接超时 | 降级到纯 L1 模式，告警 |

---

## 八、Redis 配置（appsettings.json）

```json
{
  "FusionCache": {
    "RedisConnectionString": "localhost:6379,allowAdmin=true,password=your-redis-password,connectTimeout=5000,syncTimeout=1000",
    "DistributedCacheKeyPrefix": "jeesite:",
    "BackplaneChannelName": "jeesite-fc",
    "DefaultEntryDurationMinutes": 60,
    "EnableFailSafe": true,
    "EnableOptimisticConcurrency": true,
    "JitterMaxDurationMinutes": 5,
    "MemoryCacheSizeLimitMb": 2048,
    "DistributedCacheHardTimeoutMs": 5000,
    "FactorySoftTimeoutMs": 100,
    "FactoryHardTimeoutMs": 3000
  }
}
```

| 配置项 | 说明 |
|--------|------|
| `RedisConnectionString` | Redis 连接字符串（支持 password/timeout） |
| `DistributedCacheKeyPrefix` | L2 key 前缀，避免与其他应用冲突 |
| `BackplaneChannelName` | Pub/Sub 通道名 |
| `DefaultEntryDurationMinutes` | 默认缓存有效期（分钟） |
| `EnableFailSafe` | 启用 fail-safe（DB 不可用时返回旧值） |
| `EnableOptimisticConcurrency` | 启用乐观并发控制 |
| `JitterMaxDurationMinutes` | 过期时间抖动（±此值内随机，避免雪崩） |
| `MemoryCacheSizeLimitMb` | L1 内存容量上限 |
| `FactorySoftTimeoutMs` | factory 软超时（超过后开始并行执行但继续等待） |
| `FactoryHardTimeoutMs` | factory 硬超时（超过后强制使用 fail-safe 值） |

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
| **文档** | FusionCache缓存 |
| **最后更新** | 2026-06-13 |
| **相关文档** | [22-Elasticsearch](22-Elasticsearch) · [33-深入架构剖析](33-深入架构剖析) |

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>