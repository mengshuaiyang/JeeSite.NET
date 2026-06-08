import { request } from '@/utils/request'

export const leaveApi = {
  myLeaves: (applicant: string) => request.get('/bpm/leave/my-leaves', { params: { applicant } }),
  pending: (approver: string) => request.get('/bpm/leave/pending', { params: { approver } }),
  submit: (data: any) => request.post('/bpm/leave/submit', data),
  approve: (data: any) => request.post('/bpm/leave/approve', data),
  detail: (leaveRequestId: string) => request.get('/bpm/leave/detail', { params: { leaveRequestId } })
}
