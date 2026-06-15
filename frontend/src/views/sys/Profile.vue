<template>
  <a-card title="个人中心">
    <a-row :gutter="24">
      <a-col :span="6">
        <a-card title="头像">
          <a-avatar :size="120" :src="avatarUrl" style="display:block;margin:0 auto 16px">
            <template #icon><user-outlined /></template>
          </a-avatar>
          <a-upload :beforeUpload="handleAvatarUpload" accept="image/*" :showUploadList="false">
            <a-button block><upload-outlined /> 更换头像</a-button>
          </a-upload>
        </a-card>
      </a-col>
      <a-col :span="9">
        <a-card title="基本信息">
          <a-form layout="vertical">
            <a-form-item label="登录名"><a-input :value="profile.loginCode" disabled /></a-form-item>
            <a-form-item label="姓名"><a-input v-model:value="editForm.userName" /></a-form-item>
            <a-form-item label="邮箱"><a-input v-model:value="editForm.email" /></a-form-item>
            <a-form-item label="手机"><a-input v-model:value="editForm.phone" /></a-form-item>
            <a-button type="primary" @click="saveProfile" :loading="saving">保存</a-button>
          </a-form>
        </a-card>
      </a-col>
      <a-col :span="9">
        <a-card title="修改密码" style="margin-bottom:16px">
          <a-form layout="vertical">
            <a-form-item label="旧密码"><a-input-password v-model:value="pwdForm.oldPassword" /></a-form-item>
            <a-form-item label="新密码"><a-input-password v-model:value="pwdForm.newPassword" /></a-form-item>
            <a-form-item label="确认密码"><a-input-password v-model:value="pwdForm.confirmPassword" /></a-form-item>
            <a-button type="primary" @click="changePwd" :loading="pwdSaving">修改</a-button>
          </a-form>
        </a-card>
        <a-card title="密码安全问题">
          <template v-if="questionSet">
            <p>已设置安全问题：<strong>{{ currentQuestion }}</strong></p>
            <a-divider />
          </template>
          <template v-else>
            <a-alert message="未设置安全问题" type="warning" show-icon style="margin-bottom:12px" />
          </template>
          <a-form layout="vertical">
            <a-form-item label="安全问题">
              <a-select v-model:value="questionForm.question" placeholder="请选择安全问题">
                <a-select-option value="你的生日是？">你的生日是？</a-select-option>
                <a-select-option value="你小学名称？">你小学名称？</a-select-option>
                <a-select-option value="你母亲姓名？">你母亲姓名？</a-select-option>
                <a-select-option value="你父亲姓名？">你父亲姓名？</a-select-option>
                <a-select-option value="你宠物名？">你宠物名？</a-select-option>
              </a-select>
            </a-form-item>
            <a-form-item label="答案">
              <a-input v-model:value="questionForm.answer" placeholder="请输入答案" />
            </a-form-item>
            <a-form-item label="当前密码">
              <a-input-password v-model:value="questionForm.currentPassword" placeholder="输入当前密码确认" />
            </a-form-item>
            <a-button type="primary" @click="saveQuestion" :loading="questionSaving">保存</a-button>
          </a-form>
        </a-card>
      </a-col>
    </a-row>
    <a-card title="登录信息" style="margin-top:16px">
      <a-descriptions :column="3">
        <a-descriptions-item label="机构">{{ profile.orgName }}</a-descriptions-item>
        <a-descriptions-item label="用户类型">{{ profile.userType }}</a-descriptions-item>
        <a-descriptions-item label="状态"><a-tag :color="profile.status==='0'?'green':'red'">{{ profile.status==='0'?'正常':'停用' }}</a-tag></a-descriptions-item>
        <a-descriptions-item label="最后登录">{{ profile.loginDate }}</a-descriptions-item>
        <a-descriptions-item label="创建时间">{{ profile.createDate }}</a-descriptions-item>
      </a-descriptions>
      <a-divider>桌面数据</a-divider>
      <a-descriptions :column="4">
        <a-descriptions-item label="登录次数">{{ desktop.loginCount }}</a-descriptions-item>
        <a-descriptions-item label="密码上次更新">{{ desktop.pwdUpdateDate }}</a-descriptions-item>
        <a-descriptions-item label="密码安全等级">
          <a-tag :color="desktop.pwdSecurityLevel === '高' ? 'green' : desktop.pwdSecurityLevel === '中' ? 'orange' : 'red'">{{ desktop.pwdSecurityLevel || '未评估' }}</a-tag>
        </a-descriptions-item>
        <a-descriptions-item label="安全问题">
          <a-tag :color="desktop.pwdQuestionSet ? 'green' : 'red'">{{ desktop.pwdQuestionSet ? '已设置' : '未设置' }}</a-tag>
        </a-descriptions-item>
      </a-descriptions>
    </a-card>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { profileApi, fileApi, accountApi } from '@/api'
import { message } from 'ant-design-vue'
import { UserOutlined, UploadOutlined } from '@ant-design/icons-vue'

const saving = ref(false); const pwdSaving = ref(false); const questionSaving = ref(false)
const profile = reactive<any>({ loginCode: '', userName: '', email: '', phone: '', orgName: '', userType: '', status: '', loginDate: '', createDate: '' })
const desktop = reactive<any>({ loginCount: 0, pwdUpdateDate: '', pwdSecurityLevel: '', pwdQuestionSet: false })
const avatarUrl = ref('')
const currentQuestion = ref('')
const questionSet = ref(false)

const editForm = reactive({ userName: '', email: '', phone: '' })
const pwdForm = reactive({ oldPassword: '', newPassword: '', confirmPassword: '' })
const questionForm = reactive({ question: '', answer: '', currentPassword: '' })

async function loadProfile() {
  const res = await profileApi.get()
  if (res.data) {
    Object.assign(profile, res.data)
    editForm.userName = res.data.userName || ''
    editForm.email = res.data.email || ''
    editForm.phone = res.data.phone || ''
    avatarUrl.value = res.data.avatar || ''
  }
  try {
    const deskRes = await profileApi.desktop()
    if (deskRes.data) {
      Object.assign(desktop, deskRes.data)
      questionSet.value = deskRes.data.pwdQuestionSet
      if (deskRes.data.pwdQuestionSet) {
        const qRes = await accountApi.getPasswordQuestion(deskRes.data.loginCode)
        if (qRes.data?.question) currentQuestion.value = qRes.data.question
      }
    }
  } catch (_) { /* desktop data optional */ }
}
async function saveProfile() {
  saving.value = true
  await profileApi.update(editForm)
  message.success('保存成功')
  saving.value = false
  await loadProfile()
}
async function changePwd() {
  if (!pwdForm.oldPassword || !pwdForm.newPassword) { message.warning('请填写完整'); return }
  if (pwdForm.newPassword !== pwdForm.confirmPassword) { message.warning('两次密码不一致'); return }
  pwdSaving.value = true
  await profileApi.changePassword({ oldPassword: pwdForm.oldPassword, newPassword: pwdForm.newPassword })
  message.success('密码修改成功,请重新登录')
  pwdSaving.value = false
  pwdForm.oldPassword = ''; pwdForm.newPassword = ''; pwdForm.confirmPassword = ''
}
async function saveQuestion() {
  if (!questionForm.question || !questionForm.answer || !questionForm.currentPassword) {
    message.warning('请填写完整'); return
  }
  questionSaving.value = true
  await accountApi.resetPasswordByQuestion({
    loginCode: profile.loginCode,
    question: questionForm.question,
    answer: questionForm.answer,
    newPassword: questionForm.currentPassword
  })
  message.success('安全问题设置成功')
  questionSaving.value = false
  questionForm.question = ''; questionForm.answer = ''; questionForm.currentPassword = ''
  await loadProfile()
}
async function handleAvatarUpload(file: File) {
  const res = await fileApi.upload(file, 'user_avatar', profile.userCode)
  if (res.data) {
    const url = `/api/v1/sys/file/download/${res.data.uploadId}`
    await profileApi.updateAvatar(url)
    avatarUrl.value = url
    message.success('头像已更新')
  }
  return false
}
onMounted(loadProfile)
</script>
