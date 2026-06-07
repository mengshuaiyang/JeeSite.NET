import { post, get } from './request';
export const categoryApi = {
    list: () => get('/cms/category/list'),
    tree: () => get('/cms/category/tree'),
    get: (categoryCode) => get('/cms/category/get', { categoryCode }),
    save: (data) => post('/cms/category/save', data)
};
