<template>
  <a-card title="CMS 文件模板管理">
    <template #extra>
      <a-button type="primary" @click="showAdd">新建模板</a-button>
    </template>
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="name">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="showEdit(record)">编辑</a>
            <a-popconfirm title="确定删除?" @confirm="handleDelete(record.name)">
              <a style="color:red">删除</a>
            </a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" :title="editName?'编辑模板':'新建模板'" @ok="handleSave" :confirmLoading="saving" width=800>
      <a-form layout="vertical">
        <a-form-item label="模板名称">
          <a-input v-model:value="form.name" :disabled="!!editName" placeholder="不含扩展名" />
        </a-form-item>
        <a-form-item label="模板内容 (HTML)">
          <a-textarea v-model:value="form.content" rows=20 style="font-family:monospace" />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import { fileTemplateApi } from '@/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const editName = ref<string|null>(null)
const list = ref<any[]>([])
const form = reactive({ name: '', content: '' })
const columns = [
  { title: '模板名称', dataIndex: 'name' }, { title: '文件名', dataIndex: 'fileName' },
  { title: '大小', dataIndex: 'size', customRender: ({ text }: any) => text + ' bytes' },
  { title: '更新时间', dataIndex: 'updateDate' }, { title: '操作', key: 'action' }
]

async function loadData() {
  loading.value = true
  try {
    const res = await fileTemplateApi.list()
    if (res.data) list.value = res.data
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function showAdd() { editName.value = null; form.name = ''; form.content = ''; modalOpen.value = true }
async function showEdit(record: any) {
  editName.value = record.name; form.name = record.name
  const res = await fileTemplateApi.get(record.name)
  if (res.data) form.content = res.data.content; modalOpen.value = true
}
async function handleSave() {
  saving.value = true; await fileTemplateApi.save(form.name, form.content)
  message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false
}
async function handleDelete(name: string) { await fileTemplateApi.delete(name); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
