<template>
  <a-card title="业务分类管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根分类</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="categoryCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子分类</a><a @click="handleDelete(record.categoryCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存分类" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="分类名称"><a-input v-model:value="form.categoryName" /></a-form-item>
        <a-form-item label="分类编码"><a-input v-model:value="form.viewCode" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { bizCategoryApi } from '@/api'
import { message } from 'ant-design-vue'
import type { BizCategoryDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<BizCategoryDto|null>(null)
const treeData = ref<BizCategoryDto[]>([])
const form = reactive({ categoryName: '', viewCode: '', parentCode: '0', treeSort: 1000 })
const columns = [
  { title: '分类编码', dataIndex: 'viewCode' }, { title: '分类名称', dataIndex: 'categoryName' },
  { title: '排序', dataIndex: 'treeSort' }, { title: '状态', dataIndex: 'status' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true; const res = await bizCategoryApi.tree()
  if (res.data) treeData.value = res.data; loading.value = false
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.categoryName=''; form.viewCode=''; modalOpen.value = true }
function showAddChild(parent: BizCategoryDto) { editItem.value = null; form.parentCode=parent.categoryCode; form.categoryName=''; form.viewCode=''; modalOpen.value = true }
function showEdit(row: BizCategoryDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await bizCategoryApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await bizCategoryApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
