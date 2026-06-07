<template>
  <a-card title="APP升级管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="id">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.id)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑APP升级':'新增APP升级'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="APP编码"><a-input v-model:value="form.appCode" /></a-form-item>
        <a-form-item label="标题"><a-input v-model:value="form.upTitle" /></a-form-item>
        <a-form-item label="更新内容"><a-textarea v-model:value="form.upContent" rows=4 /></a-form-item>
        <a-form-item label="版本号"><a-input-number v-model:value="form.upVersion" style="width:100%" /></a-form-item>
        <a-form-item label="类型"><a-input v-model:value="form.upType" /></a-form-item>
        <a-form-item label="APK地址"><a-input v-model:value="form.apkUrl" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { appFeedbackApi } from '@/api'
import { message } from 'ant-design-vue'
import type { AppUpgradeDto, AppUpgradeSaveDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<AppUpgradeDto|null>(null)
const list = ref<AppUpgradeDto[]>([])
const form = reactive<AppUpgradeSaveDto>({ appCode: '', upTitle: '', upContent: '', upVersion: undefined, upType: '', apkUrl: '' })
const columns = [
  { title: 'APP编码', dataIndex: 'appCode' }, { title: '标题', dataIndex: 'upTitle' },
  { title: '版本号', dataIndex: 'upVersion' }, { title: '类型', dataIndex: 'upType' },
  { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true; const res = await appFeedbackApi.upgradeList()
  if (res.data) list.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.appCode=''; form.upTitle=''; form.upContent=''; form.upVersion=undefined; form.upType=''; form.apkUrl=''; modalOpen.value = true }
function showEdit(row: AppUpgradeDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await appFeedbackApi.upgradeSave(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(id: string) { await appFeedbackApi.upgradeDelete(id); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
