import { post, get } from './request'
import type { PageRequest, PageResult, PostDto, PostSaveDto } from '@/types/api'

export const postApi = {
  list: (data: PageRequest) => post<PageResult<PostDto>>('/sys/post/list', data),
  get: (postCode: string) => get<PostDto>('/sys/post/get', { postCode }),
  save: (data: PostSaveDto) => post<PostDto>('/sys/post/save', data),
  delete: (postCode: string) => post('/sys/post/delete', { postCode })
}
