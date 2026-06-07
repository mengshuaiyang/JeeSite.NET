import { post, get } from './request';
export const langApi = {
    list: () => get('/sys/lang/list'),
    getByType: (langType) => get('/sys/lang/get-by-type', { langType }),
    save: (data) => post('/sys/lang/save', data),
    delete: (id) => post('/sys/lang/delete', { id })
};
