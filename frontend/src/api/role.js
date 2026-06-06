import { post, get } from './request';
export const roleApi = {
    list: (data) => post('/sys/role/list', data),
    get: (roleCode) => get('/sys/role/get', { roleCode }),
    save: (data) => post('/sys/role/save', data),
    delete: (roleCode) => post('/sys/role/delete', { roleCode })
};
