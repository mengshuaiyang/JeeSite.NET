import { post, get } from './request'
import type { PageRequest, PageResult, EmployeeDto, EmployeeSaveDto } from '@/types/api'

export const employeeApi = {
  list: (data: PageRequest) => post<PageResult<EmployeeDto>>('/sys/employee/list', data),
  get: (code: string) => get<EmployeeDto>('/sys/employee/get', { empCode: code }),
  save: (data: EmployeeSaveDto) => post<EmployeeDto>('/sys/employee/save', data),
  delete: (code: string) => post('/sys/employee/delete', { empCode: code })
}
