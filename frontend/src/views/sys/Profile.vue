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
        <a-card title="修改密码">
          <a-form layout="vertical">
            <a-form-item label="旧密码"><a-input-password v-model:value="pwdForm.oldPassword" /></a-form-item>
            <a-form-item label="新密码"><a-input-password v-model:value="pwdForm.newPassword" /></a-form-item>
            <a-form-item label="确认密码"><a-input-password v-model:value="pwdForm.confirmPassword" /></a-form-item>
            <a-button type="primary" @click="changePwd" :loading="pwdSaving">修改</a-button>
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
    </a-card>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { profileApi, fileApi } from '@/api'
import { message } from 'ant-design-vue'
import { UserOutlined, UploadOutlined } from '@ant-design/icons-vue'

const saving = ref(false); const pwdSaving = ref(false)
const profile = reactive<any>({ loginCode: '', userName: '', email: '', phone: '', orgName: '', userType: '', status: '', loginDate: '', createDate: '' })
const avatarUrl = ref('')

const editForm = reactive({ userName: '', email: '', phone: '' })
const pwdForm = reactive({ oldPassword: '', newPassword: '', confirmPassword: '' })

async function loadProfile() {
  const res = await profileApi.get()
  if (res.data) {
    Object.assign(profile, res.data)
    editForm.userName = res.data.userName || ''
    editForm.email = res.data.email || ''
    editForm.phone = res.data.phone || ''
    avatarUrl.value = res.data.avatar || ''
  }
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
