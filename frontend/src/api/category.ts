import { post, get } from './request'
import type { CategoryDto } from '@/types/api'

export const categoryApi = {
  list: () => get<CategoryDto[]>('/cms/category/list'),
  tree: () => get<CategoryDto[]>('/cms/category/tree'),
  get: (categoryCode: string) => get<CategoryDto>('/cms/category/get', { categoryCode }),
  save: (data: any) => post('/cms/category/save', data)
}
