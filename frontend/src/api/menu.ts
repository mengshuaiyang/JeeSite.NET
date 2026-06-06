import { post, get } from './request'
import type { PageRequest, PageResult, MenuDto, MenuSaveDto } from '@/types/api'

export const menuApi = {
  list: (data: PageRequest) => post<PageResult<MenuDto>>('/sys/menu/list', data),
  tree: (moduleCode?: string) => get<MenuDto[]>('/sys/menu/tree', { moduleCode }),
  getUserMenus: () => get<MenuDto[]>('/sys/menu/user-menus'),
  get: (menuCode: string) => get<MenuDto>('/sys/menu/get', { menuCode }),
  save: (data: MenuSaveDto) => post<MenuDto>('/sys/menu/save', data),
  delete: (menuCode: string) => post('/sys/menu/delete', { menuCode })
}
