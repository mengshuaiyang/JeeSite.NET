import { get, post } from './request'

export const onlineApi = {
  list: () => get<any[]>('/sys/online/list'),
  kick: (userCode: string) => post('/sys/online/kick', null, { params: { userCode } })
}
