<template>
  <a-card title="机构管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根机构</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="orgCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子机构</a><a @click="handleDelete(record.orgCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存机构" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="名称"><a-input v-model:value="form.orgName" /></a-form-item>
        <a-form-item label="类型"><a-input v-model:value="form.orgType" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { orgApi } from '@/api'
import { message } from 'ant-design-vue'
import type { OrganizationDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<OrganizationDto|null>(null)
const treeData = ref<OrganizationDto[]>([])
const form = reactive({ orgName: '', orgType: '', parentCode: '0', treeSort: 1000 })
const columns = [{ title: '名称', dataIndex: 'orgName' }, { title: '类型', dataIndex: 'orgType' }, { title: '类型名', dataIndex: 'orgTypeName' }, { title: '操作', key: 'action' }]
async function loadData() {
  loading.value = true; const res = await orgApi.tree()
  if (res.data) treeData.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.orgName=''; form.orgType=''; modalOpen.value = true }
function showAddChild(parent: OrganizationDto) { editItem.value = null; form.parentCode=parent.orgCode; form.orgName=''; form.orgType=''; modalOpen.value = true }
function showEdit(row: OrganizationDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await orgApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await orgApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
