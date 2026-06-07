import { post, get } from './request'
import type { TestDataDto } from '@/types/api'

export const testDataApi = {
  list: () => get<TestDataDto[]>('/test/data/list'),
  get: (id: string) => get<TestDataDto>('/test/data/get', { id }),
  save: (data: any) => post<TestDataDto>('/test/data/save', data),
  delete: (id: string) => post('/test/data/delete', { id })
}
