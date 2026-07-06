<template>
  <div class="login-container">
    <a-card class="login-card" title="JeeSite.NET 管理平台">
      <a-tabs v-model:activeKey="activeTab" centered>
        <!-- 账号密码登录 -->
        <a-tab-pane key="password" tab="账号密码登录">
          <a-form :model="form" @finish="handlePasswordLogin">
            <a-form-item name="loginCode" :rules="[{ required: true, message: '请输入登录名' }]">
              <a-input v-model:value="form.loginCode" placeholder="登录名" size="large">
                <template #prefix><user-outlined /></template>
              </a-input>
            </a-form-item>
            <a-form-item name="password" :rules="[{ required: true, message: '请输入密码' }]">
              <a-input-password v-model:value="form.password" placeholder="密码" size="large">
                <template #prefix><lock-outlined /></template>
              </a-input-password>
            </a-form-item>
            <a-form-item v-if="showCaptcha" name="validCode" :rules="[{ required: true, message: '请输入验证码' }]">
              <a-input v-model:value="form.validCode" placeholder="图形验证码" size="large" style="width:200px">
                <template #prefix><safety-outlined /></template>
              </a-input>
              <img
                :src="captchaSrc"
                @click="refreshCaptcha"
                title="点击刷新验证码"
                class="captcha-img"
              />
            </a-form-item>
            <a-form-item>
              <a-button type="primary" html-type="submit" block size="large" :loading="loading">登 录</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>

        <!-- 短信/邮件验证码登录 -->
        <a-tab-pane key="code" tab="短信/邮件验证码登录">
          <a-form :model="codeForm" @finish="handleCodeLogin">
            <a-form-item name="target" :rules="[
              { required: true, message: '请输入手机号或邮箱' },
              {
                pattern: /^(1\d{10}|[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})$/,
                message: '请输入正确的手机号或邮箱'
              }
            ]">
              <a-input v-model:value="codeForm.target" placeholder="手机号或邮箱" size="large">
                <template #prefix><mobile-outlined /></template>
              </a-input>
            </a-form-item>
            <a-form-item name="code" :rules="[{ required: true, message: '请输入验证码' }]">
              <a-input v-model:value="codeForm.code" placeholder="6 位数字验证码" size="large" style="width:220px">
                <template #prefix><safety-outlined /></template>
              </a-input>
              <a-button
                :disabled="sendCodeCountdown > 0 || sendingCode"
                size="large"
                @click="handleSendCode('login')"
                style="margin-left: 8px"
              >
                {{ sendCodeCountdown > 0 ? `${sendCodeCountdown}s 后重发` : sendingCode ? '发送中...' : '获取验证码' }}
              </a-button>
            </a-form-item>
            <a-form-item>
              <a-button type="primary" html-type="submit" block size="large" :loading="loading">登 录</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>
      </a-tabs>

      <div class="login-links">
        <a href="/register">注册账号</a>
        <a href="/forgot-password">忘记密码</a>
      </div>

      <a-divider style="margin:12px 0;font-size:12px;color:#999">第三方登录</a-divider>
      <div class="oauth-row">
        <a-tooltip title="GitHub">
          <a-button shape="circle" size="large" @click="oauthLogin('github')">
            <template #icon><github-outlined /></template>
          </a-button>
        </a-tooltip>
        <a-tooltip title="微信">
          <a-button shape="circle" size="large" @click="oauthLogin('wechat')">
            <template #icon><wechat-outlined /></template>
          </a-button>
        </a-tooltip>
        <a-tooltip title="钉钉">
          <a-button shape="circle" size="large" @click="oauthLogin('dingtalk')">
            <template #icon><dingtalk-outlined /></template>
          </a-button>
        </a-tooltip>
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onBeforeUnmount } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { message } from 'ant-design-vue'
import {
  UserOutlined, LockOutlined, SafetyOutlined, GithubOutlined, WechatOutlined,
  DingtalkOutlined, MobileOutlined
} from '@ant-design/icons-vue'
import { authApi, accountApi } from '@/api'
import { oauth2Api } from '@/api/oauth2'

const router = useRouter()
const userStore = useUserStore()

const activeTab = ref('password')
const loading = ref(false)
const showCaptcha = ref(false)
const captchaKey = ref('')
const captchaSrc = ref('')

// 账号密码表单（不预填凭证，避免硬编码默认账号密码）
const form = reactive({ loginCode: '', password: '', validCode: '' })

// 短信/邮件验证码登录
const codeForm = reactive({ target: '', code: '' })
const sendCodeCountdown = ref(0)
const sendingCode = ref(false)
let sendTimer: number | null = null

function refreshCaptcha() {
  captchaKey.value = Date.now().toString(36) + Math.random().toString(36).slice(2, 6)
  captchaSrc.value = authApi.getCaptcha(captchaKey.value)
}

/** 发送验证码（带 60 秒倒计时）。scene: login/register/reset。 */
async function handleSendCode(scene: string) {
  if (!/^(1\d{10}|[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})$/.test(codeForm.target)) {
    message.error('请输入正确的手机号或邮箱')
    return
  }
  sendingCode.value = true
  try {
    const res = await accountApi.sendValidCode({ target: codeForm.target, scene })
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

async function handlePasswordLogin() {
  loading.value = true
  try {
    const params: any = { loginCode: form.loginCode, password: form.password }
    if (showCaptcha.value) {
      params.validCode = form.validCode
      params.validCodeKey = captchaKey.value
    }
    const res = await userStore.login(params)
    if (res.code === 200) {
      router.push('/')
      return
    }
    message.error(res.message || '登录失败')
    showCaptcha.value = true
    refreshCaptcha()
  } catch {
    message.error('登录失败')
    showCaptcha.value = true
    refreshCaptcha()
  } finally {
    loading.value = false
  }
}

async function handleCodeLogin() {
  loading.value = true
  try {
    const res = await accountApi.loginByValidCode({ target: codeForm.target, code: codeForm.code })
    if (res.code === 200 && res.data) {
      userStore.saveLogin(res.data)
      message.success('登录成功')
      router.push('/')
      return
    }
    message.error(res.message || '登录失败')
  } catch {
    message.error('登录失败')
  } finally {
    loading.value = false
  }
}

function oauthLogin(provider: string) {
  window.location.href = oauth2Api.getLoginUrl(provider)
}

onBeforeUnmount(() => {
  if (sendTimer) {
    clearInterval(sendTimer)
    sendTimer = null
  }
})
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f0f2f5;
}
.login-card {
  width: 440px;
}
.captcha-img {
  cursor: pointer;
  height: 36px;
  vertical-align: middle;
  margin-left: 8px;
  border: 1px solid #eee;
  border-radius: 4px;
}
.login-links {
  display: flex;
  justify-content: space-between;
}
.oauth-row {
  display: flex;
  gap: 12px;
  justify-content: center;
}
</style>
