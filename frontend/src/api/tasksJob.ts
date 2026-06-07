import { post, get } from './request'
import type { PageRequest, PageResult, SysJobDto, JobLogDto } from '@/types/api'

export const jobApi = {
  list: (data: PageRequest) => post<PageResult<SysJobDto>>('/tasks/job/list', data),
  get: (jobId: string) => get<SysJobDto>('/tasks/job/get', { jobId }),
  save: (data: any) => post('/tasks/job/save', data),
  delete: (jobId: string) => post('/tasks/job/delete', { jobId }),
  start: (jobId: string) => post('/tasks/job/start', { jobId }),
  stop: (jobId: string) => post('/tasks/job/stop', { jobId }),
  runOnce: (jobId: string) => post('/tasks/job/run', { jobId }),
  logs: (jobId: string) => get<JobLogDto[]>('/tasks/job/logs', { jobId })
}
