import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { UserDto } from '@/types/api'
import { authApi } from '@/api/auth'
import { useAppStore } from './app'

export const useUserStore = defineStore('user', () => {
  const token = ref(localStorage.getItem('token') || '')
  const user = ref<UserDto | null>(null)

  async function login(params: { loginCode: string; password: string; validCode?: string; validCodeKey?: string }) {
    const res = await authApi.login(params)
    if (res.code === 200 && res.data) {
      token.value = res.data.token
      user.value = res.data.user
      localStorage.setItem('token', res.data.token)
      const app = useAppStore()
      if (res.data.user?.permissions) app.setPermissions(res.data.user.permissions)
      await app.loadMenus()
    }
    return res
  }

  function logout() {
    token.value = ''
    user.value = null
    localStorage.removeItem('token')
  }

  return { token, user, login, logout }
})
