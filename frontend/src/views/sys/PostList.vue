<template>
  <a-card title="岗位管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="postCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.postCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存岗位" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="名称"><a-input v-model:value="form.postName" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { postApi } from '@/api'
import { message } from 'ant-design-vue'
import type { PostDto } from '@/types/api'
const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<PostDto|null>(null)
const data = reactive({ list: [] as PostDto[], total: 0, pageNo: 1, pageSize: 20 })
const form = reactive({ postName: '' })
const columns = [{ title: '编码', dataIndex: 'postCode' }, { title: '名称', dataIndex: 'postName' }, { title: '排序', dataIndex: 'sort' }, { title: '操作', key: 'action' }]
async function loadData() {
  loading.value = true
  try {
    const res = await postApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(p: number, s: number) { data.pageNo = p; data.pageSize = s; loadData() }
function showAdd() { editItem.value = null; form.postName=''; modalOpen.value = true }
function showEdit(r: PostDto) { editItem.value = r; form.postName=r.postName; modalOpen.value = true }
async function handleSave() { saving.value = true; await postApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(c: string) { await postApi.delete(c); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
