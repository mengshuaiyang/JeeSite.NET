import request, { post, get } from './request'
import type { FileUploadDto } from '@/types/api'

export const fileApi = {
  upload: async (formData: FormData) => { const res = await request.post('/api/v1/sys/file/upload', formData, { headers: { 'Content-Type': 'multipart/form-data' } }); return res.data },
  bizList: (bizType: string, bizKey: string) => get<FileUploadDto[]>('/sys/file/biz-list', { bizType, bizKey }),
  delete: (id: string) => post('/sys/file/delete', { id })
}
