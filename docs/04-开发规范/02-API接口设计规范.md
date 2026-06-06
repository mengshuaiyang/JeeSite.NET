# API 接口设计规范

## 1. 总体规范

- 协议: HTTPS (生产环境强制)
- 格式: JSON (UTF-8)
- 版本: URL 路径版本 `/api/v1/{module}/{controller}`
- 字符编码: UTF-8
- 认证: JWT Bearer Token

## 2. URL 命名规范

```
/api/v1/{module}/{controller}/{action}

示例:
/api/v1/sys/user/list              # 用户列表
/api/v1/sys/user/get               # 获取单个用户
/api/v1/sys/user/form              # 用户表单 (新增/编辑)
/api/v1/sys/user/save              # 保存用户
/api/v1/sys/user/delete            # 删除用户
/api/v1/sys/user/enable            # 启用
/api/v1/sys/user/disable           # 停用
/api/v1/sys/user/check-login-code   # 校验登录名唯一性
/api/v1/sys/user/export            # 导出
/api/v1/sys/user/import            # 导入
/api/v1/sys/user/tree-data         # 树结构数据
/api/v1/sys/role/assign-user       # 分配用户
```

## 3. 统一响应格式

### 3.1 成功响应

```json
{
  "code": 200,
  "message": "操作成功",
  "data": { ... }
}
```

### 3.2 错误响应

```json
{
  "code": 400,
  "message": "参数错误: 用户名不能为空",
  "data": null
}
```

### 3.3 分页响应

```json
{
  "code": 200,
  "message": "操作成功",
  "data": {
    "list": [ ... ],
    "total": 100,
    "pageNo": 1,
    "pageSize": 20
  }
}
```

### 3.4 状态码定义

```csharp
public enum ApiResultCode
{
    Success         = 200,    // 操作成功
    Created         = 201,    // 创建成功
    NoContent       = 204,    // 删除成功
    BadRequest      = 400,    // 参数错误
    Unauthorized    = 401,    // 未认证
    Forbidden       = 403,    // 无权限
    NotFound        = 404,    // 资源不存在
    Conflict        = 409,    // 数据冲突
    InternalError   = 500,    // 服务器内部错误
}
```

## 4. 请求规范

### 4.1 分页请求

```json
// POST /api/v1/sys/user/list
{
  "pageNo": 1,
  "pageSize": 20,
  "sortField": "createDate",
  "sortOrder": "desc",
  "userName": "张",          // 查询条件
  "userType": "employee",
  "status": "0"
}
```

### 4.2 保存请求

```json
// POST /api/v1/sys/user/save
{
  "userCode": "",              // 空=新增, 非空=更新
  "loginCode": "zhangsan",
  "userName": "张三",
  "userType": "employee",
  "email": "zhangsan@example.com",
  "phone": "13800138000",
  "orgCode": "370000",
  "roleCodes": ["role_admin", "role_user"]
}
```

## 5. 权限验证

```csharp
// Controller 方法级别权限
[Permission("sys:user:list")]    // 列表查看权限
[Permission("sys:user:edit")]    // 编辑权限
[Permission("sys:user:delete")]  // 删除权限
```

## 6. 文件上传

```http
POST /api/v1/file/upload
Content-Type: multipart/form-data

file: <binary>
bucket: avatar
directory: /2026/06
```

## 7. 错误处理

```csharp
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = context.Exception switch
        {
            ValidationException ex => ApiResult.Fail(400, ex.Message),
            UnauthorizedAccessException => ApiResult.Fail(401, "未登录或登录已过期"),
            PermissionDeniedException => ApiResult.Fail(403, "无操作权限"),
            NotFoundException ex => ApiResult.Fail(404, ex.Message),
            BusinessException ex => ApiResult.Fail(400, ex.Message),
            _ => ApiResult.Fail(500, "服务器内部错误")
        };

        context.Result = new JsonResult(response);
        context.ExceptionHandled = true;
    }
}
```
