<template>
  <a-card title="用户管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="用户名"><a-input v-model:value="query.userName" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showAdd">新增</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="userCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.userCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editUser?'编辑用户':'新增用户'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="登录名"><a-input v-model:value="form.loginCode" /></a-form-item>
        <a-form-item label="用户名"><a-input v-model:value="form.userName" /></a-form-item>
        <a-form-item label="邮箱"><a-input v-model:value="form.email" /></a-form-item>
        <a-form-item label="手机"><a-input v-model:value="form.phone" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { userApi } from '@/api'
import { message } from 'ant-design-vue'
import type { UserDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editUser = ref<UserDto|null>(null)
const data = reactive({ list: [] as UserDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ userName: '' })
const form = reactive({ loginCode: '', userName: '', userType: 'employee', email: '', phone: '', orgCode: '' })
const columns = [
  { title: '登录名', dataIndex: 'loginCode' }, { title: '用户名', dataIndex: 'userName' },
  { title: '邮箱', dataIndex: 'email' }, { title: '手机', dataIndex: 'phone' },
  { title: '状态', dataIndex: 'status' }, { title: '创建时间', dataIndex: 'createDate' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await userApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { userName: query.userName } as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function showAdd() { editUser.value = null; form.loginCode=''; form.userName=''; form.email=''; form.phone=''; form.orgCode=''; modalOpen.value = true }
function showEdit(row: UserDto) { editUser.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await userApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await userApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
