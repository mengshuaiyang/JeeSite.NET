import { post } from './request'
import type { PageRequest, PageResult, GuestbookDto } from '@/types/api'

export const guestbookApi = {
  list: (data: PageRequest) => post<PageResult<GuestbookDto>>('/cms/guestbook/list', data),
  reply: (gbCode: string, reContent: string) => post('/cms/guestbook/reply', { gbCode, reContent }),
  delete: (gbCode: string) => post('/cms/guestbook/delete', { gbCode })
}
