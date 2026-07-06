<template>
  <a-card title="区域管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增根区域</a-button>
    <a-table :dataSource="treeData" :columns="columns" :loading="loading" rowKey="areaCode" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="showAddChild(record)">新增子级</a><a @click="handleDelete(record.areaCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="modalOpen" title="保存区域" @ok="handleSave" :confirmLoading="saving">
      <a-form :model="form" layout="vertical">
        <a-form-item label="区域名称"><a-input v-model:value="form.areaName" /></a-form-item>
        <a-form-item label="区域类型">
          <a-select v-model:value="form.areaType" placeholder="请选择">
            <a-select-option value="province">省</a-select-option>
            <a-select-option value="city">市</a-select-option>
            <a-select-option value="district">区/县</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { areaApi } from '@/api'
import { message } from 'ant-design-vue'
import type { AreaDto } from '@/types/api'

const loading = ref(false); const saving = ref(false); const modalOpen = ref(false); const editItem = ref<AreaDto|null>(null)
const treeData = ref<AreaDto[]>([])
const form = reactive({ areaName: '', areaType: 'district', parentCode: '0', treeSort: 1000 })
const columns = [{ title: '区域编码', dataIndex: 'areaCode' }, { title: '区域名称', dataIndex: 'areaName' }, { title: '区域类型', dataIndex: 'areaType' }, { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }]
async function loadData() {
  loading.value = true
  try {
    const res = await areaApi.tree()
    if (res.data) treeData.value = res.data
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function showAdd() { editItem.value = null; form.parentCode='0'; form.areaName=''; form.areaType='district'; modalOpen.value = true }
function showAddChild(parent: AreaDto) { editItem.value = null; form.parentCode=parent.areaCode; form.areaName=''; form.areaType='district'; modalOpen.value = true }
function showEdit(row: AreaDto) { editItem.value = row; Object.assign(form, row); modalOpen.value = true }
async function handleSave() { saving.value = true; await areaApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false }
async function handleDelete(code: string) { await areaApi.delete(code); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
