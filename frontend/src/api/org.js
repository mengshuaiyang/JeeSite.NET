import { post, get } from './request';
export const orgApi = {
    tree: (orgType) => get('/sys/org/tree', { orgType }),
    get: (orgCode) => get('/sys/org/get', { orgCode }),
    save: (data) => post('/sys/org/save', data),
    delete: (orgCode) => post('/sys/org/delete', { orgCode })
};
