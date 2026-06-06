<template>
  <a-card title="字典管理">
    <a-tabs>
      <a-tab-pane key="type" tab="字典类型">
        <a-button type="primary" style="margin-bottom:16px" @click="showTypeAdd">新增</a-button>
        <a-table :dataSource="typeData.list" :columns="typeColumns" :loading="loading" rowKey="dictTypeCode"
          :pagination="{ current: typeData.pageNo, pageSize: typeData.pageSize, total: typeData.total, onChange: onTypePageChange }">
          <template #bodyCell="{ record, column }">
            <template v-if="column.key==='action'">
              <a-space><a @click="showTypeEdit(record)">编辑</a><a @click="handleTypeDelete(record.dictTypeCode)">删除</a></a-space>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <a-tab-pane key="data" tab="字典数据">
        <a-form layout="inline" style="margin-bottom:16px">
          <a-form-item label="字典类型"><a-input v-model:value="dataQuery.dictType" placeholder="类型编码" /></a-form-item>
          <a-form-item><a-button type="primary" @click="loadData">查询</a-button></a-form-item>
          <a-form-item><a-button type="primary" @click="showDataAdd">新增</a-button></a-form-item>
        </a-form>
        <a-table :dataSource="dataData.list" :columns="dataColumns" :loading="loadingData" rowKey="dictCode"
          :pagination="{ current: dataData.pageNo, pageSize: dataData.pageSize, total: dataData.total, onChange: onDataPageChange }">
          <template #bodyCell="{ record, column }">
            <template v-if="column.key==='action'">
              <a-space><a @click="showDataEdit(record)">编辑</a><a @click="handleDataDelete(record.dictCode)">删除</a></a-space>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>

    <a-modal v-model:open="typeModalVisible" :title="typeModalTitle" @ok="saveType">
      <a-form layout="vertical">
        <a-form-item label="编码"><a-input v-model:value="typeForm.dictTypeCode" /></a-form-item>
        <a-form-item label="名称"><a-input v-model:value="typeForm.dictName" /></a-form-item>
        <a-form-item label="系统"><a-switch v-model:checked="typeForm.isSysBool" checkedValue="1" unCheckedValue="0" /></a-form-item>
        <a-form-item label="排序"><a-input-number v-model:value="typeForm.sort" /></a-form-item>
      </a-form>
    </a-modal>

    <a-modal v-model:open="dataModalVisible" title="字典数据" @ok="saveData">
      <a-form layout="vertical">
        <a-form-item label="类型"><a-input v-model:value="dataForm.dictType" /></a-form-item>
        <a-form-item label="标签"><a-input v-model:value="dataForm.dictLabel" /></a-form-item>
        <a-form-item label="值"><a-input v-model:value="dataForm.dictValue" /></a-form-item>
        <a-form-item label="排序"><a-input-number v-model:value="dataForm.sort" /></a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { dictApi } from '@/api'
import { message } from 'ant-design-vue'
import type { DictTypeDto, DictDataDto } from '@/types/api'
const loading = ref(false); const loadingData = ref(false)
const typeData = reactive({ list: [] as DictTypeDto[], total: 0, pageNo: 1, pageSize: 20 })
const dataData = reactive({ list: [] as DictDataDto[], total: 0, pageNo: 1, pageSize: 20 })
const dataQuery = reactive({ dictType: '' })
const typeColumns = [{ title: '编码', dataIndex: 'dictTypeCode' }, { title: '名称', dataIndex: 'dictName' }, { title: '系统', dataIndex: 'isSys' }, { title: '操作', key: 'action' }]
const dataColumns = [{ title: '类型', dataIndex: 'dictType' }, { title: '标签', dataIndex: 'dictLabel' }, { title: '值', dataIndex: 'dictValue' }, { title: '操作', key: 'action' }]

const typeModalVisible = ref(false); const typeModalTitle = ref('新增字典类型')
const typeForm = reactive({ dictTypeCode: '', dictName: '', isSysBool: '0' as string | boolean, sort: 0 })
const dataModalVisible = ref(false)
const dataForm = reactive({ dictCode: undefined as string | undefined, dictType: '', dictLabel: '', dictValue: '', sort: 0 })

async function loadType() { loading.value = true; const r = await dictApi.typeList({ pageNo: typeData.pageNo, pageSize: typeData.pageSize, entity: {} as any }); if (r.data) { typeData.list = r.data.list; typeData.total = r.data.total } loading.value = false }
async function loadData() { loadingData.value = true; const r = await dictApi.dataList({ pageNo: dataData.pageNo, pageSize: dataData.pageSize, entity: { dictType: dataQuery.dictType } as any }); if (r.data) { dataData.list = r.data.list; dataData.total = r.data.total } loadingData.value = false }
function onTypePageChange(p: number, s: number) { typeData.pageNo = p; typeData.pageSize = s; loadType() }
function onDataPageChange(p: number, s: number) { dataData.pageNo = p; dataData.pageSize = s; loadData() }
function showTypeAdd() { typeModalTitle.value = '新增字典类型'; typeForm.dictTypeCode = ''; typeForm.dictName = ''; typeForm.isSysBool = '0'; typeForm.sort = 0; typeModalVisible.value = true }
function showTypeEdit(r: DictTypeDto) { typeModalTitle.value = '编辑字典类型'; typeForm.dictTypeCode = r.dictTypeCode; typeForm.dictName = r.dictName; typeForm.isSysBool = r.isSys || '0'; typeForm.sort = r.sort || 0; typeModalVisible.value = true }
async function saveType() { await dictApi.typeSave({ dictTypeCode: typeForm.dictTypeCode || undefined, dictName: typeForm.dictName, isSys: typeForm.isSysBool as string, sort: typeForm.sort }); message.success('保存成功'); typeModalVisible.value = false; loadType() }
function showDataAdd() { dataForm.dictCode = undefined; dataForm.dictType = ''; dataForm.dictLabel = ''; dataForm.dictValue = ''; dataForm.sort = 0; dataModalVisible.value = true }
function showDataEdit(r: DictDataDto) { dataForm.dictCode = r.dictCode; dataForm.dictType = r.dictType; dataForm.dictLabel = r.dictLabel; dataForm.dictValue = r.dictValue; dataForm.sort = r.sort || 0; dataModalVisible.value = true }
async function saveData() { await dictApi.dataSave({ dictCode: dataForm.dictCode, dictType: dataForm.dictType, dictLabel: dataForm.dictLabel, dictValue: dataForm.dictValue, sort: dataForm.sort }); message.success('保存成功'); dataModalVisible.value = false; loadData() }
async function handleTypeDelete(c: string) { await dictApi.typeDelete(c); message.success('删除成功'); loadType() }
async function handleDataDelete(c: string) { await dictApi.dataDelete(c); message.success('删除成功'); loadData() }
onMounted(() => { loadType(); loadData() })
</script>
