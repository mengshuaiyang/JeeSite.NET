import { post, get } from './request';
export const bizCategoryApi = {
    tree: () => get('/sys/biz-category/tree'),
    save: (data) => post('/sys/biz-category/save', data),
    delete: (code) => post('/sys/biz-category/delete', { categoryCode: code })
};
