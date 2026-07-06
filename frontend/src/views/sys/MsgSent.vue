<template>
  <a-card title="已发送">
    <a-button type="primary" style="margin-bottom:16px" @click="showSend">发送消息</a-button>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="msgId"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='receiveType'">
          <a-tag>{{ record.receiveType }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-space><a @click="handleDelete(record.msgId)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="发送消息" @ok="handleSend" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="标题"><a-input v-model:value="form.msgTitle" /></a-form-item>
        <a-form-item label="内容"><a-textarea v-model:value="form.msgContent" /></a-form-item>
        <a-form-item label="接收类型">
          <a-select v-model:value="form.receiveType">
            <a-select-option value="all">全部</a-select-option>
            <a-select-option value="user">用户</a-select-option>
            <a-select-option value="role">角色</a-select-option>
            <a-select-option value="office">机构</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="接收编码"><a-input v-model:value="form.receiveCodes" placeholder="多个用逗号分隔" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { msgApi } from '@/api'
import { message } from 'ant-design-vue'
import type { MsgInnerDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false)
const data = reactive({ list: [] as MsgInnerDto[], total: 0, pageNo: 1, pageSize: 20 })
const form = reactive({ msgTitle: '', msgContent: '', receiveType: 'all', receiveCodes: '' })
const columns = [
  { title: '标题', dataIndex: 'msgTitle' }, { title: '接收类型', key: 'receiveType' },
  { title: '时间', dataIndex: 'createDate' }, { title: '操作', key: 'action' }
]
async function loadData() {
  loading.value = true
  try {
    const res = await msgApi.sent({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} as any })
    if (res.data) { data.list = res.data.list; data.total = res.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(page: number, size: number) { data.pageNo = page; data.pageSize = size; loadData() }
function showSend() { form.msgTitle=''; form.msgContent=''; form.receiveType='all'; form.receiveCodes=''; modalOpen.value = true }
async function handleSend() { saving.value = true; await msgApi.send(form); message.success('发送成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(msgId: string) { await msgApi.delete(msgId); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
