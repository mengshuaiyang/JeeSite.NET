import { get, post } from './request'

export const roleUserApi = {
  getUsersByRole: (roleCode: string) => get<any[]>('/sys/role/' + roleCode + '/users'),
  saveUsers: (roleCode: string, userCodes: string[]) => post<any>('/sys/role/' + roleCode + '/save-users', userCodes),
  deleteUser: (roleCode: string, userCode: string) => post<any>('/sys/role/' + roleCode + '/delete-user/' + userCode),
  getAvailableUsers: (roleCode?: string) => get<any[]>('/sys/role/available-users', { roleCode }),
}
