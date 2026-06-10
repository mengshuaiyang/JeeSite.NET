import { get, post } from './request'
import type { EmpUser } from '@/types/api'

export const empUserApi = {
  getByEmp: (empCode: string) => get<EmpUser[]>('/sys/emp-user/' + empCode),
  getByUser: (userCode: string) => get<EmpUser[]>('/sys/emp-user/by-user/' + userCode),
  list: (empCode?: string, userCode?: string) => get<EmpUser[]>('/sys/emp-user/list', { empCode, userCode }),
  save: (data: { empCode: string; userCode: string }) => post<any>('/sys/emp-user/save', data),
  delete: (data: { empCode: string; userCode: string }) => post<any>('/sys/emp-user/delete', data),
  availableEmployees: () => get<any[]>('/sys/emp-user/available-employees'),
  availableUsers: () => get<any[]>('/sys/emp-user/available-users'),
}
