import { post, get } from './request'
import type { AreaDto } from '@/types/api'

export const areaApi = {
  tree: () => get<AreaDto[]>('/sys/area/tree'),
  get: (code: string) => get<AreaDto>('/sys/area/get', { areaCode: code }),
  save: (data: any) => post<AreaDto>('/sys/area/save', data),
  delete: (code: string) => post('/sys/area/delete', { areaCode: code })
}
