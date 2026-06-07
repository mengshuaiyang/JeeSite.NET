<template>
  <a-card title="代码生成">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="表名"><a-input v-model:value="query.tableName" placeholder="搜索" /></a-form-item>
      <a-form-item><a-button @click="load">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showImport">从数据库导入</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="list" :columns="columns" rowKey="tableName" :loading="loading"
      :pagination="{ current: pageNo, pageSize, total, onChange: (p: number) => { pageNo = p; load() } }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="editTable(record)">编辑</a>
            <a @click="previewCode(record)">预览</a>
            <a-popconfirm title="确定删除?" @confirm="del(record)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="importModal" title="从数据库导入" width="800px" @ok="doImport" :confirmLoading="importing">
      <a-table :dataSource="dbTables" rowKey="tableName" :loading="dbLoading" :pagination="false" size="small"
        :rowSelection="{ selectedRowKeys, onChange: (keys: any[]) => { selectedRowKeys = keys } }">
        <template #bodyCell="{ record, column }">
          <template v-if="column.key==='action'">
            <a @click="previewDbColumns(record)">查看字段</a>
          </template>
        </template>
      </a-table>
    </a-modal>

    <a-modal v-model:open="previewModal" title="代码预览" width="90%" @ok="previewModal=false" :footer="null">
      <a-tabs v-model:activeKey="previewTab">
        <a-tab-pane v-for="item in previewItems" :key="item.fileName" :tab="item.fileName.split('/').pop() || item.fileName">
          <pre style="max-height:60vh;overflow:auto;background:#1e1e1e;color:#d4d4d4;padding:12px;border-radius:4px;font-size:13px"><code>{{ item.content }}</code></pre>
        </a-tab-pane>
      </a-tabs>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { codegenApi } from '@/api'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import type { GenTableDto, DbTableInfo, GenPreviewItem } from '@/types/api'

const router = useRouter()
const loading = ref(false); const list = ref<GenTableDto[]>([])
const pageNo = ref(1); const pageSize = ref(10); const total = ref(0)
const query = reactive({ tableName: '' })
const columns = [
  { title: '表名', dataIndex: 'tableName' }, { title: '类名', dataIndex: 'className' },
  { title: '模块', dataIndex: 'moduleCode' }, { title: '功能', dataIndex: 'functionName' },
  { title: '说明', dataIndex: 'tableComment', ellipsis: true },
  { title: '操作', key: 'action', width: 200 }
]

const importModal = ref(false); const importing = ref(false); const dbLoading = ref(false)
const dbTables = ref<DbTableInfo[]>([]); const selectedRowKeys = ref<string[]>([])

const previewModal = ref(false); const previewTab = ref(''); const previewItems = ref<GenPreviewItem[]>([])

async function load() {
  loading.value = true; const res = await codegenApi.list({ pageNo: pageNo.value, pageSize: pageSize.value, entity: query as any })
  if (res.data) { list.value = res.data.list; total.value = res.data.total }
  loading.value = false
}
async function showImport() {
  importModal.value = true; dbLoading.value = true; selectedRowKeys.value = []
  const res = await codegenApi.dbTables()
  if (res.data) dbTables.value = res.data; dbLoading.value = false
}
async function doImport() {
  if (!selectedRowKeys.value.length) { message.warning('请选择表'); return }
  importing.value = true; await codegenApi.importTables(selectedRowKeys.value)
  message.success('导入成功'); importModal.value = false; importing.value = false; load()
}
async function previewDbColumns(record: DbTableInfo) {
  const res = await codegenApi.dbColumns(record.tableName)
  const cols = res.data?.map((c: any) => `${c.columnName} (${c.columnType} → ${c.netType})`).join('\n') || ''
  message.info(`表 ${record.tableName} 的字段:\n${cols}`, 6)
}
function editTable(record: GenTableDto) { router.push(`/codegen/table/edit?tableName=${record.tableName}`) }
async function previewCode(record: GenTableDto) {
  const res = await codegenApi.preview(record.tableName)
  if (res.data) { previewItems.value = res.data; previewTab.value = res.data[0]?.fileName || ''; previewModal.value = true }
}
async function del(record: GenTableDto) { await codegenApi.delete(record.tableName); message.success('已删除'); load() }
onMounted(load)
</script>
