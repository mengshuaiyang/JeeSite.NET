<template>
  <a-card title="收件箱">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="标题"><a-input v-model:value="query.msgTitle" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="msgId"
      :customRow="rowClickHandler"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='status'">
          <a-tag :color="record.status==='0'?'blue':'default'">{{ record.status==='0'?'未读':'已读' }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-space><a @click.stop="handleDelete(record.msgId)">删除</a></a-space>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { msgApi } from '@/api'
import { message } from 'ant-design-vue'
import type { MsgInnerDto } from '@/types/api'

const loading = ref(false)
const data = reactive({ list: [] as MsgInnerDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ msgTitle: '' })
const columns = [
  { title: '标题', dataIndex: 'msgTitle' }, { title: '发件人', dataIndex: 'senderName' },
  { title: '时间', dataIndex: 'createDate' }, { title: '状态', key: 'status' },
  { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await msgApi.inbox({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { msgTitle: query.msgTitle } as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function rowClickHandler(record: MsgInnerDto) {
  return { onClick: async () => { if (record.status === '0') { await msgApi.markRead(record.msgId); message.success('已标记为已读'); loadData() } } }
}
async function handleDelete(msgId: string) { await msgApi.delete(msgId); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
