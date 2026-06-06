import { post, get } from './request'
import type { PageRequest, PageResult, ModuleDto, ModuleSaveDto } from '@/types/api'

export const moduleApi = {
  list: (data: PageRequest) => post<PageResult<ModuleDto>>('/sys/module/list', data),
  get: (moduleCode: string) => get<ModuleDto>('/sys/module/get', { moduleCode }),
  save: (data: ModuleSaveDto) => post<ModuleDto>('/sys/module/save', data),
  delete: (moduleCode: string) => post('/sys/module/delete', { moduleCode })
}
