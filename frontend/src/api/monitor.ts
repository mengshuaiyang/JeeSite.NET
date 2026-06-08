import { get } from './request'
import type { ApiResult, ServerInfo } from '@/types/api'

export const monitorApi = {
  getServerInfo: () => get<ServerInfo>('/sys/monitor/server'),
}
