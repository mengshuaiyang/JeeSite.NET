<template>
  <a-card title="留言管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="姓名"><a-input v-model:value="query.name" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="gbCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="openReply(record)">回复</a><a @click="handleDelete(record.gbCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="回复留言" @ok="handleReply" :confirmLoading="saving">
      <a-form :model="replyForm" layout="vertical">
        <a-form-item label="回复内容"><a-textarea v-model:value="replyForm.reContent" rows=4 /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { guestbookApi } from '@/api'
import { message } from 'ant-design-vue'
import type { GuestbookDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const data = reactive({ list: [] as GuestbookDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ name: '' })
const replyForm = reactive({ gbCode: '', reContent: '' })
const columns = [
  { title: '类型', dataIndex: 'gbType' }, { title: '内容', dataIndex: 'content' },
  { title: '姓名', dataIndex: 'name' }, { title: '邮箱', dataIndex: 'email' },
  { title: '状态', dataIndex: 'status' }, { title: '时间', dataIndex: 'createDate' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  const res = await guestbookApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { name: query.name } as any })
  if (res.data) { data.list = res.data.list; data.total = res.data.total }
  loading.value = false
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function openReply(row: GuestbookDto) { replyForm.gbCode = row.gbCode; replyForm.reContent = row.reContent || ''; modalOpen.value = true }
async function handleReply() { saving.value = true; await guestbookApi.reply(replyForm.gbCode, replyForm.reContent); message.success('回复成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await guestbookApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
