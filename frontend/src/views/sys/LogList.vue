<template>
  <a-card title="操作日志">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="类型"><a-input v-model:value="query.logType" placeholder="access/error" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="logId"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }" />
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { logApi } from '@/api'
import type { LogDto } from '@/types/api'
const loading = ref(false)
const data = reactive({ list: [] as LogDto[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ logType: '' })
const columns = [{ title: '标题', dataIndex: 'logTitle' }, { title: '类型', dataIndex: 'logType' }, { title: '方法', dataIndex: 'requestMethod' }, { title: '耗时(ms)', dataIndex: 'executeTime' }, { title: '用户', dataIndex: 'userName' }, { title: 'IP', dataIndex: 'remoteIp' }, { title: '时间', dataIndex: 'createDate' }]
async function loadData() { loading.value = true; const r = await logApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { logType: query.logType } as any }); if (r.data) { data.list = r.data.list; data.total = r.data.total } loading.value = false }
function onPageChange(p: number, s: number) { data.pageNo = p; data.pageSize = s; loadData() }
onMounted(loadData)
</script>
