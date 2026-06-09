import { get, post } from './request'
import type { ApiResult } from '@/types/api'

export interface LeaveDto {
  leaveRequestId: string
  applicant: string
  leaveType: string
  startDate: string
  endDate: string
  durationDays: number
  reason?: string
  status: string
  submitDate?: string
  history?: any[]
}

export const leaveApi = {
  myLeaves: (applicant: string) => get<LeaveDto[]>('/bpm/leave/my-leaves', { applicant }),
  pending: (approver: string) => get<LeaveDto[]>('/bpm/leave/pending', { approver }),
  submit: (data: any) => post<any>('/bpm/leave/submit', data),
  approve: (data: any) => post<any>('/bpm/leave/approve', data),
  detail: (leaveRequestId: string) => get<LeaveDto>('/bpm/leave/detail', { leaveRequestId })
}
