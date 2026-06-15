<template>
  <div class="auth-container">
    <a-card title="找回密码" style="width:480px">
      <a-tabs v-model:activeKey="activeTab" centered>
        <!-- 流程 1：通过密保问题 -->
        <a-tab-pane key="question" tab="密保问题">
          <a-form :model="questionForm" @finish="handleResetByQuestion" layout="vertical">
            <a-form-item label="登录账号" name="loginCode" :rules="[{ required: true, message: '请输入登录名' }]">
              <a-input v-model:value="questionForm.loginCode" size="large" />
            </a-form-item>
            <a-form-item v-if="questionStage === 2">
              <a-alert :message="'安全问题:' + questionForm.question" type="info" show-icon />
            </a-form-item>
            <a-form-item v-if="questionStage === 2" label="答案" name="answer" :rules="[{ required: true, message: '请输入答案' }]">
              <a-input v-model:value="questionForm.answer" size="large" />
            </a-form-item>
            <a-form-item v-if="questionStage === 2" label="新密码" name="newPassword" :rules="[
              { required: true, message: '请输入新密码' },
              { min: 6, message: '密码至少 6 位' }
            ]">
              <a-input-password v-model:value="questionForm.newPassword" size="large" placeholder="请输入新密码" />
            </a-form-item>
            <a-form-item>
              <a-button type="primary" block size="large" :loading="loading" html-type="submit">
                {{ questionStage === 1 ? '获取问题' : '重置密码' }}
              </a-button>
            </a-form-item>
            <div style="text-align:center">
              <a href="/login">返回登录</a>
            </div>
          </a-form>
        </a-tab-pane>

        <!-- 流程 2：通过手机/邮箱验证码 -->
        <a-tab-pane key="code" tab="手机/邮箱验证码">
          <a-form :model="codeForm" @finish="handleResetByCode" layout="vertical">
            <a-form-item label="手机号或邮箱" name="target" :rules="[
              { required: true, message: '请输入手机号或邮箱' },
              {
                pattern: /^(1\d{10}|[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})$/,
                message: '请输入正确的手机号或邮箱'
              }
            ]">
              <a-input v-model:value="codeForm.target" size="large" placeholder="手机号或邮箱" />
            </a-form-item>
            <a-form-item label="验证码" name="code" :rules="[{ required: true, message: '请输入验证码' }]">
              <a-input v-model:value="codeForm.code" size="large" style="width:240px" placeholder="6 位数字验证码" />
              <a-button
                :disabled="sendCodeCountdown > 0 || sendingCode"
                size="large"
                @click="handleSendCode"
                style="margin-left: 8px"
              >
                {{ sendCodeCountdown > 0 ? `${sendCodeCountdown}s 后重发` : sendingCode ? '发送中...' : '获取验证码' }}
              </a-button>
            </a-form-item>
            <a-form-item label="新密码" name="newPassword" :rules="[
              { required: true, message: '请输入新密码' },
              { min: 6, message: '密码至少 6 位' }
            ]">
              <a-input-password v-model:value="codeForm.newPassword" size="large" placeholder="请输入新密码" />
            </a-form-item>
            <a-form-item>
              <a-button type="primary" block size="large" :loading="loading" html-type="submit">重置密码</a-button>
            </a-form-item>
            <div style="text-align:center">
              <a href="/login">返回登录</a>
            </div>
          </a-form>
        </a-tab-pane>
      </a-tabs>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onBeforeUnmount } from 'vue'
import { message } from 'ant-design-vue'
import { accountApi } from '@/api'

const activeTab = ref('code')
const loading = ref(false)
const sendCodeCountdown = ref(0)
const sendingCode = ref(false)
let sendTimer: number | null = null

const codeForm = reactive({ target: '', code: '', newPassword: '' })
const questionForm = reactive({ loginCode: '', question: '', answer: '', newPassword: '' })
const questionStage = ref(1)

/** 发送验证码（60 秒倒计时）。 */
async function handleSendCode() {
  if (!/^(1\d{10}|[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})$/.test(codeForm.target)) {
    message.error('请输入正确的手机号或邮箱')
    return
  }
  sendingCode.value = true
  try {
    const res = await accountApi.sendValidCode({ target: codeForm.target, scene: 'reset' })
    if (res.code === 200) {
      message.success('验证码已发送')
      sendCodeCountdown.value = 60
      sendTimer = window.setInterval(() => {
        sendCodeCountdown.value--
        if (sendCodeCountdown.value <= 0 && sendTimer) {
          clearInterval(sendTimer)
          sendTimer = null
        }
      }, 1000)
    } else {
      message.error(res.message || '发送失败')
    }
  } catch {
    message.error('发送失败')
  } finally {
    sendingCode.value = false
  }
}

/** 通过验证码重置密码。 */
async function handleResetByCode() {
  loading.value = true
  try {
    const res = await accountApi.resetPasswordByCode({
      target: codeForm.target,
      code: codeForm.code,
      newPassword: codeForm.newPassword
    })
    if (res.code === 200) {
      message.success('密码已重置，请重新登录')
      setTimeout(() => (window.location.href = '/login'), 800)
    } else {
      message.error(res.message || '操作失败')
    }
  } catch {
    message.error('操作失败')
  } finally {
    loading.value = false
  }
}

/** 通过密保问题重置密码（两阶段：先获取问题，再提交答案+密码）。 */
async function handleResetByQuestion() {
  loading.value = true
  try {
    if (questionStage.value === 1) {
      const res = await accountApi.getPasswordQuestionByLogin({ loginCode: questionForm.loginCode })
      if (res.code === 200 && res.data?.hasQuestion) {
        questionForm.question = res.data.question || ''
        questionStage.value = 2
      } else {
        message.error(res.message || '该账号未设置安全问题')
      }
    } else {
      const res = await accountApi.resetPasswordByQuestion({
        loginCode: questionForm.loginCode,
        answer: questionForm.answer,
        newPassword: questionForm.newPassword
      })
      if (res.code === 200) {
        message.success('密码已重置，请重新登录')
        setTimeout(() => (window.location.href = '/login'), 800)
      } else {
        message.error(res.message || '操作失败')
      }
    }
  } catch {
    message.error('操作失败')
  } finally {
    loading.value = false
  }
}

onBeforeUnmount(() => {
  if (sendTimer) {
    clearInterval(sendTimer)
    sendTimer = null
  }
})
</script>

<style scoped>
.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f0f2f5;
}
</style>
