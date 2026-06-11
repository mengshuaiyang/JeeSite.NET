<template>
  <a-card title="审计跟踪">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="审计类型"><a-input v-model:value="query.auditType" placeholder="login/logout/change" /></a-form-item>
      <a-form-item label="登录名"><a-input v-model:value="query.loginCode" placeholder="筛选用户" /></a-form-item>
      <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="auditId"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }" />
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { auditApi } from '@/api/audit'

const loading = ref(false)
const data = reactive({ list: [] as any[], total: 0, pageNo: 1, pageSize: 20 })
const query = reactive({ auditType: '', loginCode: '' })

const columns = [
  { title: '审计类型', dataIndex: 'auditType', width: 120 },
  { title: '登录名', dataIndex: 'loginCode', width: 120 },
  { title: '用户名', dataIndex: 'userName', width: 120 },
  { title: '机构', dataIndex: 'officeName', width: 150 },
  { title: '菜单', dataIndex: 'menuCode', width: 150 },
  { title: '审计结果', dataIndex: 'auditResult', width: 100 },
  { title: '时间', dataIndex: 'createDate', width: 180 },
]

async function loadData() {
  loading.value = true
  const r = await auditApi.list({
    pageNo: data.pageNo, pageSize: data.pageSize,
    entity: { auditType: query.auditType || undefined, loginCode: query.loginCode || undefined }
  })
  if (r.data) { data.list = r.data.list; data.total = r.data.total }
  loading.value = false
}

function onPageChange(p: number, s: number) { data.pageNo = p; data.pageSize = s; loadData() }
onMounted(loadData)
</script>
