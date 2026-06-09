<template>
  <div class="auth-container">
    <a-card title="用户注册" style="width:440px">
      <a-form :model="form" @finish="handleRegister" layout="vertical">
        <a-form-item label="登录名" name="loginCode" :rules="[{required:true,message:'请输入登录名'}]">
          <a-input v-model:value="form.loginCode" placeholder="登录名" size="large" />
        </a-form-item>
        <a-form-item label="密码" name="password" :rules="[{required:true,message:'请输入密码'},{min:6,message:'密码至少6位'}]">
          <a-input-password v-model:value="form.password" placeholder="密码" size="large" />
        </a-form-item>
        <a-form-item label="确认密码" name="confirmPassword" :rules="[{required:true,message:'请确认密码'}]">
          <a-input-password v-model:value="form.confirmPassword" placeholder="确认密码" size="large" />
        </a-form-item>
        <a-form-item label="姓名" name="userName">
          <a-input v-model:value="form.userName" placeholder="姓名" size="large" />
        </a-form-item>
        <a-form-item label="邮箱" name="email">
          <a-input v-model:value="form.email" placeholder="邮箱" size="large" type="email" />
        </a-form-item>
        <a-form-item label="手机" name="phone">
          <a-input v-model:value="form.phone" placeholder="手机号" size="large" />
        </a-form-item>
        <a-form-item>
          <a-button type="primary" html-type="submit" block size="large" :loading="loading">注 册</a-button>
        </a-form-item>
        <div style="text-align:center">
          <a href="/login">已有账号？去登录</a>
        </div>
      </a-form>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { authApi } from '@/api'
import { message } from 'ant-design-vue'

const router = useRouter()
const loading = ref(false)
const form = reactive({ loginCode: '', password: '', confirmPassword: '', userName: '', email: '', phone: '' })

async function handleRegister() {
  if (form.password !== form.confirmPassword) { message.warning('两次密码不一致'); return }
  loading.value = true
  try {
    const res = await authApi.register({ loginCode: form.loginCode, password: form.password, userName: form.userName, email: form.email, phone: form.phone })
    if (res.code === 200) {
      message.success('注册成功，请登录')
      router.push('/login')
    } else message.error(res.message || '注册失败')
  } catch { message.error('注册失败') }
  finally { loading.value = false }
}
</script>

<style scoped>
.auth-container { display: flex; justify-content: center; align-items: center; min-height: 100vh; background: #f0f2f5; }
</style>
