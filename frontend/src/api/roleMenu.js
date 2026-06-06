import { post, get } from './request';
export const roleMenuApi = {
    getMenuCodes: (roleCode) => get('/sys/role-menu/get-menu-codes', { roleCode }),
    save: (roleCode, menuCodes) => post('/sys/role-menu/save', { roleCode, menuCodes })
};
