import { get, post } from './request'

export const aiChatApi = {
  chat: (data: { message: string; categoryCode?: string; enableTools?: boolean; history?: any[] }) =>
    post<any>('/cms/ai/chat', data),
  chatJson: (data: { message: string; categoryCode?: string; enableTools?: boolean; history?: any[] }) =>
    post<any>('/cms/ai/chat/json', data),
  chatEntity: (data: { message: string; entityType: string; categoryCode?: string }) =>
    post<any>('/cms/ai/chat/entity', data),
  getTools: () => get<any>('/cms/ai/tools')
}
