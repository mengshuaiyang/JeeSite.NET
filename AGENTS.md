# AGENTS.md — JeeSite.NET 项目配置

本文件为 AI 辅助开发提供项目级上下文配置。

## 项目概述

- 项目名称: JeeSite.NET
- 描述: 基于 .NET 10 的企业级快速开发平台，参考 JeeSite 5 架构
- 技术栈: .NET 10, ASP.NET Core, EF Core, Vue 3, Ant Design Vue
- 包管理: NuGet + npm/pnpm
- 缓存: FusionCache 2.6 (L1 Memory + L2 Redis)
- 数据库支持: SqlServer, Sqlite, PostgreSQL/GaussDB, 达梦 DM8, 人大金仓 KingbaseES
- 部署: Docker Compose (SQL Server + Redis + MinIO + Elasticsearch + WebApi + Vue/nginx + Loki + Grafana)

## 编码规范

### C#
- 命名空间: `JeeSiteNET.{Layer}.{Module}`
- 类命名: PascalCase
- 接口前缀: `I`
- 异步方法: Async 后缀
- 文件范围命名空间

### TypeScript/Vue
- 组件: PascalCase
- 文件/变量: camelCase
- 类型定义: PascalCase + `Type` 后缀

## 项目结构 (新建模块时遵守)

```
JeeSiteNET.Modules.{Name}/
├── Domain/           # 领域实体
├── Application/      # 应用服务 + DTO
├── Infrastructure/   # EF Core + 仓储
└── Controllers/      # API 控制器
```

## 关键约定

- 统一响应: `ApiResult<T>` { Code, Message, Data }
- 分页: `PageRequest<T>` → `PageResult<T>`
- API 路由: `api/v1/{module}/{controller}`
- 数据库表: `{Prefix}_{Name}` (如 Sys_User)
- 主键: 有意义的编码（非 UUID）

## JeeSite5 功能完全度

| 功能 | 状态 |
|---|---|
| 用户/角色/菜单/机构/公司/岗位/区域/字典/配置/模块管理 | ✅ |
| 日志/审计/在线用户/缓存/系统监控 | ✅ |
| 消息(收件/发件/模板/推送队列) | ✅ |
| 国际化/业务分类/文件管理 | ✅ |
| 员工管理(EmpUser/EmpOffice/EmpPost) | ✅ |
| 角色数据/字段权限 | ✅ |
| CMS(站点/栏目/文章/评论/留言/标签/访问统计/举报) | ✅ |
| 代码生成(多DB introspect + generate/download ZIP) | ✅ |
| 任务调度(Quartz.NET) | ✅ |
| 工作流(Elsa 3.x 请假审批) | ✅ |
| OAuth2(GitHub/WeChat/DingTalk) + CAS + LDAP | ✅ |
| AI Chat(DeepSeek/OpenAI) | ✅ |
| OSS 存储(local/S3/MinIO/Aliyun) | ✅ |
| Elasticsearch 全文搜索 | ✅ |
| Excel 导入导出 | ✅ |
| 密码强度校验/PwdUpdateRecord 历史 | ✅ |
| 多设备登录控制(Token吊销) | ✅ |
| 报表统计/访问日志 | ✅ |
| Docker Compose 全服务部署 | ✅ |

**Parity: 100%** — 全部 19 项核心 + 6 项边缘工具类已补齐

## 深度对比验证 (JeeSite5 源码 D:\Projects\jeesite5-v5.springboot3)

| 分析报告中提及的"差异项" | 实际状态 | 说明 |
|---|---|---|
| CAS 认证 | ✅ 已实现 | `CasAuthController`, `CasAuthUtil` |
| LDAP 认证 | ✅ 已实现 | `LdapAuthController`, `LdapAuthUtil` |
| MCP Server | ✅ 已实现 | `McpController` (JSON-RPC 2.0) |
| SM3 国密 | ✅ 已实现 | `EncryptUtil.Sm3()` |
| 拼音转换 | ✅ 已实现 | `PinyinUtil` |
| 身份证验证 | ✅ 已实现 | `IdcardUtil` |
| Word 导出 | ✅ 已实现 | `WordUtil` |
| 密码强度 | ✅ 已实现 | `PasswordStrengthUtil` |
| 验证码 | ✅ 已实现 | `CaptchaUtil` |
| 数据权限/字段权限 | ✅ 已实现 | DataScopeService, FieldScopeService |
| Excel 导入导出 | ✅ 已实现 | ExcelService, ExcelFieldAttribute |
| **AI Tools 框架** | ✅ 已实现 | `AiToolAttribute` + `IAiTool` + `AiToolRegistry` + 3 内置工具, 集成 DeepSeek Function Calling |
| **SM2/SM4 加密** | ✅ 已实现 | `Sm2Util` (ECDSA签名/验签+混合加密) + `Sm4Util` (CBC模式, S盒+密钥扩展) |
| **UserAgent 解析** | ✅ 已实现 | `UserAgentUtil` 浏览器/OS/设备检测 |
| **DiffMatchPatch** | ✅ 已实现 | `DiffMatchPatch` 文本差异对比 + 补丁应用 |
| **ImageGeoUtils** | ✅ 已实现 | `ImageGeoUtil` SkiaSharp 图片 GPS 信息提取 |
| **VideoUtils** | ✅ 已实现 | `VideoUtil` FFmpeg 截图/转码/信息读取 |

**功能完全度: 100%** — 全部 19 项核心 + 6 项边缘工具类已补齐

## 命令速查

- `dotnet build` — 编译
- `dotnet test` — 运行测试
- `dotnet ef migrations add {Name}` — 添加迁移
- `dotnet ef migrations add {Name} -- "{ConnStr}"` — 指定连接字符串 (默认 SqlServer)
- `dotnet ef migrations add {Name} -- "{ConnStr}" "PostgreSQL"` — 指定 provider (PostgreSQL/Dm/KingbaseES)
- `dotnet ef database update` — 更新数据库
- `docker compose up -d` — 启动全部 Docker 容器
- `docker compose down` — 停止全部 Docker 容器
- `docker compose build webapi` — 单独构建 WebApi 镜像
- `docker compose up -d minio elasticsearch` — 启动存储和搜索服务
- `docker compose run --rm minio-setup` — 手动创建 MinIO bucket (自动部署时自动执行)
- `curl http://localhost:9200/_cluster/health` — 验证 ES 集群状态
- `curl http://localhost:9000/minio/health/live` — 验证 MinIO 健康状态
