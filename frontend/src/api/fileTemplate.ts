import { get, post } from './request'

export const fileTemplateApi = {
  list: () => get<any[]>('/cms/file-template/list'),
  get: (name: string) => get<any>('/cms/file-template/get', { name }),
  save: (name: string, content: string) => post<any>('/cms/file-template/save', { name, content }),
  delete: (name: string) => post<any>('/cms/file-template/delete', { name })
}
