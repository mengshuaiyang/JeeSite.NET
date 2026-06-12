<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 31. API 接口规范

> 本文档介绍 JeeSite.NET 的 RESTful API 接口规范、统一响应格式、错误码与请求示例。
> 最后更新: 2026-06-12

---

## 🌐 基础信息

### 基础 URL

```
http://localhost:5000/api/v1
```

### 统一响应格式

所有 API 返回统一格式：

```json
{
  "Code": 200,
  "Message": "操作成功",
  "Data": { ... }
}
```

### 错误码规范

| 错误码 | 含义 |
|--------|------|
| 200 | 成功 |
| 400 | 请求参数错误 |
| 401 | 未登录或登录过期 |
| 403 | 无操作权限 |
| 404 | 资源不存在 |
| 500 | 服务器内部错误 |

### 请求头

```
Content-Type: application/json
Authorization: Bearer <token>
```

---

## 🔐 认证接口

### 登录

**POST** `/api/v1/sys/auth/login`

请求体：

```json
{
  "loginCode": "admin",
  "password": "admin",
  "rememberMe": false
}
```

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| loginCode | string | 是 | 登录账号 |
| password | string | 是 | 密码 |
| rememberMe | bool | 否 | 是否记住我 |

响应示例：

```json
{
  "Code": 200,
  "Message": "操作成功",
  "Data": {
    "userCode": "admin",
    "userName": "管理员",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expireTime": "2026-06-12T14:00:00Z"
  }
}
```

### 退出登录

**POST** `/api/v1/sys/auth/logout`

### 短信/邮件验证码登录

**POST** `/api/v1/sys/auth/login-by-code`

请求体：

```json
{
  "phoneOrEmail": "admin@example.com",
  "code": "123456"
}
```

---

## 👤 用户管理接口

### 获取用户列表（分页）

**GET** `/api/v1/sys/user/page`

请求参数：

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| pageNo | int | 否 | 页码，默认 1 |
| pageSize | int | 否 | 每页条数，默认 20 |
| userCode | string | 否 | 用户编码（模糊查询） |
| userName | string | 否 | 用户名称（模糊查询） |
| status | string | 否 | 状态过滤 |

响应示例：

```json
{
  "Code": 200,
  "Message": "操作成功",
  "Data": {
    "List": [
      {
        "userCode": "admin",
        "userName": "管理员",
        "email": "admin@example.com",
        "phone": "13800138000",
        "status": "0",
        "createDate": "2026-01-01T10:00:00Z"
      }
    ],
    "Total": 1,
    "PageNo": 1,
    "PageSize": 20
  }
}
```

### 获取用户详情

**GET** `/api/v1/sys/user/{userCode}`

### 创建用户

**POST** `/api/v1/sys/user`

请求体：

```json
{
  "userCode": "user001",
  "userName": "张三",
  "password": "Abc123456",
  "email": "zhangsan@example.com",
  "phone": "13800138001",
  "status": "0",
  "orgCode": "dept001",
  "roleCodes": ["role001", "role002"]
}
```

### 更新用户

**PUT** `/api/v1/sys/user/{userCode}`

请求体：

```json
{
  "userName": "张三",
  "email": "zhangsan@example.com",
  "phone": "13800138001",
  "status": "0"
}
```

### 删除用户

**DELETE** `/api/v1/sys/user/{userCode}`

### 批量删除用户

**DELETE** `/api/v1/sys/user/batch`

请求体：

```json
{
  "userCodes": ["user001", "user002"]
}
```

---

## 🎭 角色管理接口

### 获取角色列表

**GET** `/api/v1/sys/role/page`

| 参数 | 类型 | 说明 |
|------|------|------|
| pageNo | int | 页码 |
| pageSize | int | 每页条数 |
| roleCode | string | 角色编码过滤 |
| roleName | string | 角色名称过滤 |

### 获取角色详情

**GET** `/api/v1/sys/role/{roleCode}`

### 创建角色

**POST** `/api/v1/sys/role`

```json
{
  "roleCode": "admin",
  "roleName": "超级管理员",
  "description": "系统超级管理员",
  "status": "0",
  "menuCodes": ["menu001", "menu002"],
  "dataScopeType": "ALL"
}
```

### 更新角色

**PUT** `/api/v1/sys/role/{roleCode}`

### 删除角色

**DELETE** `/api/v1/sys/role/{roleCode}`

---

## 📋 菜单管理接口

### 获取菜单列表

**GET** `/api/v1/sys/menu`

| 参数 | 类型 | 说明 |
|------|------|------|
| parentCode | string | 父菜单编码（返回树形结构） |

### 获取菜单详情

**GET** `/api/v1/sys/menu/{menuCode}`

### 创建菜单

**POST** `/api/v1/sys/menu`

```json
{
  "menuCode": "menu001",
  "menuName": "系统管理",
  "parentCode": "",
  "menuType": "M",
  "url": "/sys",
  "icon": "setting",
  "sortNo": 1,
  "status": "0",
  "permissions": ["sys:user:view", "sys:user:edit"]
}
```

---

## 📚 字典管理接口

### 获取字典类型列表

**GET** `/api/v1/sys/dict/type`

### 获取字典数据列表

**GET** `/api/v1/sys/dict/data/{dictType}`

### 创建字典类型

**POST** `/api/v1/sys/dict/type`

```json
{
  "dictType": "sys_user_status",
  "dictName": "用户状态",
  "description": "用户状态枚举"
}
```

### 创建字典数据

**POST** `/api/v1/sys/dict/data`

```json
{
  "dictType": "sys_user_status",
  "dictValue": "0",
  "dictLabel": "正常",
  "sortNo": 1
}
```

---

## 📄 内容管理接口

### 获取文章列表

**GET** `/api/v1/cms/article/page`

| 参数 | 类型 | 说明 |
|------|------|------|
| pageNo | int | 页码 |
| pageSize | int | 每页条数 |
| categoryCode | string | 栏目编码 |
| title | string | 标题（模糊查询） |
| status | string | 状态 |

### 获取文章详情

**GET** `/api/v1/cms/article/{articleId}`

### 创建文章

**POST** `/api/v1/cms/article`

```json
{
  "categoryCode": "news",
  "title": "文章标题",
  "summary": "文章摘要",
  "content": "<p>文章内容</p>",
  "author": "作者",
  "status": "0",
  "tags": ["tag1", "tag2"]
}
```

### 更新文章

**PUT** `/api/v1/cms/article/{articleId}`

### 删除文章

**DELETE** `/api/v1/cms/article/{articleId}`

---

## 🔄 任务调度接口

### 获取任务列表

**GET** `/api/v1/tasks/job/page`

### 创建任务

**POST** `/api/v1/tasks/job`

```json
{
  "jobName": "数据同步任务",
  "jobGroup": "default",
  "cronExpression": "0 0 2 * * ?",
  "jobClass": "JeeSiteNET.Modules.Tasks.Jobs.SyncJob",
  "params": "{\"source\": \"db\", \"target\": \"redis\"}",
  "status": "0"
}
```

### 立即执行

**POST** `/api/v1/tasks/job/{jobId}/run`

### 暂停 / 恢复任务

**POST** `/api/v1/tasks/job/{jobId}/pause`
**POST** `/api/v1/tasks/job/{jobId}/resume`

---

## 💡 代码生成接口

### 获取生成表列表

**GET** `/api/v1/codegen/table/page`

### 创建生成表配置

**POST** `/api/v1/codegen/table`

```json
{
  "tableName": "sys_test",
  "tableComment": "测试表",
  "moduleName": "Sys",
  "className": "Test",
  "baseClassName": "DataEntity",
  "columns": [
    {
      "columnName": "id",
      "columnComment": "主键",
      "dataType": "string",
      "isPrimaryKey": true,
      "isRequired": true
    }
  ]
}
```

### 生成代码

**POST** `/api/v1/codegen/generate`

```json
{
  "tableId": "1",
  "generateTypes": ["entity", "service", "controller", "dto", "vue"]
}
```

### 下载 ZIP

**GET** `/api/v1/codegen/table/download`

---

## 📲 应用管理接口

### 获取升级列表

**GET** `/api/v1/app/upgrade/page`

### 创建升级记录

**POST** `/api/v1/app/upgrade`

```json
{
  "version": "1.0.1",
  "description": "修复 bug",
  "downloadUrl": "https://example.com/app.zip",
  "forceUpdate": false
}
```

### 获取反馈列表

**GET** `/api/v1/app/comment/page`

### 创建反馈

**POST** `/api/v1/app/comment`

```json
{
  "content": "建议增加 XX 功能",
  "contact": "user@example.com",
  "appVersion": "1.0.0"
}
```

---

## 📝 请求示例

### cURL

```bash
# 登录
curl -X POST http://localhost:5000/api/v1/sys/auth/login \
  -H "Content-Type: application/json" \
  -d '{"loginCode": "admin", "password": "admin"}'

# 获取用户列表
curl -X GET http://localhost:5000/api/v1/sys/user/page?pageNo=1&pageSize=20 \
  -H "Authorization: Bearer <token>"

# 创建用户
curl -X POST http://localhost:5000/api/v1/sys/user \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"userCode": "user001", "userName": "张三", "password": "Abc123456"}'
```

### TypeScript / JavaScript

```typescript
import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5000/api/v1'
})

// 设置 token
api.interceptors.request.use(config => {
  config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`
  return config
})

// 登录
const login = async (loginCode: string, password: string) => {
  const response = await api.post('/sys/auth/login', { loginCode, password })
  return response.data
}

// 获取用户列表
const getUserList = async (pageNo: number, pageSize: number) => {
  const response = await api.get('/sys/user/page', {
    params: { pageNo, pageSize }
  })
  return response.data
}
```

---

## 📖 相关文档

- [快速入门](01-快速入门) — 搭建开发环境
- [系统架构概览](02-系统架构概览) — 架构设计
- [开发规范与最佳实践](30-开发规范与最佳实践) — 编码规范
- [Sys系统管理模块](03-Sys系统管理模块) — 核心业务模块

---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>
