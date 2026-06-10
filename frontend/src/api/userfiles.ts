import { get, del } from './request'
import type { FileUploadDto } from '@/types/api'

export const userfilesApi = {
  list: (bizType?: string) => get<FileUploadDto[]>('/sys/userfiles/list', { bizType }),
  previewUrl: (uploadId: string) => `/api/v1/sys/userfiles/preview/${uploadId}`,
  delete: (uploadId: string) => del(`/sys/userfiles/${uploadId}`),
}
