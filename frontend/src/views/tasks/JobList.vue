<template>
  <a-card title="任务调度">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增任务</a-button>
    <a-table :dataSource="jobs" :columns="columns" rowKey="jobId" :loading="loading"
      :pagination="{ current: pageNo, pageSize, total, onChange: (p: number) => { pageNo = p; load() } }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='runStatus'">
          <a-tag :color="record.runStatus==='running'?'green':'default'">{{ record.runStatus==='running'?'运行中':'停止' }}</a-tag>
        </template>
        <template v-if="column.key==='status'">
          <a-tag :color="record.status==='0'?'green':'red'">{{ record.status==='0'?'正常':'停用' }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="showEdit(record)">编辑</a>
            <a-popconfirm v-if="record.runStatus!=='running'" title="启动?" @confirm="startJob(record)"><a style="color:green">启动</a></a-popconfirm>
            <a-popconfirm v-else title="停止?" @confirm="stopJob(record)"><a style="color:orange">停止</a></a-popconfirm>
            <a @click="runOnce(record)">执行一次</a>
            <a @click="showLogs(record)">日志</a>
            <a-popconfirm title="确定删除?" @confirm="deleteJob(record)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="modalOpen" :title="isEdit?'编辑任务':'新增任务'" @ok="handleSave" :confirmLoading="saving">
      <a-form layout="vertical">
        <a-form-item label="任务名称" required><a-input v-model:value="form.jobName" /></a-form-item>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="任务分组"><a-input v-model:value="form.jobGroup" placeholder="DEFAULT" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="Cron 表达式" required><a-input v-model:value="form.cronExpression" placeholder="0/5 * * * * ?" /></a-form-item></a-col>
        </a-row>
        <a-form-item label="程序集"><a-input v-model:value="form.assemblyName" placeholder="JeeSiteNET.Modules.Tasks" /></a-form-item>
        <a-form-item label="类名"><a-input v-model:value="form.className" placeholder="JeeSiteNET.Modules.Tasks.Jobs.SampleJob" /></a-form-item>
        <a-form-item label="描述"><a-textarea v-model:value="form.description" :rows="2" /></a-form-item>
      </a-form>
    </a-modal>

    <a-drawer v-model:open="logDrawer" title="执行日志" placement="right" width="640px">
      <a-table :dataSource="logs" :columns="logColumns" rowKey="logId" :pagination="false" size="small">
        <template #bodyCell="{ record, column }">
          <template v-if="column.key==='result'">
            <a-tag :color="record.result==='success'?'green':'red'">{{ record.result }}</a-tag>
          </template>
        </template>
      </a-table>
    </a-drawer>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { jobApi } from '@/api'
import { message } from 'ant-design-vue'
import type { SysJobDto, JobLogDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const isEdit = ref(false)
const jobs = ref<SysJobDto[]>([])
const pageNo = ref(1); const pageSize = ref(10); const total = ref(0)
const form = reactive({ jobName: '', jobGroup: 'DEFAULT', cronExpression: '', assemblyName: '', className: '', description: '', jobId: undefined as string | undefined })
const logDrawer = ref(false); const logs = ref<JobLogDto[]>([])

const columns = [
  { title: '任务名称', dataIndex: 'jobName' },
  { title: '分组', dataIndex: 'jobGroup', width: 100 },
  { title: 'Cron', dataIndex: 'cronExpression', width: 140 },
  { title: '运行状态', key: 'runStatus', width: 80 },
  { title: '状态', key: 'status', width: 60 },
  { title: '描述', dataIndex: 'description', ellipsis: true },
  { title: '操作', key: 'action', width: 280 }
]
const logColumns = [
  { title: '运行时间', dataIndex: 'runDate', width: 160 },
  { title: '结果', key: 'result', width: 80 },
  { title: '耗时(ms)', dataIndex: 'duration', width: 80 },
  { title: '错误信息', dataIndex: 'errorMessage', ellipsis: true }
]

async function load() {
  loading.value = true; const res = await jobApi.list({ pageNo: pageNo.value, pageSize: pageSize.value })
  if (res.data) { jobs.value = res.data.list; total.value = res.data.total }
  loading.value = false
}
function showAdd() { isEdit.value = false; form.jobId = undefined; form.jobName = ''; form.jobGroup = 'DEFAULT'; form.cronExpression = ''; form.assemblyName = ''; form.className = ''; form.description = ''; modalOpen.value = true }
function showEdit(r: SysJobDto) { isEdit.value = true; Object.assign(form, r); modalOpen.value = true }
async function handleSave() {
  saving.value = true; await jobApi.save({ ...form }); message.success('保存成功'); modalOpen.value = false; saving.value = false; load()
}
async function deleteJob(r: SysJobDto) { await jobApi.delete(r.jobId); message.success('已删除'); load() }
async function startJob(r: SysJobDto) { await jobApi.start(r.jobId); message.success('已启动'); load() }
async function stopJob(r: SysJobDto) { await jobApi.stop(r.jobId); message.success('已停止'); load() }
async function runOnce(r: SysJobDto) { await jobApi.runOnce(r.jobId); message.success('已触发'); load() }
async function showLogs(r: SysJobDto) {
  const res = await jobApi.logs(r.jobId)
  if (res.data) logs.value = res.data; logDrawer.value = true
}
onMounted(load)
</script>
