import { post, get } from './request'
import type { PageRequest, PageResult, UserDto, UserSaveDto } from '@/types/api'

export const userApi = {
  list: (data: PageRequest) => post<PageResult<UserDto>>('/sys/user/list', data),
  get: (userCode: string) => get<UserDto>('/sys/user/get', { userCode }),
  save: (data: UserSaveDto) => post<UserDto>('/sys/user/save', data),
  delete: (userCode: string) => post('/sys/user/delete', { userCode })
}
