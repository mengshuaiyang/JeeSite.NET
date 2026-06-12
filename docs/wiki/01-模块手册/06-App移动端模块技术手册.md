# App 移动端模块技术手册

## 一、模块概述

### 1.1 模块定位

App 模块面向 **APP / H5 / 小程序** 端，提供相对独立的后端 API。它与 Sys 模块的权限体系解耦但保持兼容：移动端既可以使用 JeeSite.NET 的同一套账号体系，也可以接入独立的 AppUser 账号（通过 OAuth2、微信登录等）。

本手册重点描述两个已内置的核心功能：

- **意见反馈**：移动端提交反馈，管理后台查看 / 回复 / 更新状态
- **版本升级**：移动端在启动时请求「检查升级」接口，根据 `versionCode` 与平台判断是否需要升级（含强制升级与静默升级）

- 模块所在程序集：`JeeSiteNET.Modules.App`
- API 根路径：`/api/v1/app/`
- 前端入口（管理后台）：`frontend/src/views/app/`

### 1.2 技术依赖

- `JeeSiteNET.Modules.Sys`：用户、字典、日志、菜单
- 可选依赖（用于推送）：`JeeSiteNET.Modules.Tasks`（定时扫描推送任务）

### 1.3 典型调用流程

- **启动阶段**：APP → `GET /api/v1/app/upgrade/check?platform=Android&versionCode=123` → 返回是否需要升级、下载地址、是否强制
- **使用阶段**：APP → `POST /api/v1/app/comment/submit` 提交意见反馈，带上设备信息
- **管理阶段**：运营人员在后台查看反馈 → 回复 / 更新状态

## 二、核心实体

### 2.1 AppComment（意见反馈）

命名空间：`JeeSiteNET.Modules.App.Domain.Entities.AppComment`

| 字段 | 类型 | 含义 |
|------|------|------|
| Id | long | 主键 |
| Title | string | 反馈标题（可选） |
| Content | string | 反馈内容（必填） |
| Contact | string | 联系方式（邮箱/手机，可选） |
| UserCode | string | 登录用户 user_code（匿名提交时为空） |
| DeviceInfo | string | 设备信息（JSON 字符串），如：型号/系统版本/APP 版本/分辨率/网络 |
| Status | string | `0` 未处理 / `1` 处理中 / `2` 已完成 |
| Reply | string | 后台回复内容 |
| HandlerCode | string | 处理人 user_code |
| HandleDate | DateTime? | 处理时间 |
| CreateDate | DateTime | 提交时间 |
| Remarks | string | 备注 |

### 2.2 AppUpgrade（版本管理）

命名空间：`JeeSiteNET.Modules.App.Domain.Entities.AppUpgrade`

| 字段 | 类型 | 含义 |
|------|------|------|
| Id | long | 主键 |
| VersionName | string | 展示版本号，如 `1.2.3` |
| VersionCode | int | 数值版本号（用于比较大小；必须递增） |
| Platform | string | `Android` / `iOS` / `H5` |
| Title | string | 更新标题 |
| Content | string | 更新内容（支持换行） |
| DownloadUrl | string | 下载地址（APK 或 App Store 链接） |
| FileSize | long | 文件大小（KB） |
| Md5 | string | 文件 MD5（可用于下载后校验） |
| ForceUpgrade | bool | 是否强制升级（true 时 APP 应弹窗禁止继续使用旧版本） |
| IsPublished | bool | 是否已发布（未发布不会被 check 接口返回） |
| MinVersion | int? | 最低兼容版本（APP 当前 versionCode 小于此值时必须升级） |
| PublishDate | DateTime? | 发布时间 |
| ExpireDate | DateTime? | 失效时间（可选；过期后不再返回） |
| CreateBy | string | 创建人 |
| CreateDate | DateTime | 创建时间 |
| UpdateBy | string | 更新人 |
| UpdateDate | DateTime | 更新时间 |
| Remarks | string | 备注 |

### 2.3 AppUser（可选扩展）

如需独立的 App 账号体系，可在模块新增 `AppUser` 实体，字段建议：

| 字段 | 类型 | 含义 |
|------|------|------|
| Id | long | 主键 |
| UserCode | string | 业务主键（用于鉴权 token） |
| Mobile | string | 手机号 |
| Email | string | 邮箱 |
| PasswordHash | string | 已哈希的密码 |
| Nickname | string | 昵称 |
| Avatar | string | 头像 URL |
| Platform | string | 注册平台（Android / iOS / H5 / WechatMini） |
| OpenId | string | 第三方 OpenId（可选） |
| Status | string | 0 正常 / 1 停用 |
| LastLoginAt | DateTime? | 最近登录时间 |
| LastLoginIp | string | 最近登录 IP |
| CreateDate | DateTime | 创建时间 |

配套能力建议：注册 / 登录 / 登出 / 找回密码 / 修改资料 / 上传头像。

## 三、核心服务

### 3.1 AppCommentService

命名空间：`JeeSiteNET.Modules.App.Application.Services.AppCommentService`

| 方法 | 说明 |
|------|------|
| SubmitAsync(AppCommentSubmitDto dto, string? userCode) | 移动端提交反馈（写入表） |
| GetPagedListAsync(string? status, string? keyword, int pageNo, int pageSize) | 管理后台分页列表 |
| GetByIdAsync(long id) | 反馈详情 |
| ReplyAsync(long id, string reply, string handlerCode) | 管理员回复（同时把 Status 改为 2） |
| UpdateStatusAsync(long id, string status, string handlerCode) | 更新状态 |
| DeleteAsync(long id) | 删除反馈 |

### 3.2 AppUpgradeService

命名空间：`JeeSiteNET.Modules.App.Application.Services.AppUpgradeService`

| 方法 | 说明 |
|------|------|
| CheckUpgradeAsync(string platform, int currentVersionCode) | 检查升级：返回最新版本信息、是否需要升级、是否强制 |
| GetPagedListAsync(string? platform, int pageNo, int pageSize) | 管理后台列表 |
| GetByIdAsync(long id) | 详情 |
| CreateAsync(AppUpgradeCreateDto dto) | 发布新版本（写入 IsPublished=false 需由管理员切换） |
| UpdateAsync(long id, AppUpgradeUpdateDto dto) | 编辑版本 |
| DeleteAsync(long id) | 删除版本 |
| PublishAsync(long id) | 发布 |
| UnpublishAsync(long id) | 撤回发布 |

**CheckUpgrade 核心逻辑**（伪代码）：

```
SELECT TOP 1 *
FROM   AppUpgrade
WHERE  Platform = @platform
  AND  IsPublished = 1
  AND  (MinVersion IS NULL OR MinVersion <= @currentVersionCode)
  AND  (ExpireDate IS NULL OR ExpireDate > GETDATE())
ORDER BY VersionCode DESC

if (record == null) return { hasUpgrade: false };

return new
{
    hasUpgrade = record.VersionCode > currentVersionCode,
    forceUpgrade = record.ForceUpgrade
                 || (record.MinVersion.HasValue && currentVersionCode < record.MinVersion.Value),
    version = record
};
```

## 四、控制器与 API

### 4.1 AppController

命名空间：`JeeSiteNET.Modules.App.Controllers.AppController`

#### 4.1.1 移动端（匿名/用户）

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| CheckUpgrade | GET | `/api/v1/app/upgrade/check` | 查询升级（`?platform=Android&versionCode=123`） |
| SubmitComment | POST | `/api/v1/app/comment/submit` | 提交意见反馈 |

#### 4.1.2 管理后台（需要权限）

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| GetCommentList | GET | `/api/v1/app/comment/list` | 反馈列表（查询参数：status/keyword/pageNo/pageSize） |
| GetComment | GET | `/api/v1/app/comment/{id}` | 反馈详情 |
| ReplyComment | POST | `/api/v1/app/comment/{id}/reply` | 回复 |
| UpdateCommentStatus | PUT | `/api/v1/app/comment/{id}/status` | 更新状态 |
| DeleteComment | DELETE | `/api/v1/app/comment/{id}` | 删除 |
| GetUpgradeList | GET | `/api/v1/app/upgrade/list` | 版本列表（查询参数：platform/pageNo/pageSize） |
| CreateUpgrade | POST | `/api/v1/app/upgrade` | 发布新版本 |
| UpdateUpgrade | PUT | `/api/v1/app/upgrade/{id}` | 编辑版本 |
| DeleteUpgrade | DELETE | `/api/v1/app/upgrade/{id}` | 删除版本 |
| PublishUpgrade | POST | `/api/v1/app/upgrade/{id}/publish` | 发布 |
| UnpublishUpgrade | POST | `/api/v1/app/upgrade/{id}/unpublish` | 撤回发布 |

### 4.2 请求 / 响应示例

#### 提交反馈（POST /api/v1/app/comment/submit）

请求：

```json
{
  "title": "登录失败",
  "content": "输入正确密码后仍提示用户名或密码错误，错误码=1002。",
  "contact": "support@example.com",
  "deviceInfo": "{\"model\":\"Pixel 7\",\"os\":\"Android 14\",\"appVer\":\"1.2.0\",\"resolution\":\"1080x2400\",\"network\":\"4G\"}"
}
```

响应（200）：

```json
{
  "success": true,
  "message": "已收到您的反馈，感谢支持！",
  "data": { "id": 101, "createDate": "2026-06-12T10:30:00+08:00" }
}
```

#### 检查升级（GET /api/v1/app/upgrade/check?platform=Android&versionCode=100）

响应（200）：

```json
{
  "hasUpgrade": true,
  "forceUpgrade": false,
  "version": {
    "versionName": "1.2.3",
    "versionCode": 123,
    "title": "性能优化与 Bug 修复",
    "content": "- 修复登录偶发失败问题\n- 优化首页加载速度\n- 支持深色模式",
    "downloadUrl": "https://download.example.com/app/android/1.2.3.apk",
    "fileSize": 48120,
    "md5": "a1b2c3d4..."
  }
}
```

当 `forceUpgrade = true` 时，APP 应当阻塞升级流程，直到用户升级完成。

## 五、前端页面（管理后台）

| 页面 | 文件 | 说明 |
|------|------|------|
| 意见反馈列表 | `views/app/AppCommentList.vue` | 列表 + 搜索 + 查看详情 + 回复 + 更新状态 + 删除 |
| 版本管理 | `views/app/AppUpgradeList.vue` | 列表 + 新增 / 编辑 / 删除 + 发布 / 撤回发布 |

前端 API 封装见 `frontend/src/api/appFeedback.ts`。

## 六、APP 端集成示例

### 6.1 启动时检查升级（伪代码）

```ts
const { data } = await api.get('/app/upgrade/check', {
    params: { platform: 'Android', versionCode: 100 }
});

if (data.hasUpgrade) {
    if (data.forceUpgrade) {
        showForceUpgradeDialog(data.version);
    } else {
        showOptionalUpgradeDialog(data.version);
    }
}
```

### 6.2 意见反馈表单

```ts
await api.post('/app/comment/submit', {
    title: '登录失败',
    content: '输入正确密码后仍提示错误',
    contact: 'support@example.com',
    deviceInfo: JSON.stringify({
        model: deviceInfo.model,
        os: deviceInfo.os,
        appVer: deviceInfo.appVer,
        resolution: deviceInfo.resolution,
        network: deviceInfo.network
    })
});
```

### 6.3 DeviceInfo 推荐字段

```jsonc
{
  "model": "Pixel 7",              // 设备型号
  "os": "Android 14",              // 系统版本
  "appVer": "1.2.0",               // APP 版本
  "sdkVer": 34,                    // SDK Level / iOS 版本号
  "resolution": "1080x2400",       // 分辨率
  "network": "4G",                 // Wi-Fi / 4G / 5G / 离线
  "operator": "中国移动",           // 运营商（可选）
  "channel": "googleplay",         // 渠道号
  "locale": "zh-CN",               // 语言/地区
  "deviceId": "xxxx"               // 设备标识（谨慎使用，注意合规）
}
```

## 七、扩展方向

### 7.1 AppUser 账号体系（推荐）

- `POST /api/v1/app/user/register`：手机号 + 短信验证码注册
- `POST /api/v1/app/user/login`：登录获取 Token（或使用 JWT）
- `POST /api/v1/app/user/logout`：登出
- `POST /api/v1/app/user/password/reset`：找回密码
- `PUT /api/v1/app/user/profile`：修改资料
- `POST /api/v1/app/user/avatar`：上传头像

配套服务：`AppUserService`，配合 `JeeSiteNET.Modules.Sys` 的 `ValidCodeService` 处理短信验证码。

### 7.2 消息推送

- 集成个推 / 极光推送 / 华为推送 / 小米推送 / FCM / APNs
- 实体：`AppPushMessage`（标题/内容/目标/状态/失败原因）
- 接口：`POST /api/v1/app/push/send`（后台发）、`POST /api/v1/app/push/bind`（APP 上传 Push Token）
- 调度：使用 `Tasks` 模块的定时任务定期扫描「到期推送」

### 7.3 Banner / 启动广告

- 实体：`AppBanner`、`AppSplash`
- 接口：`GET /api/v1/app/banners?position=home_top`、`GET /api/v1/app/splash`
- 管理后台：在 `views/app/AppBannerList.vue` 维护

### 7.4 支付接口封装

- 微信支付（JSAPI / APP / H5 / Native / 小程序）
- 支付宝（APP / H5 / 当面付）
- 统一调用入口：`POST /api/v1/app/pay/create`、`POST /api/v1/app/pay/notify/{channel}`
- 订单、退款、对账等可作为子领域扩展

### 7.5 统计分析

- `POST /api/v1/app/stats/pv`：页面访问上报
- `POST /api/v1/app/stats/event`：自定义事件上报
- `POST /api/v1/app/stats/crash`：崩溃日志上报
- `GET /api/v1/app/stats/daily`：日活 / 新增 / 留存（后台看板）
- 可接入自建分析服务或第三方（如 GrowingIO / 神策）

### 7.6 其他常见能力

- 常见问题 FAQ：`AppFaq` 实体 + 列表接口
- 关于我们 / 隐私政策 / 用户协议：由 `AppUpgrade` 类模式扩展为 `AppDocument`
- 客服工单系统：以 `AppComment` 为基础扩展「对话式回复」
- APP 配置下发：`GET /api/v1/app/config`，下发主题、Tab 是否可见、实验分组等

## 八、模块主要文件结构

```
modules/JeeSiteNET.Modules.App/
├─ Application/
│   ├─ DTOs/
│   │   └─ AppDto.cs
│   └─ Services/
│       └─ AppService.cs        // 内含 AppCommentService 与 AppUpgradeService
├─ Controllers/
│   └─ AppController.cs
├─ Domain/
│   ├─ Entities/
│   │   ├─ AppComment.cs
│   │   └─ AppUpgrade.cs
│   └─ Interfaces/
│       ├─ IAppCommentRepository.cs
│       └─ IAppUpgradeRepository.cs
├─ Infrastructure/
│   ├─ EntityConfigurations/
│   │   ├─ AppCommentConfiguration.cs
│   │   └─ AppUpgradeConfiguration.cs
│   └─ Repositories/
│       ├─ AppCommentRepository.cs
│       └─ AppUpgradeRepository.cs
├─ AppModuleInstaller.cs
└─ JeeSiteNET.Modules.App.csproj

frontend/src/
├─ api/
│   └─ appFeedback.ts
└─ views/app/
    ├─ AppCommentList.vue
    └─ AppUpgradeList.vue
```

## 九、权限建议

| 权限 | 推荐菜单路径 |
|------|-------------|
| `app:comment:list` | 应用管理 → 意见反馈 |
| `app:comment:reply` | 应用管理 → 意见反馈（回复按钮） |
| `app:upgrade:list` | 应用管理 → 版本管理 |
| `app:upgrade:edit` | 应用管理 → 版本管理（新增/编辑/发布） |
| `app:upgrade:delete` | 应用管理 → 版本管理（删除） |

API 鉴权说明：

- `*/api/v1/app/upgrade/check` 与 `*/api/v1/app/comment/submit` 对移动端 **允许匿名访问**（提交反馈时 userCode 可为空）。
- 其余 `*/api/v1/app/*` 接口要求后台 **登录并具备权限**（与 Sys 模块统一的 Token 机制一致）。
