import { post, get } from './request';
export const jobApi = {
    list: (data) => post('/tasks/job/list', data),
    get: (jobId) => get('/tasks/job/get', { jobId }),
    save: (data) => post('/tasks/job/save', data),
    delete: (jobId) => post('/tasks/job/delete', { jobId }),
    start: (jobId) => post('/tasks/job/start', { jobId }),
    stop: (jobId) => post('/tasks/job/stop', { jobId }),
    runOnce: (jobId) => post('/tasks/job/run', { jobId }),
    logs: (jobId) => get('/tasks/job/logs', { jobId })
};
