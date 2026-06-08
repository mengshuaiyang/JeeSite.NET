<template>
  <a-card title="我的请假申请">
    <template #extra><a-button type="primary" @click="showSubmit">新建申请</a-button></template>
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="leaveRequestId">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='leaveType'">{{ typeMap[record.leaveType] || record.leaveType }}</template>
        <template v-else-if="column.key==='status'">
          <a-tag :color="statusColor(record.status)">{{ statusMap[record.status] }}</a-tag>
        </template>
        <template v-else-if="column.key==='action'">
          <a @click="showDetail(record)">详情</a>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="submitOpen" title="新建请假申请" @ok="handleSubmit" :confirmLoading="submitting" width=600>
      <a-form :model="form" layout="vertical">
        <a-form-item label="请假类型">
          <a-select v-model:value="form.leaveType">
            <a-select-option value="annual">年假</a-select-option>
            <a-select-option value="sick">病假</a-select-option>
            <a-select-option value="personal">事假</a-select-option>
            <a-select-option value="marriage">婚假</a-select-option>
          </a-select>
        </a-form-item>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="开始日期"><a-date-picker v-model:value="form.startDate" style="width:100%" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="结束日期"><a-date-picker v-model:value="form.endDate" style="width:100%" /></a-form-item></a-col>
        </a-row>
        <a-form-item label="请假原因"><a-textarea v-model:value="form.reason" rows=3 /></a-form-item>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="经理审批人"><a-input v-model:value="form.managerApprover" placeholder="用户编码" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="HR确认人"><a-input v-model:value="form.hrApprover" placeholder="用户编码" /></a-form-item></a-col>
        </a-row>
      </a-form>
    </a-modal>
    <a-modal v-model:open="detailOpen" title="请假详情" width=600 :footer="null">
      <a-descriptions :column="2" bordered size="small">
        <a-descriptions-item label="类型">{{ typeMap[detail.leaveType] }}</a-descriptions-item>
        <a-descriptions-item label="状态"><a-tag :color="statusColor(detail.status)">{{ statusMap[detail.status] }}</a-tag></a-descriptions-item>
        <a-descriptions-item label="开始日期">{{ detail.startDate?.slice(0,10) }}</a-descriptions-item>
        <a-descriptions-item label="结束日期">{{ detail.endDate?.slice(0,10) }}</a-descriptions-item>
        <a-descriptions-item label="天数">{{ detail.durationDays }}</a-descriptions-item>
        <a-descriptions-item label="提交时间">{{ detail.submitDate?.slice(0,16) }}</a-descriptions-item>
        <a-descriptions-item label="原因" :span="2">{{ detail.reason }}</a-descriptions-item>
      </a-descriptions>
      <a-divider>审批记录</a-divider>
      <a-timeline>
        <a-timeline-item v-for="h in detail.history" :key="h.recordId" :color="h.result==='approved'?'green':h.result==='rejected'?'red':'orange'">
          <template #dot v-if="h.result==='approved'">✓</template>
          <p><strong>{{ h.activityName }}</strong> — {{ h.result==='approved'?'已通过':h.result==='rejected'?'已驳回':'待审批' }}</p>
          <p v-if="h.comment">备注: {{ h.comment }}</p>
          <p style="font-size:12px;color:#999">{{ h.completedDate?.slice(0,16) }}</p>
        </a-timeline-item>
      </a-timeline>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import { leaveApi } from '@/api'

const currentUser = localStorage.getItem('loginCode') || 'admin'
const loading = ref(false); const submitting = ref(false); const submitOpen = ref(false); const detailOpen = ref(false)
const list = ref<any[]>([]); const detail = ref<any>({ history: [] })
const form = reactive({ leaveType: 'annual', startDate: '', endDate: '', reason: '', managerApprover: 'admin', hrApprover: 'admin' })
const typeMap: Record<string,string> = { annual: '年假', sick: '病假', personal: '事假', marriage: '婚假' }
const statusMap: Record<string,string> = { draft: '草稿', pending: '审批中', hr_pending: 'HR确认中', approved: '已通过', rejected: '已驳回' }
const statusColor = (s: string) => ({ draft: 'default', pending: 'orange', hr_pending: 'blue', approved: 'green', rejected: 'red' }[s] || 'default')
const columns = [
  { title: '类型', dataIndex: 'leaveType', key: 'leaveType' },
  { title: '天数', dataIndex: 'durationDays' },
  { title: '原因', dataIndex: 'reason' },
  { title: '状态', dataIndex: 'status', key: 'status' },
  { title: '提交时间', dataIndex: 'submitDate', customRender: ({ text }: any) => text?.slice(0,16) || '-' },
  { title: '操作', key: 'action' }
]
async function loadData() { loading.value = true; const res = await leaveApi.myLeaves(currentUser); if (res.data) list.value = res.data; loading.value = false }
function showSubmit() { form.leaveType = 'annual'; form.startDate = ''; form.endDate = ''; form.reason = ''; form.managerApprover = 'admin'; form.hrApprover = 'admin'; submitOpen.value = true }
async function handleSubmit() {
  submitting.value = true; await leaveApi.submit({ applicant: currentUser, ...form, managerName: '管理员', hrName: '管理员' })
  message.success('提交成功'); submitOpen.value = false; loadData(); submitting.value = false
}
async function showDetail(record: any) { const res = await leaveApi.detail(record.leaveRequestId); if (res.data) detail.value = res.data; detailOpen.value = true }
onMounted(loadData)
</script>
