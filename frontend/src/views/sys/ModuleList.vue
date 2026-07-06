<template>
  <a-card title="模块管理">
    <a-button type="primary" style="margin-bottom:16px" @click="showAdd">新增</a-button>
    <a-table :dataSource="data.list" :columns="columns" :loading="loading" rowKey="moduleCode"
      :pagination="{ current: data.pageNo, pageSize: data.pageSize, total: data.total, onChange: onPageChange }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='action'">
          <a-space><a @click="showEdit(record)">编辑</a><a @click="handleDelete(record.moduleCode)">删除</a></a-space>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="modalVisible" :title="modalTitle" @ok="save">
      <a-form layout="vertical">
        <a-form-item label="编码"><a-input v-model:value="form.moduleCode" :disabled="isEdit" /></a-form-item>
        <a-form-item label="名称"><a-input v-model:value="form.moduleName" /></a-form-item>
        <a-form-item label="版本"><a-input v-model:value="form.moduleVersion" /></a-form-item>
        <a-form-item label="启用"><a-switch v-model:checked="form.isEnabledBool" checkedValue="1" unCheckedValue="0" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { moduleApi } from '@/api'
import { message } from 'ant-design-vue'
import type { ModuleDto } from '@/types/api'
const loading = ref(false)
const data = reactive({ list: [] as ModuleDto[], total: 0, pageNo: 1, pageSize: 20 })
const columns = [{ title: '编码', dataIndex: 'moduleCode' }, { title: '名称', dataIndex: 'moduleName' }, { title: '版本', dataIndex: 'moduleVersion' }, { title: '启用', dataIndex: 'isEnabled' }, { title: '操作', key: 'action' }]
const modalVisible = ref(false); const modalTitle = ref('新增模块'); const isEdit = ref(false)
const form = reactive({ moduleCode: '', moduleName: '', moduleVersion: '', isEnabledBool: '1' as string | boolean })
async function loadData() {
  loading.value = true
  try {
    const r = await moduleApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} as any })
    if (r.data) { data.list = r.data.list; data.total = r.data.total }
  } catch (e: any) {
    message.error(e?.message || '加载失败')
  } finally {
    loading.value = false
  }
}
function onPageChange(p: number, s: number) { data.pageNo = p; data.pageSize = s; loadData() }
function showAdd() { isEdit.value = false; modalTitle.value = '新增模块'; form.moduleCode = ''; form.moduleName = ''; form.moduleVersion = ''; form.isEnabledBool = '1'; modalVisible.value = true }
function showEdit(r: ModuleDto) { isEdit.value = true; modalTitle.value = '编辑模块'; form.moduleCode = r.moduleCode; form.moduleName = r.moduleName; form.moduleVersion = r.moduleVersion || ''; form.isEnabledBool = r.isEnabled || '1'; modalVisible.value = true }
async function save() { await moduleApi.save({ moduleCode: isEdit.value ? form.moduleCode : form.moduleCode || undefined, moduleName: form.moduleName, moduleVersion: form.moduleVersion, isEnabled: form.isEnabledBool as string }); message.success('保存成功'); modalVisible.value = false; loadData() }
async function handleDelete(k: string) { await moduleApi.delete(k); message.success('删除成功'); loadData() }
onMounted(loadData)
</script>
