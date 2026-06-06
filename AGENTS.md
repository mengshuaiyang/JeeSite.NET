# AGENTS.md — JeeSite.NET 项目配置

本文件为 AI 辅助开发提供项目级上下文配置。

## 项目概述

- 项目名称: JeeSite.NET
- 描述: 基于 .NET 10 的企业级快速开发平台，参考 JeeSite 5 架构
- 技术栈: .NET 10, ASP.NET Core, EF Core, Vue 3, Ant Design Vue
- 包管理: NuGet + npm/pnpm
- 缓存: FusionCache 2.6 (L1 Memory + L2 Redis)
- 部署: Docker Compose (SQL Server + Redis + WebApi + Vue/nginx)

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

## 命令速查

- `dotnet build` — 编译
- `dotnet test` — 运行测试
- `dotnet ef migrations add {Name}` — 添加迁移
- `dotnet ef database update` — 更新数据库
- `docker compose up -d` — 启动全部 Docker 容器
- `docker compose down` — 停止全部 Docker 容器
- `docker compose build webapi` — 单独构建 WebApi 镜像
