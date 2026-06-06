import { post, get } from './request';
export const configApi = {
    list: (data) => post('/sys/config/list', data),
    get: (configKey) => get('/sys/config/get', { configKey }),
    save: (data) => post('/sys/config/save', data),
    delete: (configKey) => post('/sys/config/delete', { configKey })
};
