<template>
  <a-card title="国际化管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="语言类型">
        <a-select v-model:value="query.langType" style="width:120px" allowClear @change="loadData">
          <a-select-option value="zh_CN">中文</a-select-option>
          <a-select-option value="en_US">英文</a-select-option>
          <a-select-option value="ja">日文</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showAdd">新增</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data" :columns="columns" :loading="loading" rowKey="id">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.id)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑翻译':'新增翻译'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="模块编码"><a-input v-model:value="form.moduleCode" /></a-form-item>
        <a-form-item label="翻译编码"><a-input v-model:value="form.langCode" /></a-form-item>
        <a-form-item label="翻译文本"><a-input v-model:value="form.langText" /></a-form-item>
        <a-form-item label="语言类型">
          <a-select v-model:value="form.langType">
            <a-select-option value="zh_CN">中文</a-select-option>
            <a-select-option value="en_US">英文</a-select-option>
            <a-select-option value="ja">日文</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { langApi } from '@/api'
import { message } from 'ant-design-vue'
import type { LangDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<LangDto|null>(null)
const data = ref<LangDto[]>([])
const query = reactive({ langType: undefined as string | undefined })
const form = reactive({ moduleCode: '', langCode: '', langText: '', langType: 'zh_CN' })
const columns = [
  { title: '模块', dataIndex: 'moduleCode' }, { title: '翻译编码', dataIndex: 'langCode' },
  { title: '翻译文本', dataIndex: 'langText' }, { title: '语言类型', dataIndex: 'langType' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  if (query.langType) {
    const res = await langApi.getByType(query.langType)
    if (res.data) data.value = res.data
  } else {
    const res = await langApi.list()
    if (res.data) data.value = res.data
  }
  loading.value = false
}
function showAdd() { editItem.value = null; form.moduleCode=''; form.langCode=''; form.langText=''; form.langType='zh_CN'; modalOpen.value = true }
function showEdit(row: LangDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await langApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(id: string) { await langApi.delete(id); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
