import { get, post } from './request'

export const auditApi = {
  list: (params?: any) => post<any[]>('/sys/audit/list', params || {}),
  getList: (pageNo = 1, pageSize = 20, params?: { auditType?: string; loginCode?: string }) =>
    get<any>('/sys/audit/list', { pageNo, pageSize, ...params }),
}
