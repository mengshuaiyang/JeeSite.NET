import request, { get, post, del } from './request'
import type { FileUploadDto, FileUploadResult } from '@/types/api'

const base = '/sys/file'

export const fileApi = {
  upload: async (file: File, bizType?: string, bizKey?: string) => {
    const formData = new FormData()
    formData.append('file', file)
    if (bizType) formData.append('bizType', bizType)
    if (bizKey) formData.append('bizKey', bizKey)
    const res = await request.post(`/api/v1${base}/upload${bizType || bizKey ? `?bizType=${bizType || ''}&bizKey=${bizKey || ''}` : ''}`, formData, { headers: { 'Content-Type': 'multipart/form-data' } })
    return res.data as { code: number; data: FileUploadResult }
  },
  list: (bizType?: string, bizKey?: string) => get<FileUploadDto[]>(`${base}/biz`, { bizType: bizType || '', bizKey: bizKey || '' }),
  downloadUrl: (uploadId: string) => `/api/v1${base}/download/${uploadId}`,
  delete: (uploadId: string) => del(`${base}/${uploadId}`)
}
