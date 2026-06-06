import { post, get } from './request';
export const logApi = {
    list: (data) => post('/sys/log/list', data),
    get: (logId) => get('/sys/log/get', { logId })
};
