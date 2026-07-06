import { defineStore } from 'pinia'
import { ref } from 'vue'
import { menuApi } from '@/api/menu'
import { authApi } from '@/api/auth'
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
  const sysCodes = ref<string[]>([])
  const currentSysCode = ref<string>('')
  const permissions = ref<string[]>([])
  const darkMode = ref(localStorage.getItem('darkMode') === 'true')

  function toggleCollapsed() { collapsed.value = !collapsed.value }

  function toggleDarkMode() {
    darkMode.value = !darkMode.value
    localStorage.setItem('darkMode', String(darkMode.value))
    document.documentElement.setAttribute('data-theme', darkMode.value ? 'dark' : 'light')
  }

  async function loadMenus(sysCode?: string) {
    const res = await menuApi.getUserMenus(sysCode || currentSysCode.value || undefined)
    if (res.data) {
      menus.value = res.data.filter(m => m.isShow !== '0').map(menuToTree)
    }
  }

  async function loadSysCodes() {
    const res = await menuApi.getSysCodes()
    if (res.data) {
      sysCodes.value = res.data
      if (!currentSysCode.value && res.data.length) currentSysCode.value = res.data[0]
    }
  }

  function switchSysCode(code: string) {
    currentSysCode.value = code
    loadMenus(code)
  }

  // 权限是否已从服务端加载完成（防止刷新后权限为空被误判为“全部允许”的越权时间窗）
  const ready = ref(false)
  let bootstrapping: Promise<void> | null = null

  function setPermissions(list: string[]) { permissions.value = list }
  function hasPermission(p: string | string[]) {
    // 未加载完成 / 无权限时一律拒绝；后端 [Permission] 才是最终鉴权权威
    if (permissions.value.length === 0) return false
    if (Array.isArray(p)) {
      if (p.length === 0) return true
      return p.some((code) => permissions.value.includes(code))
    }
    return permissions.value.includes(p)
  }

  // 应用启动时使用已存在的 Token 拉取最新权限，关闭刷新后的越权时间窗
  function bootstrap() {
    if (bootstrapping) return bootstrapping
    bootstrapping = (async () => {
      if (!localStorage.getItem('token')) { ready.value = true; return }
      try {
        const res = await authApi.getAuthInfo()
        if (res.code === 200 && res.data?.permissions) {
          permissions.value = res.data.permissions
        }
      } catch {
        // 忽略：前端权限仅用于菜单/按钮显隐，后端才是鉴权权威
      } finally {
        ready.value = true
      }
    })()
    return bootstrapping
  }

  return { collapsed, menus, sysCodes, currentSysCode, permissions, ready, darkMode, toggleCollapsed, toggleDarkMode, loadMenus, loadSysCodes, switchSysCode, setPermissions, hasPermission, bootstrap }
})
