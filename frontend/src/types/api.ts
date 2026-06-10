export interface ApiResult<T = any> {
  code: number
  message: string
  data?: T
}

export interface PageRequest<T = any> {
  pageNo: number
  pageSize: number
  sortField?: string
  sortOrder?: string
  entity?: T
}

export interface PageResult<T> {
  list: T[]
  total: number
  pageNo: number
  pageSize: number
}

export interface LoginDto {
  loginCode: string
  password: string
  validCode?: string
  validCodeKey?: string
}

export interface LoginResult {
  token: string
  expires: string
  user: UserDto
  isValidCodeLogin?: boolean
}

export interface UserDto {
  userCode: string
  loginCode: string
  userName: string
  userType: string
  avatar?: string
  email?: string
  phone?: string
  orgCode?: string
  orgName?: string
  status?: string
  loginDate?: string
  createDate?: string
  permissions?: string[]
}

export interface UserSaveDto {
  userCode?: string
  loginCode: string
  userName: string
  userType: string
  email?: string
  phone?: string
  orgCode?: string
  roleCodes?: string[]
}

export interface RoleDto {
  roleCode: string
  roleName: string
  roleType?: string
  isSys?: string
  userType?: string
  sort?: number
  status?: string
  createDate?: string
}

export interface RoleSaveDto {
  roleCode?: string
  roleName: string
  roleType?: string
  isSys?: string
  userType?: string
  sort?: number
}

export interface MenuDto {
  menuCode: string
  menuName: string
  menuHref?: string
  menuTarget?: string
  menuIcon?: string
  permission?: string
  weight?: number
  isShow?: string
  moduleCode?: string
  parentCode: string
  parentCodes: string
  treeSort: number
  treeNames: string
  treeLevel: number
  treeLeaf: string
  status?: string
  children?: MenuDto[]
}

export interface MenuSaveDto {
  menuCode?: string
  menuName: string
  menuHref?: string
  menuTarget?: string
  menuIcon?: string
  permission?: string
  weight?: number
  isShow?: string
  moduleCode?: string
  parentCode: string
  treeSort: number
}

export interface OrganizationSaveDto {
  orgCode?: string
  orgName: string
  orgType?: string
  parentCode?: string
  treeSort?: number
}

export interface DictTypeSaveDto {
  dictTypeCode?: string
  dictName: string
  isSys?: string
  sort?: number
  status?: string
}

export interface DictDataSaveDto {
  dictCode?: string
  dictType: string
  dictLabel: string
  dictValue: string
  parentCode?: string
  sort?: number
}

export interface PostSaveDto {
  postCode?: string
  postName: string
  orgCode?: string
  sort?: number
}

export interface ConfigSaveDto {
  configKey?: string
  configName: string
  configValue?: string
  isSys?: string
}

export interface ModuleSaveDto {
  moduleCode?: string
  moduleName: string
  moduleVersion?: string
  mainClass?: string
  isEnabled?: string
}

export interface OrganizationDto {
  orgCode: string
  orgName: string
  orgType?: string
  orgTypeName?: string
  parentCode: string
  parentCodes: string
  treeSort: number
  treeNames: string
  treeLevel: number
  treeLeaf: string
  status?: string
  children?: OrganizationDto[]
}

export interface DictTypeDto {
  dictTypeCode: string
  dictName: string
  isSys?: string
  sort?: number
  status?: string
}

export interface DictDataDto {
  dictCode: string
  dictType: string
  dictLabel: string
  dictValue: string
  sort?: number
  parentCode?: string
  treeLeaf?: string
  status?: string
  children?: DictDataDto[]
}

export interface PostDto {
  postCode: string
  postName: string
  orgCode?: string
  sort?: number
  status?: string
}

export interface ConfigDto {
  configKey: string
  configName: string
  configValue?: string
  isSys?: string
  status?: string
}

export interface ModuleDto {
  moduleCode: string
  moduleName: string
  moduleVersion?: string
  mainClass?: string
  isEnabled?: string
  status?: string
}

export interface LogDto {
  logId: string
  logType?: string
  logTitle?: string
  requestUri?: string
  requestMethod?: string
  executeTime?: number
  userCode?: string
  userName?: string
  orgCode?: string
  remoteIp?: string
  createDate?: string
}

// --- P1-1 员工 ---
export interface EmployeeDto {
  empCode: string; empNo?: string; empName: string
  companyCode?: string; companyName?: string
  officeCode?: string; officeName?: string
  email?: string; phone?: string; mobile?: string
  status?: string
}
export interface EmployeeSaveDto {
  empCode?: string; empNo?: string; empName: string
  companyCode?: string; officeCode?: string
  email?: string; phone?: string; mobile?: string
}

// --- P1-2 公司 + 区划 ---
export interface CompanyDto {
  companyCode: string; companyName: string; viewCode?: string
  areaCode?: string; areaName?: string
  parentCode: string; treeSort: number; status?: string
  children?: CompanyDto[]
}
export interface CompanySaveDto {
  companyCode?: string; companyName: string; viewCode?: string
  areaCode?: string; parentCode?: string; treeSort?: number
}
export interface AreaDto {
  areaCode: string; areaName: string; areaType?: string
  parentCode: string; treeSort: number; status?: string
  children?: AreaDto[]
}

// --- P0-3 数据权限 ---
export interface RoleDataScopeDto {
  id?: string; roleCode: string; menuCode: string
  scopeType?: string; scopeData?: string
}

// --- P0-4 字段权限 ---
export interface RoleFieldScopeDto {
  id: string; roleCode: string; menuCode: string
  entityName: string; entityLabel: string; entityClass: string
  fieldConfig?: string
}

// --- P2-1 文件管理 ---
export interface FileDto {
  fileId: string; fileName: string; fileSize?: number
  fileMd5?: string; fileExt?: string; filePath: string
  createBy?: string; createDate?: string
}
export interface FileUploadResult {
  uploadId: string; fileId: string; fileName: string
  fileSize: number; fileUrl: string
}
export interface FileUploadDto {
  uploadId: string; fileId: string; fileName: string
  fileSize: number; fileUrl: string
  bizType?: string; bizKey?: string
}

// --- P2-2 消息 ---
export interface MsgInnerDto {
  msgId: string; msgTitle: string; msgContent?: string
  senderCode?: string; senderName?: string
  receiveType?: string; receiveCodes?: string
  status?: string; createDate?: string
}
export interface MsgInnerSaveDto {
  msgId?: string; msgTitle: string; msgContent?: string
  receiveType?: string; receiveCodes?: string
}
export interface MsgTemplateDto {
  templateId: string; templateName: string; templateKey: string
  templateContent: string; templateType?: string
  status?: string
}
export interface MsgTemplateSaveDto {
  templateId?: string; templateName: string; templateKey: string
  templateContent: string; templateType?: string
}

// --- P2-3 CMS 评论/留言/标签 ---
export interface CmsCommentDto {
  commentCode: string; categoryCode: string; articleCode: string
  parentCode?: string; articleTitle: string; content: string
  name?: string; status?: string; createDate?: string
}
export interface GuestbookDto {
  gbCode: string; gbType: string; content: string; name: string
  email: string; phone: string; workUnit: string
  reContent?: string; status?: string; createDate?: string
}
export interface TagDto {
  tagName: string; clickNum: number; articleCount?: number
}

// --- P3-1 国际化 ---
export interface LangDto {
  id: string; moduleCode: string; langCode: string
  langText: string; langType: string
}
export interface LangSaveDto {
  id?: string; moduleCode: string; langCode: string
  langText: string; langType: string
}

// --- P3-2 APP ---
export interface AppCommentDto {
  id: string; category?: string; content?: string; contact?: string
  replyContent?: string; status?: string; createDate?: string
}
export interface AppUpgradeDto {
  id: string; appCode?: string; upTitle?: string
  upVersion?: number; upType?: string; apkUrl?: string
  status?: string
}
export interface AppUpgradeSaveDto {
  id?: string; appCode?: string; upTitle?: string; upContent?: string
  upVersion?: number; upType?: string; apkUrl?: string
}

// --- P3-3 业务分类 ---
export interface BizCategoryDto {
  categoryCode: string; viewCode?: string; categoryName: string
  parentCode: string; treeSort: number; status?: string
  children?: BizCategoryDto[]
}

// --- P3-4 测试 ---
export interface TestDataDto {
  id: string; testInput?: string; testTextarea?: string
  testSelect?: string; testDate?: string; status?: string
}

// --- 代码生成 ---
export interface GenTableDto {
  tableName: string; className: string; moduleCode: string
  functionName?: string; functionAuthor?: string; tableComment?: string
  tplCategory?: string; businessName?: string; status?: string
  columns?: GenTableColumnDto[]
}
export interface GenTableColumnDto {
  columnId: string; columnName: string; columnSort?: number
  columnComment?: string; columnType?: string; netType?: string
  propertyName?: string; maxLength?: number; isPk?: string
  isNullable?: string; isInsert?: string; isEdit?: string
  isList?: string; isQuery?: string; queryType?: string
  htmlType?: string; dictType?: string; status?: string
}
export interface GenTableSaveDto {
  tableName: string; className: string; moduleCode: string
  functionName?: string; functionAuthor?: string; tableComment?: string
  tplCategory?: string; businessName?: string
  columns?: GenTableColumnSaveDto[]
}
export interface GenTableColumnSaveDto {
  columnName: string; columnSort?: number; columnComment?: string
  columnType?: string; netType?: string; propertyName?: string
  maxLength?: number; isPk?: string; isNullable?: string
  isInsert?: string; isEdit?: string; isList?: string
  isQuery?: string; queryType?: string; htmlType?: string; dictType?: string
}
export interface DbTableInfo {
  tableName: string; tableComment?: string
}
export interface GenPreviewItem {
  fileName: string; content: string
}

// --- 任务调度 ---
export interface SysJobDto {
  jobId: string; jobName: string; jobGroup?: string
  cronExpression: string; assemblyName?: string; className?: string
  description?: string; runStatus?: string; status?: string
}
export interface SysJobSaveDto {
  jobId?: string; jobName: string; jobGroup?: string
  cronExpression: string; assemblyName?: string; className?: string
  description?: string
}
export interface JobLogDto {
  logId: string; jobName?: string; result?: string
  errorMessage?: string; duration?: number; runDate?: string
}

// --- CMS 文章 ---
export interface CategoryDto {
  categoryCode: string; categoryName: string
  categoryType?: string; image?: string; link?: string
  keywords?: string; description?: string; isShow?: string
  siteCode?: string; parentCode: string; treeSort: number
  treeLeaf?: string; treeLevel?: number; status?: string
  children?: CategoryDto[]
}
export interface CategorySaveDto {
  categoryCode?: string; categoryName: string
  categoryType?: string; image?: string; link?: string
  keywords?: string; description?: string; isShow?: string
  siteCode?: string; parentCode?: string; treeSort?: number
}
export interface ArticleDataDto {
  articleCode: string; content?: string
  relation?: string; isCanComment?: string
}
export interface ArticleDto {
  articleCode: string; categoryCode: string; title: string
  subtitle?: string; summary?: string; author?: string
  source?: string; image?: string; tags?: string
  isTop?: string; isRecommend?: string; isHot?: string
  clickCount?: number; publishDate?: string
  categoryName?: string; status?: string
  articleData?: ArticleDataDto; posIds?: string[]
}
export interface DiskInfo {
  name: string; driveType: string; driveFormat: string
  totalSize: number; availableFreeSpace: number; usedSpace: number
  usagePercent: number
}

export interface ServerInfo {
  osName: string; osVersion: string; osArchitecture: string
  processArchitecture: string; machineName: string; runtimeVersion: string
  processorCount: number; startTime: string; uptimeDays: number
  processMemoryWorkingSet: number; processMemoryPrivateBytes: number
  processMemoryVirtualBytes: number; gcTotalMemory: number
  threadCount: number; handleCount: number
  cpuUsagePercent: number
  disks: DiskInfo[]
}

export interface ArticleSaveDto {
  articleCode?: string; categoryCode: string; title: string
  subtitle?: string; summary?: string; content?: string
  author?: string; source?: string; image?: string
  tags?: string; isTop?: string; isRecommend?: string
  isHot?: string; publishDate?: string
}

export interface EmpUser {
  empCode: string; userCode: string
  empName?: string; loginCode?: string; userName?: string
  createBy?: string; createDate?: string
}
