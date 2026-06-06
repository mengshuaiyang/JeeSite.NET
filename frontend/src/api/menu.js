import { post, get } from './request';
export const menuApi = {
    list: (data) => post('/sys/menu/list', data),
    tree: (moduleCode) => get('/sys/menu/tree', { moduleCode }),
    getUserMenus: () => get('/sys/menu/user-menus'),
    get: (menuCode) => get('/sys/menu/get', { menuCode }),
    save: (data) => post('/sys/menu/save', data),
    delete: (menuCode) => post('/sys/menu/delete', { menuCode })
};
