import { defineStore } from 'pinia'
import { ref } from 'vue'
import { menuApi } from '@/api/menu'
import type { MenuDto } from '@/types/api'

export interface MenuTreeNode {
  key: string
  title: string
  icon?: string
  children?: MenuTreeNode[]
}

function menuToTree(dto: MenuDto): MenuTreeNode {
  const node: MenuTreeNode = { key: dto.menuHref || dto.menuCode, title: dto.menuName, icon: dto.menuIcon }
  if (dto.children?.length) node.children = dto.children.filter(c => c.isShow !== '0').map(menuToTree)
  return node
}

export const useAppStore = defineStore('app', () => {
  const collapsed = ref(false)
  const menus = ref<MenuTreeNode[]>([])
  const permissions = ref<string[]>([])
  const darkMode = ref(localStorage.getItem('darkMode') === 'true')

  function toggleCollapsed() { collapsed.value = !collapsed.value }

  function toggleDarkMode() {
    darkMode.value = !darkMode.value
    localStorage.setItem('darkMode', String(darkMode.value))
    document.documentElement.setAttribute('data-theme', darkMode.value ? 'dark' : 'light')
  }

  async function loadMenus() {
    const res = await menuApi.getUserMenus()
    if (res.data) {
      menus.value = res.data.filter(m => m.isShow !== '0').map(menuToTree)
    }
  }

  function setPermissions(list: string[]) { permissions.value = list }
  function hasPermission(p: string) { return permissions.value.length === 0 || permissions.value.includes(p) }

  return { collapsed, menus, permissions, darkMode, toggleCollapsed, toggleDarkMode, loadMenus, setPermissions, hasPermission }
})
