import { post } from './request'
import type { PageRequest, PageResult, CmsCommentDto } from '@/types/api'

export const cmsCommentApi = {
  list: (data: PageRequest) => post<PageResult<CmsCommentDto>>('/cms/comment/list', data),
  audit: (commentCode: string, status: string, auditComment?: string) => post('/cms/comment/audit', { commentCode, status, auditComment }),
  delete: (commentCode: string) => post('/cms/comment/delete', { commentCode })
}
