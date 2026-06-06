import { post, get } from './request'
import type { PageRequest, PageResult, LogDto } from '@/types/api'

export const logApi = {
  list: (data: PageRequest) => post<PageResult<LogDto>>('/sys/log/list', data),
  get: (logId: string) => get<LogDto>('/sys/log/get', { logId })
}
