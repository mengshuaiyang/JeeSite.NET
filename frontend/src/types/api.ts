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
}

export interface LoginResult {
  token: string
  expires: string
  user: UserDto
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
}

export interface DictDataSaveDto {
  dictCode?: string
  dictType: string
  dictLabel: string
  dictValue: string
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
  status?: string
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
