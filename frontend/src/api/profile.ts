import { post, get } from './request'
import type { UserDto } from '@/types/api'

export const profileApi = {
  get: () => get<UserDto>('/sys/profile'),
  update: (data: any) => post('/sys/profile/update', data),
  changePassword: (data: { oldPassword: string; newPassword: string }) => post('/sys/profile/password', data),
  updateAvatar: (avatarUrl: string) => post<string>('/sys/profile/avatar', { avatarUrl })
}
