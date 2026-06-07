# JeeSite.NET 快速开始

## 前置条件

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20 LTS+](https://nodejs.org/)
- [pnpm](https://pnpm.io/installation)
- [SQL Server 2022+](https://www.microsoft.com/sql-server)（或 Docker）
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)（可选，用于 Redis）

## 5 分钟启动

### 选项 A: Docker Compose（推荐，无需本地安装数据库）

```bash
# 克隆仓库后直接运行
docker compose up -d

# 等所有服务就绪后访问
# 前端: http://localhost:3000
# API:  http://localhost:5000
# Swagger: http://localhost:5000/swagger

# 默认登录: admin / admin
```

### 选项 B: 本地开发

#### 1. 启动 Redis（可选）

```bash
docker run -d --name redis -p 6379:6379 redis:7-alpine
```

#### 2. 启动后端

```bash
# 数据库连接串默认使用本地 SQL Server（Windows 集成认证）
# 如需修改，编辑 src/JeeSiteNET.Web.Api/appsettings.Development.json

dotnet run --project src/JeeSiteNET.Web.Api

# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

#### 3. 启动前端

```bash
cd frontend
pnpm install
pnpm dev

# 前端: http://localhost:5173
```

#### 4. 登录

打开 http://localhost:5173，使用 `admin / admin` 登录。

## 项目结构

```
jeesite.net/
├── src/                          # 后端核心
│   ├── JeeSiteNET.Core/          #   核心库 (实体、安全、缓存、工具)
│   ├── JeeSiteNET.Infrastructure/ #   EF Core、拦截器、数据权限
│   └── JeeSiteNET.Web.Api/       #   Web API 主机
├── modules/                      # 业务模块
│   ├── Sys/                      #   系统管理 (用户/角色/菜单/字典/文件)
│   ├── Cms/                      #   内容管理 (栏目/文章/评论)
│   ├── CodeGen/                  #   代码生成 (表导入/Scriban 模板)
│   ├── Tasks/                    #   定时任务 (Quartz.NET)
│   ├── Bpm/                      #   工作流 (规划中)
│   └── App/                      #   移动端 API
├── frontend/                     # Vue 3 + Ant Design Vue 4
└── docs/                         # 项目文档
```

## 常用命令

```bash
# 后端编译
dotnet build

# 后端测试
dotnet test

# 数据库迁移
dotnet ef migrations add {Name}
dotnet ef database update

# 前端构建
cd frontend && pnpm build

# Docker 部署
docker compose up -d
```

## 了解更多

- [部署运维指南](/docs/07-部署运维/部署文档.md)
- [Swagger API 文档](http://localhost:5000/swagger)（开发/Docker 环境）
- [项目文档汇总](/docs/)
