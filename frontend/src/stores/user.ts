import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { UserDto, LoginResult } from '@/types/api'
import { authApi } from '@/api/auth'
import { useAppStore } from './app'
import { connectSignalR, disconnectSignalR } from '@/utils/signalr'

/** 全局用户状态管理：登录、登出、权限缓存。 */
export const useUserStore = defineStore('user', () => {
  const token = ref<string>(localStorage.getItem('token') || '')
  const user = ref<UserDto | null>(null)

  /** 账号密码登录。 */
  async function login(params: {
    loginCode: string
    password: string
    validCode?: string
    validCodeKey?: string
  }) {
    const res = await authApi.login(params)
    if (res.code === 200 && res.data) saveLogin(res.data)
    return res
  }

  /** 统一的登录结果落库 / 缓存处理（供账号密码与验证码登录复用）。 */
  function saveLogin(data: LoginResult) {
    token.value = data.token
    user.value = data.user
    localStorage.setItem('token', data.token)
    const app = useAppStore()
    if (data.user?.permissions) app.setPermissions(data.user.permissions)
    app.loadSysCodes().catch(() => {})
    app.loadMenus().catch(() => {})
    connectSignalR(data.token)
  }

  /** 登出：清理本地状态、断开实时连接。 */
  function logout() {
    disconnectSignalR()
    token.value = ''
    user.value = null
    localStorage.removeItem('token')
  }

  return { token, user, login, logout, saveLogin }
})
