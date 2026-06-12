
$wikiPath = "d:\Projects\jeesite.net\wiki"

function Extract-Range($lines, $startLine, $endLine) {
    if ($endLine -eq -1) { $endLine = $lines.Count }
    $result = New-Object System.Collections.Generic.List[string]
    for ($i = $startLine; $i -lt $endLine; $i++) {
        $result.Add($lines[$i])
    }
    return $result.ToArray()
}

function Generate-TOC($contentLines) {
    $toc = New-Object System.Collections.Generic.List[string]
    foreach ($line in $contentLines) {
        if ($line -match '^(#{2,4})\s+(.+)$') {
            $level = $matches[1].Length
            $text = $matches[2]
            $cleanText = $text -replace '`', '' -replace '\*', ''
            if ($level -eq 2) {
                $toc.Add("- **$cleanText**")
            } elseif ($level -eq 3) {
                $toc.Add("  - $cleanText")
            } else {
                $toc.Add("    - $cleanText")
            }
        }
    }
    return $toc.ToArray()
}

function Build-Document($title, $description, $role, $readTime, $relatedDocs, $bodyLines, $quickRef) {
    $output = New-Object System.Collections.Generic.List[string]
    $output.Add("<div align=`"right`">")
    $output.Add("  <a href=`"Home`">← 返回首页</a>")
    $output.Add("</div>")
    $output.Add("")
    $output.Add("---")
    $output.Add("")
    $output.Add("# $title")
    $output.Add("")
    $output.Add("> $description")
    $output.Add(">")
    $output.Add("> **适用角色**：$role")
    $output.Add("> **阅读时间**：约 $readTime 分钟")
    $output.Add("> **相关文档**：$relatedDocs")
    $output.Add("> 最后更新: 2026-06-13")
    $output.Add("")
    $output.Add("---")
    $output.Add("")
    $output.Add("## 📋 目录")
    $output.Add("")
    
    $toc = Generate-TOC $bodyLines
    foreach ($t in $toc) { $output.Add($t) }
    
    $output.Add("")
    $output.Add("---")
    $output.Add("")
    
    foreach ($b in $bodyLines) { $output.Add($b) }
    
    $output.Add("")
    $output.Add("---")
    $output.Add("")
    $output.Add("## 💡 快速参考")
    $output.Add("")
    foreach ($qr in $quickRef) { $output.Add($qr) }
    $output.Add("")
    $output.Add("---")
    $output.Add("")
    $output.Add("<div align=`"center`">")
    $output.Add("  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>")
    $output.Add("</div>")
    
    return $output.ToArray()
}

function Get-BodyBetween($sourcePath, $startMarker, $endMarker) {
    $lines = Get-Content $sourcePath -Encoding UTF8
    $startIdx = -1
    $endIdx = $lines.Count
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -like $startMarker) {
            $startIdx = $i
            break
        }
    }
    
    if ($endMarker) {
        for ($i = $startIdx + 1; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -like $endMarker) {
                $endIdx = $i
                break
            }
        }
    }
    
    if ($startIdx -lt 0) { return @() }
    
    $body = New-Object System.Collections.Generic.List[string]
    for ($i = $startIdx + 1; $i -lt $endIdx; $i++) {
        $body.Add($lines[$i])
    }
    return $body.ToArray()
}

function Get-FullBody($sourcePath) {
    $lines = Get-Content $sourcePath -Encoding UTF8
    $body = New-Object System.Collections.Generic.List[string]
    $started = $false
    foreach ($line in $lines) {
        if ($line.StartsWith("# ")) { $started = $true; continue }
        if ($started) { $body.Add($line) }
    }
    return $body.ToArray()
}

Write-Host "=== 任务 1: 拆分 09-工具类综合手册.md ===" -ForegroundColor Green

$sourceFile = "$wikiPath\09-工具类综合手册.md"

# 09 - 加密与国密
$body09 = Get-BodyBetween $sourceFile "## 第一部分*" "## 第二部分*"
$quick09 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **核心类** | EncryptUtil, Sm2Util, Sm4Util, RsaUtil |",
    "| **对称加密** | AES-CBC / SM4-CBC + PKCS7 填充 |",
    "| **非对称加密** | RSA-2048 / SM2 (国密椭圆曲线) |",
    "| **哈希** | MD5, SHA-1, SHA-256, SHA-512, SM3 |",
    "| **密钥管理** | 环境变量注入 / 密钥轮换 / 禁止硬编码 |",
    "| **典型场景** | 密码存储、敏感字段加密、接口签名、文件完整性 |"
)
$doc09 = Build-Document "加密与国密" "JeeSite.NET 在 `src/JeeSiteNET.Core/Utils/` 目录下提供的通用加密工具类与国密算法封装（AES / RSA / MD5 / SHA / SM2 / SM3 / SM4）。" "后端开发人员、安全工程师" "12" "[12-HTML清洗](12-HTML清洗) · [19-数据与字段权限](19-数据与字段权限)" $body09 $quick09
$doc09 | Out-File "$wikiPath\09-加密与国密.md" -Encoding UTF8
Write-Host "OK 09-加密与国密.md"

# 10 - 文件与媒体
$body10 = Get-BodyBetween $sourceFile "## 第二部分*" "## 第三部分*"
$quick10 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **视频处理** | VideoUtil → FFmpeg 截图/转码/信息读取 |",
    "| **图片 GPS** | ImageGeoUtil → EXIF GPS 信息提取 |",
    "| **文件安全** | FileSecurityUtil → 扩展名白名单/签名校验/路径遍历检测 |",
    "| **分片上传** | ChunkUploadService → 大文件分片、断点续传 |",
    "| **文档预览** | PreviewService → LibreOffice + Pdfium 预览 |",
    "| **黑名单** | .exe .dll .bat .cmd .ps1 .js .sh .php 等禁止上传 |"
)
$doc10 = Build-Document "文件与媒体" "文件上传安全校验、视频处理、图片 GPS 信息、分片上传、Office 文档预览等工具类的使用说明。" "全栈开发人员" "10" "[13-验证码与识别](13-验证码与识别)" $body10 $quick10
$doc10 | Out-File "$wikiPath\10-文件与媒体.md" -Encoding UTF8
Write-Host "OK 10-文件与媒体.md"

# 11 - 文本与差异
$body11 = Get-BodyBetween $sourceFile "## 第三部分*" "## 第四部分*"
$quick11 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **差异比较** | DiffMatchPatch → 文本差异、补丁应用、相似度计算 |",
    "| **拼音转换** | PinyinUtil → 带声调/无声调拼音、首字母、全拼连写 |",
    "| **身份证校验** | IdcardUtil → 18位校验、出生日期/性别/区域提取 |",
    "| **字符串扩展** | StringExtensions → Slug/脱敏/HTML 剥离等 |",
    "| **时间工具** | DateTimeUtil → 友好时间字符串/边界/时间戳 |"
)
$doc11 = Build-Document "文本与差异" "文本差异比较、拼音转换、身份证校验、字符串扩展方法等工具类的使用说明。" "全栈开发人员" "8" "[10-文件与媒体](10-文件与媒体)" $body11 $quick11
$doc11 | Out-File "$wikiPath\11-文本与差异.md" -Encoding UTF8
Write-Host "OK 11-文本与差异.md"

# 12 - HTML清洗
$body12 = Get-BodyBetween $sourceFile "## 第四部分*" "## 第五部分*"
$quick12 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **核心类** | HtmlSanitizerUtil → 基于白名单的 HTML 清洗 |",
    "| **清洗强度** | SanitizeRich (富文本) / SanitizeStrict (严格) / SanitizeForPreview (预览) |",
    "| **白名单策略** | 标签/属性/CSS/URL协议 四层白名单，其余一律移除 |",
    "| **XSS 防护** | script/iframe/onerror/javascript: 等危险向量全部过滤 |",
    "| **深度防御** | 后端清洗 + 前端 DOMPurify + CSP 响应头 |"
)
$doc12 = Build-Document "HTML清洗" "富文本内容的白名单清洗策略，防 XSS 攻击，含默认白名单配置、扩展方式及四层防御体系。" "全栈开发人员、安全工程师" "10" "[09-加密与国密](09-加密与国密) · [19-数据与字段权限](19-数据与字段权限)" $body12 $quick12
$doc12 | Out-File "$wikiPath\12-HTML清洗.md" -Encoding UTF8
Write-Host "OK 12-HTML清洗.md"

# 13 - 验证码与识别
$body13 = Get-BodyBetween $sourceFile "## 第五部分*" "## 第六部分*"
$quick13 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **图形验证码** | CaptchaUtil → 随机字符/算术验证码、噪点干扰 |",
    "| **UA 解析** | UserAgentUtil → 浏览器/OS/设备类型识别 |",
    "| **条码/二维码** | BarcodeUtil → Code128/QRCode 生成与识别 |",
    "| **短信/邮件** | ValidCodeService → 场景化验证码、5分钟有效、60秒冷却 |",
    "| **安全策略** | 一次性使用、IP 限流、失败锁定、敏感操作短信校验 |"
)
$doc13 = Build-Document "验证码与识别" "图形验证码生成、User-Agent 解析、条码/二维码生成识别、短信/邮件验证码服务等工具类。" "全栈开发人员" "8" "[10-文件与媒体](10-文件与媒体)" $body13 $quick13
$doc13 | Out-File "$wikiPath\13-验证码与识别.md" -Encoding UTF8
Write-Host "OK 13-验证码与识别.md"

# 14 - Excel导入导出
$body14 = Get-BodyBetween $sourceFile "## 第六部分*" "## 综合附录*"
$quick14 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **核心服务** | ExcelService → 基于 NPOI 的导入/导出/模板下载 |",
    "| **列注解** | ExcelFieldAttribute → 指定列名、顺序、格式、类型 |",
    "| **自定义类型** | IExcelFieldType 接口 → 可扩展任意字段类型 |",
    "| **导入流程** | 表头校验 → 类型转换 → 行级错误收集 → 批量写入 |",
    "| **导出格式** | xlsx / xls 双格式，支持合并单元格、列宽自动 |",
    "| **安全防护** | 文件大小限制、行数量限制、单元格最大长度、禁止公式 |"
)
$doc14 = Build-Document "Excel导入导出" "NPOI 驱动的 Excel 导入导出服务，ExcelFieldAttribute 列注解，IExcelFieldType 自定义字段类型体系。" "全栈开发人员" "10" "[03-Sys系统管理](03-Sys系统管理)" $body14 $quick14
$doc14 | Out-File "$wikiPath\14-Excel导入导出.md" -Encoding UTF8
Write-Host "OK 14-Excel导入导出.md"

Write-Host "=== 任务 2: 拆分 10-安全与权限综合手册.md ===" -ForegroundColor Yellow

$sourceFile10 = "$wikiPath\10-安全与权限综合手册.md"

# 15 - JWT 认证
$body15 = Get-BodyBetween $sourceFile10 "## 第一部分*" "## 第二部分*"
$quick15 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **核心服务** | AuthService → 登录/刷新/注销/获取用户信息 |",
    "| **Token 结构** | Header + Payload (sub/name/corp_code/roles/permissions) + HMAC-SHA256 签名 |",
    "| **有效期** | Access Token 默认 12 小时，Refresh Token 30 天（可配置） |",
    "| **权限标识** | sys:user:add / sys:user:edit 等模块:实体:操作三级体系 |",
    "| **吊销机制** | jwt:revoked:{jti} Redis 黑名单 → 中间件校验 |",
    "| **安全建议** | HTTPS 传输、Secret 环境变量注入、失败锁定、密码强度校验 |"
)
$doc15 = Build-Document "JWT认证" "基于 JWT 的无状态认证体系：Token 生成/刷新/吊销、权限标识、在线用户管理、前端集成。" "全栈开发人员、安全工程师" "12" "[16-OAuth2登录](16-OAuth2登录) · [19-数据与字段权限](19-数据与字段权限)" $body15 $quick15
$doc15 | Out-File "$wikiPath\15-JWT认证.md" -Encoding UTF8
Write-Host "OK 15-JWT认证.md"

# 16 - OAuth2 登录
$body16 = Get-BodyBetween $sourceFile10 "## 第二部分*" "## 第三部分*"
$quick16 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **支持平台** | GitHub / 微信开放平台 / 钉钉开放平台 |",
    "| **协议** | OAuth 2.0 Authorization Code Flow |",
    "| **核心服务** | OAuth2Service + IOAuth2Provider 接口 |",
    "| **关联策略** | provider + provider_user_id → 本地账号，邮箱匹配自动关联 |",
    "| **关联表** | sys_oauth2_user (provider, provider_user_id, user_code) |",
    "| **安全** | state 参数防 CSRF、HTTPS 强制、Token 服务端交换 |"
)
$doc16 = Build-Document "OAuth2登录" "通过 GitHub / 微信 / 钉钉 OAuth 2.0 协议实现第三方账号登录，含本地账号关联与自定义 Provider 扩展。" "全栈开发人员" "10" "[15-JWT认证](15-JWT认证) · [17-CAS单点登录](17-CAS单点登录)" $body16 $quick16
$doc16 | Out-File "$wikiPath\16-OAuth2登录.md" -Encoding UTF8
Write-Host "OK 16-OAuth2登录.md"

# 17 - CAS 单点登录
$body17 = Get-BodyBetween $sourceFile10 "## 第三部分*" "## 第四部分*"
$quick17 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **标准** | Apereo CAS Protocol 3.0（兼容 2.0） |",
    "| **角色** | JeeSite.NET 作为 CAS Client (Service) |",
    "| **服务** | CasAuthService → Challenge/Callback/Logout |",
    "| **Ticket 校验** | GET /p3/serviceValidate → XML 解析 |",
    "| **属性映射** | 可配置 LDAP/CAS 属性 → sys_user 字段映射 |",
    "| **自动创建** | AutoCreateUser = true 时 CAS 用户首次登录自动开户 |"
)
$doc17 = Build-Document "CAS单点登录" "作为 CAS Client 接入企业 Apereo CAS Server 实现统一身份认证，支持属性映射、自动开户、单点登出。" "系统管理员、架构师" "10" "[15-JWT认证](15-JWT认证) · [18-LDAP认证](18-LDAP认证)" $body17 $quick17
$doc17 | Out-File "$wikiPath\17-CAS单点登录.md" -Encoding UTF8
Write-Host "OK 17-CAS单点登录.md"

# 18 - LDAP 认证
$body18 = Get-BodyBetween $sourceFile10 "## 第四部分*" "## 第五部分*"
$quick18 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **支持目录** | Active Directory (AD) / OpenLDAP |",
    "| **核心类** | LdapAuthService → Bind/Search/用户同步 |",
    "| **搜索过滤** | (&(objectClass=user)(sAMAccountName={0})) |",
    "| **安全** | 强制 LDAPS (636/3269)、BindDn 最小权限、密码不入日志 |",
    "| **同步机制** | LdapSyncJob → 定时增量同步 AD 用户到本地 |",
    "| **字段映射** | displayName/mail/telephoneNumber → user_name/email/mobile |"
)
$doc18 = Build-Document "LDAP认证" "企业 AD / OpenLDAP 目录集成，LDAPS 安全连接、用户属性同步、最小权限 BindDn 配置。" "系统管理员、安全工程师" "9" "[15-JWT认证](15-JWT认证) · [17-CAS单点登录](17-CAS单点登录)" $body18 $quick18
$doc18 | Out-File "$wikiPath\18-LDAP认证.md" -Encoding UTF8
Write-Host "OK 18-LDAP认证.md"

# 19 - 数据权限与字段权限
$body19 = Get-BodyBetween $sourceFile10 "## 第五部分*" "## 综合附录*"
$quick19 = @(
    "| 项目 | 关键信息 |",
    "|------|---------|",
    "| **权限体系** | 菜单权限 + 数据权限（7类） + 字段权限（可见/只读/隐藏） |",
    "| **数据权限类型** | 全部 / 本公司 / 本公司及以下 / 本机构 / 本机构及以下 / 仅本人 / 自定义 SQL |",
    "| **实现方式** | EF Core QueryFilter + 全局拦截器 + 行级 WHERE 注入 |",
    "| **字段权限** | FieldScopeService → 根据角色动态处理 DTO 返回 |",
    "| **优先级规则** | 多角色取最高权限；菜单可覆盖降级角色权限 |",
    "| **实体标记** | IDataScoped 接口 或 [DataScope] 特性 |"
)
$doc19 = Build-Document "数据与字段权限" "企业级数据权限体系：7 类行级数据权限 + 字段级可见/只读/隐藏 + 菜单级覆盖规则。" "架构师、安全工程师" "12" "[15-JWT认证](15-JWT认证) · [33-深入架构剖析](33-深入架构剖析)" $body19 $quick19
$doc19 | Out-File "$wikiPath\19-数据与字段权限.md" -Encoding UTF8
Write-Host "OK 19-数据与字段权限.md"

Write-Host "=== 任务 1 & 2 完成 ===" -ForegroundColor Green
