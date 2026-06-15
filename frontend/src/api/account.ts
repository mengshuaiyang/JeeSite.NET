import { get, post } from './request'

export const accountApi = {
  getPasswordQuestion: (loginCode: string) => get<any>('/sys/account/password-question-by-login', { loginCode }),
  resetPasswordByQuestion: (data: { loginCode: string; question: string; answer: string; newPassword: string }) =>
    post<any>('/sys/account/reset-password-by-question', data)
}
