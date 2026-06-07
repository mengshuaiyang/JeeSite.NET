import { post, get } from './request'
import type { RoleDataScopeDto } from '@/types/api'

export const dataScopeApi = {
  getRoleScopes: (roleCode: string) => get<RoleDataScopeDto[]>('/sys/role-data-scope/list', { roleCode }),
  saveRoleScopes: (data: any) => post('/sys/role-data-scope/save', data)
}
