# JeeSite.NET

基于 .NET 10 的企业级快速开发平台 — JeeSite 的 .NET 生态移植与重构。

## 项目定位

参考 JeeSite 5 的架构设计理念，在 .NET 技术生态上构建可扩展、模块化的企业级应用开发平台。
遵循 **微内核 + 插件架构**、**松耦合设计**、**低代码能力** 三大核心原则。

## 技术栈概要

| 层级 | 技术选型 |
|---|---|
| 运行时 | .NET 10 LTS |
| 后端框架 | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| 身份认证 | ASP.NET Core Identity + JWT |
| 缓存 | FusionCache (Memory + Redis) |
| 消息队列 | RabbitMQ + MassTransit |
| 工作流 | Elsa Workflows 3.x |
| 前端(分离) | Vue 3 + Vite + Ant Design Vue 4 + TypeScript |
| 前端(全栈) | Blazor / Razor Pages + Bootstrap 5 |
| 数据库 | SQL Server / MySQL / PostgreSQL |

## 文档导航

- [架构分析报告](docs/01-分析调研/01-JeeSite架构分析报告.md) — JeeSite 5 架构深度分析
- [技术映射](docs/01-分析调研/02-Java与DotNet技术映射.md) — Java ↔ .NET 技术对照
- [总体架构设计](docs/02-架构设计/01-系统总体架构设计.md)
- [分层架构设计](docs/02-架构设计/02-分层架构设计.md)
- [开发规范](docs/04-开发规范) — 完整开发标准体系
- [开发路线图](docs/05-开发计划/01-总体开发路线图.md)

## 项目结构

```
JeeSite.NET.sln
├── src/
│   ├── JeeSiteNET.Core/                # 核心层
│   ├── JeeSiteNET.Infrastructure/       # 基础设施
│   ├── JeeSiteNET.Web.Api/             # Web API
│   └── JeeSiteNET.Web.Host/            # 全栈版
├── modules/
│   ├── JeeSiteNET.Modules.Sys/         # 系统管理模块
│   └── ...
├── frontend/
│   └── jeesite-vue/                    # Vue 3 分离前端
├── docs/                               # 架构文档
└── templates/                          # 代码生成模板
```

## 许可

Apache License 2.0
