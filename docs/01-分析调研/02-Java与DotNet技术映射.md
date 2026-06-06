# Java 与 .NET 技术映射对照表

> 本文档系统性地将 JeeSite 5 使用的 Java 技术栈映射到 .NET 10 生态，
> 为 JeeSite.NET 的技术选型提供决策依据。

## 1. 框架层映射

| JeeSite (Java) | JeeSite.NET (C#) | 说明 |
|---|---|---|
| Spring Boot 3.5 | ASP.NET Core 10 | 核心框架，均为官方推荐的企业级框架 |
| Spring Framework 6 | Microsoft.Extensions.* | DI/配置/日志等基础设施 |
| Spring MVC | ASP.NET Core MVC | Web框架 |
| Spring AOP | Castle.Core / 内置过滤器中间件 | AOP面向切面 |
| Apache Shiro 2 | ASP.NET Core Identity + JWT | 认证授权 |
| Maven | NuGet + dotnet CLI | 包管理/构建 |
| Spring.factories 自动配置 | IServiceCollection Extension | 自动注册机制 |
| yml/properties 配置 | appsettings.json | 配置文件格式 |

## 2. 数据层映射

| JeeSite (Java) | JeeSite.NET (C#) | 说明 |
|---|---|---|
| MyBatis 3.5 | Entity Framework Core 10 | ORM |
| Druid 1.2 | EF Core 内置连接池 | 数据源管理 |
| J2Cache (Caffeine+Redis) | FusionCache (Memory+Redis) | 二级缓存 |
| @Table / @Column 注解 | 自研增强 Attribute | ORM增强 |
| @JoinTable 关联查询 | EF Core .Include() / 导航属性 | 关联查询 |
| MyBatis Mapper XML | EF Core Fluent API | SQL映射 |
| MyBatis Pagination | EF Core .Skip()/.Take() | 分页 |
| ShardingSphere | EF Core 分片插件 / 读写分离 | 分库分表 |

## 3. 前端映射

| JeeSite (Java) | JeeSite.NET (C#) | 说明 |
|---|---|---|
| Vue 3 + Vite + TS | Vue 3 + Vite + TS (相同) | 分离版前端 |
| Ant Design Vue 4 | Ant Design Vue 4 (相同) | UI组件库 |
| Beetl 模板引擎 | Razor Pages / Blazor | 全栈版 |
| Bootstrap + AdminLTE | Bootstrap 5 + AdminLTE 适配 | 经典UI |
| Vben Admin | 自研 Vue Admin 框架 | 管理后台框架 |
| Node.js 编译 | Node.js 编译 (相同) | 前端构建 |

## 4. 中间件映射

| JeeSite (Java) | JeeSite.NET (C#) | 说明 |
|---|---|---|
| Redis | Redis (相同) | 缓存/Session |
| RabbitMQ | RabbitMQ + MassTransit | 消息队列 |
| ElasticSearch | ElasticSearch (相同) | 全文检索 |
| Flowable 7/8 | Elsa Workflows 3.x | 工作流引擎 |
| Quartz | Hangfire / Quartz.NET | 定时任务 |
| Nacos | Consul / K8s | 服务发现/配置中心 |
| Sentinel | Polly | 熔断降级 |
| Seata | CAP (outbox) | 分布式事务 |
| Zipkin / SkyWalking | OpenTelemetry | 链路追踪 |
| Spring Cloud Gateway | YARP / Ocelot | 网关 |
| MinIO / OSS | MinIO / 阿里云OSS (相同) | 对象存储 |
| Swagger | Swagger / Scalar | API文档 |

## 5. 核心设计模式映射

### 5.1 CRUD 泛型基类

```java
// JeeSite Java
public class BaseService<D extends CrudDao<T>, T extends BaseEntity<T>> {
    public T get(T entity) { ... }
    public List<T> findList(T entity) { ... }
    public Page<T> findPage(Page<T> page, T entity) { ... }
    public void save(T entity) { ... }
    public void delete(T entity) { ... }
}
```

```csharp
// JeeSite.NET C#
public abstract class CrudService<TEntity, TDto>
    where TEntity : BaseEntity<TEntity>
{
    public virtual Task<TDto> GetAsync(TEntity entity) { ... }
    public virtual Task<List<TDto>> FindListAsync(TEntity entity) { ... }
    public virtual Task<PageResult<TDto>> FindPageAsync(PageRequest<TEntity> request) { ... }
    public virtual Task SaveAsync(TEntity entity) { ... }
    public virtual Task DeleteAsync(TEntity entity) { ... }
}
```

### 5.2 树表辅助字段机制

```
┌──────────────────────────────────────────────┐
│                  TreeEntity                     │
│  parent_code: 上级编码                          │
│  parent_codes: 所有上级编码链                    │
│  tree_sort: 当前层级排序                         │
│  tree_sorts: 整树排序路径                        │
│  tree_leaf: 是否叶子节点                         │
│  tree_level: 层次级别                            │
│  tree_names: 全路径名称                          │
└──────────────────────────────────────────────┘
```

Java 端使用 MyBatis 自动维护，.NET 端使用 EF Core `SaveChanges` 拦截器自动维护。

### 5.3 数据权限过滤

```
// JeeSite Java (MyBatis拦截器注入SQL)
@Intercepts({@Signature(type=StatementHandler.class, method="prepare", args={Connection.class, Integer.class})})
public class DataScopeInterceptor implements Interceptor {
    // 自动注入 dataScope SQL 条件
}

// JeeSite.NET C# (EF Core Global Query Filter)
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>().HasQueryFilter(u => u.OfficeCode == _currentUser.OfficeCode);
}
```

## 6. 项目结构映射

```
JeeSite Java                          JeeSite.NET C#
─────────────────                     ─────────────────
jeesite-common/         →             JeeSiteNET.Core/
jeesite-module-core/    →             JeeSiteNET.Modules.Sys/
jeesite-module-cms/     →             JeeSiteNET.Modules.Cms/
jeesite-module-bpm/     →             JeeSiteNET.Modules.Bpm/
jeesite-web/            →             JeeSiteNET.Web/
jeesite-web-api/        →             JeeSiteNET.Web.Api/
parent/pom.xml          →             Directory.Build.props
```

## 7. 差异化分析

### Java 特有但 .NET 无直接对应的

| 概念 | .NET 替代方案 |
|---|---|
| MyBatis Mapper XML | EF Core Fluent API / LINQ |
| Spring.factories | Assembly scanning + DI 扩展方法 |
| Beetl 模板引擎 | Razor View Engine |
| Shiro Realm | IUserStore / SignInManager |
| Druid 监控 | EF Core 拦截器 + Prometheus |

### .NET 特有优势

| 优势 | 说明 |
|---|---|
| LINQ | 类型安全的查询，编译期检查 |
| EF Core Migrations | 内置数据库迁移，无需手动 SQL |
| Source Generators | 编译时代码生成，零反射开销 |
| Blazor | 用 C# 写前端，全栈统一 |
| Nullable Reference Types | 编译期空引用检查 |
| Hot Reload | 热重载开发体验 |

## 8. 技术选型总结

**推荐 .NET 技术栈组合**:
- ASP.NET Core 10 + Minimal API / MVC
- EF Core 10 + Fluent API + 自研 EntityBuilder
- FusionCache (Memory + Redis 二级缓存)
- ASP.NET Core Identity + JWT + Policy-Based Authorization
- Elsa Workflows 3.x (工作流引擎)
- MassTransit + RabbitMQ (事件总线)
- Hangfire (后台任务)
- OpenTelemetry (可观测性)
