<template>
  <div class="callback-container">
    <a-spin size="large" />
    <p style="margin-top:16px">正在登录...</p>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'
import { get } from '@/api/request'
import type { UserDto } from '@/types/api'
import { message } from 'ant-design-vue'

const route = useRoute()
const router = useRouter()
const userStore = useUserStore()
const appStore = useAppStore()

onMounted(async () => {
  const token = route.query.token as string | undefined
  const error = route.query.error as string | undefined

  if (error) {
    message.error(error)
    router.replace('/login')
    return
  }

  if (!token) {
    message.error('登录失败：缺少令牌')
    router.replace('/login')
    return
  }

  localStorage.setItem('token', token)
  userStore.token = token

  try {
    const res = await get<UserDto>('/sys/profile')
    if (res.code === 200 && res.data) {
      userStore.user = res.data
      if (res.data.permissions) appStore.setPermissions(res.data.permissions)
      await appStore.loadMenus()
      router.replace('/')
    } else {
      message.error(res.message || '获取用户信息失败')
      router.replace('/login')
    }
  } catch {
    message.error('获取用户信息失败')
    router.replace('/login')
  }
})
</script>

<style scoped>
.callback-container { display: flex; flex-direction: column; justify-content: center; align-items: center; min-height: 100vh; background: #f0f2f5; }
</style>
