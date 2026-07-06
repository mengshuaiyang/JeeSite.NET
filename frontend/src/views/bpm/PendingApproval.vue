<template>
  <a-card title="待审批">
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="leaveRequestId">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='leaveType'">{{ typeMap[record.leaveType] || record.leaveType }}</template>
        <template v-else-if="column.key==='status'">
          <a-tag :color="record.status==='pending'?'orange':'blue'">{{ record.status==='pending'?'经理审批':'HR确认' }}</a-tag>
        </template>
        <template v-else-if="column.key==='action'">
          <a-button size="small" type="primary" @click="handleApprove(record, 'approved')">通过</a-button>
          <a-button size="small" danger style="margin-left:8px" @click="handleReject(record)">驳回</a-button>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import { leaveApi } from '@/api'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
const currentUser = userStore.user?.loginCode || ''
const loading = ref(false); const list = ref<any[]>([])

const typeMap: Record<string,string> = { annual: '年假', sick: '病假', personal: '事假', marriage: '婚假' }
const columns = [
  { title: '申请人', dataIndex: 'applicant' },
  { title: '类型', dataIndex: 'leaveType', key: 'leaveType' },
  { title: '天数', dataIndex: 'durationDays' },
  { title: '原因', dataIndex: 'reason' },
  { title: '当前步骤', dataIndex: 'status', key: 'status' },
  { title: '提交时间', dataIndex: 'submitDate', customRender: ({ text }: any) => text?.slice(0,16) || '-' },
  { title: '操作', key: 'action' }
]

async function loadData() {
  loading.value = true
  try {
    const res = await leaveApi.pending(currentUser)
    if (res.data) list.value = res.data
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
async function handleApprove(record: any, result: string) {
  const step = record.status === 'pending' ? 'manager' : 'hr'
  await leaveApi.approve({ leaveRequestId: record.leaveRequestId, step, result, operator: currentUser, nextAssigneeName: '管理员' })
  message.success('操作成功'); loadData()
}
function handleReject(record: any) {
  Modal.confirm({ title: '确定驳回?', onOk: async () => {
    const step = record.status === 'pending' ? 'manager' : 'hr'
    await leaveApi.approve({ leaveRequestId: record.leaveRequestId, step, result: 'rejected', operator: currentUser })
    message.success('已驳回'); loadData()
  }})
}
onMounted(loadData)
</script>
