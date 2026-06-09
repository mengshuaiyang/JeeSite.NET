<template>
  <div>
    <a-row :gutter="[16, 16]">
      <a-col :span="6" v-for="card in statCards" :key="card.key">
        <a-card hoverable :loading="loading" :body-style="{ textAlign: 'center' }">
          <template #title>
            <span>{{ card.title }}</span>
          </template>
          <div :style="{ fontSize: '28px', fontWeight: 600, color: card.color }">{{ card.value }}</div>
        </a-card>
      </a-col>
    </a-row>

    <a-card title="最近登录" style="margin-top: 16px">
      <a-table :dataSource="stats?.recentLogins" :columns="loginColumns" row-key="loginCode" :pagination="{ pageSize: 5 }" :loading="loading" size="small" />
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { dashboardApi } from '@/api'
import type { DashboardStats } from '@/api/dashboard'

const loading = ref(false)
const stats = ref<DashboardStats>()
const todayLogins = computed(() => stats.value?.logCountToday ?? 0)

const statCards = computed(() => [
  { key: 'users', title: '用户数', color: '#1890ff', value: stats.value?.userCount ?? '-' },
  { key: 'roles', title: '角色数', color: '#52c41a', value: stats.value?.roleCount ?? '-' },
  { key: 'menus', title: '菜单数', color: '#722ed1', value: stats.value?.menuCount ?? '-' },
  { key: 'orgs', title: '机构数', color: '#fa8c16', value: stats.value?.orgCount ?? '-' },
  { key: 'posts', title: '岗位数', color: '#13c2c2', value: stats.value?.postCount ?? '-' },
  { key: 'dicts', title: '字典数', color: '#eb2f96', value: stats.value?.dictCount ?? '-' },
  { key: 'todayLogins', title: '今日登录', color: '#faad14', value: todayLogins.value },
  { key: 'monitor', title: '服务器', color: '#a0d911', value: '在线' }
])

const loginColumns = [
  { title: '用户名', dataIndex: 'userName', key: 'userName' },
  { title: '登录账号', dataIndex: 'loginCode', key: 'loginCode' },
  { title: '登录时间', dataIndex: 'loginDate', key: 'loginDate' },
  { title: 'IP 地址', dataIndex: 'ipAddress', key: 'ipAddress' }
]

async function load() {
  loading.value = true
  try {
    const res = await dashboardApi.getStats()
    if (res.data) stats.value = res.data
  } finally {
    loading.value = false
  }
}

onMounted(load)
</script>
