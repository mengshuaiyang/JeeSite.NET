import { post, get } from './request'
import type { PageRequest, PageResult, ConfigDto, ConfigSaveDto } from '@/types/api'

export const configApi = {
  list: (data: PageRequest) => post<PageResult<ConfigDto>>('/sys/config/list', data),
  get: (configKey: string) => get<ConfigDto>('/sys/config/get', { configKey }),
  save: (data: ConfigSaveDto) => post<ConfigDto>('/sys/config/save', data),
  delete: (configKey: string) => post('/sys/config/delete', { configKey })
}
