import { get, post } from './request'
import type { LoginDto, LoginResult } from '@/types/api'

export const authApi = {
  /** 账号密码登录。 */
  login: (data: LoginDto) => post<LoginResult>('/sys/auth/login', data),
  /** 获取图形验证码图片地址（用于 login 场景的人机验证）。 */
  getCaptcha: (key: string) => `/api/v1/sys/validCode/image/${key}`,
  /** 验证图形验证码（返回 JSON）。 */
  validateCaptcha: (key: string, code: string) => get('/sys/validCode/validate/' + key, { code }),
  /** 用户注册。 */
  register: (data: { loginCode: string; password: string; userName?: string; email?: string; phone?: string }) =>
    post<any>('/sys/auth/register', data),
  /** 忘记密码流程（邮箱验证）。 */
  forgotPassword: (data: { loginCode: string; email: string }) =>
    post<any>('/sys/auth/forgot-password', data),
  /** 通过一次性 token 重置密码。 */
  resetPassword: (data: { token: string; newPassword: string }) =>
    post<any>('/sys/auth/reset-password', data),
  /** 获取当前登录用户的完整认证信息（用户信息 + 权限字符串 + 角色 + 系统编码）。 */
  getAuthInfo: () => get<any>('/sys/auth/info'),
  /** 获取当前登录用户可用的菜单路由树，用于前端动态路由。 */
  getMenuRoute: (sysCode?: string) =>
    get<any>('/sys/auth/menu-route', sysCode ? { sysCode } : undefined),
  /** 通过短信/邮件验证码完成登录。 */
  loginByCode: (data: { target: string; code: string }) =>
    post<LoginResult>('/sys/auth/login-by-code', data)
}
