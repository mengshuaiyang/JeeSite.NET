import { describe, it, expect, beforeEach } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'
import { useAppStore } from '@/stores/app'

describe('app store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    localStorage.clear()
  })

  it('toggles collapsed', () => {
    const store = useAppStore()
    expect(store.collapsed).toBe(false)
    store.toggleCollapsed()
    expect(store.collapsed).toBe(true)
    store.toggleCollapsed()
    expect(store.collapsed).toBe(false)
  })

  it('toggles darkMode and persists', () => {
    const store = useAppStore()
    expect(store.darkMode).toBe(false)
    store.toggleDarkMode()
    expect(store.darkMode).toBe(true)
    expect(localStorage.getItem('darkMode')).toBe('true')
    store.toggleDarkMode()
    expect(store.darkMode).toBe(false)
    expect(localStorage.getItem('darkMode')).toBe('false')
  })

  it('sets and checks permissions', () => {
    const store = useAppStore()
    store.setPermissions(['sys:user:list', 'sys:role:list'])
    expect(store.hasPermission('sys:user:list')).toBe(true)
    expect(store.hasPermission('sys:menu:list')).toBe(false)
  })

  it('hasPermission returns true when permissions is empty', () => {
    const store = useAppStore()
    store.setPermissions([])
    expect(store.hasPermission('anything')).toBe(true)
  })
})
