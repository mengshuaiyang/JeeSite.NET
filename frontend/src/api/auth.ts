import { get, post } from './request'
import type { LoginDto, LoginResult } from '@/types/api'

export const authApi = {
  login: (data: LoginDto) => post<LoginResult>('/sys/auth/login', data),
  getCaptcha: (key: string) => `/api/v1/sys/validCode/image/${key}`,
  validateCaptcha: (key: string, code: string) => get('/sys/validCode/validate/' + key, { code })
}
