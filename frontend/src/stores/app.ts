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

  function toggleCollapsed() { collapsed.value = !collapsed.value }

  async function loadMenus() {
    const res = await menuApi.getUserMenus()
    if (res.data) {
      menus.value = res.data.filter(m => m.isShow !== '0').map(menuToTree)
    }
  }

  function setPermissions(list: string[]) { permissions.value = list }
  function hasPermission(p: string) { return permissions.value.length === 0 || permissions.value.includes(p) }

  return { collapsed, menus, permissions, toggleCollapsed, loadMenus, setPermissions, hasPermission }
})
