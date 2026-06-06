import { post, get } from './request'
import type { PageRequest, PageResult, RoleDto, RoleSaveDto } from '@/types/api'

export const roleApi = {
  list: (data: PageRequest) => post<PageResult<RoleDto>>('/sys/role/list', data),
  get: (roleCode: string) => get<RoleDto>('/sys/role/get', { roleCode }),
  save: (data: RoleSaveDto) => post<RoleDto>('/sys/role/save', data),
  delete: (roleCode: string) => post('/sys/role/delete', { roleCode })
}
