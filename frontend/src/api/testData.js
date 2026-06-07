import { post, get } from './request';
export const testDataApi = {
    list: () => get('/test/data/list'),
    get: (id) => get('/test/data/get', { id }),
    save: (data) => post('/test/data/save', data),
    delete: (id) => post('/test/data/delete', { id })
};
