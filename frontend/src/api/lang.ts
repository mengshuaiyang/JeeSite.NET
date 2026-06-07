import { post, get } from './request'
import type { LangDto, LangSaveDto } from '@/types/api'

export const langApi = {
  list: () => get<LangDto[]>('/sys/lang/list'),
  getByType: (langType: string) => get<LangDto[]>('/sys/lang/get-by-type', { langType }),
  save: (data: LangSaveDto) => post<LangDto>('/sys/lang/save', data),
  delete: (id: string) => post('/sys/lang/delete', { id })
}
