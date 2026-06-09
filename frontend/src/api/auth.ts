import { get, post } from './request'
import type { LoginDto, LoginResult } from '@/types/api'

export const authApi = {
  login: (data: LoginDto) => post<LoginResult>('/sys/auth/login', data),
  getCaptcha: (key: string) => `/api/v1/sys/validCode/image/${key}`,
  validateCaptcha: (key: string, code: string) => get('/sys/validCode/validate/' + key, { code }),
  register: (data: { loginCode: string; password: string; userName?: string; email?: string; phone?: string }) =>
    post<any>('/sys/auth/register', data),
  forgotPassword: (data: { loginCode: string; email: string }) =>
    post<any>('/sys/auth/forgot-password', data),
  resetPassword: (data: { token: string; newPassword: string }) =>
    post<any>('/sys/auth/reset-password', data)
}
