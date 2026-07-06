<template>
  <a-card title="APP评论管理">
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="id">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="openReply(record)">回复</a><a @click="handleDelete(record.id)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="回复评论" @ok="handleReply" :confirmLoading="saving">
      <a-form :model="replyForm" layout="vertical">
        <a-form-item label="回复内容"><a-textarea v-model:value="replyForm.replyContent" rows=4 /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { appFeedbackApi } from '@/api'
import { message } from 'ant-design-vue'
import type { AppCommentDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const list = ref<AppCommentDto[]>([])
const replyForm = reactive({ id: '', replyContent: '' })
const columns = [
  { title: '分类', dataIndex: 'category' }, { title: '内容', dataIndex: 'content' },
  { title: '联系方式', dataIndex: 'contact' }, { title: '回复', dataIndex: 'replyContent' },
  { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await appFeedbackApi.commentList()
    if (res.data) list.value = res.data
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function openReply(row: AppCommentDto) { replyForm.id = row.id; replyForm.replyContent = row.replyContent || ''; modalOpen.value = true }
async function handleReply() { saving.value = true; await appFeedbackApi.commentReply(replyForm.id, replyForm.replyContent); message.success('回复成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(id: string) { await appFeedbackApi.commentDelete(id); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
