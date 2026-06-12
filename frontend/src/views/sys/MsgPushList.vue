<template>
  <a-card title="推送记录">
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="id"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='pushStatus'">
          <a-tag :color="record.pushStatus==='success'?'green':'orange'">{{ pushStatusLabel[record.pushStatus ?? ''] || record.pushStatus }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-popconfirm v-if="record.pushStatus!=='success'" title="确定重试?" @confirm="retryPush(record)">
            <a>重试</a>
          </a-popconfirm>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="sendOpen" title="发送推送" @ok="handleSend" :confirmLoading="sending">
      <a-form layout="vertical">
        <a-form-item label="消息类型"><a-input v-model:value="form.msgType" placeholder="pc/email/sms/wechat" /></a-form-item>
        <a-form-item label="标题"><a-input v-model:value="form.msgTitle" /></a-form-item>
        <a-form-item label="内容"><a-textarea v-model:value="form.msgContent" :rows="3" /></a-form-item>
        <a-form-item label="接收用户编码"><a-input v-model:value="form.receiveUserCode" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import { post } from '@/api/request'
import type { PageResult } from '@/types/api'

interface PushRecord {
  id: string
  msgTitle?: string
  msgType?: string
  receiveUserName?: string
  sendUserName?: string
  pushStatus?: 'pending' | 'success' | 'fail' | 'retrying'
  sendDate?: string
}

const loading = ref(false); const sending = ref(false); const sendOpen = ref(false)
const data = reactive({ list: [] as PushRecord[], total: 0, pageNo: 1, pageSize: 20 })
const form = reactive({ msgType: 'pc', msgTitle: '', msgContent: '', receiveUserCode: '' })
const columns: Array<{ title: string; dataIndex?: string; key?: string; width?: number }> = [
  { title: '标题', dataIndex: 'msgTitle', width: 200 },
  { title: '类型', dataIndex: 'msgType', width: 80 },
  { title: '接收人', dataIndex: 'receiveUserName', width: 120 },
  { title: '发送人', dataIndex: 'sendUserName', width: 120 },
  { title: '状态', key: 'pushStatus', width: 80 },
  { title: '发送时间', dataIndex: 'sendDate', width: 160 },
  { title: '操作', key: 'action', width: 80 }
]

const pushStatusLabel: Record<string, string> = { pending: '待推送', success: '已成功', fail: '失败', retrying: '重试中' }

async function loadData() {
  loading.value = true
  try {
    const res = await post<PageResult<PushRecord>>('/sys/msg/push/list', { pageNo: data.pageNo, pageSize: data.pageSize })
    if (res.code === 200 && res.data) { data.list = res.data.list; data.total = res.data.total }
  } finally { loading.value = false }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }

async function handleSend() {
  sending.value = true
  try {
    const res = await post<any>('/sys/msg/push/send', { ...form })
    if (res.code === 200) { message.success('发送成功'); sendOpen.value = false; loadData() }
    else message.error(res.message)
  } finally { sending.value = false }
}

async function retryPush(record: PushRecord) {
  const res = await post<any>('/sys/msg/push/retry', { id: record.id })
  if (res.code === 200) { message.success('已重新推送'); loadData() }
}

onMounted(loadData)
</script>
