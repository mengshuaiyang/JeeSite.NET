<template>
  <a-card title="缓存管理">
    <a-space style="margin-bottom:16px">
      <a-button type="primary" @click="loadKeys">刷新</a-button>
      <a-button danger @click="handleClearAll">清除全部缓存</a-button>
    </a-space>
    <a-table :dataSource="keys" :columns="columns" :loading="loading" rowKey="key" />
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { cacheApi } from '@/api'
import { message, Modal } from 'ant-design-vue'

const loading = ref(false)
const keys = ref<string[]>([])
const columns = [
  { title: '缓存键', dataIndex: 'key' },
  { title: '操作', key: 'action' }
]

async function loadKeys() {
  loading.value = true
  const r = await cacheApi.keys()
  if (r.data) keys.value = r.data
  loading.value = false
}

async function handleClearAll() {
  Modal.confirm({
    title: '确认操作',
    content: '确定要清除所有缓存吗？',
    onOk: async () => {
      await cacheApi.clearAll()
      message.success('缓存已清除')
      loadKeys()
    }
  })
}

onMounted(loadKeys)
</script>
