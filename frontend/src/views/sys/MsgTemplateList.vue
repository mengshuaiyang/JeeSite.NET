<template>
  <a-card title="消息模板">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="templateId"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.templateId)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑模板':'新增模板'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="模板名称"><a-input v-model:value="form.templateName" /></a-form-item>
        <a-form-item label="模板键"><a-input v-model:value="form.templateKey" /></a-form-item>
        <a-form-item label="模板内容"><a-textarea v-model:value="form.templateContent" rows="4" /></a-form-item>
        <a-form-item label="模板类型"><a-input v-model:value="form.templateType" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { msgApi } from '@/api'
import { message } from 'ant-design-vue'
import type { MsgTemplateDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<MsgTemplateDto|null>(null)
const data = reactive({ list: [] as MsgTemplateDto[], total: 0, pageNo: 1, pageSize: 20 })
const form = reactive({ templateName: '', templateKey: '', templateContent: '', templateType: '' })
const columns = [
  { title: '模板名称', dataIndex: 'templateName' }, { title: '模板键', dataIndex: 'templateKey' },
  { title: '类型', dataIndex: 'templateType' }, { title: '状态', dataIndex: 'status' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await msgApi.templateList({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function showAdd() { editItem.value = null; form.templateName=''; form.templateKey=''; form.templateContent=''; form.templateType=''; modalOpen.value = true }
function showEdit(row: MsgTemplateDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await msgApi.templateSave(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(id: string) { await msgApi.templateDelete(id); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
