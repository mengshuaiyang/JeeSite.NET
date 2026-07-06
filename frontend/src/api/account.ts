import { get, post } from './request'
import type { LoginResult } from '@/types/api'

export const accountApi = {
  getPasswordQuestion: (loginCode: string) => get<any>('/sys/account/password-question-by-login', { loginCode }),
  resetPasswordByQuestion: (data: { loginCode: string; answer: string; newPassword: string; question?: string }) =>
    post<any>('/sys/account/reset-password-by-question', data),
  /** 发送短信/邮件验证码（登录或找回密码场景）。 */
  sendValidCode: (data: { target: string; scene: string }) =>
    post<any>('/sys/account/send-valid-code', data),
  /** 使用验证码直接登录。 */
  loginByValidCode: (data: { target: string; code: string }) =>
    post<LoginResult>('/sys/account/login-by-valid-code', data),
  /** 通过验证码重置密码。 */
  resetPasswordByCode: (data: { target: string; code: string; newPassword: string }) =>
    post<any>('/sys/account/reset-password-by-code', data),
  /** 按登录码获取安全问题（[FromBody]，用于自助找回）。 */
  getPasswordQuestionByLogin: (data: { loginCode: string }) =>
    post<any>('/sys/account/password-question-by-login', data)
}
