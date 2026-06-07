import { post, get } from './request';
export const dataScopeApi = {
    getRoleScopes: (roleCode) => get('/sys/role-data-scope/list', { roleCode }),
    saveRoleScopes: (data) => post('/sys/role-data-scope/save', data)
};
