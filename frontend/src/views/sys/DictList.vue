<template>
  <a-card title="字典管理">
    <a-tabs v-model:activeKey="activeTab">
      <a-tab-pane key="type" tab="字典类型">
        <a-button type="primary" style="margin-bottom:16px" @click="showTypeAdd">新增</a-button>
        <a-table :dataSource="typeData.list" :columns="typeColumns" :loading="loading" rowKey="dictTypeCode"
          :pagination="{ current: typeData.pageNo, pageSize: typeData.pageSize, total: typeData.total, onChange: onTypePageChange }">
          <template #bodyCell="{ record, column }">
            <template v-if="column.key==='status'">
              <a-tag :color="record.status==='0'?'green':'red'">{{ record.status==='0'?'正常':'停用' }}</a-tag>
            </template>
            <template v-if="column.key==='action'">
              <a-space>
                <a @click="showTypeEdit(record)">编辑</a>
                <a @click="selectTypeAndSwitch(record.dictTypeCode)">管理数据</a>
                <a-popconfirm title="确定删除?" @confirm="handleTypeDelete(record.dictTypeCode)"><a style="color:red">删除</a></a-popconfirm>
              </a-space>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <a-tab-pane key="data" tab="字典数据" force-render>
        <a-form layout="inline" style="margin-bottom:16px">
          <a-form-item label="字典类型">
            <a-select v-model:value="dataQuery.dictType" style="width:200px" allowClear placeholder="选择字典类型" @change="loadData">
              <a-select-option v-for="t in typeData.list" :key="t.dictTypeCode" :value="t.dictTypeCode">{{ t.dictTypeCode }} - {{ t.dictName }}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item label="标签"><a-input v-model:value="dataQuery.dictLabel" placeholder="搜索标签" /></a-form-item>
          <a-form-item><a-button @click="loadData">查询</a-button></a-form-item>
          <a-form-item><a-button type="primary" @click="showDataAdd">新增</a-button></a-form-item>
        </a-form>
        <a-table :dataSource="dataData.list" :columns="dataColumns" :loading="loadingData" rowKey="dictCode"
          :pagination="{ current: dataData.pageNo, pageSize: dataData.pageSize, total: dataData.total, onChange: onDataPageChange }"
          :expandable="{ childrenColumnName: 'children', rowExpandable: (r: any) => r.children?.length > 0 }">
          <template #bodyCell="{ record, column }">
            <template v-if="column.key==='status'">
              <a-tag :color="record.status==='0'?'green':'red'">{{ record.status==='0'?'正常':'停用' }}</a-tag>
            </template>
            <template v-if="column.key==='action'">
              <a-space>
                <a @click="showDataEdit(record)">编辑</a>
                <a @click="showDataAddChild(record)">新增子项</a>
                <a-popconfirm title="确定删除?" @confirm="handleDataDelete(record.dictCode)"><a style="color:red">删除</a></a-popconfirm>
              </a-space>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>

    <a-modal v-model:open="typeModalVisible" :title="typeModalTitle" @ok="saveType" :confirmLoading="typeSaving">
      <a-form layout="vertical">
        <a-form-item label="编码"><a-input v-model:value="typeForm.dictTypeCode" :disabled="isTypeEdit" /></a-form-item>
        <a-form-item label="名称"><a-input v-model:value="typeForm.dictName" /></a-form-item>
        <a-form-item label="系统字典"><a-switch v-model:checked="typeForm.isSysBool" checkedValue="1" unCheckedValue="0" /></a-form-item>
        <a-form-item label="排序"><a-input-number v-model:value="typeForm.sort" style="width:100%" /></a-form-item>
      </a-form>
    </a-modal>

    <a-modal v-model:open="dataModalVisible" :title="dataModalTitle" @ok="saveData" :confirmLoading="dataSaving" width="600px">
      <a-form layout="vertical">
        <a-form-item label="字典类型">
          <a-select v-model:value="dataForm.dictType" style="width:100%">
            <a-select-option v-for="t in typeData.list" :key="t.dictTypeCode" :value="t.dictTypeCode">{{ t.dictTypeCode }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="标签"><a-input v-model:value="dataForm.dictLabel" /></a-form-item></a-col>
          <a-col :span="12"><a-form-item label="数据值"><a-input v-model:value="dataForm.dictValue" /></a-form-item></a-col>
        </a-row>
        <a-form-item v-if="dataData.list.length" label="父级"><a-tree-select v-model:value="dataForm.parentCode" :treeData="dataData.list" allowClear placeholder="顶级"
          :fieldNames="{label:'dictLabel',value:'dictCode',children:'children'}" style="width:100%" /></a-form-item>
        <a-row :gutter="16">
          <a-col :span="12"><a-form-item label="排序"><a-input-number v-model:value="dataForm.sort" style="width:100%" /></a-form-item></a-col>
        </a-row>
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { dictApi } from '@/api'
import { message } from 'ant-design-vue'
import type { DictTypeDto, DictDataDto } from '@/types/api'

const activeTab = ref('type')
const loading = ref(false); const loadingData = ref(false)
const typeSaving = ref(false); const dataSaving = ref(false)

const typeData = reactive({ list: [] as DictTypeDto[], total: 0, pageNo: 1, pageSize: 20 })
const dataData = reactive({ list: [] as DictDataDto[], total: 0, pageNo: 1, pageSize: 20 })
const dataQuery = reactive({ dictType: '', dictLabel: '' })

const typeColumns = [
  { title: '编码', dataIndex: 'dictTypeCode' },
  { title: '名称', dataIndex: 'dictName' },
  { title: '系统', dataIndex: 'isSys', width: 60 },
  { title: '状态', key: 'status', width: 70 },
  { title: '排序', dataIndex: 'sort', width: 60 },
  { title: '操作', key: 'action', width: 200 }
]
const dataColumns = [
  { title: '标签', dataIndex: 'dictLabel' },
  { title: '数据值', dataIndex: 'dictValue' },
  { title: '排序', dataIndex: 'sort', width: 60 },
  { title: '状态', key: 'status', width: 70 },
  { title: '操作', key: 'action', width: 200 }
]

const typeModalVisible = ref(false); const typeModalTitle = ref('新增字典类型')
const isTypeEdit = computed(() => typeModalTitle.value === '编辑字典类型')
const typeForm = reactive({ dictTypeCode: '', dictName: '', isSysBool: '0' as string | boolean, sort: 0 })

const dataModalVisible = ref(false); const dataModalTitle = ref('新增字典数据')
const dataForm = reactive({ dictCode: undefined as string | undefined, dictType: '', dictLabel: '', dictValue: '', parentCode: undefined as string | undefined, sort: 0 })

async function loadType() { loading.value = true; const r = await dictApi.typeList({ pageNo: typeData.pageNo, pageSize: typeData.pageSize, entity: {} as any }); if (r.data) { typeData.list = r.data.list; typeData.total = r.data.total } loading.value = false }
async function loadData() { if (!dataQuery.dictType) { dataData.list = []; dataData.total = 0; return } loadingData.value = true; const r = await dictApi.dataList({ pageNo: dataData.pageNo, pageSize: dataData.pageSize, entity: { dictType: dataQuery.dictType, dictLabel: dataQuery.dictLabel } as any }); if (r.data) { dataData.list = r.data.list; dataData.total = r.data.total } loadingData.value = false }
function onTypePageChange(p: number, s: number) { typeData.pageNo = p; typeData.pageSize = s; loadType() }
function onDataPageChange(p: number, s: number) { dataData.pageNo = p; dataData.pageSize = s; loadData() }
function selectTypeAndSwitch(code: string) { dataQuery.dictType = code; activeTab.value = 'data'; loadData() }

function showTypeAdd() { typeModalTitle.value = '新增字典类型'; typeForm.dictTypeCode = ''; typeForm.dictName = ''; typeForm.isSysBool = '0'; typeForm.sort = 0; typeModalVisible.value = true }
function showTypeEdit(r: DictTypeDto) { typeModalTitle.value = '编辑字典类型'; typeForm.dictTypeCode = r.dictTypeCode; typeForm.dictName = r.dictName; typeForm.isSysBool = r.isSys || '0'; typeForm.sort = r.sort || 0; typeModalVisible.value = true }
async function saveType() { typeSaving.value = true; await dictApi.typeSave({ dictTypeCode: typeForm.dictTypeCode || undefined, dictName: typeForm.dictName, isSys: typeForm.isSysBool as string, sort: typeForm.sort }); message.success('保存成功'); typeModalVisible.value = false; typeSaving.value = false; loadType() }

function showDataAdd() { dataModalTitle.value = '新增字典数据'; resetDataForm(); dataForm.dictType = dataQuery.dictType; dataModalVisible.value = true }
function showDataAddChild(parent: DictDataDto) { dataModalTitle.value = '新增字典数据'; resetDataForm(); dataForm.dictType = parent.dictType; dataForm.parentCode = parent.dictCode; dataModalVisible.value = true }
function showDataEdit(r: DictDataDto) { dataModalTitle.value = '编辑字典数据'; dataForm.dictCode = r.dictCode; dataForm.dictType = r.dictType; dataForm.dictLabel = r.dictLabel; dataForm.dictValue = r.dictValue; dataForm.parentCode = r.parentCode || undefined; dataForm.sort = r.sort || 0; dataModalVisible.value = true }
function resetDataForm() { dataForm.dictCode = undefined; dataForm.dictType = ''; dataForm.dictLabel = ''; dataForm.dictValue = ''; dataForm.parentCode = undefined; dataForm.sort = 0 }
async function saveData() { dataSaving.value = true; await dictApi.dataSave({ dictCode: dataForm.dictCode, dictType: dataForm.dictType, dictLabel: dataForm.dictLabel, dictValue: dataForm.dictValue, parentCode: dataForm.parentCode, sort: dataForm.sort }); message.success('保存成功'); dataModalVisible.value = false; dataSaving.value = false; loadData() }
async function handleTypeDelete(c: string) { await dictApi.typeDelete(c); message.success('删除成功'); loadType() }
async function handleDataDelete(c: string) { await dictApi.dataDelete(c); message.success('删除成功'); loadData() }
onMounted(loadType)
</script>
