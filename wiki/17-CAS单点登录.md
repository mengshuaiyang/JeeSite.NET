<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 17 CAS单点登录

> 作为 CAS Client 接入企业 Apereo CAS Server 实现统一身份认证，支持属性映射、自动开户、单点登出。
>
> **适用角色**：系统管理员、架构师
> **阅读时间**：约 10 分钟
> **相关文档**：[15-JWT认证](15-JWT认证) · [18-LDAP认证](18-LDAP认证)
> 最后更新: 2026-06-13

---

## 📋 目录

  - [一、协议概述](#一、协议概述)
    - [1.1 CAS 3.0 serviceValidate 响应示例](#11-cas-30-servicevalidate-响应示例)
  - [二、配置项 (appsettings.json)](#二、配置项-appsettingsjson)
    - [2.1 字段说明](#21-字段说明)
  - [三、核心服务：CasAuthService](#三、核心服务：casauthservice)
    - [3.1 ChallengeAsync(returnUrl)](#31-challengeasyncreturnurl)
    - [3.2 CallbackAsync(ticket, returnUrl)](#32-callbackasyncticket-returnurl)
    - [3.3 LogoutAsync()](#33-logoutasync)
  - [四、控制器端点](#四、控制器端点)
    - [CasAuthController](#casauthcontroller)
    - [4.1 前端按钮示例](#41-前端按钮示例)
  - [五、用户属性映射](#五、用户属性映射)
    - [5.1 字段映射表](#51-字段映射表)
    - [5.2 appsettings.json 中的映射示例](#52-appsettingsjson-中的映射示例)
    - [5.3 自动创建用户的默认角色](#53-自动创建用户的默认角色)
  - [六、常见问题与排错](#六、常见问题与排错)
    - [6.1 ticket not recognized](#61-ticket-not-recognized)
    - [6.2 用户信息为空但 ticket 有效](#62-用户信息为空但-ticket-有效)
    - [6.3 重定向循环](#63-重定向循环)
    - [6.4 SLO（单点登出）不生效](#64-slo（单点登出）不生效)
  - [七、安全注意事项](#七、安全注意事项)

---


本文档描述 JeeSite.NET 如何作为 **CAS Client（Service）** 接入企业内部 Apereo CAS
Server（Protocol 3.0+），实现"登录一次即可访问所有接入 CAS 的业务系统"的 SSO 体验。

---

### 一、协议概述

- **标准**：Apereo CAS Protocol 3.0（兼容 2.0）。
- **角色**：
  - `CAS Server`：企业统一认证中心（典型部署：https://cas.example.com/cas）。
  - `CAS Service (Client)`：JeeSite.NET 应用自身。
  - `Principal`：最终用户。
- **典型场景**：企业内部多系统均接入 CAS；用户访问 JeeSite.NET 时，若尚未在 CAS
  登录，则自动跳转 CAS 登录页面，登录后携带 Ticket 返回 JeeSite.NET。
- **认证流程（简化）**：

```
用户访问 /
  └─> 未登录 → 重定向到 CAS Server /login?service=...
         └─> 用户在 CAS 登录（账号/密码 + 双因素可选）
                └─> CAS Server 签发 ST (Service Ticket)
                       └─> 302 回到 JeeSite.NET /cas/callback?ticket=ST-xxx
                              └─> JeeSite.NET 使用 ST 调用 /p3/serviceValidate
                                     校验 → 解析 XML → 构造本地 JWT
                                            └─> 返回前端首页
```

#### 1.1 CAS 3.0 serviceValidate 响应示例

```xml
<cas:serviceResponse xmlns:cas='http://www.yale.edu/tp/cas'>
  <cas:authenticationSuccess>
    <cas:user>zhangsan</cas:user>
    <cas:attributes>
      <cas:email>zhangsan@example.com</cas:email>
      <cas:displayName>张三</cas:displayName>
      <cas:department>技术中心</cas:department>
      <cas:employeeNumber>10086</cas:employeeNumber>
    </cas:attributes>
  </cas:authenticationSuccess>
</cas:serviceResponse>
```

---

### 二、配置项 (appsettings.json)

```json
{
  "Cas": {
    "Enabled": true,
    "ServerUrl": "https://cas.example.com/cas",
    "ServiceUrl": "https://your-app.com/api/v1/sys/cas/callback",
    "LoginUrl": "https://cas.example.com/cas/login",
    "LogoutUrl": "https://cas.example.com/cas/logout",
    "ValidateUrl": "https://cas.example.com/cas/p3/serviceValidate",
    "DefaultCorpCode": "C001",
    "DefaultOrgCode": "O001",
    "AutoCreateUser": true
  }
}
```

#### 2.1 字段说明

| 字段 | 类型 | 默认值 | 说明 |
|-----|------|-------|------|
| `Enabled` | `bool` | `false` | 是否启用 CAS 登录入口。 |
| `ServerUrl` | `string` | — | CAS Server 根路径。 |
| `ServiceUrl` | `string` | — | JeeSite.NET 回调地址，会作为 `service` 参数签名。 |
| `LoginUrl` | `string` | `{ServerUrl}/login` | CAS 登录页面。 |
| `LogoutUrl` | `string` | `{ServerUrl}/logout` | CAS 登出页面。 |
| `ValidateUrl` | `string` | `{ServerUrl}/p3/serviceValidate` | CAS 3.0 Ticket 校验接口。 |
| `DefaultCorpCode` | `string` | — | 自动创建用户时分配的默认公司。 |
| `DefaultOrgCode` | `string` | — | 自动创建用户时分配的默认机构。 |
| `AutoCreateUser` | `bool` | `true` | CAS 登录成功但本地无账号时，是否自动创建。 |

---

### 三、核心服务：CasAuthService

位于：`JeeSiteNET.Modules.Sys/Application/Services/CasAuthService.cs`

#### 3.1 ChallengeAsync(returnUrl)

```csharp
public string ChallengeAsync(string returnUrl = null)
{
    // service 参数必须与 ServiceUrl 一致，否则 CAS 校验失败
    var service = _casOptions.ServiceUrl;
    var redirect = string.IsNullOrEmpty(returnUrl)
        ? service
        : $"{service}?returnUrl={Uri.EscapeDataString(returnUrl)}";

    return $"{_casOptions.LoginUrl}?service={Uri.EscapeDataString(redirect)}";
}
```

控制器对 `/api/v1/sys/cas/login` 的请求执行 **302 重定向** 到上述 URL。

#### 3.2 CallbackAsync(ticket, returnUrl)

```
1. 构造 serviceValidate 请求：
   GET {ValidateUrl}?ticket={ticket}&service={ServiceUrl}&format=XML
2. 读取 XML 响应：
   ├── <cas:user> → login_code（必须存在，否则拒绝）
   └── <cas:attributes> → email / displayName / department 等
3. 在 sys_user 表中按 login_code 查询：
   ├── 命中 → 刷新最近登录信息与字段映射
   └── 未命中且 AutoCreateUser = true → 基于属性映射创建新用户
4. 构造本地 JWT（与 JWT 认证机制文档完全一致）
5. 302 到前端首页 / 或 returnUrl 指定页面
```

关键伪代码：

```csharp
public async Task<(string Token, string ReturnUrl)> CallbackAsync(string ticket, string returnUrl)
{
    var xml = await _httpClient.GetStringAsync(
        $"{_casOptions.ValidateUrl}?ticket={ticket}&service={_casOptions.ServiceUrl}");

    var (casUser, attributes) = ParseCasResponse(xml);
    var user = await _userRepository.GetByCodeAsync(casUser) ??
               (_casOptions.AutoCreateUser
                   ? await CreateUserFromCas(casUser, attributes)
                   : throw new UnauthorizedAccessException("CAS 用户未在本地开户"));

    var token = await _authService.CreateTokenAsync(user);
    return (token.Token, returnUrl ?? "/");
}
```

#### 3.3 LogoutAsync()

CAS 登出有两层：

1. **本地登出**：调用 `AuthService.LogoutAsync(userCode)`，将当前 jti 加入黑名单；
2. **单点登出（SLO, Single Log-Out）**：浏览器跳转
   `{LogoutUrl}?service={ServiceUrl}`，由 CAS Server 通知所有接入系统清理会话。

若只需要本地登出，可在前端单独调用 `/sys/auth/logout`，不触发 CAS 登出。

---

### 四、控制器端点

#### CasAuthController

位于：`JeeSiteNET.Modules.Sys/Controllers/CasAuthController.cs`

| 方法 | 路径 | 说明 |
|-----|------|------|
| `GET` | `/api/v1/sys/cas/login` | 发起 CAS 登录（302 到 CAS Server 登录页）。 |
| `GET` | `/api/v1/sys/cas/callback` | CAS 回调，解析 `ticket` 参数并校验。 |
| `POST` / `GET` | `/api/v1/sys/cas/logout` | CAS 登出（同时跳转 CAS Server 登出页）。 |

#### 4.1 前端按钮示例

```vue
<el-button
  v-if="authConfig.casEnabled"
  type="success"
  @click="window.location.href='/api/v1/sys/cas/login'">
  企业 CAS 登录
</el-button>
```

---

### 五、用户属性映射

#### 5.1 字段映射表

CAS `<cas:attributes>` 中的字段名由企业 IAM 配置决定，JeeSite.NET 通过配置文件
支持自定义映射：

| JeeSite.NET 字段 | 默认读取的 CAS attribute | 说明 |
|------------------|--------------------------|------|
| `login_code`     | `<cas:user>`             | 必选，用作唯一登录账号。 |
| `user_name`      | `displayName` / `name`   | 展示用姓名。 |
| `email`          | `mail` / `email`         | 用于通知与找回密码。 |
| `mobile`         | `mobile` / `telephoneNumber` | 用于短信通知。 |
| `corp_code`      | —（取自 DefaultCorpCode） | 新用户默认归属公司。 |
| `org_code`       | `department`             | 可配置由 attribute 映射或使用 DefaultOrgCode。 |
| `avatar`         | `avatar`                 | 可选。 |

#### 5.2 appsettings.json 中的映射示例

```json
{
  "Cas": {
    "AttributeMappings": {
      "user_name": "displayName",
      "email": "mail",
      "mobile": "telephoneNumber",
      "org_code": "department"
    }
  }
}
```

未配置的字段使用默认值（由 `Default*` 决定）。

#### 5.3 自动创建用户的默认角色

- 若 `AutoCreateUser = true`，新用户默认加入「普通员工」角色（`role_code = employee`）；
- 该默认角色可在 `sys_config`（`sys:cas:default_role`）中按公司覆盖；
- 管理员需定期审核"自动创建"的账号，避免越权访问。

---

### 六、常见问题与排错

#### 6.1 `ticket not recognized`

- `service` 参数与 CAS Server 中登记的 Service 白名单不完全一致（路径、大小写、
  结尾 `/`）；
- ST 为一次性票据，且有效期通常仅 10 秒，重放或延迟会导致失败。

#### 6.2 用户信息为空但 ticket 有效

- 检查 `serviceValidate` 是否返回 `<cas:authenticationFailure>`；
- 该账号可能在 CAS Server 上被禁用或未授权访问本 Service。

#### 6.3 重定向循环

- 应用在 `/cas/callback` 校验成功后签发了 JWT，但前端未正确保存；
- 检查 `https://` 是否与应用 Base URL 匹配，避免跨协议 Cookie 丢失。

#### 6.4 SLO（单点登出）不生效

- CAS Server 会向各 Service 的 `/logout` 端点发送后台回调清除票据；
- 若部署在反向代理后，需确保 ServiceUrl 对外可被 CAS Server 访问。

---

### 七、安全注意事项

1. **HTTPS 强制**：CAS Server 与 Service 之间通信必须走 HTTPS，避免 ST 被窃听。
2. **service 严格匹配**：禁止通配符，避免钓鱼网站伪造 service 窃取票据。
3. **ST 一次性使用**：应用需对已使用的 ST 在本地做 5 分钟黑名单，防止重放。
4. **自动创建用户受控**：建议默认角色仅保留最基本权限，升级需由管理员审批。
5. **日志审计**：CAS 登录事件写入 `sys_log`，字段 `login_from = "CAS"`，
   并记录 `cas:user` 与 ticket 摘要，便于溯源。

---

**第三部分小结**：CAS SSO 为 JeeSite.NET 提供了企业级统一入口能力。通过 `CasAuthService`
封装 Ticket 校验与属性映射，结合本地 JWT 签发机制，可实现与既有账号体系的平滑集成。
建议在正式上线前完成 CAS Server 白名单、HTTPS 证书与默认角色权限三项评审。

---


---

## 💡 快速参考

### 核心类与接口表

| 类型 | 名称 | 命名空间 | 说明 |
|------|------|---------|------|
| Service | `CasAuthService` | `JeeSiteNET.Modules.Sys.Application.Services` | CAS 主服务（Challenge/Callback/Logout/属性映射） |
| Util | `CasTicketValidator` | `JeeSiteNET.Core.Utils` | CAS 3.0 / 2.0 ticket 校验工具类 |
| Controller | `CasAuthController` | `JeeSiteNET.Modules.Sys.Controllers` | CAS API（login/callback/logout） |
| Entity | `User` | `JeeSiteNET.Modules.Sys.Domain.Entities` | 本地用户主体（CAS 属性映射后写入） |

### 常用 API 速查表

| 方法 | API | 说明 | 对应服务方法 |
|------|-----|------|-------------|
| `GET` | `/api/v1/sys/cas/login` | 发起 CAS 登录（302 到 CAS Server 登录页） | `CasAuthService.ChallengeAsync(returnUrl)` |
| `GET` | `/api/v1/sys/cas/callback?ticket=ST-xxx` | CAS 回调（校验 ticket 并签发本地 JWT） | `CasAuthService.CallbackAsync(ticket, returnUrl)` |
| `POST` | `/api/v1/sys/cas/logout` | CAS 单点登出（本地 + CAS Server 登出通知） | `CasAuthService.LogoutAsync()` |

### 最小工作示例（C# 代码块）

```csharp
// ===== 1. Challenge（发起 CAS 登录）=====
// CasAuthController
public IActionResult Login(string returnUrl = null)
{
    var casServer = _casOptions.ServerUrl;
    var service = _casOptions.ServiceUrl;
    var redirect = string.IsNullOrEmpty(returnUrl)
        ? service
        : $"{service}?returnUrl={Uri.EscapeDataString(returnUrl)}";
    return Redirect($"{casServer}/login?service={Uri.EscapeDataString(redirect)}");
}

// ===== 2. Callback（校验 ST 并签发 JWT）=====
public async Task<IActionResult> Callback(string ticket, string returnUrl)
{
    // 调用 CAS /p3/serviceValidate 校验 ticket
    var xml = await _httpClient.GetStringAsync(
        $"{_casOptions.ValidateUrl}?ticket={ticket}&service={_casOptions.ServiceUrl}");

    // 解析 XML 响应
    var (casUser, attributes) = CasTicketValidator.Parse(xml);
    var user = await _userRepository.GetByCodeAsync(casUser)
        ?? (_casOptions.AutoCreateUser
            ? await _userService.CreateFromAttributesAsync(casUser, attributes)
            : throw new UnauthorizedAccessException("CAS 用户未开户"));

    // 刷新用户属性（email/displayName/org_code）
    await _userService.RefreshFromCasAttributes(user, attributes);

    // 签发本地 JWT（与文档 15 的 Token 体系完全一致）
    var jwt = await _authService.CreateTokenAsync(user);
    return Redirect(returnUrl ?? "/");
}

// ===== 3. 前端（Vue 3）按钮 =====
// Login.vue
function loginWithCas() {
    window.location.href = "/api/v1/sys/cas/login";
}
```

### 配置项清单表

| 配置键 | 默认值 | 数据类型 | 说明 | 必填 |
|--------|--------|---------|------|------|
| `Auth:Cas:Enabled` | `false` | bool | 是否启用 CAS 登录入口 | ✅ |
| `Auth:Cas:ServerUrl` | (空) | string | CAS Server 根路径（如 https://cas.example.com/cas） | ✅ |
| `Auth:Cas:ServiceUrl` | (空) | string | 本应用回调地址（需在 CAS Server 白名单中） | ✅ |
| `Auth:Cas:Protocol` | `CAS3` | string | 协议版本（`CAS2` / `CAS3`） | ⬜ |
| `Auth:Cas:AutoCreateUser` | `true` | bool | 未匹配本地账号时是否自动创建 | ⬜ |
| `Auth:Cas:DefaultRole` | `employee` | string | 自动创建用户的默认角色 | ⬜ |
| `Auth:Cas:DefaultCorpCode` | (空) | string | 自动创建用户默认公司 | ⬜ |
| `Auth:Cas:DefaultOrgCode` | (空) | string | 自动创建用户默认机构 | ⬜ |
| `Auth:Cas:AttributeMappings:email` | `mail` | string | CAS 属性 → 本地字段映射 | ⬜ |

---

## ❓ 常见问题（3-5 个）

**Q1. ticket 校验失败 "ticket not recognized" 是什么原因？**
最常见原因是 `service` 参数与 CAS Server 中登记的白名单不完全一致（包含结尾斜杠、大小写等）。也可能是 ticket 为一次性票据，已被重放或过期（通常仅 10 秒有效）。

**Q2. CAS 用户属性为空但 ticket 有效？**
这通常意味着 CAS Server 上该账号未授权访问本 Service，或属性释放策略中未包含 email、displayName 等。请联系企业 IAM 管理员检查属性释放配置。

**Q3. 如何实现 SLO（单点登出）？**
用户在 CAS 登出后，CAS Server 会向后端 `/logout` 端点发送后台回调，JeeSite.NET 会自动清理该用户的所有 jti 黑名单。同时浏览器也会跳转到 CAS 登出页完成全系统登出。

**Q4. 自动创建的账号如何管控权限？**
默认角色为 `employee`（普通员工），管理员可在「系统管理 → 角色管理」中调整。建议对自动创建的账号定期审计，并通过属性映射同步 org_code/dept 等字段后，再授予相应数据权限。

**Q5. 多机构用户如何映射？**
可通过 `AttributeMappings` 将 CAS 返回的自定义属性（如 `department`、`employeeNumber`）映射到本地 `sys_user.org_code` / `corp_code` 等字段。若无该属性，可使用默认值。

---

## 📚 相关文档（上一篇/同系列/下一篇 + 跨系列）

| 类别 | 文档 | 说明 |
|------|------|------|
| 上一篇 | [16-OAuth2 登录](16-OAuth2登录) | 第三方 OAuth 方案（与 CAS 可并存） |
| 同系列 | [15-JWT 认证](15-JWT认证) | JWT 基础认证（CAS 登录后签发同一 JWT） |
| 同系列 | [18-LDAP 认证](18-LDAP认证) | AD / OpenLDAP 目录认证 |
| 同系列 | [19-数据与字段权限](19-数据与字段权限) | 行级/列级权限（CAS 用户同样适用 |
| 下一篇 | [20-AI 智能问答](20-AI智能问答) | AI 能力集成 |
| 跨系列 | [33-深入架构剖析](33-深入架构剖析) | 身份认证在整体架构中的位置 |

---

## 🚀 下一步（推荐阅读）

1. 确认企业 CAS Server 的 `server`/`validate` 地址与回调地址白名单已正确登记。
2. 与企业 IAM 管理员核对属性释放策略，确保 email/displayName 等必要字段已释放。
3. 评审 `employee` 默认角色的权限集合，并对自动创建用户进行审计。
4. 配合文档 **[19-数据与字段权限](19-数据与字段权限)** 将 CAS 属性映射到数据权限。
5. 如需更精细同步，参考 **[18-LDAP 认证](18-LDAP认证)** 定期同步目录用户。

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>