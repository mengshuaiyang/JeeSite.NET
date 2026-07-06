<template>
  <a-card title="公司管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根公司</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="companyCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子级</a><a @click="handleDelete(record.companyCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存公司" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="公司名称"><a-input v-model:value="form.companyName" /></a-form-item>
        <a-form-item label="公司编码"><a-input v-model:value="form.viewCode" /></a-form-item>
        <a-form-item label="区域编码"><a-input v-model:value="form.areaCode" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { companyApi } from '@/api'
import { message } from 'ant-design-vue'
import type { CompanyDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<CompanyDto|null>(null)
const treeData = ref<CompanyDto[]>([])
const form = reactive({ companyName: '', viewCode: '', areaCode: '', parentCode: '0', treeSort: 1000 })
const columns = [{ title: '公司编码', dataIndex: 'viewCode' }, { title: '公司名称', dataIndex: 'companyName' }, { title: '区域', dataIndex: 'areaName' }, { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }]
async function loadData() {
  loading.value = true
  try {
    const res = await companyApi.tree()
    if (res.data) treeData.value = res.data
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.companyName=''; form.viewCode=''; form.areaCode=''; modalOpen.value = true }
function showAddChild(parent: CompanyDto) { editItem.value = null; form.parentCode=parent.companyCode; form.companyName=''; form.viewCode=''; form.areaCode=''; modalOpen.value = true }
function showEdit(row: CompanyDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await companyApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await companyApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
