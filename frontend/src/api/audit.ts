import { get, post } from './request'
import type { PageResult, LogDto } from '@/types/api'

export const auditApi = {
  list: (params?: any) => post<PageResult<LogDto>>('/sys/audit/list', params || {}),
  getList: (pageNo = 1, pageSize = 20, params?: { auditType?: string; loginCode?: string }) =>
    get<PageResult<LogDto>>('/sys/audit/list', { pageNo, pageSize, ...params }),
}
