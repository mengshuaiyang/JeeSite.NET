import { request } from '@/utils/request'

export const fileTemplateApi = {
  list: () => request.get('/cms/file-template/list'),
  get: (name: string) => request.get('/cms/file-template/get', { params: { name } }),
  save: (name: string, content: string) => request.post('/cms/file-template/save', { name, content }),
  delete: (name: string) => request.post('/cms/file-template/delete', { name })
}
