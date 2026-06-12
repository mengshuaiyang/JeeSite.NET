<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# OAuth2登录

> 通过 GitHub / 微信 / 钉钉 OAuth 2.0 协议实现第三方账号登录，含本地账号关联与自定义 Provider 扩展。
>
> **适用角色**：全栈开发人员
> **阅读时间**：约 10 分钟
> **相关文档**：[15-JWT认证](15-JWT认证) · [17-CAS单点登录](17-CAS单点登录)
> 最后更新: 2026-06-13

---

## 📋 目录

  - [一、支持的平台](#一、支持的平台)
  - [二、通用 OAuth 2.0 流程](#二、通用-oauth-20-流程)
    - [2.1 CSRF 防护（state 参数）](#21-csrf-防护（state-参数）)
    - [2.2 本地账号关联策略](#22-本地账号关联策略)
  - [三、配置项 (appsettings.json)](#三、配置项-appsettingsjson)
    - [3.1 通用字段说明](#31-通用字段说明)
  - [四、核心服务：OAuth2Service](#四、核心服务：oauth2service)
    - [4.1 ChallengeAsync(provider, returnUrl)](#41-challengeasyncprovider-returnurl)
    - [4.2 CallbackAsync(provider, code, state, returnUrl)](#42-callbackasyncprovider-code-state-returnurl)
  - [五、核心服务：各 Provider 实现](#五、核心服务：各-provider-实现)
    - [5.1 GitHubOAuth2Provider](#51-githuboauth2provider)
    - [5.2 WeChatOAuth2Provider（微信开放平台）](#52-wechatoauth2provider（微信开放平台）)
    - [5.3 DingTalkOAuth2Provider（钉钉开放平台）](#53-dingtalkoauth2provider（钉钉开放平台）)
  - [六、用户关联表](#六、用户关联表)
    - [6.1 sysoauth2user](#61-sysoauth2user)
    - [6.2 绑定与解绑](#62-绑定与解绑)
  - [七、控制器端点](#七、控制器端点)
    - [OAuth2Controller](#oauth2controller)
    - [典型请求链路示例（GitHub）](#典型请求链路示例（github）)
  - [八、安全注意事项](#八、安全注意事项)
  - [九、前端集成（views/Login.vue / OAuth2Callback.vue）](#九、前端集成（views-loginvue-oauth2callbackvue）)

---


本文档描述 JeeSite.NET 如何通过 OAuth 2.0 / OpenID Connect 协议集成
**GitHub / 微信 / 钉钉** 第三方账号登录，以及各平台的 Provider 实现与本地账号关联策略。

---

### 一、支持的平台

| 平台 | 协议 | 用途 | 核心特性 |
|-----|------|------|---------|
| GitHub | OAuth 2.0 + OpenID Connect | 开源社区/内部开发者登录 | 通过 `access_token` 调用 `/user`、`/user/emails` 获取资料与邮箱。 |
| 微信开放平台 | OAuth 2.0 | 公众/企业用户使用微信扫码登录 | `snsapi_login` 获取 `unionid`，作为第三方唯一标识。 |
| 钉钉开放平台 | OAuth 2.0 | 企业员工使用钉钉账号登录 | 通过 `sns_gettoken` + `getuserinfo_bycode` 获取 `userid`。 |

> 可扩展新的 Provider：实现 `IOAuth2Provider` 接口后在 `OAuth2Service` 注册即可。

---

### 二、通用 OAuth 2.0 流程

```
用户点击"GitHub 登录" →
   └─> 前端跳转 /api/v1/sys/oauth2/challenge?provider=github
       └─> 后端构造 OAuth 授权 URL 并重定向到 GitHub
           └─> 用户在 GitHub 登录并授权
               └─> GitHub 回调 /api/v1/sys/oauth2/callback?code=xxx&state=xxx
                   └─> 后端用 code 换取 access_token →
                       用 token 获取用户信息 →
                       创建或关联本地 sys_user 账号 →
                       生成本地 JWT token →
                       重定向到前端首页（带 token 参数）
```

#### 2.1 CSRF 防护（state 参数）

- `challenge` 阶段：生成随机 `state`（32 字节），写入 Redis：
  `oauth2:state:{state} = provider + returnUrl`，TTL = 5 分钟；
- `callback` 阶段：必须校验 `state` 是否已颁发，未通过则拒绝；
- 若启用 PKCE（对移动端/公开客户端推荐），`code_verifier` 同样保存在 Redis。

#### 2.2 本地账号关联策略

1. 通过 `provider + provider_user_id` 在 `sys_oauth2_user` 表查询是否存在关联；
2. **若已关联**：直接更新该用户的最近登录信息，返回 JWT；
3. **若未关联但邮箱匹配**（`sys_user.email == provider_email`）：自动关联到该本地账号；
4. **若完全未匹配**：创建新的 `sys_user`（自动生成 `loginCode`、随机密码），
   并插入一条 `sys_oauth2_user` 关联记录；
5. 用户后续可在「个人设置 → 账号绑定」中手动绑定/解绑。

---

### 三、配置项 (appsettings.json)

```json
{
  "OAuth2": {
    "GitHub": {
      "ClientId": "your-github-client-id",
      "ClientSecret": "your-github-client-secret",
      "CallbackUrl": "https://your-app.com/api/v1/sys/oauth2/callback",
      "Scope": "read:user user:email"
    },
    "WeChat": {
      "AppId": "your-wechat-appid",
      "AppSecret": "your-wechat-app-secret",
      "CallbackUrl": "https://your-app.com/api/v1/sys/oauth2/callback"
    },
    "DingTalk": {
      "AppKey": "your-dingtalk-appkey",
      "AppSecret": "your-dingtalk-app-secret",
      "CallbackUrl": "https://your-app.com/api/v1/sys/oauth2/callback"
    }
  }
}
```

#### 3.1 通用字段说明

| 字段 | 说明 |
|-----|------|
| `ClientId` / `AppId` / `AppKey` | 第三方平台分配的应用标识。 |
| `ClientSecret` / `AppSecret` | 敏感密钥，务必通过环境变量或密钥管理工具注入。 |
| `CallbackUrl` | 回调地址，**必须与第三方平台登记的地址完全一致**。 |
| `Scope` | 需要向用户申请的权限范围（默认可留空使用平台推荐值）。 |

---

### 四、核心服务：OAuth2Service

位于：`JeeSiteNET.Modules.Sys/Application/Services/OAuth2/OAuth2Service.cs`

#### 4.1 ChallengeAsync(provider, returnUrl)

```csharp
public async Task<string> ChallengeAsync(string provider, string returnUrl)
{
    var providerInstance = _providerFactory.Get(provider);
    var state = RandomNumberGenerator.GetHexString(32);

    // state 写入 Redis，5 分钟有效
    await _redis.StringSetAsync(
        $"oauth2:state:{state}",
        JsonSerializer.Serialize(new { provider, returnUrl }),
        TimeSpan.FromMinutes(5));

    return providerInstance.BuildChallengeUrl(state);
}
```

控制器返回 `302 Redirect` 到上述 URL，将用户导向第三方授权页面。

#### 4.2 CallbackAsync(provider, code, state, returnUrl)

```
1. 校验 state（Redis 中存在则一次性消费）
2. 调用 providerInstance.ExchangeCodeForTokenAsync(code) → access_token
3. 调用 providerInstance.GetUserInfoAsync(access_token)
   ├── provider_user_id（第三方唯一标识）
   ├── display_name（第三方昵称）
   ├── email（可选，取决于 Scope）
   └── avatar_url（可选）
4. 在 sys_oauth2_user 中按 (provider, provider_user_id) 查询
   ├── 命中 → 走关联用户登录
   └── 未命中 → 按 email 反查或创建新用户
5. 写入登录日志，生成 JWT
6. 302 重定向到前端首页：/oauth2-callback?token=xxx
```

**失败处理**：任何一步失败均重定向到 `/#/login?oauth-error=xxx`，前端提示具体原因。

---

### 五、核心服务：各 Provider 实现

所有 Provider 均实现 `IOAuth2Provider` 接口：

```csharp
public interface IOAuth2Provider
{
    string Name { get; }                        // 唯一标识，如 "github"
    string BuildChallengeUrl(string state);     // 构造授权 URL
    Task<string> ExchangeCodeForTokenAsync(string code);
    Task<OAuth2UserInfo> GetUserInfoAsync(string accessToken);
}

public class OAuth2UserInfo
{
    public string ProviderUserId { get; set; }  // 第三方用户唯一 ID
    public string DisplayName   { get; set; }
    public string Email         { get; set; }
    public string AvatarUrl     { get; set; }
}
```

#### 5.1 GitHubOAuth2Provider

**授权 URL**：
```
https://github.com/login/oauth/authorize
  ?client_id={ClientId}
  &redirect_uri={CallbackUrl}
  &state={state}
  &scope=read:user user:email
```

**换取 Token**：`POST https://github.com/login/oauth/access_token`

**获取用户信息**：
- `GET https://api.github.com/user` → 返回登录账号、昵称、头像等；
- `GET https://api.github.com/user/emails` → 获取主邮箱（用于本地账号匹配）。

`ProviderUserId` = GitHub `node_id`（跨应用稳定）或 `id`。

#### 5.2 WeChatOAuth2Provider（微信开放平台）

**授权 URL**：
```
https://open.weixin.qq.com/connect/qrconnect
  ?appid={AppId}
  &redirect_uri={CallbackUrl}
  &response_type=code
  &scope=snsapi_login
  &state={state}
```

**换取 access_token**：
```
GET https://api.weixin.qq.com/sns/oauth2/access_token
  ?appid={AppId}
  &secret={AppSecret}
  &code={code}
  &grant_type=authorization_code
→ 返回 access_token, openid, unionid
```

**获取用户信息**：
```
GET https://api.weixin.qq.com/sns/userinfo
  ?access_token=...&openid=...&lang=zh_CN
→ 返回 nickname, headimgurl, unionid
```

`ProviderUserId` = `unionid`（企业跨应用统一标识）。

#### 5.3 DingTalkOAuth2Provider（钉钉开放平台）

**授权 URL**：
```
https://login.dingtalk.com/oauth2/auth
  ?redirect_uri={CallbackUrl}
  &response_type=code
  &client_id={AppKey}
  &scope=openid
  &state={state}
  &prompt=consent
```

**换取 user_access_token**：
```
POST https://api.dingtalk.com/v1.0/oauth2/userAccessToken
{ "clientId": "{AppKey}", "clientSecret": "{AppSecret}",
  "code": "{code}", "grantType": "authorization_code" }
```

**获取用户信息**：
```
POST https://api.dingtalk.com/v1.0/contact/users/me
Header: x-acs-dingtalk-access-token: {accessToken}
→ 返回 nick, avatarUrl, unionId, openId
```

`ProviderUserId` = `unionId`。

---

### 六、用户关联表

#### 6.1 sys_oauth2_user

| 列 | 类型 | 说明 |
|----|------|------|
| `id` | `bigint PK` | 主键（自增或雪花）。 |
| `provider` | `varchar(32)` | `github` / `wechat` / `dingtalk` 等。 |
| `provider_user_id` | `varchar(128)` | 第三方返回的唯一用户 ID（unionid/node_id 等）。 |
| `user_code` | `varchar(64) FK → sys_user.user_code` | 关联到的本地账号。 |
| `login_code_hint` | `varchar(64)` | 冗余字段，便于展示"此第三方账号绑定到 xxx"。 |
| `bind_date` | `datetime` | 绑定时间。 |
| `last_login_date` | `datetime` | 通过该第三方最近登录时间。 |
| `status` | `tinyint` | 0=正常，1=已解绑。 |

**唯一约束**：`(provider, provider_user_id)` 唯一，确保同一第三方账号不会重复关联。

#### 6.2 绑定与解绑

- **绑定**（`POST /api/v1/sys/oauth2/bind`）：已登录用户通过 challenge → callback
  流程获取第三方账号后，将 `provider_user_id ↔ user_code` 写入关联表；
- **解绑**（`POST /api/v1/sys/oauth2/unbind?provider=xxx`）：将对应记录 `status` 置为
  `1`，后续不再允许该第三方账号自动登录（需重新绑定）；
- 保留至少一种登录方式：不允许用户同时解绑所有第三方账号且未设置密码。

---

### 七、控制器端点

#### OAuth2Controller

位于：`JeeSiteNET.Modules.Sys/Controllers/OAuth2Controller.cs`

| 方法 | 路径 | 说明 |
|-----|------|------|
| `GET` | `/api/v1/sys/oauth2/challenge` | 发起第三方登录，必选 `?provider=xxx`，可选 `&returnUrl=xxx`。 |
| `GET` | `/api/v1/sys/oauth2/callback` | 第三方回调，解析 `code` 与 `state`，完成登录。 |
| `GET` | `/api/v1/sys/oauth2/providers` | 返回当前已启用的第三方登录列表，供登录页动态渲染按钮。 |
| `POST` | `/api/v1/sys/oauth2/bind` | 已登录用户绑定第三方账号（需要走 challenge/callback 流程）。 |
| `POST` | `/api/v1/sys/oauth2/unbind` | 已登录用户解绑指定 provider 的账号。 |

#### 典型请求链路示例（GitHub）

```
GET /api/v1/sys/oauth2/challenge?provider=github
  → 302 https://github.com/login/oauth/authorize?client_id=...&state=abc

（用户在 GitHub 授权）
  ↓
GitHub 302 → https://your-app.com/api/v1/sys/oauth2/callback?code=xyz&state=abc
  ↓
服务端：校验 state → 换 token → 读用户 → 创建/关联本地账号 → 签发 JWT
  ↓
302 → https://your-app.com/#/oauth2-callback?token=eyJhbGci...
  ↓
前端：读取 query.token → 写入 localStorage → 跳转首页
```

---

### 八、安全注意事项

1. **`ClientSecret/AppSecret` 严禁入库或写入源码**，推荐通过环境变量或 KMS 注入。
2. **CallbackUrl 必须显式注册**，禁止使用 `redirect_uri` 通配符或动态拼接。
3. **state 参数必须校验且一次性**，避免 CSRF。
4. **Token 传输必须在服务端完成**：`code → access_token` 的过程不能暴露给浏览器。
5. **新账号默认权限**：第三方登录自动创建的账号默认仅拥有"普通员工"角色，
   需管理员在后台升级权限，避免越权。
6. **Https 强制**：所有 OAuth 回调必须通过 HTTPS，避免中间人窃取 code/token。

---

### 九、前端集成（views/Login.vue / OAuth2Callback.vue）

```ts
// 1. 登录页根据 /oauth2/providers 动态渲染按钮
const { data: providers } = await useRequest.get<string[]>("/sys/oauth2/providers");

// 2. 点击 "GitHub 登录"
function loginWithGitHub() {
  window.location.href = `/api/v1/sys/oauth2/challenge?provider=github`;
}

// 3. OAuth2Callback.vue 在挂载时解析 query 中的 token
//    成功 → 写入 userStore.token → 跳转 "/"
//    失败 → 读取 error 参数 → 提示后跳回 "/login"
```

---

**第二部分小结**：JeeSite.NET 通过统一的 `IOAuth2Provider` 抽象屏蔽了各平台协议差异，
管理员只需在 `appsettings.json` 填入对应 AppKey/Secret 即可一键启用。
关联表 `sys_oauth2_user` 保证同一第三方账号在本地始终映射到唯一 `sys_user`，
并支持用户在登录后自主绑定/解绑。

---


---

## 💡 快速参考

| 项目 | 关键信息 |
|------|---------|
| **支持平台** | GitHub / 微信开放平台 / 钉钉开放平台 |
| **协议** | OAuth 2.0 Authorization Code Flow |
| **核心服务** | OAuth2Service + IOAuth2Provider 接口 |
| **关联策略** | provider + provider_user_id → 本地账号，邮箱匹配自动关联 |
| **关联表** | sys_oauth2_user (provider, provider_user_id, user_code) |
| **安全** | state 参数防 CSRF、HTTPS 强制、Token 服务端交换 |

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>