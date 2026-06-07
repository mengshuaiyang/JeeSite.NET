<template>
  <a-card title="角色字段权限">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="选择角色">
        <a-select v-model:value="selectedRole" style="width:200px" placeholder="选择角色" @change="onRoleChange">
          <a-select-option v-for="r in roles" :key="r.roleCode" :value="r.roleCode">{{ r.roleName }}</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item label="选择菜单">
        <a-select v-model:value="selectedMenu" style="width:200px" placeholder="选择菜单" @change="loadFieldScopes">
          <a-select-option v-for="m in allMenus" :key="m.menuCode" :value="m.menuCode">{{ m.menuName }}</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item><a-button type="primary" @click="addEntity">新增实体</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="scopes" :columns="columns" rowKey="entityClass" :loading="loading" :pagination="false">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='entityName'">
          <a-input v-model:value="record.entityName" placeholder="实体名" />
        </template>
        <template v-if="column.key==='entityLabel'">
          <a-input v-model:value="record.entityLabel" placeholder="显示名" />
        </template>
        <template v-if="column.key==='fieldConfig'">
          <a-textarea v-model:value="record.fieldConfig" :rows="2" placeholder='{"fieldName":"visible|editable"}' style="width:300px" />
        </template>
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="saveScope(record)">保存</a>
            <a-popconfirm title="确定删除?" @confirm="deleteScope(record)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { roleApi, menuApi, roleFieldScopeApi } from '@/api'
import { message } from 'ant-design-vue'
import type { RoleDto, MenuDto } from '@/types/api'

const loading = ref(false); const selectedRole = ref(''); const selectedMenu = ref('')
const roles = ref<RoleDto[]>([]); const allMenus = ref<any[]>([]); const scopes = ref<any[]>([])

const columns = [
  { title: '实体名', key: 'entityName' }, { title: '显示名', key: 'entityLabel' },
  { title: '字段配置(JSON)', key: 'fieldConfig' }, { title: '操作', key: 'action' }
]

async function loadRoles() {
  const res = await roleApi.list({ pageNo: 1, pageSize: 999 })
  if (res.data) roles.value = res.data.list
}
async function loadFlatMenus() {
  const res = await menuApi.tree()
  if (res.data) {
    const flat: any[] = []
    function walk(items: MenuDto[]) { for (const m of items) { flat.push(m); if (m.children) walk(m.children) } }
    walk(res.data); allMenus.value = flat
  }
}
async function loadFieldScopes() {
  if (!selectedRole.value || !selectedMenu.value) return
  loading.value = true
  const res = await roleFieldScopeApi.getByRoleMenu(selectedRole.value, selectedMenu.value)
  if (res.data) scopes.value = res.data
  loading.value = false
}
function onRoleChange() { scopes.value = []; selectedMenu.value = '' }
function addEntity() {
  scopes.value.push({ entityName: '', entityLabel: '', entityClass: '', fieldConfig: '{}', roleCode: selectedRole.value, menuCode: selectedMenu.value })
}
async function saveScope(record: any) {
  record.roleCode = selectedRole.value; record.menuCode = selectedMenu.value
  await roleFieldScopeApi.save(record)
  message.success('保存成功')
}
async function deleteScope(record: any) {
  if (record.id) await roleFieldScopeApi.delete(record.id)
  scopes.value = scopes.value.filter((s: any) => s !== record)
  message.success('删除成功')
}
onMounted(async () => { await loadRoles(); await loadFlatMenus() })
</script>
