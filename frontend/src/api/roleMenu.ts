import { post, get } from './request'

export const roleMenuApi = {
  getMenuCodes: (roleCode: string) => get<string[]>('/sys/role-menu/get-menu-codes', { roleCode }),
  save: (roleCode: string, menuCodes: string[]) => post('/sys/role-menu/save', { roleCode, menuCodes })
}
