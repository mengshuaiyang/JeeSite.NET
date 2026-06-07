import { post, get } from './request';
export const roleDataScopeApi = {
    getByRole: (roleCode) => get(`/sys/role-data-scope/role/${roleCode}`),
    save: (data) => post('/sys/role-data-scope', data),
    delete: (roleCode, menuCode) => post(`/sys/role-data-scope/${roleCode}/${menuCode}`, {}),
    deleteByRole: (roleCode) => post(`/sys/role-data-scope/role/${roleCode}`, {})
};
