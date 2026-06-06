# JeeSite 5 架构分析报告

> 分析日期: 2026-06-06
> 分析来源: https://jeesite.com/docs/ (官方文档)
> JeeSite 版本: v5.17.1

## 1. 平台概述

JeeSite 是基于 Spring Boot 的 Java 快速开发平台，低代码、轻量级，是一款企业级快速开发解决方案。
后端基于 Spring Boot + Shiro + MyBatis，前端支持 Vue3 分离版和 Beetl+Bootstrap 经典版两套技术栈。

### 核心理念

- **大道至简**: 把握设计的"度"，避免过度设计
- **微内核 + 插件架构**: 核心小而精，功能以模块插件形式扩展
- **松耦合设计**: 模块可独立增减、独立部署
- **不侵入、可扩展**: 封装但不限制，提供扩展接口
- **一套后端支撑两套前端**: 同一服务支撑分离版和全栈版

## 2. 分层架构分析

### 2.1 总体架构

```
┌─────────────────────────────────────────────┐
│              表现层 (Web)                     │
│  ┌─────────────────┐  ┌──────────────────┐   │
│  │  分离版 (Vue3)   │  │ 全栈版 (Beetl)   │   │
│  └────────┬────────┘  └────────┬─────────┘   │
│           │ REST API           │ MVC+Beetl    │
├───────────┴────────────────────┴─────────────┤
│           控制层 (Controller)                  │
│        @RequiresPermissions 权限拦截           │
├──────────────────────────────────────────────┤
│           业务层 (Service)                     │
│     CrudService / TreeService 泛型基类         │
│     数据权限过滤 + 事务管理                     │
├──────────────────────────────────────────────┤
│           持久层 (Dao / MyBatis)               │
│     @Table / @Column / @JoinTable 注解体系     │
│     自动生成CRUD SQL + 查询条件                  │
├──────────────────────────────────────────────┤
│              基础设施                            │
│  Shiro Auth  │  J2Cache  │  Druid  │  Quartz  │
└──────────────────────────────────────────────┘
```

### 2.2 关键分层设计

**Entity 层**
- `BaseEntity<T>`: 基础实体，含 `createBy`/`createDate`/`updateBy`/`updateDate`/`status`/`remarks`
- `DataEntity<T>`: 继承 BaseEntity，含 isNewRecord/delFlag
- `TreeEntity<T>`: 继承 DataEntity，树结构实体，含辅助字段体系

**Service 层**
- `CrudService<D,T>`: 通用 CRUD 泛型基类
- `TreeService<D,T>`: 树表 CRUD 泛型基类，自动维护辅助字段
- `QueryService<D,T>`: 只读查询基类

**Dao 层**
- `CrudDao<T>`: 通用 CRUD 接口
- `TreeDao<T>`: 树表 DAO 接口
- MyBatis Mapper XML 只处理复杂查询，基础 CRUD 由注解自动生成

**Controller 层**
- `BaseController`: 基础控制器，自动绑定验证、国际化、日志

## 3. 核心机制分析

### 3.1 @Table 注解体系

独创的 MyBatis 增强注解，定义在类头上，可一览物理表结构:

```java
@Table(name="js_sys_user", alias="a", columns={
    @Column(includeEntity=DataEntity.class),
    @Column(name="user_code", attrName="userCode", label="用户编码", isPK=true),
    @Column(name="login_code", attrName="loginCode", label="登录名", queryType=QueryType.LIKE),
    @Column(name="user_name", attrName="userName", label="用户名称"),
    @Column(name="email", attrName="email", label="电子邮箱"),
    @Column(name="phone", attrName="phone", label="手机号码"),
    @Column(name="user_type", attrName="userType", label="用户类型"),
}, orderBy="a.user_code"
)
public class User extends DataEntity<User> {
    private String userCode;
    private String loginCode;
    private String userName;
    // ... getter/setter
}
```

**设计要点**:
- `includeEntity`: 继承父类字段配置
- `queryType`: 自动生成查询条件（LIKE/EQ/BETWEEN 等）
- `isPK`: 主键标记
- `attrName`: 属性名与字段名映射
- 80% 的基础 SQL 由注解自动生成，无需写 mapper.xml

### 3.2 树表设计 (TreeEntity)

树表通过辅助字段实现高效的树形结构查询，不依赖数据库递归语法:

| 字段 | 说明 | 作用 |
|---|---|---|
| `parent_code` | 节点上级编码 | 指向父节点 |
| `parent_codes` | 所有上级编码 (如 `0,370000,371000,`) | 快速检索所有下级 |
| `tree_sort` | 当前层级排序 (decimal) | 同级排序 |
| `tree_sorts` | 完整排序号 (10位数字组成) | 整树排序 |
| `tree_leaf` | 是否叶子节点 (0/1) | 判断末级 |
| `tree_level` | 节点层次级别 (0开始) | 分级查询/缩进 |
| `tree_names` | 节点全名称 (用"/"分隔) | 快速获取完整路径 |

**核心优势**:
- `parent_codes LIKE '0,370000,%'` — 右LIKE支持索引，查询所有下级
- `tree_level <= 1` — 只查一级和二级
- `tree_sorts ASC` — 整树排序
- 增删改时自动级联维护子节点辅助字段

### 3.3 数据权限机制

**三层权限控制**:
1. **登录认证**: Shiro 身份认证
2. **功能权限**: 菜单 + 按钮，基于 RBAC 的角色-权限-用户
3. **数据权限**: 行级别数据过滤

**数据权限规则**:
- 控制粒度: 全部、本公司、本部门及下属、本部门、仅本人、自定义
- 支持管理权限和拥有权限的区分
- 支持通过界面配置化的数据权限规则
- 通过 `getDataScope()` 注入 SQL 条件实现过滤

### 3.4 模块化架构

**模块发现机制**:
- `spring.factories` 自动配置
- `@Module` 注解标记模块入口
- 模块表 (`js_sys_module`) 记录注册信息

**模块数据库升级**:
- 模块目录下包含 `/db/upgrade/` 升级脚本
- 启动时自动检测版本并执行升级
- 升级脚本按版本号命名，自动排序执行

**模块管理功能**:
- 界面化启用/停用模块
- 菜单与模块挂钩，停用模块时自动停用关联菜单
- 支持代码生成创建新模块工程

### 3.5 代码生成器

**生成模板体系**:
- `crud` — 单表增删改查
- `crud_master` — 主子表
- `crud_tree` — 树表
- `crud_cloud` — 微服务版
- `query` — 仅查询

**生成内容**: Entity / Dao / MapperXML / Service / Controller / HTML 视图 / 菜单SQL

### 3.6 安全设计

**身份认证安全**:
- 登录失败次数 → 显示验证码 → 锁定账号
- 密码加密传输
- 同一设备多地登录控制
- 会话超时管理

**密码策略**:
- 初始密码修改策略
- 密码修改周期管理
- 5级密码强度验证（长度/大写/小写/数字/符号）
- 密码历史不可重复

**审计安全**:
- 操作日志（前后数据diff差异分析）
- 密码审计 / 菜单权限审计 / 用户权限审计
- 三员管理（涉密系统）

### 3.7 缓存体系 J2Cache

两级缓存架构:
- L1: Caffeine (本地内存，极快)
- L2: Redis (分布式共享)
- 解决高并发下 Redis 网络瓶颈
- 支持集群 Session 共享
- 统一 `CacheUtils` 工具类

## 4. 技术栈全景

| 分层 | 技术 | 说明 |
|---|---|---|
| 主框架 | Spring Boot 3.5 | 基础框架 |
| 安全 | Apache Shiro 2.1 | 认证授权 |
| 持久层 | MyBatis 3.5 + @Table | 独创注解增强 |
| 连接池 | Alibaba Druid 1.2 | 监控 + 慢SQL |
| 缓存 | J2Cache (Caffeine + Redis) | 二级缓存 |
| 前端(分离) | Vue3 + Vite + AntdV 4 + TS | 现代前端 |
| 前端(经典) | Beetl + Bootstrap + AdminLTE | 全栈经典 |
| 工作流 | Flowable 7/8 | BPMN2.0 |
| 定时任务 | Quartz | 集群调度 |
| 全文检索 | ElasticSearch 8/9 | 可选 |

## 5. 关键设计原则总结

1. **注解驱动优于代码生成**: @Table注解自动生成SQL，而非反复生成代码
2. **约定优于配置**: 标准化的命名、分层、目录结构
3. **泛型基类消除重复**: CRUD/Service/Controller 大量使用泛型继承
4. **字段冗余换取性能**: 树表辅助字段、数据冗余设计
5. **模块自治**: 每个模块自带数据库升级脚本、国际化资源、静态资源
6. **低代码开放能力**: 在线建表 → 代码生成 → 部署运行
