import { useAppStore } from '@/stores/app'

export function usePermission() {
  const appStore = useAppStore()

  function has(permission: string): boolean {
    return appStore.hasPermission(permission)
  }

  function hasAny(permissions: string[]): boolean {
    if (!permissions || permissions.length === 0) return true
    return permissions.some((p) => appStore.hasPermission(p))
  }

  function hasAll(permissions: string[]): boolean {
    if (!permissions || permissions.length === 0) return true
    return permissions.every((p) => appStore.hasPermission(p))
  }

  function getAll(): string[] {
    return appStore.permissions
  }

  return { has, hasAny, hasAll, getAll }
}
