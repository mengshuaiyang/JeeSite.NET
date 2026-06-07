<template>
  <a-card title="栏目管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根栏目</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="categoryCode" defaultExpandAllRows
      :pagination="false">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='type'">{{ typeMap[record.categoryType || ''] || record.categoryType }}</template>
        <template v-if="column.key==='isShow'">
          <a-tag :color="record.isShow==='1'?'green':'default'">{{ record.isShow==='1'?'显示':'隐藏' }}</a-tag>
        </template>
        <template v-if="column.key==='status'">
          <a-tag :color="record.status==='0'?'green':'red'">{{ record.status==='0'?'正常':'停用' }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="showEdit(record)">编辑</a>
            <a @click="showAddChild(record)">新增子栏目</a>
            <a-popconfirm title="确定删除?" @confirm="handleDelete(record.categoryCode)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="isEdit?'编辑栏目':'新增栏目'" @ok="handleSave" :confirmLoading="saving" width="640px">
      <a-form :model="form" layout="vertical">
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="栏目名" required><a-input v-model:value="form.categoryName" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="类型"><a-select v-model:value="form.categoryType">
            <a-select-option value="article">文章</a-select-option>
            <a-select-option value="link">链接</a-select-option>
            <a-select-option value="picture">图片</a-select-option>
          </a-select></a-form-item></a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="排序"><a-input-number v-model:value="form.treeSort" style="width:100%" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="显示"><a-switch v-model:checked="form.isShowBool" checkedValue="1" unCheckedValue="0" /></a-form-item></a-col>
        </a-row>
        <a-form-item label="链接"><a-input v-model:value="form.link" placeholder="外部链接" /></a-form-item>
        <a-form-item label="关键字"><a-input v-model:value="form.keywords" placeholder="逗号分隔" /></a-form-item>
        <a-form-item label="描述"><a-textarea v-model:value="form.description" :rows="2" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { categoryApi } from '@/api'
import { message } from 'ant-design-vue'
import type { CategoryDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const isEdit = ref(false)
const treeData = ref<CategoryDto[]>([])
const form = reactive({ categoryName: '', categoryType: 'article', treeSort: 1000, parentCode: '0', link: '', keywords: '', description: '', isShowBool: '1' as string | boolean, categoryCode: undefined as string | undefined })
const typeMap: Record<string, string> = { article: '文章', link: '链接', picture: '图片' }
const columns = [
  { title: '栏目名称', dataIndex: 'categoryName' },
  { title: '类型', key: 'type', width: 70 },
  { title: '排序', dataIndex: 'treeSort', width: 60 },
  { title: '显示', key: 'isShow', width: 60 },
  { title: '状态', key: 'status', width: 60 },
  { title: '操作', key: 'action', width: 200 }
]

async function loadData() {
  loading.value = true; const res = await categoryApi.tree()
  if (res.data) treeData.value = res.data; loading.value = false
}
function showAdd() { isEdit.value = false; form.categoryCode = undefined; form.categoryName = ''; form.categoryType = 'article'; form.treeSort = 1000; form.parentCode = '0'; form.link = ''; form.keywords = ''; form.description = ''; form.isShowBool = '1'; modalOpen.value = true }
function showEdit(r: CategoryDto) { isEdit.value = true; form.categoryCode = r.categoryCode; form.categoryName = r.categoryName; form.categoryType = r.categoryType || 'article'; form.treeSort = r.treeSort; form.parentCode = r.parentCode; form.link = r.link || ''; form.keywords = r.keywords || ''; form.description = r.description || ''; form.isShowBool = r.isShow || '1'; modalOpen.value = true }
function showAddChild(parent: CategoryDto) { isEdit.value = false; form.categoryCode = undefined; form.categoryName = ''; form.categoryType = 'article'; form.treeSort = 1000; form.parentCode = parent.categoryCode; form.link = ''; form.keywords = ''; form.description = ''; form.isShowBool = '1'; modalOpen.value = true }
async function handleSave() {
  saving.value = true
  await categoryApi.save({ categoryCode: form.categoryCode, categoryName: form.categoryName, categoryType: form.categoryType, parentCode: form.parentCode, treeSort: form.treeSort, link: form.link, keywords: form.keywords, description: form.description, isShow: form.isShowBool as string })
  message.success('保存成功'); modalOpen.value = false; saving.value = false; loadData()
}
async function handleDelete(code: string) { await categoryApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
