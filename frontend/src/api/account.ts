import { get, post } from './request'
import type { LoginResult } from '@/types/api'

export const accountApi = {
  /** 获取当前用户安全信息（密码等级、最后登录、冻结信息等）。 */
  getSecurityInfo: () => get<any>('/sys/account/security'),
  /** 设置或更新用户的密保问题与答案。 */
  setPasswordQuestion: (data: { question: string; answer: string }) =>
    post<any>('/sys/account/password-question', data),
  /** 根据登录码获取用户已设的安全问题，用于自助找回。 */
  getPasswordQuestionByLogin: (data: { loginCode: string }) =>
    post<any>('/sys/account/password-question-by-login', data),
  /** 通过安全问题答案重置密码。 */
  resetPasswordByQuestion: (data: { loginCode: string; answer: string; newPassword: string }) =>
    post<any>('/sys/account/reset-password-by-question', data),
  /** 向目标（手机号/邮箱）发送验证码，scene: login/register/reset。 */
  sendValidCode: (data: { target: string; scene: string }) =>
    post<any>('/sys/account/send-valid-code', data),
  /** 校验验证码是否正确（返回消息即可）。 */
  verifyValidCode: (data: { target: string; scene: string; code: string }) =>
    post<any>('/sys/account/verify-valid-code', data),
  /** 使用短信/邮件验证码完成登录，返回 token 与用户信息。 */
  loginByValidCode: (data: { target: string; code: string }) =>
    post<LoginResult>('/sys/account/login-by-valid-code', data),
  /** 通过验证码重置用户密码。 */
  resetPasswordByCode: (data: { target: string; code: string; newPassword: string }) =>
    post<any>('/sys/account/reset-password-by-code', data),
  /** 解锁被冻结的用户账号。 */
  unlockUser: (userCode: string) => post<any>('/sys/account/unlock', userCode)
}
