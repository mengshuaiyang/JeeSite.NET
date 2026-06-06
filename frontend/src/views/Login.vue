<template>
  <div class="login-container">
    <a-card title="JeeSite.NET 管理平台" style="width:400px">
      <a-form :model="form" @finish="handleLogin">
        <a-form-item name="loginCode" :rules="[{required:true,message:'请输入登录名'}]">
          <a-input v-model:value="form.loginCode" placeholder="登录名" size="large">
            <template #prefix><user-outlined /></template>
          </a-input>
        </a-form-item>
        <a-form-item name="password" :rules="[{required:true,message:'请输入密码'}]">
          <a-input-password v-model:value="form.password" placeholder="密码" size="large">
            <template #prefix><lock-outlined /></template>
          </a-input-password>
        </a-form-item>
        <a-form-item>
          <a-button type="primary" html-type="submit" block size="large" :loading="loading">登 录</a-button>
        </a-form-item>
      </a-form>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { message } from 'ant-design-vue'
import { UserOutlined, LockOutlined } from '@ant-design/icons-vue'

const router = useRouter()
const userStore = useUserStore()
const loading = ref(false)
const form = reactive({ loginCode: 'admin', password: 'admin' })

async function handleLogin() {
  loading.value = true
  try {
    const res = await userStore.login(form.loginCode, form.password)
    if (res.code === 200) router.push('/')
    else message.error(res.message || '登录失败')
  } catch { message.error('登录失败') }
  finally { loading.value = false }
}
</script>

<style scoped>
.login-container { display: flex; justify-content: center; align-items: center; min-height: 100vh; background: #f0f2f5; }
</style>
