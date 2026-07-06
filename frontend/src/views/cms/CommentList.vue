<template>
  <a-card title="评论管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="文章标题"><a-input v-model:value="query.articleTitle" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="commentCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="openAudit(record, 'pass')">审核通过</a>
            <a @click="openAudit(record, 'refuse')">审核拒绝</a>
            <a @click="handleDelete(record.commentCode)">删除</a>
          </a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="审核评论" @ok="handleAudit" :confirmLoading="saving">
      <a-form :model="auditForm" layout="vertical">
        <a-form-item label="审核备注"><a-textarea v-model:value="auditForm.auditComment" rows=4 /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { cmsCommentApi } from '@/api'
import { message } from 'ant-design-vue'
import type { CmsCommentDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const data = reactive({ list: [] as CmsCommentDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ articleTitle: '' })
const auditForm = reactive({ commentCode: '', status: 'pass', auditComment: '' })
const columns = [
  { title: '文章标题', dataIndex: 'articleTitle' }, { title: '内容', dataIndex: 'content' },
  { title: '评论人', dataIndex: 'name' }, { title: '状态', dataIndex: 'status' },
  { title: '时间', dataIndex: 'createDate' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await cmsCommentApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { articleTitle: query.articleTitle } as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function openAudit(row: CmsCommentDto, status: string) { auditForm.commentCode = row.commentCode; auditForm.status = status; auditForm.auditComment = ''; modalOpen.value = true }
async function handleAudit() { saving.value = true; await cmsCommentApi.audit(auditForm.commentCode, auditForm.status, auditForm.auditComment); message.success('审核完成'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await cmsCommentApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
