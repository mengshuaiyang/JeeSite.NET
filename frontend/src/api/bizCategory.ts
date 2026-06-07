import { post, get } from './request'
import type { BizCategoryDto } from '@/types/api'

export const bizCategoryApi = {
  tree: () => get<BizCategoryDto[]>('/sys/biz-category/tree'),
  save: (data: any) => post<BizCategoryDto>('/sys/biz-category/save', data),
  delete: (code: string) => post('/sys/biz-category/delete', { categoryCode: code })
}
