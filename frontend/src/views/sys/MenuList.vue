<template>
  <a-card title="菜单管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根菜单</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="menuCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子菜单</a><a @click="handleDelete(record.menuCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editItem?'编辑菜单':'新增菜单'" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="菜单名"><a-input v-model:value="form.menuName" /></a-form-item>
        <a-form-item label="路由"><a-input v-model:value="form.menuHref" /></a-form-item>
        <a-form-item label="权限标识"><a-input v-model:value="form.permission" /></a-form-item>
        <a-form-item label="图标"><a-input v-model:value="form.menuIcon" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { menuApi } from '@/api'
import { message } from 'ant-design-vue'
import type { MenuDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<MenuDto|null>(null)
const treeData = ref<MenuDto[]>([])
const form = reactive({ menuName: '', menuHref: '', permission: '', menuIcon: '', parentCode: '0', treeSort: 1000 })
const columns = [
  { title: '名称', dataIndex: 'menuName' }, { title: '路由', dataIndex: 'menuHref' },
  { title: '权限', dataIndex: 'permission' }, { title: '图标', dataIndex: 'menuIcon' },
  { title: '排序', dataIndex: 'treeSort' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true; const res = await menuApi.tree()
  if (res.data) treeData.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.menuName=''; form.menuHref=''; form.permission=''; form.menuIcon=''; modalOpen.value = true }
function showAddChild(parent: MenuDto) { editItem.value = null; form.parentCode=parent.menuCode; form.menuName=''; form.menuHref=''; form.permission=''; form.menuIcon=''; modalOpen.value = true }
function showEdit(row: MenuDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await menuApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await menuApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
