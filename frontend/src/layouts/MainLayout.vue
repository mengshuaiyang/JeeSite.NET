<template>
  <a-layout style="min-height: 100vh">
    <a-layout-sider v-model:collapsed="app.collapsed" theme="dark" collapsible>
      <div class="logo">{{ app.collapsed ? 'JS' : 'JeeSite.NET' }}</div>
      <a-menu theme="dark" mode="inline" v-model:selectedKeys="selectedKeys" @click="onMenuClick">
        <template v-for="item in app.menus" :key="item.key">
          <a-menu-item v-if="!item.children?.length" :key="item.key">
            <component :is="getIcon(item.icon)" v-if="item.icon" />
            {{ item.title }}
          </a-menu-item>
          <a-sub-menu v-else :key="item.key">
            <template #title>
              <component :is="getIcon(item.icon)" v-if="item.icon" />
              <span>{{ item.title }}</span>
            </template>
            <a-menu-item v-for="child in item.children" :key="child.key">{{ child.title }}</a-menu-item>
          </a-sub-menu>
        </template>
      </a-menu>
    </a-layout-sider>
    <a-layout>
      <a-layout-header class="header">
        <menu-unfold-outlined v-if="app.collapsed" @click="app.toggleCollapsed" />
        <menu-fold-outlined v-else @click="app.toggleCollapsed" />
        <span style="margin-left: 16px">欢迎, {{ userStore.user?.userName || '用户' }}</span>
        <a-button type="link" style="float:right" @click="handleLogout">退出</a-button>
      </a-layout-header>
      <a-layout-content class="content">
        <router-view />
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { ref, h } from 'vue'
import { useRouter } from 'vue-router'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import {
  PieChartOutlined, SettingOutlined, MenuUnfoldOutlined, MenuFoldOutlined,
  UserOutlined, TeamOutlined, AppstoreOutlined, FileTextOutlined,
  SafetyOutlined, ToolOutlined, FolderOutlined, ProfileOutlined
} from '@ant-design/icons-vue'

const iconMap: Record<string, any> = {
  'pie-chart-outlined': PieChartOutlined,
  'setting-outlined': SettingOutlined,
  'user-outlined': UserOutlined,
  'team-outlined': TeamOutlined,
  'appstore-outlined': AppstoreOutlined,
  'file-text-outlined': FileTextOutlined,
  'safety-outlined': SafetyOutlined,
  'tool-outlined': ToolOutlined,
  'folder-outlined': FolderOutlined,
  'profile-outlined': ProfileOutlined
}

const router = useRouter()
const app = useAppStore()
const userStore = useUserStore()
const selectedKeys = ref<string[]>([])

function getIcon(name?: string) { return name && iconMap[name] ? iconMap[name] : AppstoreOutlined }
function onMenuClick({ key }: { key: string }) { router.push(key) }
function handleLogout() { userStore.logout(); router.push('/login') }
</script>

<style scoped>
.logo { height: 64px; line-height: 64px; text-align: center; color: #fff; font-size: 18px; font-weight: bold; }
.header { background: #fff; padding: 0 24px; display: flex; align-items: center; }
.content { margin: 24px; min-height: 280px; }
</style>
