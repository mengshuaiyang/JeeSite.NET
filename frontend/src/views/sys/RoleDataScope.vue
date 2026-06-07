<template>
  <a-card title="角色数据权限">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="选择角色">
        <a-select v-model:value="selectedRole" style="width:240px" placeholder="请选择角色" @change="onRoleChange">
          <a-select-option v-for="r in roles" :key="r.roleCode" :value="r.roleCode">{{ r.roleName }}</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item><a-button type="primary" @click="saveAll" :loading="saving">保存全部</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="menuTree" :columns="columns" rowKey="menuCode" :loading="loading"
      :pagination="false" defaultExpandAllRows>
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='ruleType'">
          <a-select v-model:value="record.ruleType" style="width:140px" @change="(val: string) => onRuleTypeChange(record, val)">
            <a-select-option value="all">全部数据</a-select-option>
            <a-select-option value="custom">自定义</a-select-option>
            <a-select-option value="org">本机构</a-select-option>
            <a-select-option value="org_child">机构及下属</a-select-option>
            <a-select-option value="self">仅本人</a-select-option>
          </a-select>
        </template>
        <template v-if="column.key==='ruleConfig'">
          <a-input v-model:value="record.ruleConfig" placeholder="自定义数据范围" :disabled="record.ruleType!=='custom'" style="width:200px" />
        </template>
        <template v-if="column.key==='ruleName'">
          <a-input v-model:value="record.ruleName" placeholder="规则名称" style="width:160px" />
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { roleApi, menuApi, roleDataScopeApi } from '@/api'
import { message } from 'ant-design-vue'
import type { RoleDto, MenuDto } from '@/types/api'

const loading = ref(false); const saving = ref(false)
const selectedRole = ref('')
const roles = ref<RoleDto[]>([])
const menuTree = ref<any[]>([])

const columns = [
  { title: '菜单名称', dataIndex: 'menuName' },
  { title: '规则名称', key: 'ruleName' },
  { title: '数据范围', key: 'ruleType' },
  { title: '规则配置', key: 'ruleConfig' }
]

async function loadRoles() {
  const res = await roleApi.list({ pageNo: 1, pageSize: 999 })
  if (res.data) roles.value = res.data.list
}
async function loadMenus() {
  const res = await menuApi.tree()
  if (res.data) {
    menuTree.value = flattenTree(res.data)
  }
}
function flattenTree(items: MenuDto[], parent?: any): any[] {
  const result: any[] = []
  for (const item of items) {
    const row: any = { ...item, ruleType: 'all', ruleConfig: '', ruleName: '' }
    if (parent) row._parent = parent
    result.push(row)
    if (item.children) result.push(...flattenTree(item.children, row))
  }
  return result
}
async function loadScopes() {
  if (!selectedRole.value) return
  loading.value = true
  const res = await roleDataScopeApi.getByRole(selectedRole.value)
  if (res.data) {
    const map = new Map(res.data.map((s: any) => [s.menuCode, s]))
    for (const row of menuTree.value) {
      const scope = map.get(row.menuCode)
      if (scope) {
        row.ruleType = scope.ruleType || 'all'
        row.ruleConfig = scope.ruleConfig || ''
        row.ruleName = scope.ruleName || ''
      } else {
        row.ruleType = 'all'
        row.ruleConfig = ''
        row.ruleName = ''
      }
    }
  }
  loading.value = false
}
function onRoleChange() { loadScopes() }
function onRuleTypeChange(record: any, val: string) { record.ruleType = val }
async function saveAll() {
  if (!selectedRole.value) { message.warning('请先选择角色'); return }
  saving.value = true
  for (const row of menuTree.value) {
    await roleDataScopeApi.save({ roleCode: selectedRole.value, menuCode: row.menuCode, ruleName: row.ruleName, ruleType: row.ruleType, ruleConfig: row.ruleConfig })
  }
  message.success('保存成功')
  saving.value = false
}
onMounted(async () => { await loadRoles(); await loadMenus() })
</script>
