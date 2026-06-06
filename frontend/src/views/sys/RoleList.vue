<template>
  <a-card title="角色管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="角色名"><a-input v-model:value="query.roleName" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showAdd">新增</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="roleCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.roleCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑角色':'新增角色'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="角色名"><a-input v-model:value="form.roleName" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { roleApi } from '@/api'
import { message } from 'ant-design-vue'
import type { RoleDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<RoleDto|null>(null)
const data = reactive({ list: [] as RoleDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ roleName: '' })
const form = reactive({ roleName: '' })
const columns = [
  { title: '角色编码', dataIndex: 'roleCode' }, { title: '角色名', dataIndex: 'roleName' },
  { title: '角色类型', dataIndex: 'roleType' }, { title: '状态', dataIndex: 'status' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  const res = await roleApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { roleName: query.roleName } as any })
  if (res.data) { data.list = res.data.list; data.total = res.data.total }
  loading.value = false
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function showAdd() { editItem.value = null; form.roleName=''; modalOpen.value = true }
function showEdit(row: RoleDto) { editItem.value = row; form.roleName=row.roleName; modalOpen.value = true }
async function handleSave() { saving.value = true; await roleApi.save({...form}); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await roleApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
