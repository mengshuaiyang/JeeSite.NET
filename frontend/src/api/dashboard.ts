import { get } from './request'
import type { ApiResult } from '@/types/api'

export interface RecentLogin {
  userName?: string
  loginCode?: string
  loginDate?: string
  ipAddress?: string
}

export interface DashboardStats {
  userCount: number
  roleCount: number
  menuCount: number
  orgCount: number
  postCount: number
  dictCount: number
  logCountToday: number
  recentLogins: RecentLogin[]
}

export const dashboardApi = {
  getStats: () => get<DashboardStats>('/sys/dashboard/stats'),
}
