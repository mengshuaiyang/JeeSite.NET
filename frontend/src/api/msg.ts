import { post, get } from './request'
import type { PageRequest, PageResult, MsgInnerDto, MsgInnerSaveDto, MsgTemplateDto, MsgTemplateSaveDto } from '@/types/api'

export const msgApi = {
  inbox: (data: PageRequest) => post<PageResult<MsgInnerDto>>('/sys/msg/inbox', data),
  sent: (data: PageRequest) => post<PageResult<MsgInnerDto>>('/sys/msg/sent', data),
  send: (data: MsgInnerSaveDto) => post<MsgInnerDto>('/sys/msg/send', data),
  markRead: (msgId: string) => post('/sys/msg/mark-read', { msgId }),
  delete: (msgId: string) => post('/sys/msg/delete', { msgId }),
  templateList: (data: PageRequest) => post<PageResult<MsgTemplateDto>>('/sys/msg/template-list', data),
  templateSave: (data: MsgTemplateSaveDto) => post<MsgTemplateDto>('/sys/msg/template-save', data),
  templateDelete: (id: string) => post('/sys/msg/template-delete', { templateId: id })
}
