<template>
  <a-card title="测试树管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根节点</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="treeCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子节点</a><a @click="handleDelete(record.treeCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存节点" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="名称"><a-input v-model:value="form.treeName" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { post, get } from '@/api/request'
import { message } from 'ant-design-vue'

const api = { tree: () => get<any[]>('/test/tree/list'), save: (d: any) => post('/test/tree/save', d), delete: (c: string) => post('/test/tree/delete', { treeCode: c }) }

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<any>(null)
const treeData = ref<any[]>([])
const form = reactive({ treeName: '', treeSort: 1000, parentCode: '0' })
const columns = [{ title: '名称', dataIndex: 'treeName' }, { title: '排序', dataIndex: 'treeSort' }, { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }]
async function loadData() {
  loading.value = true; const res = await api.tree()
  if (res.data) treeData.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.treeName=''; modalOpen.value = true }
function showAddChild(parent: any) { editItem.value = null; form.parentCode=parent.treeCode; form.treeName=''; modalOpen.value = true }
function showEdit(row: any) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await api.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await api.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
