import { post, get } from './request';
export const fieldScopeApi = {
    list: (roleCode) => get('/sys/role-field-scope/list', { roleCode }),
    save: (data) => post('/sys/role-field-scope/save', data)
};
