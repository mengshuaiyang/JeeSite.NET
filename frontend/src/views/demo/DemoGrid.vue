<template>
  <a-card title="表格演示 (Ant Design Vue)">
    <template #extra>
      <a-space>
        <a-input-search v-model:value="searchKeyword" placeholder="搜索名称..." style="width:200px" @search="loadData" />
        <a-button type="primary" @click="loadData">刷新</a-button>
      </a-space>
    </template>
    <a-table
      :dataSource="data"
      :columns="columns"
      :loading="loading"
      :pagination="pagination"
      rowKey="id"
      @change="handleTableChange"
    >
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='status'">
          <a-tag :color="record.status==='已审核'?'green':record.status==='待审核'?'orange':'default'">{{ record.status }}</a-tag>
        </template>
        <template v-else-if="column.key==='amount'">¥{{ record.amount.toFixed(2) }}</template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { message } from 'ant-design-vue'

const loading = ref(false)
const data = ref<any[]>([])
const searchKeyword = ref('')
const pagination = reactive({ current: 1, pageSize: 20, total: 0, showSizeChanger: true, showTotal: (t: number) => `共 ${t} 条` })
const columns = [
  { title: 'ID', dataIndex: 'id', width: 80 },
  { title: '名称', dataIndex: 'name' },
  { title: '分类', dataIndex: 'category' },
  { title: '状态', dataIndex: 'status', key: 'status' },
  { title: '金额', dataIndex: 'amount', key: 'amount' },
  { title: '创建日期', dataIndex: 'createDate' },
  { title: '创建人', dataIndex: 'creator' }
]

async function loadData() {
  loading.value = true
  // Simulated API return; in production, call testDemoApi.gridData()
  data.value = Array.from({ length: pagination.pageSize }, (_, i) => {
    const n = (pagination.current - 1) * pagination.pageSize + i + 1
    return { id: n, name: `演示数据 ${n}`, category: ['技术','财务','人事','行政'][n % 4], status: n % 3 === 0 ? '已审核' : n % 3 === 1 ? '待审核' : '草稿', amount: Math.round(Math.random() * 10000) / 100, createDate: new Date(Date.now() - n * 86400000).toISOString().slice(0,10), creator: ['张三','李四','王五','赵六'][n % 4] }
  })
  pagination.total = 186
  loading.value = false
}
function handleTableChange(pag: any) { pagination.current = pag.current; pagination.pageSize = pag.pageSize; loadData() }
onMounted(loadData)
</script>
