import { post, get } from './request'
import type { RoleDataScopeDto } from '@/types/api'

export const roleDataScopeApi = {
  getByRole: (roleCode: string) => get<RoleDataScopeDto[]>(`/sys/role-data-scope/role/${roleCode}`),
  save: (data: any) => post('/sys/role-data-scope', data),
  delete: (roleCode: string, menuCode: string) => post(`/sys/role-data-scope/${roleCode}/${menuCode}`, {}),
  deleteByRole: (roleCode: string) => post(`/sys/role-data-scope/role/${roleCode}`, {})
}
