<template>
  <div class="auth-container">
    <a-card title="找回密码" style="width:440px">
      <template v-if="step === 1">
        <a-form :model="form" @finish="handleForgotPassword" layout="vertical">
          <a-form-item label="登录名" name="loginCode" :rules="[{required:true,message:'请输入登录名'}]">
            <a-input v-model:value="form.loginCode" placeholder="登录名" size="large" />
          </a-form-item>
          <a-form-item label="注册邮箱" name="email" :rules="[{required:true,message:'请输入邮箱'},{type:'email',message:'邮箱格式不正确'}]">
            <a-input v-model:value="form.email" placeholder="注册时填写的邮箱" size="large" />
          </a-form-item>
          <a-form-item>
            <a-button type="primary" html-type="submit" block size="large" :loading="loading">发送重置邮件</a-button>
          </a-form-item>
          <div style="text-align:center">
            <a href="/login">返回登录</a>
          </div>
        </a-form>
      </template>
      <template v-else>
        <a-result status="success" title="邮件已发送" :sub-title="`重置密码链接已发送至 ${form.email}`">
          <template #extra>
            <a-button type="primary" @click="router.push('/login')">返回登录</a-button>
          </template>
        </a-result>
      </template>
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
const step = ref(1)
const form = reactive({ loginCode: '', email: '' })

async function handleForgotPassword() {
  loading.value = true
  try {
    const res = await authApi.forgotPassword({ loginCode: form.loginCode, email: form.email })
    if (res.code === 200) step.value = 2
    else message.error(res.message || '操作失败')
  } catch { message.error('操作失败') }
  finally { loading.value = false }
}
</script>

<style scoped>
.auth-container { display: flex; justify-content: center; align-items: center; min-height: 100vh; background: #f0f2f5; }
</style>
