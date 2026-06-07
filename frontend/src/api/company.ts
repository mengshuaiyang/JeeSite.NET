import { post, get } from './request'
import type { CompanyDto } from '@/types/api'

export const companyApi = {
  tree: () => get<CompanyDto[]>('/sys/company/tree'),
  get: (code: string) => get<CompanyDto>('/sys/company/get', { companyCode: code }),
  save: (data: any) => post<CompanyDto>('/sys/company/save', data),
  delete: (code: string) => post('/sys/company/delete', { companyCode: code })
}
