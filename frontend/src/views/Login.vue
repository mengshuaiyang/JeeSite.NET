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
        <a-form-item v-if="showCaptcha" name="validCode" :rules="[{required:true,message:'请输入验证码'}]">
          <a-input v-model:value="form.validCode" placeholder="验证码" size="large" style="width:200px">
            <template #prefix><safety-outlined /></template>
          </a-input>
          <img :src="captchaSrc" @click="refreshCaptcha" title="点击刷新验证码" style="cursor:pointer;height:36px;vertical-align:middle;margin-left:8px" />
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
import { UserOutlined, LockOutlined, SafetyOutlined } from '@ant-design/icons-vue'
import { authApi } from '@/api/auth'

const router = useRouter()
const userStore = useUserStore()
const loading = ref(false)
const showCaptcha = ref(false)
const captchaKey = ref('')
const captchaSrc = ref('')

function refreshCaptcha() {
  captchaKey.value = Date.now().toString(36) + Math.random().toString(36).slice(2, 6)
  captchaSrc.value = authApi.getCaptcha(captchaKey.value)
}

const form = reactive({ loginCode: 'admin', password: 'admin', validCode: '' })

async function handleLogin() {
  loading.value = true
  try {
    const params: any = { loginCode: form.loginCode, password: form.password }
    if (showCaptcha.value) {
      params.validCode = form.validCode
      params.validCodeKey = captchaKey.value
    }
    const res = await userStore.login(params)
    if (res.code === 200) router.push('/')
    else {
      message.error(res.message || '登录失败')
      showCaptcha.value = true
      refreshCaptcha()
    }
  } catch { message.error('登录失败'); showCaptcha.value = true; refreshCaptcha() }
  finally { loading.value = false }
}
</script>

<style scoped>
.login-container { display: flex; justify-content: center; align-items: center; min-height: 100vh; background: #f0f2f5; }
</style>
