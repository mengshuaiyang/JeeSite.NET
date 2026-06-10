import { get, post } from './request'

export const accountApi = {
  getSecurityInfo: () => get<any>('/sys/account/security'),
  setPasswordQuestion: (data: { question: string; answer: string }) => post<any>('/sys/account/password-question', data),
  loginByValidCode: (data: { loginCode: string; validCode: string }) => post<any>('/sys/account/login-by-valid-code', data),
  unlockUser: (userCode: string) => post<any>('/sys/account/unlock', userCode),
}
