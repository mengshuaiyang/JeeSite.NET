import { post, get } from './request'
import type { RoleFieldScopeDto } from '@/types/api'

export const roleFieldScopeApi = {
  getByRoleMenu: (roleCode: string, menuCode: string) => get<RoleFieldScopeDto[]>('/sys/role-field-scope', { roleCode, menuCode }),
  save: (data: any) => post('/sys/role-field-scope', data),
  delete: (id: string) => post(`/sys/role-field-scope/${id}`, {})
}
