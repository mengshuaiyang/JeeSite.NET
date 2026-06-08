<template>
  <a-card :title="isNew ? '新建表配置' : '编辑表配置'" v-if="loaded">
    <a-form layout="vertical">
      <a-row :gutter="16">
        <a-col :span="6"><a-form-item label="表名" required><a-input v-model:value="form.tableName" :disabled="!isNew" /></a-form-item></a-col>
        <a-col :span="6"><a-form-item label="类名" required><a-input v-model:value="form.className" /></a-form-item></a-col>
        <a-col :span="6"><a-form-item label="模块" required><a-input v-model:value="form.moduleCode" placeholder="如 Sys" /></a-form-item></a-col>
        <a-col :span="6"><a-form-item label="功能名"><a-input v-model:value="form.functionName" placeholder="如 用户管理" /></a-form-item></a-col>
      </a-row>
      <a-row :gutter="16">
        <a-col :span="6"><a-form-item label="作者"><a-input v-model:value="form.functionAuthor" /></a-form-item></a-col>
        <a-col :span="6"><a-form-item label="业务名"><a-input v-model:value="form.businessName" placeholder="如 user" /></a-form-item></a-col>
        <a-col :span="6"><a-form-item label="模板分类">
          <a-select v-model:value="form.tplCategory">
            <a-select-option value="crud">单表 (CRUD)</a-select-option>
            <a-select-option value="tree">树表 (Tree)</a-select-option>
            <a-select-option value="query">仅查询 (Query)</a-select-option>
            <a-select-option value="service">仅服务 (Service)</a-select-option>
          </a-select>
        </a-form-item></a-col>
        <a-col :span="6"><a-form-item label="备注"><a-input v-model:value="form.tableComment" /></a-form-item></a-col>
      </a-row>
    </a-form>

    <h4 style="margin:16px 0 8px">字段配置</h4>
    <a-table :dataSource="form.columns" :columns="colColumns" rowKey="columnName" :pagination="false" size="small">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='isPk'"><a-checkbox :checked="record.isPk==='1'" @change="record.isPk=$event.target.checked?'1':'0'" /></template>
        <template v-if="column.key==='isInsert'"><a-checkbox :checked="record.isInsert!=='0'" @change="record.isInsert=$event.target.checked?'1':'0'" /></template>
        <template v-if="column.key==='isEdit'"><a-checkbox :checked="record.isEdit!=='0'" @change="record.isEdit=$event.target.checked?'1':'0'" /></template>
        <template v-if="column.key==='isList'"><a-checkbox :checked="record.isList!=='0'" @change="record.isList=$event.target.checked?'1':'0'" /></template>
        <template v-if="column.key==='isQuery'"><a-checkbox :checked="record.isQuery==='1'" @change="record.isQuery=$event.target.checked?'1':'0'" /></template>
        <template v-if="column.key==='queryType'">
          <a-select v-model:value="record.queryType" style="width:90px" size="small">
            <a-select-option value="EQ">=</a-select-option><a-select-option value="LIKE">LIKE</a-select-option>
            <a-select-option value="GT">></a-select-option><a-select-option value="LT"><</a-select-option>
            <a-select-option value="BETWEEN">BETWEEN</a-select-option>
          </a-select>
        </template>
        <template v-if="column.key==='htmlType'">
          <a-select v-model:value="record.htmlType" style="width:110px" size="small">
            <a-select-option value="input">文本框</a-select-option><a-select-option value="textarea">文本域</a-select-option>
            <a-select-option value="select">下拉框</a-select-option><a-select-option value="radio">单选框</a-select-option>
            <a-select-option value="checkbox">复选框</a-select-option><a-select-option value="datepicker">日期</a-select-option>
            <a-select-option value="datetimepicker">时间</a-select-option><a-select-option value="image">图片</a-select-option>
            <a-select-option value="file">文件</a-select-option><a-select-option value="editor">编辑器</a-select-option>
          </a-select>
        </template>
        <template v-if="column.key==='dictType'"><a-input v-model:value="record.dictType" size="small" style="width:110px" /></template>
      </template>
    </a-table>

    <div style="margin-top:16px">
      <a-button type="primary" @click="handleSave" :loading="saving">保存</a-button>
      <a-button style="margin-left:8px" @click="$router.back()">返回</a-button>
    </div>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { codegenApi } from '@/api'
import { useRoute, useRouter } from 'vue-router'
import { message } from 'ant-design-vue'

const route = useRoute(); const router = useRouter()
const isNew = computed(() => !route.query.tableName)
const saving = ref(false); const loaded = ref(false)

const form = reactive({ tableName: '', className: '', moduleCode: '', functionName: '', functionAuthor: '', businessName: '', tplCategory: 'crud', tableComment: '', columns: [] as any[] })
const colColumns = [
  { title: '列名', dataIndex: 'columnName', width: 100 },
  { title: '属性名', dataIndex: 'propertyName', width: 100 },
  { title: '类型', dataIndex: 'netType', width: 70 },
  { title: '主键', key: 'isPk', width: 40 },
  { title: '新增', key: 'isInsert', width: 40 },
  { title: '编辑', key: 'isEdit', width: 40 },
  { title: '列表', key: 'isList', width: 40 },
  { title: '查询', key: 'isQuery', width: 40 },
  { title: '查询方式', key: 'queryType', width: 100 },
  { title: '显示类型', key: 'htmlType', width: 120 },
  { title: '字典', key: 'dictType', width: 120 }
]

async function load() {
  const tn = route.query.tableName as string
  if (!tn) { loaded.value = true; return }
  const res = await codegenApi.get(tn)
  if (res.data) {
    form.tableName = res.data.tableName; form.className = res.data.className
    form.moduleCode = res.data.moduleCode; form.functionName = res.data.functionName || ''
    form.functionAuthor = res.data.functionAuthor || ''; form.businessName = res.data.businessName || ''
    form.tplCategory = res.data.tplCategory || 'crud'
    form.tableComment = res.data.tableComment || ''; form.columns = res.data.columns || []
  }
  loaded.value = true
}
async function handleSave() {
  saving.value = true; await codegenApi.save({ ...form }); message.success('保存成功'); router.push('/codegen/table')
  saving.value = false
}
onMounted(load)
</script>
