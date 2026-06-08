<template>
  <div>
    <a-page-header title="系统监控" sub-title="服务器运行状态" />

    <a-row :gutter="16">
      <a-col :span="6">
        <a-card title="CPU 使用率" size="small">
          <div style="text-align:center;padding:16px">
            <a-progress type="dashboard" :percent="info.cpuUsagePercent" :stroke-color="cpuColor" />
            <p style="margin-top:8px;color:#999">核心数: {{ info.processorCount }}</p>
          </div>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card title="进程内存" size="small">
          <div style="text-align:center;padding:16px">
            <a-progress type="dashboard" :percent="memPercent" :stroke-color="memColor" format=""/>
            <p style="margin-top:8px;color:#999">工作集: {{ formatBytes(info.processMemoryWorkingSet) }}</p>
            <p style="color:#999">GC 内存: {{ formatBytes(info.gcTotalMemory) }}</p>
          </div>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card title="运行时间" size="small">
          <div style="text-align:center;padding:16px;font-size:24px;font-weight:bold;color:#1890ff">
            {{ info.uptimeDays }} 天
            <p style="font-size:13px;font-weight:normal;color:#999;margin-top:8px">
              启动时间: {{ dayjs(info.startTime).format('YYYY-MM-DD HH:mm:ss') }}
            </p>
          </div>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card title="线程/句柄" size="small">
          <div style="text-align:center;padding:16px">
            <a-statistic title="线程数" :value="info.threadCount" />
            <a-statistic title="句柄数" :value="info.handleCount" style="margin-top:12px" />
          </div>
        </a-card>
      </a-col>
    </a-row>

    <a-card title="服务器信息" size="small" style="margin-top:16px">
      <a-descriptions :column="2" bordered size="small">
        <a-descriptions-item label="操作系统">{{ info.osName }}</a-descriptions-item>
        <a-descriptions-item label="系统版本">{{ info.osVersion }}</a-descriptions-item>
        <a-descriptions-item label="系统架构">{{ info.osArchitecture }}</a-descriptions-item>
        <a-descriptions-item label="进程架构">{{ info.processArchitecture }}</a-descriptions-item>
        <a-descriptions-item label="主机名">{{ info.machineName }}</a-descriptions-item>
        <a-descriptions-item label="运行平台">{{ info.runtimeVersion }}</a-descriptions-item>
      </a-descriptions>
    </a-card>

    <a-card title="磁盘使用情况" size="small" style="margin-top:16px">
      <a-table :data-source="info.disks" :columns="diskColumns" row-key="name" :pagination="false" size="small">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'usage'">
            <a-progress :percent="record.usagePercent" :stroke-color="record.usagePercent > 85 ? '#ff4d4f' : '#52c41a'" />
          </template>
          <template v-if="column.key === 'total'">
            {{ formatBytes(record.totalSize) }}
          </template>
          <template v-if="column.key === 'used'">
            {{ formatBytes(record.usedSpace) }}
          </template>
          <template v-if="column.key === 'free'">
            {{ formatBytes(record.availableFreeSpace) }}
          </template>
        </template>
      </a-table>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { monitorApi } from '@/api/monitor'
import type { ServerInfo } from '@/types/api'
import dayjs from 'dayjs'

const info = ref<ServerInfo>({
  osName: '', osVersion: '', osArchitecture: '', processArchitecture: '',
  machineName: '', runtimeVersion: '', processorCount: 0, startTime: '',
  uptimeDays: 0, processMemoryWorkingSet: 0, processMemoryPrivateBytes: 0,
  processMemoryVirtualBytes: 0, gcTotalMemory: 0, threadCount: 0, handleCount: 0,
  cpuUsagePercent: 0, disks: []
})

const diskColumns = [
  { title: '盘符', dataIndex: 'name', key: 'name', width: 100 },
  { title: '类型', dataIndex: 'driveType', key: 'driveType', width: 100 },
  { title: '文件系统', dataIndex: 'driveFormat', key: 'driveFormat', width: 100 },
  { title: '总容量', key: 'total', width: 120 },
  { title: '已用', key: 'used', width: 120 },
  { title: '可用', key: 'free', width: 120 },
  { title: '使用率', key: 'usage', width: 200 },
]

const cpuColor = computed(() => {
  const cpu = info.value.cpuUsagePercent
  if (cpu > 80) return '#ff4d4f'
  if (cpu > 50) return '#faad14'
  return '#52c41a'
})

const memPercent = computed(() => {
  const max = 500 * 1024 * 1024
  return Math.min(Math.round(info.value.processMemoryWorkingSet / max * 100), 100)
})

const memColor = computed(() => {
  const p = memPercent.value
  if (p > 80) return '#ff4d4f'
  if (p > 50) return '#faad14'
  return '#52c41a'
})

function formatBytes(bytes: number): string {
  if (!bytes) return '0 B'
  const units = ['B', 'KB', 'MB', 'GB', 'TB']
  const i = Math.floor(Math.log(bytes) / Math.log(1024))
  return (bytes / Math.pow(1024, i)).toFixed(2) + ' ' + units[i]
}

onMounted(async () => {
  const res = await monitorApi.getServerInfo()
  if (res.code === 200 && res.data) info.value = res.data
})
</script>
