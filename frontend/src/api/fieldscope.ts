import { post, get } from './request'
import type { RoleFieldScopeDto } from '@/types/api'

export const fieldScopeApi = {
  list: (roleCode: string) => get<RoleFieldScopeDto[]>('/sys/role-field-scope/list', { roleCode }),
  save: (data: any) => post('/sys/role-field-scope/save', data)
}
