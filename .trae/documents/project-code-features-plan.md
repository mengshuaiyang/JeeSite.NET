# JeeSite.NET 项目代码功能完善计划（Brainstorming）

> 计划生成日期: 2026-06-15  
> 适用范围: `d:\Projects\jeesite.net` 全项目  
> 参考基线: JeeSite5 (`D:\Projects\jeesite5-v5.springboot3`) 全量源码扫描  
> 决策依据: `docs/06-过程记录/04-功能差异比对清单.md`、`docs/05-开发计划/08-待建任务与增强项清单.md`、`docs/08-项目依赖分析/01-项目结构与依赖关系分析.md`

---

## 一、项目现状速览

### 1.1 当前技术栈

| 层 | 技术 | 版本 | 说明 |
|---|------|------|------|
| 运行时 | .NET / ASP.NET Core | 10.0 | 主框架 |
| ORM | Entity Framework Core | 9.0 | 含 SqlServer/Sqlite/PostgreSQL/达梦/金仓多 Provider |
| 缓存 | FusionCache | 2.6 | L1 Memory + L2 Redis 两级缓存 |
| 工作流 | Elsa Workflows | 3.6 | 可视化流程引擎 |
| 定时任务 | Quartz.NET | 3.14 | 作业调度 |
| 认证 | JWT Bearer + Cookie + CAS + LDAP | 10.0.7 | 多认证方式 |
| 前端 | Vue 3 + Pinia + Ant Design Vue | 3.5 / 4.2 | SPA 架构 |
| 容器化 | Docker Compose | — | 含 SQL Server / Redis / MinIO / Elasticsearch / Loki / Grafana |
| AI | DeepSeek Function Calling + AI Tools | — | 3 内置工具 + MCP Server |

### 1.2 当前功能完成度

| 维度 | 完成度 | 说明 |
|---|-------|------|
| 核心业务功能（对标 JeeSite5） | **100%** | 38/38 控制器、34/34 服务、42/42 工具、24/24 实体 |
| JeeSite.NET 超集功能 | 16 项新增 | 密码强度、多设备登录、企业多租户、分块上传、文件预览、系统监控、Word 导出、MCP Server、飞书钉钉、Elasticsearch、SignalR、国产数据库多 Provider、代码生成多数据库、Docker Compose、AI Tools 框架、国密 |
| 待建增强项（P2） | 4 项 | 见 2.1 节 |
| 待观察项（P3） | 6 项 | 见 2.2 节 |

### 1.3 工程化现状扫描

| 维度 | 现状 | 评估 |
|---|------|------|
| 代码注释 | 2026-06-15 已完成约 146 个核心 .cs 文件的 `///` 注释补充 | 核心目录覆盖良好，边缘工具类/前端 TS 待完善 |
| 单元测试 | `tests/JeeSiteNET.Core.Tests` 项目存在，55 tests passed | 仅 Core 层，模块层/前端无测试 |
| 集成测试 | 未设立独立集成测试项目 | 可加强 |
| 前端 E2E | `frontend/e2e/` (login.spec.ts, cms-front.spec.ts) | 基础覆盖，可扩展 |
| 前端单元测试 | `frontend/src/__tests__/` (appStore.test.ts, util.test.ts) | 基础覆盖 |
| CI/CD | `.github/workflows/ci.yml` | 存在，可扩展更多检查步骤 |
| 文档 | `docs/` 目录丰富，Wiki 作为 Git 子模块管理 | 需持续维护 |
| 命名规范 | 总体遵循 `AGENTS.md` 约定 | 需定期 review |

---

## 二、功能完善项：缺失与增强

### 2.1 P2 级增强项（核心增强，近期应实施）

| # | 项目 | 现状 | 完善思路 | 预计工作量 | 涉及文件 |
|---|------|------|---------|-----------|---------|
| **P2-1** | **短信/邮件验证码登录** | `AccountController` 的 `login-by-valid-code` 端点返回"功能实现中"占位响应；`ValidCodeService`/`SmsService`/`EmailService` 骨架存在但业务流程未贯通 | 1. `ValidCodeService` 补充生成-校验-过期清理；2. `SmsService`/`EmailService` 接入真实发送渠道（阿里云 SMS、SMTP），3. `AccountController` 补充 `send-valid-code`、`login-by-valid-code`、`reset-password-by-code` 3 端点+限流；4. 前端 `login.vue`/`forgot-password.vue` 增加验证码流程 | 2-3 天 | `modules/Sys/Controllers/AccountController.cs`、`modules/Sys/Application/Services/ValidCodeService.cs`、`modules/Sys/Application/Services/SmsService.cs`、`modules/Sys/Application/Services/EmailService.cs`、`frontend/src/views/Login.vue`、`frontend/src/views/ForgotPassword.vue` |
| **P2-2** | **菜单树 / 路由 / 权限聚合端点** | 前端 `appStore.loadMenus()` 仅拉取菜单树，缺少 `authInfo`（聚合菜单+权限字符串+用户信息）与 `menuRoute`（Vue Router 可消费路由表） | 1. `AuthService` 新增 `GetAuthInfoAsync(userCode)` 返回菜单树+权限列表+用户概要；2. 新增 `GetMenuRouteAsync(userCode)` 生成 `{path, component, meta}`；3. `AuthController` 增加 `/auth-info`、`/menu-route`、`/menu-tree` 3 端点；4. 前端 `appStore.ts`、`router/index.ts` 重构为动态加载 | 2 天 | `modules/Sys/Application/Services/AuthService.cs`、`modules/Sys/Controllers/AuthController.cs`、`frontend/src/stores/app.ts`、`frontend/src/router/index.ts` |
| **P2-3** | **UEditor 完整协议兼容（ActionMap）** | `EditorController` 仅实现基础上传/下载，未实现 UEditor JS 前端所需 `ActionMap` 的 15+ 子操作（crawlRemote、removefile、listfile、listimage、catchimage、imageManager、config、uploadscrawl、uploadvideo 等） | 1. 新增 `UEditorActionHandler` 中间件（按 `action` 参数路由）；2. 实现 `ConfigManager`、`ImageHunter`、`FileManager`、`PathFormat`、`BaseState/ActionState/MultiState` 等类；3. 将 `EditorController` 改造为调用 `UEditorActionHandler` 统一入口 | 3-4 天 | `src/JeeSiteNET.Core/UEditor/` 目录下新增文件 |
| **P2-4** | **Excel 自定义字段类型扩展点** | `ExcelFieldAttribute` 仅注解式，无 `FieldType` 继承扩展体系；复杂类型（金额、Office、Company、Area、Role）需在 `ExcelService` 写死 | 1. 抽象 `IExcelFieldType` 接口（`GetFieldValue`、`SetFieldValue`、`Format`）；2. 内置 MoneyType、DecimalType、OfficeType、CompanyType、PostListType、RoleListType、AreaType 8 个实现；3. `ExcelService` 扫描属性上的类型注册器（Type Register Pattern） | 2 天 | `src/JeeSiteNET.Core/Utils/Excel/` 新增目录结构 |

### 2.2 P3 级增强项（锦上添花，中期可考虑）

| # | 项目 | 完善思路 | 预计工作量 |
|---|------|---------|-----------|
| P3-1 | 富文本编辑器增强 | Vditor 深度集成（拼写检查、Markdown 切换） | 1-2 天 |
| P3-2 | 移动端响应式深化 | 梳理所有 Vue 页面，保证 <768px 视觉一致 | 2 天 |
| P3-3 | SQL 性能监控面板 | 集成 MiniProfiler 或 EF Core 拦截器+前端可视化 | 2 天 |
| P3-4 | 日志拦截器增强 | 新增操作日志自动记录中间件（方法级，含参数+耗时） | 1 天 |
| P3-5 | AI Tools AOP 自动注册 | 参考 JeeSite5 `AiToolSubjectAspect`，通过 Source Generator / Interceptor 实现 `[AiTool]` 自动注册到 `AiToolRegistry` | 2-3 天 |
| P3-6 | 国际化词条覆盖检查 | Lang 表缺失项扫描脚本，对照前端 `t('xxx')` 提取缺失 key | 1 天 |

---

## 三、工程化完善项：质量与可维护性

### 3.1 单元测试与集成测试体系

**问题**: 仅 Core 层 `JeeSiteNET.Core.Tests` 有 55 测试；模块层（Sys/Cms/Bpm/CodeGen）缺失单元测试；无集成测试项目。

**完善项**:

| # | 内容 | 思路 | 涉及文件 |
|---|------|------|---------|
| T-1 | 模块层单元测试项目 | 新建 `tests/JeeSiteNET.Modules.Tests`，按模块分区，测试 Service/Controller。使用 `EF Core InMemory`/`SQLite InMemory` | `tests/JeeSiteNET.Modules.Tests/` 新建目录 |
| T-2 | 前端单元测试扩展 | `frontend/src/__tests__/` 扩展至 stores、utils、composables、views（关键组件） | `frontend/src/__tests__/` |
| T-3 | 前端 E2E 扩展 | `frontend/e2e/` 补充登录、菜单、用户管理、CMS 文章发布等关键业务路径 | `frontend/e2e/` |
| T-4 | 测试覆盖率门槛 | 使用 `coverlet.collector` 生成覆盖率报告，设定目标 >= 70%，低于阈值 CI 失败 | `tests/` + `.github/workflows/ci.yml` |

### 3.2 代码注释与 XML 文档

**问题**: 2026-06-15 完成的注释覆盖主要为后端核心文件；前端 TS/Vue 仍缺少 JSDoc 注释；部分工具类边缘文件（如 `Class1.cs` 等残遗命名空间文件）待整理或删除。

**完善项**:

| # | 内容 | 思路 |
|---|------|------|
| D-1 | 前端 TS 文件 JSDoc 注释 | `api/*.ts`、`stores/*.ts`、`utils/*.ts`、`composables/*.ts`、`directives/*.ts` 添加 JSDoc 格式注释 |
| D-2 | 边缘工具类 / 遗漏文件补注释 | 全局搜索 `class .*` 无 `/// <summary>` 的 .cs 文件，批量补齐 |
| D-3 | 文档生成（可选） | 使用 `docfx` 生成 API 参考文档站点，纳入 Wiki |

### 3.3 代码清理与规范化

**问题**: 存在一些残留的占位文件（如 `Class1.cs`）、命名不一致、TODO 未清理。

**完善项**:

| # | 内容 | 思路 |
|---|------|------|
| C-1 | 清理残留模板文件 | 移除或合并 `Class1.cs`、空目录等 |
| C-2 | 命名约定一致性检查 | 编写脚本校验 `namespace JeeSiteNET.{Layer}.{Module}` 规则；实体/服务/DTO 命名一致性 |
| C-3 | `#nullable` 启用 | 全局在 .csproj 中启用 `<Nullable>enable</Nullable>`，修复警告（分阶段，避免一次性大规模修改） |
| C-4 | `async`/`await` 一致性 | 所有异步方法确保 `Async` 后缀，避免 `async void` |
| C-5 | `using` 声明风格统一 | 统一使用文件顶部 `using` 或全局 `GlobalUsings.cs` |

### 3.4 CI/CD 增强

**问题**: 已有 `.github/workflows/ci.yml` 基础构建，缺少代码质量检查、安全扫描、测试覆盖、Docker 镜像推送。

**完善项**:

| # | 内容 | 思路 |
|---|------|------|
| CI-1 | 集成 SonarQube / SonarCloud | 代码质量、重复率、潜在 bug 扫描 |
| CI-2 | 安全扫描 | 集成 `dotnet list package --vulnerable`、`Snyk` 或 `GitHub Dependabot` |
| CI-3 | 测试覆盖率门禁 | 见 T-4 |
| CI-4 | Docker 镜像构建 & 推送 | Workflow 中 `docker buildx` 构建 `webapi`、`frontend` 镜像并推送至私有 Registry |
| CI-5 | 文档自动生成与部署 | docfx 构建后部署 Wiki/GitHub Pages |

---

## 四、架构与性能完善项

### 4.1 缓存策略深化

**问题**: FusionCache 已接入，但各模块的缓存粒度、过期策略、缓存穿透保护缺乏统一约定。

**完善项**:

| # | 内容 | 思路 |
|---|------|------|
| A-1 | 统一缓存策略文档 | 约定不同数据类型的 TTL、缓存键命名规则（`{Module}:{Entity}:{Id}`）、fallback 超时 |
| A-2 | 缓存穿透保护 | 为空值设置短 TTL（1-5 分钟）；批量查询防击穿 |
| A-3 | 热点数据预热 | 启动时加载字典、配置、角色、菜单到缓存 |
| A-4 | 缓存命中率监控 | 通过 `FusionCache` 事件写入监控日志，纳入 Prometheus 指标（如可接入） |

### 4.2 数据库访问优化

| # | 内容 | 思路 |
|---|------|------|
| A-5 | EF Core 查询性能 | 审查高频查询是否使用 `AsNoTracking`、`Select` 投影、避免 `N+1` |
| A-6 | 索引审查 | 对所有外键+常用查询列建立索引，写脚本比对 EF Core Migration 与实际索引 |
| A-7 | 读写分离（可选） | PostgreSQL / SqlServer 支持只读副本时引入 `IDbConnectionResolver` |

### 4.3 前端性能

| # | 内容 | 思路 |
|---|------|------|
| A-8 | 路由级代码分割 | 所有路由 `import()` 懒加载，首屏 JS 体积 <500KB |
| A-9 | 组件缓存 | 对 `MainLayout` 内容区域、菜单渲染等实现 `v-memo`/`KeepAlive` |
| A-10 | Tree Shaking | 确认 Ant Design Vue 按需加载，移除未用组件 |
| A-11 | 图片懒加载 | CMS 文章图片统一 `loading="lazy"` |

---

## 五、安全完善项

### 5.1 现有安全能力回顾

已实施：SM2/SM3/SM4 国密、HTML 富文本 XSS 清洗、文件安全（扩展名/签名/路径遍历）、CSP/Permissions-Policy 响应头、按钮级权限 `v-permission`、路由守卫权限检查、多设备 Token 吊销、密码强度+历史校验。

### 5.2 进一步完善

| # | 项目 | 思路 |
|---|------|------|
| S-1 | CSRF 加固 | 对传统表单/文件上传路径确保 `ValidateAntiForgeryToken`；对 API 明确仅 Bearer Token |
| S-2 | SQL 注入审查 | 确认所有 `FromSqlRaw`、`ExecuteSqlRaw` 使用参数化查询；禁止字符串拼接 |
| S-3 | 敏感数据脱敏 | 用户手机号、邮箱、身份证号在返回前端时脱敏显示 |
| S-4 | 审计日志完整性 | 确认关键操作（登录、授权、密码修改、数据删除）写入审计表 |
| S-5 | 依赖项安全更新 | 定期扫描 NuGet/npm 漏洞，设置 Dependabot |
| S-6 | 配置与密钥管理 | `appsettings.json` 中的密钥使用 `dotnet user-secrets` / Azure Key Vault / HashiCorp Vault 管理（根据部署环境） |

---

## 六、文档与协作完善项

| # | 项目 | 思路 |
|---|------|------|
| DOC-1 | Wiki 维护 SOP | 约定 Wiki 更新触发条件（重大功能、新增模块、架构变更） |
| DOC-2 | API 参考自动生成 | docfx + Swagger UI 组合，保证接口文档实时同步 |
| DOC-3 | 贡献者指南 | 在 Wiki 增加 `CONTRIBUTING.md`（分支策略、PR 模板、代码 Review 流程） |
| DOC-4 | 部署与运维手册 | Docker Compose 升级指南、数据库迁移流程、灾难恢复脚本 |

---

## 七、优先级与实施路线图

### 7.1 优先级矩阵

| 优先级 | 范围 | 建议实施窗口 | 典型条目 |
|-------|------|------------|---------|
| **高 (P2)** | 缺失但业务必需 | 下一到两个迭代内完成 | 短信验证码登录、菜单路由聚合端点 |
| **中 (P3)** | 增强/优化，不阻塞主流程 | 接下来 1-2 个月 | UEditor 完整协议、Excel 自定义类型、AI Tools AOP |
| **低** | 工程化/架构/性能/文档 | 持续性迭代 | 测试覆盖扩展、缓存策略、性能优化、CI 增强 |

### 7.2 建议实施顺序

```
Phase 1：P2 级业务功能补齐（4-7 天累计）
├── P2-1 短信/邮件验证码登录 + 前端流程
├── P2-2 菜单树/路由/权限聚合端点 + 前端改造
├── P2-3 UEditor 完整协议兼容（可选，视业务是否依赖 UEditor JS）
└── P2-4 Excel 自定义字段类型（可选，视业务是否有复杂字段需求）

Phase 2：P3 级增强 + 工程化起步（持续）
├── 单元测试项目 (T-1 ~ T-4)
├── 前端 JSDoc 注释 (D-1)
├── 代码清理 (C-1 ~ C-5)
└── CI/CD 增强 (CI-1 ~ CI-5)

Phase 3：架构与性能优化（持续性）
├── 缓存策略 (A-1 ~ A-4)
├── 数据库性能 (A-5 ~ A-7)
└── 前端性能 (A-8 ~ A-11)

Phase 4：安全与文档（持续性）
├── 安全加固 (S-1 ~ S-6)
└── 文档完善 (DOC-1 ~ DOC-4)
```

---

## 八、风险与注意事项

| # | 风险 | 影响 | 应对 |
|---|------|------|------|
| R-1 | `#nullable` 全局开启可能引发大面积警告 | 阻断编译/降低信心 | 分项目/分目录渐进开启，避免一次性全量修改 |
| R-2 | 动态路由改造（P2-2）可能引入前端路由 Bug | 影响已登录用户体验 | 保留静态路由 fallback，灰度验证；充分 E2E 测试 |
| R-3 | UEditor 协议实现（P2-3）涉及 SSRF 防护 | 安全风险 | 内网 IP 白名单/黑名单、超时限制、最大文件大小限制 |
| R-4 | 短信/邮件服务接入需外部账户与费用 | 成本 + 配置管理 | 使用环境变量/密钥管理，本地开发提供 Mock |
| R-5 | 测试覆盖率门槛可能推迟合并 | 团队效率 | 初期目标 50%，分阶段提升 |

---

## 九、验收指标

| 指标 | 当前 | 目标 |
|------|------|------|
| P2 项完成率 | 0/4 | 100% |
| 核心模块单元测试 | Core 55 tests | + Modules 100+ tests |
| 测试覆盖率 | 未测量 | >= 70% |
| 前端关键路径 E2E | login, cms-front | + user-menu / article / role |
| 后端 XML 注释覆盖率 | 核心目录完成 | 100% 公共 API 有 `///` 注释 |
| 前端 JSDoc 覆盖率 | 未实施 | `api/`、`stores/`、`utils/` 全覆盖 |
| CI 门禁 | build only | build + test + coverage + security scan |
| 安全漏洞数 | 未测量 | 0 Critical / 0 High |

---

## 十、下一步行动（执行计划前置步骤）

1. **确认业务优先级**：与业务方确认是否确实依赖 UEditor JS 前端、是否需要验证码登录，以决定 P2-3/P2-4 是否实施
2. **拆分任务到 GitHub Issues / 项目管理工具**：每条完善项按 `Phase / Priority / Module` 打标签
3. **首次代码质量基线**：运行 `dotnet build`、`dotnet test`、`eslint`、记录基线数据
4. **逐步实施 Phase 1**：优先解决 P2-1、P2-2（对业务价值最高），然后视情况 P2-3、P2-4
5. **持续维护本计划**：每月回顾更新一次 `project-code-features-plan.md`

---

*文档结束*
