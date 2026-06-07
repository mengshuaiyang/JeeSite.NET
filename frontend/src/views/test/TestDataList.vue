<template>
  <a-card title="测试数据管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="id">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.id)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑测试数据':'新增测试数据'" @ok="handleSave" :confirmLoading="saving" width=600>
      <a-form :model="form" layout="vertical">
        <a-form-item label="文本框"><a-input v-model:value="form.testInput" /></a-form-item>
        <a-form-item label="文本域"><a-textarea v-model:value="form.testTextarea" rows=4 /></a-form-item>
        <a-form-item label="下拉框">
          <a-select v-model:value="form.testSelect" style="width:100%">
            <a-select-option value="option1">选项1</a-select-option>
            <a-select-option value="option2">选项2</a-select-option>
            <a-select-option value="option3">选项3</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="单选框">
          <a-radio-group v-model:value="form.testRadio">
            <a-radio value="radio1">单选1</a-radio>
            <a-radio value="radio2">单选2</a-radio>
            <a-radio value="radio3">单选3</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="日期"><a-date-picker v-model:value="form.testDate" style="width:100%" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { testDataApi } from '@/api'
import { message } from 'ant-design-vue'
import type { TestDataDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<TestDataDto|null>(null)
const list = ref<TestDataDto[]>([])
const form = reactive({ testInput: '', testTextarea: '', testSelect: '', testRadio: '', testDate: '' })
const columns = [
  { title: '文本框', dataIndex: 'testInput' }, { title: '下拉框', dataIndex: 'testSelect' },
  { title: '单选框', dataIndex: 'testRadio' }, { title: '日期', dataIndex: 'testDate' },
  { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true; const res = await testDataApi.list()
  if (res.data) list.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.testInput=''; form.testTextarea=''; form.testSelect=''; form.testRadio=''; form.testDate=''; modalOpen.value = true }
function showEdit(row: TestDataDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await testDataApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(id: string) { await testDataApi.delete(id); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
