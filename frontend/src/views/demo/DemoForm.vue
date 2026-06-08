<template>
  <a-card title="表单演示 (Ant Design Vue)">
    <a-tabs v-model:activeKey="activeTab">
      <a-tab-pane key="basic" tab="基础表单">
        <a-form :model="basicForm" layout="vertical">
          <a-row :gutter="16">
            <a-col :span="12"><a-form-item label="文本框"><a-input v-model:value="basicForm.text" /></a-form-item></a-col>
            <a-col :span="12"><a-form-item label="数字输入"><a-input-number v-model:value="basicForm.number" style="width:100%" /></a-form-item></a-col>
          </a-row>
          <a-form-item label="文本域"><a-textarea v-model:value="basicForm.textarea" rows=3 /></a-form-item>
          <a-row :gutter="16">
            <a-col :span="8"><a-form-item label="下拉框"><a-select v-model:value="basicForm.select"><a-select-option value="a">选项 A</a-select-option><a-select-option value="b">选项 B</a-select-option><a-select-option value="c">选项 C</a-select-option></a-select></a-form-item></a-col>
            <a-col :span="8"><a-form-item label="单选框"><a-radio-group v-model:value="basicForm.radio"><a-radio value="1">是</a-radio><a-radio value="0">否</a-radio></a-radio-group></a-form-item></a-col>
            <a-col :span="8"><a-form-item label="开关"><a-switch v-model:checked="basicForm.switch" /></a-form-item></a-col>
          </a-row>
          <a-form-item label="日期"><a-date-picker v-model:value="basicForm.date" style="width:100%" /></a-form-item>
          <a-form-item><a-button type="primary" @click="handleBasicSubmit">提交</a-button></a-form-item>
        </a-form>
      </a-tab-pane>
      <a-tab-pane key="advanced" tab="高级表单">
        <a-form :model="advForm" layout="vertical">
          <a-row :gutter="16">
            <a-col :span="12"><a-form-item label="滑块"><a-slider v-model:value="advForm.slider" :min="0" :max="100" /></a-form-item></a-col>
            <a-col :span="12"><a-form-item label="评分"><a-rate v-model:value="advForm.rate" /></a-form-item></a-col>
          </a-row>
          <a-form-item><a-button type="primary" @click="handleAdvSubmit">提交</a-button></a-form-item>
        </a-form>
      </a-tab-pane>
      <a-tab-pane key="search" tab="搜索表单">
        <a-form :model="searchForm" layout="inline">
          <a-form-item label="关键词"><a-input v-model:value="searchForm.keyword" placeholder="搜索..." /></a-form-item>
          <a-form-item label="日期范围"><a-range-picker v-model:value="searchForm.dateRange" /></a-form-item>
          <a-form-item label="分类"><a-select v-model:value="searchForm.category" placeholder="请选择" style="width:150px"><a-select-option value="">全部</a-select-option><a-select-option value="tech">技术</a-select-option><a-select-option value="finance">财务</a-select-option></a-select></a-form-item>
          <a-form-item><a-button type="primary" @click="handleSearch">搜索</a-button><a-button style="margin-left:8px" @click="handleReset">重置</a-button></a-form-item>
        </a-form>
      </a-tab-pane>
    </a-tabs>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { message } from 'ant-design-vue'

const activeTab = ref('basic')
const basicForm = reactive({ text: '', number: 0, textarea: '', select: 'a', radio: '1', switch: true, date: '' })
const advForm = reactive({ slider: 50, rate: 3 })
const searchForm = reactive({ keyword: '', dateRange: [], category: '' })

function handleBasicSubmit() { message.success(`基础表单提交: ${JSON.stringify(basicForm)}`) }
function handleAdvSubmit() { message.success(`高级表单提交: ${JSON.stringify(advForm)}`) }
function handleSearch() { message.success(`搜索: ${JSON.stringify(searchForm)}`) }
function handleReset() { searchForm.keyword = ''; searchForm.dateRange = []; searchForm.category = '' }
</script>
