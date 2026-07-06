<template>
  <a-card title="员工管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="姓名"><a-input v-model:value="query.empName" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showAdd">新增</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="empCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.empCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑员工':'新增员工'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="员工号"><a-input v-model:value="form.empNo" /></a-form-item>
        <a-form-item label="姓名"><a-input v-model:value="form.empName" /></a-form-item>
        <a-form-item label="邮箱"><a-input v-model:value="form.email" /></a-form-item>
        <a-form-item label="电话"><a-input v-model:value="form.phone" /></a-form-item>
        <a-form-item label="手机"><a-input v-model:value="form.mobile" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { employeeApi } from '@/api'
import { message } from 'ant-design-vue'
import type { EmployeeDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<EmployeeDto|null>(null)
const data = reactive({ list: [] as EmployeeDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ empName: '' })
const form = reactive({ empNo: '', empName: '', email: '', phone: '', mobile: '' })
const columns = [
  { title: '员工号', dataIndex: 'empNo' }, { title: '姓名', dataIndex: 'empName' },
  { title: '邮箱', dataIndex: 'email' }, { title: '手机', dataIndex: 'mobile' },
  { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await employeeApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { empName: query.empName } as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function showAdd() { editItem.value = null; form.empNo=''; form.empName=''; form.email=''; form.phone=''; form.mobile=''; modalOpen.value = true }
function showEdit(row: EmployeeDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await employeeApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await employeeApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
