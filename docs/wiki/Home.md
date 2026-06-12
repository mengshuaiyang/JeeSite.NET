# JeeSite.NET Wiki 文档中心

> JeeSite.NET —— 基于 .NET 10 / ASP.NET Core / EF Core / Vue 3 / Ant Design Vue 的企业级快速开发平台 Wiki。
> 最后更新：2026-06-12

---

## 🚀 快速入门（从这里开始）

- **[快速开始](../QUICKSTART.md)** — 5 分钟启动本地开发环境（数据库 / Redis / 前后端）
- **[系统操作说明](../系统操作说明.md)** — 管理员和业务用户操作手册（用户 / 角色 / 菜单 / 机构 / 内容管理）
- **[部署文档](../07-部署运维/部署文档.md)** — 生产环境部署（IIS / Kestrel / Docker / Nginx / Linux）

---

## 📚 模块技术手册（每个模块一份完整文档）

### 系统管理（Sys）
- **[Sys 模块技术手册](./01-模块手册/01-Sys模块技术手册.md)**
  - 用户 / 角色 / 菜单 / 机构 / 公司 / 岗位 / 区域 / 字典 / 配置 / 模块管理
  - 消息 / 审计 / 日志 / 在线用户 / 缓存 / 监控 / 文件管理
  - **三层权限体系**：菜单权限 + 数据权限 + 字段权限

### 内容管理（CMS）
- **[CMS 模块技术手册](./01-模块手册/02-Cms模块技术手册.md)**
  - 站点 / 栏目 / 文章 / 标签 / 评论 / 留言 / 举报 / 访问统计
  - AI 智能问答与向量库
  - 富文本编辑器与 HTML 清洗

### 代码生成（CodeGen）
- **[CodeGen 模块技术手册](./01-模块手册/03-CodeGen模块技术手册.md)**
  - 数据库表导入 / 列配置 / 模板生成 / 前后端代码生成
  - Scriban 模板自定义 / Excel 导入导出模板

### 任务调度（Tasks）
- **[Tasks 任务调度模块技术手册](./01-模块手册/04-Tasks任务调度模块技术手册.md)**
  - Quartz.NET / Cron 表达式 / 自定义 Job / 分布式任务
  - 任务执行日志 / 失败自动重试 / 手动触发

### 工作流（BPM）
- **[BPM 工作流模块技术手册](./01-模块手册/05-Bpm工作流模块技术手册.md)**
  - Elsa 工作流引擎 / 请假审批 / 审批记录
  - 工作流模板 / 表单绑定 / 权限控制

### 移动端（App）
- **[App 移动端模块技术手册](./01-模块手册/06-App模块技术手册.md)**
  - 意见反馈 / 版本升级管理 / 推送通知

---

## 🧰 工具类手册

- **[加密与国密](./02-工具类手册/01-加密与国密工具.md)**
  - AES / RSA / MD5 / SHA / SM2 / SM3 / SM4 封装
  - 证书管理 / 数字签名 / 国密合规

- **[文件与媒体处理](./02-工具类手册/02-文件与媒体处理工具.md)**
  - FFmpeg 音视频转码 / ImageGeo 图像地理信息 / 文件安全扫描
  - 分片上传 / 文件存储抽象（本地 / OSS / MinIO）
  - 文档预览（Office → PDF / HTML）

- **[文本与差异处理](./02-工具类手册/03-文本与差异处理工具.md)**
  - DiffMatchPatch 文本差异 / 拼音转换 / 身份证校验 / Levenshtein 距离

- **[富文本与 HTML 清洗](./02-工具类手册/04-富文本与HTML清洗工具.md)**
  - HtmlSanitizerUtil 白名单清洗 / XSS 防护 / 恶意标签过滤

- **[验证码与识别](./02-工具类手册/05-验证码与识别工具.md)**
  - 图形验证码（滑动 / 旋转 / 算术）/ User-Agent 解析 / 条码 & 二维码生成

- **[Excel 导入导出](./02-工具类手册/06-Excel导入导出工具.md)**
  - ExcelService + ExcelFieldAttribute + 自定义字段类型体系
  - 百万级数据流式导出 / 导入模板自动生成

---

## 🔐 安全与认证

- **[JWT 认证机制](./03-安全与认证/01-JWT认证机制.md)**
  - 完整登录 / 刷新 / 吊销流程与权限标识系统
  - Access Token + Refresh Token + JWT 黑名单

- **[OAuth2 第三方登录](./03-安全与认证/02-OAuth2第三方登录.md)**
  - GitHub / 微信 / 钉钉 / 企业微信 / Gitee 集成

- **[CAS 单点登录](./03-安全与认证/03-CAS单点登录.md)**
  - Apereo CAS Protocol 3.0 服务端与客户端集成

- **[LDAP 认证](./03-安全与认证/04-LDAP认证.md)**
  - Active Directory / OpenLDAP 对接

- **[数据权限与字段权限](./03-安全与认证/05-数据权限与字段权限.md)**
  - 三层权限体系（菜单 / 数据 / 字段）
  - 组织机构数据权限（本人 / 本部门 / 本部门及下级 / 全部）

---

## ✨ 高级特性

- **[AI 智能问答与向量库](./04-高级特性/01-AI智能问答与向量库.md)**
  - RAG（检索增强生成） / 向量检索 / LLM 集成（DeepSeek / OpenAI / Azure OpenAI）
  - 文章向量化（pgvector / Qdrant）/ 智能问答 UI / AI 写作助手

- **[MCP 服务协议](./04-高级特性/02-MCP服务协议.md)**
  - JSON-RPC 2.0 协议与 AI Tools 框架
  - 工具自动发现 / 参数校验 / 审计日志 / 速率限制

- **[Elasticsearch 全文搜索](./04-高级特性/03-Elasticsearch全文搜索.md)**
  - 中文分词（IK Analyzer）/ 同义词 / 词库热更新
  - 索引管理（实时索引 / 定时索引 / 全量重建 / 冷热分离）
  - 搜索建议（suggest）/ 聚合桶 / 热门搜索统计

- **[FusionCache 缓存架构](./04-高级特性/04-FusionCache缓存架构.md)**
  - 双层缓存（Memory L1 + Redis L2）
  - 失效广播（Pub/Sub）
  - 穿透 / 击穿 / 雪崩防护
  - 缓存预热 / fail-safe

- **[前端路由权限与按钮权限](./04-高级特性/05-前端路由权限与按钮权限.md)**
  - `v-permission` 指令 / `usePermission` composable
  - 路由级权限（router beforeEach guard）
  - 按钮级权限（v-permission / v-if）
  - 字段级权限（动态 Form Schema）

---

## 📖 项目架构与规范文档

- **[架构设计文档](../02-架构设计/)** — 系统总体架构 / 分层架构 / 模块化架构 / 数据库架构 / 多租户设计
- **[技术选型文档](../03-技术选型/)** — 后端 / 前端 / 中间件 / 数据库
- **[开发规范文档](../04-开发规范/)** — 代码规范 / API 接口规范 / 数据库规范 / 前端规范 / 项目结构规范
- **[对比分析与开发计划](../对比分析与开发计划.md)** — 与 JeeSite5 功能完全度对比

---

## ⚠️ 故障排查与 FAQ

- **[系统操作说明 — 故障排查章节](../系统操作说明.md#十三故障排查)**
- **[功能完全度最终对照报告](../06-过程记录/05-功能完全度最终对照报告（2026-06-12）.md)**

---

## 📌 项目代码快速索引

| 目录 | 说明 |
|------|------|
| `src/JeeSiteNET.Core/Utils` | 工具类（42+） |
| `src/JeeSiteNET.Core/UEditor` | UEditor 协议 |
| `src/JeeSiteNET.Infrastructure` | EF Core / 仓储 / 拦截器 / 数据库迁移 |
| `src/JeeSiteNET.Web.Api` | Web API 主机 / 中间件 / `Program.cs` |
| `modules/JeeSiteNET.Modules.Sys` | 系统管理模块 |
| `modules/JeeSiteNET.Modules.Cms` | 内容管理模块 |
| `modules/JeeSiteNET.Modules.CodeGen` | 代码生成模块 |
| `modules/JeeSiteNET.Modules.Tasks` | 任务调度模块 |
| `modules/JeeSiteNET.Modules.Bpm` | 工作流模块 |
| `modules/JeeSiteNET.Modules.App` | 移动端模块 |
| `frontend/src/views` | 前端页面（Vue 3 SFC） |
| `frontend/src/api` | 前端 API 封装（axios） |
| `frontend/src/stores` | 前端状态管理（Pinia） |
| `frontend/src/router` | 前端路由（Vue Router + 动态菜单） |
| `frontend/src/composables` | Vue 3 组合式函数（usePermission 等） |
| `frontend/src/directives` | Vue 3 指令（v-permission 等） |

---

## 📅 文档更新日志

- **2026-06-12**：创建 Wiki 首页索引 / 模块手册 6 篇 / 工具类手册 6 篇 / 安全与认证 5 篇 / 高级特性 5 篇，总计 **22 篇**文档

---

> 🔗 **JeeSite.NET 项目位置**：`d:\Projects\jeesite.net`
> 📝 **Wiki 位置**：`d:\Projects\jeesite.net\docs\wiki\`
