import { post, get } from './request';
export const articleApi = {
    list: (data) => post('/cms/article/list', data),
    get: (articleCode) => get('/cms/article/get', { articleCode }),
    save: (data) => post('/cms/article/save', data),
    delete: (articleCode) => post('/cms/article/delete', { articleCode })
};
