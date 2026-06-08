<template>
  <a-card title="在线用户">
    <a-button type="primary" style="margin-bottom:16px" @click="loadList">刷新</a-button>
    <a-table :dataSource="list" :columns="columns" :loading="loading" rowKey="userCode">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-popconfirm title="确定要强制下线该用户吗？" @confirm="handleKick(record.userCode)">
            <a-button type="link" danger>强制下线</a-button>
          </a-popconfirm>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { onlineApi } from '@/api'
import { message } from 'ant-design-vue'

const loading = ref(false)
const list = ref<any[]>([])
const columns = [
  { title: '用户编码', dataIndex: 'userCode' },
  { title: '用户名', dataIndex: 'userName' },
  { title: '登录账号', dataIndex: 'loginCode' },
  { title: 'IP 地址', dataIndex: 'ipAddress' },
  { title: '登录时间', dataIndex: 'loginTime' },
  { title: '最后活跃', dataIndex: 'lastActivity' },
  { title: '操作', key: 'action' }
]

async function loadList() {
  loading.value = true
  const r = await onlineApi.list()
  if (r.data) list.value = r.data
  loading.value = false
}

async function handleKick(userCode: string) {
  await onlineApi.kick(userCode)
  message.success('已强制下线')
  loadList()
}

onMounted(loadList)
</script>
