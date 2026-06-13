<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 18 LDAP认证

> 企业 AD / OpenLDAP 目录集成，LDAPS 安全连接、用户属性同步、最小权限 BindDn 配置。
>
> **适用角色**：系统管理员、安全工程师
> **阅读时间**：约 9 分钟
> **相关文档**：[15-JWT认证](15-JWT认证) · [17-CAS单点登录](17-CAS单点登录)
> 最后更新: 2026-06-13

---

## 📋 目录

  - [一、适用场景](#一、适用场景)
  - [二、配置项 (appsettings.json)](#二、配置项-appsettingsjson)
    - [2.1 字段说明](#21-字段说明)
  - [三、核心服务：LdapAuthService](#三、核心服务：ldapauthservice)
    - [3.1 LoginAsync(loginCode, password)](#31-loginasynclogincode-password)
    - [3.2 SyncUsersAsync() — 定时同步](#32-syncusersasync-—-定时同步)
  - [四、控制器端点](#四、控制器端点)
    - [LdapAuthController](#ldapauthcontroller)
  - [五、Active Directory vs OpenLDAP 差异速查](#五、active-directory-vs-openldap-差异速查)
  - [六、安全注意事项](#六、安全注意事项)
    - [6.1 强制 LDAPS](#61-强制-ldaps)
    - [6.2 BindDn 权限最小化](#62-binddn-权限最小化)
    - [6.3 密码不落地](#63-密码不落地)
    - [6.4 BindPassword 轮换](#64-bindpassword-轮换)
    - [6.5 登录失败处理](#65-登录失败处理)
    - [6.6 过滤注入防护](#66-过滤注入防护)
  - [七、常见错误对照表](#七、常见错误对照表)

---


本文档描述 JeeSite.NET 如何接入企业 **Active Directory (AD)** 或 **OpenLDAP**
作为账号源，通过 LDAP/LDAPS 协议完成用户密码校验与属性同步。

---

### 一、适用场景

| 场景 | 说明 |
|-----|------|
| **企业内部 AD** | 员工账号集中管理在 Windows Server Active Directory，邮箱/部门/职位等信息自动同步。 |
| **OpenLDAP** | 开源目录服务，常用于 Linux 环境统一账号。 |
| **LDAPS (SSL/TLS)** | 生产环境 **强烈建议**，使用端口 636 或 3269 (GC SSL)；禁止使用明文 389。 |
| **定期同步** | 可配置后台任务（`LdapSyncJob`）按小时/天增量同步用户。 |

---

### 二、配置项 (appsettings.json)

```json
{
  "Ldap": {
    "Enabled": true,
    "Server": "ldap.example.com",
    "Port": 389,
    "UseSsl": false,
    "BaseDn": "OU=Users,DC=example,DC=com",
    "Domain": "EXAMPLE",
    "BindDn": "CN=JeeSiteService,OU=ServiceAccounts,DC=example,DC=com",
    "BindPassword": "service-account-password",
    "SearchFilter": "(&(objectClass=user)(sAMAccountName={0}))",
    "AttributeMappings": {
      "login_code": "sAMAccountName",
      "user_name":  "displayName",
      "email":      "mail",
      "mobile":     "telephoneNumber"
    }
  }
}
```

#### 2.1 字段说明

| 字段 | 类型 | 默认值 | 说明 |
|-----|------|-------|------|
| `Enabled` | `bool` | `false` | 是否启用 LDAP 登录。 |
| `Server` | `string` | — | LDAP 服务器地址，可为多值（逗号分隔）用于故障转移。 |
| `Port` | `int` | `389` | LDAP 端口，明文 389，LDAPS 636，GC 3268/3269。 |
| `UseSsl` | `bool` | `false` | 是否启用 SSL/TLS；生产环境必须为 `true`。 |
| `BaseDn` | `string` | — | 搜索用户的起始 DN，如 `OU=Users,DC=example,DC=com`。 |
| `Domain` | `string` | — | Active Directory 域前缀（可选），用于 `DOMAIN\user` 绑定。 |
| `BindDn` | `string` | — | 系统绑定账号 DN（具备"读取用户属性"权限即可）。 |
| `BindPassword` | `string` | — | 系统绑定账号密码；**必须通过环境变量/KMS 注入**。 |
| `SearchFilter` | `string` | AD 见左 | `{0}` 会被替换为用户输入的 login_code；OpenLDAP 可改为 `(&(objectClass=inetOrgPerson)(uid={0}))`。 |
| `AttributeMappings` | `object` | 见上 | 将 LDAP 属性映射到 `sys_user` 字段。 |

---

### 三、核心服务：LdapAuthService

位于：`JeeSiteNET.Modules.Sys/Application/Services/LdapAuthService.cs`

底层依赖：`Novell.Directory.Ldap.NETStandard`（跨平台实现，支持 .NET Core / Linux）。

#### 3.1 LoginAsync(loginCode, password)

```
1. 建立 LDAP 连接（使用 UseSsl / Port）
   ├── 服务器证书自定义校验（可选，企业自签证书需要添加到信任）
   └── 使用 BindDn / BindPassword 绑定（系统级只读权限）

2. 使用 SearchFilter 在 BaseDn 下搜索用户
   ├── BaseDn + Scope = SUB
   └── 取回 DN + 所需属性（AttributeMappings 中的 key）

3. 使用搜索到的用户 DN + 用户输入 password 再次 Bind（真正验证密码）
   ├── Bind 成功 → 继续
   └── Bind 失败（如 INVALID_CREDENTIALS）→ 返回"账号或密码错误"

4. 在本地 sys_user 中按 login_code 查找：
   ├── 命中 → 按 LDAP 属性刷新 user_name / email / mobile
   └── 未命中 → 按配置决定是否自动创建（默认创建，归属 DefaultCorpCode）

5. 生成 JWT（同 JWT 认证机制），返回 LoginResultDto
```

关键伪代码：

```csharp
public async Task<LoginResultDto> LoginAsync(string loginCode, string password)
{
    using var conn = new LdapConnection();
    conn.SecureSocketLayer = _ldapOptions.UseSsl;
    conn.Connect(_ldapOptions.Server, _ldapOptions.Port);
    conn.Bind(_ldapOptions.BindDn, _ldapOptions.BindPassword);

    var filter = string.Format(_ldapOptions.SearchFilter, LdapFilter.Escape(loginCode));
    var results = conn.Search(
        _ldapOptions.BaseDn,
        LdapConnection.SCOPE_SUB,
        filter,
        _ldapOptions.AttributeNames,
        false);

    if (!results.HasMore()) throw new UnauthorizedAccessException("LDAP 用户不存在");

    var entry = results.Next();
    // 用用户 DN 重新 Bind，验证密码
    conn.Bind(entry.DN, password);

    var user = MapEntryToUser(entry);
    return await _authService.CreateTokenAsync(user);
}
```

#### 3.2 SyncUsersAsync() — 定时同步

当管理员启用"LDAP 定时同步"时，`LdapSyncJob` 后台任务按配置频率执行：

1. 搜索 BaseDn 下所有用户；
2. 按 `login_code` 与本地用户比对：
   - 新增：自动创建本地账号（默认角色）；
   - 更新：刷新 user_name / email / mobile / org_code；
   - 禁用：LDAP 中 `userAccountControl` 含 `UF_ACCOUNTDISABLE` 的用户，
     本地 `status` 同步为禁用。
3. 记录同步摘要（新增 N / 更新 M / 禁用 K）到 `sys_log`。

> 大组织建议采用"增量同步"——仅拉取 `modifyTimestamp >= lastSyncTime` 的条目，
> 降低 AD 负载。

---

### 四、控制器端点

#### LdapAuthController

位于：`JeeSiteNET.Modules.Sys/Controllers/LdapAuthController.cs`

| 方法 | 路径 | 说明 |
|-----|------|------|
| `POST` | `/api/v1/sys/ldap/login` | 使用 LDAP 账号登录，Body: `{ loginCode, password }`。 |
| `POST` | `/api/v1/sys/ldap/sync`  | 管理员手动触发 LDAP 用户增量同步。 |

---

### 五、Active Directory vs OpenLDAP 差异速查

| 事项 | Active Directory | OpenLDAP（常见 inetOrgPerson 模式） |
|-----|------------------|-------------------------------------|
| objectClass | `user` | `inetOrgPerson` / `posixAccount` |
| 登录 ID 属性 | `sAMAccountName` | `uid` |
| 邮箱属性 | `mail` | `mail` |
| 姓名属性 | `displayName` / `cn` | `cn` / `displayName` |
| 禁用位 | `userAccountControl & 0x0002` | 常见使用 `pwdAccountLockedTime` 或自定义 |
| 系统 Bind DN | `CN=svc,OU=Svc,DC=example,DC=com` | `cn=admin,dc=example,dc=com` |
| 推荐过滤器 | `(&(objectClass=user)(sAMAccountName={0}))` | `(&(objectClass=inetOrgPerson)(uid={0}))` |

---

### 六、安全注意事项

#### 6.1 强制 LDAPS

- 端口 636（LDAPS）或 3269（Global Catalog over SSL）；
- 企业自签证书需将 CA 根证书安装到主机信任库；
- **禁止通过 389 明文传输用户密码**。

#### 6.2 BindDn 权限最小化

- 仅授予"读取所有用户属性"权限；
- 禁止授予"写入/修改用户密码"权限；
- 禁止使用域管理员账号作为 BindDn。

#### 6.3 密码不落地

- `BindPassword` 只在内存中使用，**不出现在日志、异常堆栈**；
- 用户输入的 password 在 `LoginAsync` 完成后立即释放（`SecureString` 封装）；
- 审计日志仅记录"loginCode + 成功/失败"，不记录敏感字段。

#### 6.4 BindPassword 轮换

- 建议每 90 天更换 BindDn 密码；
- 通过配置中心下发新版本，应用无需重启；
- 旧版本密码保留 48 小时作为回退窗口。

#### 6.5 登录失败处理

- 与本地账号统一使用 Redis 计数器；
- 连续失败 5 次锁定 30 分钟；
- 针对 LDAP 返回的 `LDAP_INVALID_CREDENTIALS`、`LDAP_NO_SUCH_OBJECT`、
  `LDAP_CONSTRAINT_VIOLATION`（AD 锁）等错误，映射为用户可理解的提示，
  但不暴露"账号是否存在"的判断（防止账号枚举攻击）。

#### 6.6 过滤注入防护

- `LdapFilter.Escape(loginCode)` 将 `\ * ( ) NUL` 等字符转义；
- 禁止将用户输入直接拼接到 `SearchFilter` 而不转义。

---

### 七、常见错误对照表

| 现象 | 可能原因 |
|-----|---------|
| 应用启动阶段报"LDAP 连接失败" | 网络不通 / 防火墙未放行 389/636 / 证书链不完整。 |
| Bind 失败 "Invalid credentials" | BindDn 密码错误 / 账号被锁定 / 只读账号权限不足。 |
| 用户搜索结果为空 | BaseDn 不对 / SearchFilter 错（`sAMAccountName` vs `uid`）。 |
| 用户密码正确但始终失败 | 用户 DN 拼写错误，或用户在 AD 中"禁止交互式登录"。 |
| 自签证书报 SSL 错误 | 需将 CA 证书添加到 Linux 的 `/etc/ssl/certs` 或 Windows 本机信任区。 |

---

**第四部分小结**：LDAP 认证为 JeeSite.NET 提供了与企业 IAM 体系的"零改动"对接能力。
实施要点可归结为三条：**启用 LDAPS**、**使用最小权限 BindDn**、**用户属性映射正确**。
对大型组织建议配合 `LdapSyncJob` 做增量同步，避免登录时频繁读取 AD。

---


---

## 💡 快速参考

### 核心类与接口表

| 类型 | 名称 | 命名空间 | 说明 |
|------|------|---------|------|
| Service | `LdapAuthService` | `JeeSiteNET.Modules.Sys.Application.Services` | LDAP 主服务（认证 + 同步） |
| Util | `LdapFilter` | `JeeSiteNET.Core.Utils` | LDAP 过滤器转义工具（防注入） |
| Controller | `LdapAuthController` | `JeeSiteNET.Modules.Sys.Controllers` | LDAP API（login/sync） |
| Entity | `User` | `JeeSiteNET.Modules.Sys.Domain.Entities` | 本地用户主体 |

### 常用 API 速查表

| 方法 | API | 说明 | 对应服务方法 |
|------|-----|------|-------------|
| `POST` | `/api/v1/sys/ldap/login` | LDAP 账号登录（Body: `{ loginCode, password }`） | `LdapAuthService.LoginAsync(loginCode, password)` |
| `POST` | `/api/v1/sys/ldap/sync` | 管理员手动触发 LDAP 用户增量同步 | `LdapAuthService.SyncUsersAsync()` |

### 最小工作示例（C# 代码块）

```csharp
// ===== 1. LDAP 登录 =====
// LdapAuthController
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    using var conn = new LdapConnection();
    conn.SecureSocketLayer = _ldapOptions.UseSsl;
    conn.Connect(_ldapOptions.Server, _ldapOptions.Port);

    // 系统级 Bind（只读权限），搜索用户 DN
    conn.Bind(_ldapOptions.BindDn, _ldapOptions.BindPassword);
    var filter = string.Format(_ldapOptions.SearchFilter,
        LdapFilter.Escape(dto.LoginCode));
    var results = conn.Search(_ldapOptions.BaseDn, LdapConnection.SCOPE_SUB,
        filter, null, false);

    if (!results.HasMore())
        return Unauthorized("账号不存在");

    var entry = results.Next();
    // 用户级 Bind（真正校验密码）
    try { conn.Bind(entry.DN, dto.Password); }
    catch { return Unauthorized("密码错误"); }

    // 绑定/刷新本地用户
    var user = await _ldapAuthService.SyncOrCreate(entry);
    var jwt = await _authService.CreateTokenAsync(user);
    return Ok(new { jwt.Token, jwt.ExpiresIn });
}

// ===== 2. 定时同步（LdapSyncJob）=====
// 背景任务：按配置频率（如每天 02:00）从 LDAP 全量或增量同步用户
// - 新增：本地不存在的账号 → 自动创建
// - 更新：email/displayName/org_code 变化 → 刷新
// - 禁用：LDAP 中 UF_ACCOUNTDISABLE 标志位的账号 → 本地 status = 1
public async Task<int> SyncUsersAsync()
{
    // 读取 LDAP 所有用户
    var entries = await _ldapAuthService.SearchAllUsers();
    var (added, updated, disabled) = (0, 0, 0);
    foreach (var entry in entries)
    {
        var loginCode = entry.Attributes["sAMAccountName"][0];
        var user = await _userRepository.GetByCodeAsync(loginCode);
        if (user == null) { await CreateFromLdap(entry); added++; }
        else if (await RefreshFromLdap(user, entry)) updated++;
        if (IsDisabled(entry)) { user.Status = 1; disabled++; }
    }
    return added + updated + disabled;
}

// ===== 3. 前端（Vue 3）集成 =====
// Login.vue - 与本地账号登录同表单，后台按配置自动选择认证通道
const { data } = await request.post("/sys/ldap/login", { loginCode, password });
userStore.token = data.token;
```

### 配置项清单表

| 配置键 | 默认值 | 数据类型 | 说明 | 必填 |
|--------|--------|---------|------|------|
| `Auth:Ldap:Enabled` | `false` | bool | 是否启用 LDAP 登录 | ✅ |
| `Auth:Ldap:Server` | (空) | string | LDAP 服务器地址（多值逗号分隔可用于故障转移） | ✅ |
| `Auth:Ldap:Port` | `389` | int | 端口：明文 389 / LDAPS 636 / GC 3268/3269 | ✅ |
| `Auth:Ldap:UseSsl` | `false` | bool | 是否启用 SSL/TLS（**生产环境必须为 true**） | ✅ |
| `Auth:Ldap:BaseDn` | (空) | string | 用户搜索根 DN（如 OU=Users,DC=example,DC=com） | ✅ |
| `Auth:Ldap:BindDn` | (空) | string | 系统只读 Bind DN（**最小权限原则，仅可读**） | ✅ |
| `Auth:Ldap:BindPassword` | (空) | string | Bind 密码（**必须通过环境变量注入**） | ✅ |
| `Auth:Ldap:SearchFilter` | AD: `(&(objectClass=user)(sAMAccountName={0}))` | string | 用户查询过滤器（`{0}` 为 loginCode，会自动转义） | ✅ |
| `Auth:Ldap:AttributeMappings:email` | `mail` | string | LDAP 属性 → 本地字段映射 | ⬜ |
| `Auth:Ldap:AttributeMappings:user_name` | `displayName` | string | 同上 | ⬜ |
| `Auth:Ldap:AttributeMappings:mobile` | `telephoneNumber` | string | 同上 | ⬜ |
| `Auth:Ldap:DefaultRole` | `employee` | string | 自动创建用户的默认角色 | ⬜ |

---

## ❓ 常见问题（3-5 个）

**Q1. 为何生产环境必须使用 LDAPS（端口 636）？**
明文 389 会暴露用户密码与目录属性，易被中间人劫持。启用 `UseSsl: true` 后使用端口 636，企业自签证书需将 CA 根证书添加到主机信任区。

**Q2. BindDn 权限最小化到什么程度？**
仅需「读取用户属性」权限，禁止写入或修改用户信息。建议使用独立服务账号，不是域管理员。

**Q3. BindPassword 如何安全存放？**
**绝对不能出现在 appsettings.json 或源码中**。推荐：环境变量、云 KMS（Azure Key Vault / 阿里云 KMS）、HashiCorp Vault。密码每 90 天轮换一次，旧版本保留 48 小时作为回退窗口。

**Q4. 如何防止过滤注入攻击？**
所有用户输入必须通过 `LdapFilter.Escape(loginCode)` 转义，将 `\ * ( ) NUL` 等特殊字符替换为 `\XX` 十六进制形式。禁止使用字符串拼接构造过滤器。

**Q5. LDAP 与本地账号登录如何共存？**
JeeSite.NET 采用**并行认证策略**：先尝试本地密码登录（sys_user），若失败且 LDAP 已启用，则调用 `LdapAuthService.LoginAsync` 做 LDAP 认证，成功后同步本地账号属性。管理员可在配置中指定优先级。

---

## 📚 相关文档（上一篇/同系列/下一篇 + 跨系列）

| 类别 | 文档 | 说明 |
|------|------|------|
| 上一篇 | [17-CAS 单点登录](17-CAS单点登录) | 企业级 CAS SSO（与 LDAP 同源，可二选一或并存） |
| 同系列 | [15-JWT 认证](15-JWT认证) | JWT 基础认证（LDAP 登录后签发同一 JWT） |
| 同系列 | [16-OAuth2 登录](16-OAuth2登录) | 第三方 OAuth 方案 |
| 同系列 | [19-数据与字段权限](19-数据与字段权限) | 行级/列级权限（LDAP 用户同样适用 |
| 下一篇 | [20-AI 智能问答](20-AI智能问答) | AI 能力集成 |
| 跨系列 | [33-深入架构剖析](33-深入架构剖析) | 身份认证/权限在整体架构中的位置 |

---

## 🚀 下一步（推荐阅读）

1. **检查企业 AD 或 OpenLDAP** 是否允许应用服务器访问 389/636 端口。
2. **获取最小权限 Bind 账号**（仅可读用户属性），配置好 `BindDn` / `BindPassword`。
3. **测试 `SearchFilter` 是否正确返回目标 OU 下用户**。
4. **启用 `UseSsl: true`**（生产环境强制），并添加企业自签 CA 证书到信任区。
5. **配合文档 [19-数据与字段权限](19-数据与字段权限)**，将 LDAP `department` 属性映射到 `org_code`，自动获得正确的数据权限。

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>