<template>
  <a-card title="参数配置">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="configKey"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.configKey)">删除</a></a-space>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="modalVisible" :title="modalTitle" @ok="save">
      <a-form layout="vertical">
        <a-form-item label="键"><a-input v-model:value="form.configKey" :disabled="isEdit" /></a-form-item>
        <a-form-item label="名称"><a-input v-model:value="form.configName" /></a-form-item>
        <a-form-item label="值"><a-input v-model:value="form.configValue" /></a-form-item>
        <a-form-item label="系统"><a-switch v-model:checked="form.isSysBool" checkedValue="1" unCheckedValue="0" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { configApi } from '@/api'
import { message } from 'ant-design-vue'
import type { ConfigDto } from '@/types/api'
const loading = ref(false)
const data = reactive({ list: [] as ConfigDto[], total: 0, pageNo: 1, pageSize: 20 })
const columns = [{ title: '键', dataIndex: 'configKey' }, { title: '名称', dataIndex: 'configName' }, { title: '值', dataIndex: 'configValue' }, { title: '系统', dataIndex: 'isSys' }, { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }]
const modalVisible = ref(false); const modalTitle = ref('新增参数'); const isEdit = ref(false)
const form = reactive({ configKey: '', configName: '', configValue: '', isSysBool: '0' as string | boolean })
async function loadData() { loading.value = true; const r = await configApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} as any }); if (r.data) { data.list = r.data.list; data.total = r.data.total } loading.value = false }
function onPageChange(p: number, s: number) { data.pageNo = p; data.pageSize = s; loadData() }
function showAdd() { isEdit.value = false; modalTitle.value = '新增参数'; form.configKey = ''; form.configName = ''; form.configValue = ''; form.isSysBool = '0'; modalVisible.value = true }
function showEdit(r: ConfigDto) { isEdit.value = true; modalTitle.value = '编辑参数'; form.configKey = r.configKey; form.configName = r.configName; form.configValue = r.configValue || ''; form.isSysBool = r.isSys || '0'; modalVisible.value = true }
async function save() { await configApi.save({ configKey: isEdit.value ? form.configKey : form.configKey || undefined, configName: form.configName, configValue: form.configValue, isSys: form.isSysBool as string }); message.success('保存成功'); modalVisible.value = false; loadData() }
async function handleDelete(k: string) { await configApi.delete(k); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
