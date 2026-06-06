import { post, get } from './request'
import type { OrganizationDto, OrganizationSaveDto } from '@/types/api'

export const orgApi = {
  tree: (orgType?: string) => get<OrganizationDto[]>('/sys/org/tree', { orgType }),
  get: (orgCode: string) => get<OrganizationDto>('/sys/org/get', { orgCode }),
  save: (data: OrganizationSaveDto) => post<OrganizationDto>('/sys/org/save', data),
  delete: (orgCode: string) => post('/sys/org/delete', { orgCode })
}
