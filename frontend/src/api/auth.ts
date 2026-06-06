import { post } from './request'
import type { LoginDto, LoginResult } from '@/types/api'

export const authApi = {
  login: (data: LoginDto) => post<LoginResult>('/sys/auth/login', data)
}
