import { post, get } from './request';
export const roleFieldScopeApi = {
    getByRoleMenu: (roleCode, menuCode) => get('/sys/role-field-scope', { roleCode, menuCode }),
    save: (data) => post('/sys/role-field-scope', data),
    delete: (id) => post(`/sys/role-field-scope/${id}`, {})
};
