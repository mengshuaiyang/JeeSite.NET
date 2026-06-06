import { post, get } from './request';
export const moduleApi = {
    list: (data) => post('/sys/module/list', data),
    get: (moduleCode) => get('/sys/module/get', { moduleCode }),
    save: (data) => post('/sys/module/save', data),
    delete: (moduleCode) => post('/sys/module/delete', { moduleCode })
};
