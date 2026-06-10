import { post } from './request'

export const switchApi = {
  switchRole: (roleCode: string) => post<any>('/sys/switch/role/' + roleCode),
  switchPost: (postCode: string) => post<any>('/sys/switch/post/' + postCode),
  switchSkin: (skinName: string) => post<any>('/sys/switch/skin/' + skinName),
}
