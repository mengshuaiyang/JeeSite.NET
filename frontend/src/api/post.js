import { post, get } from './request';
export const postApi = {
    list: (data) => post('/sys/post/list', data),
    get: (postCode) => get('/sys/post/get', { postCode }),
    save: (data) => post('/sys/post/save', data),
    delete: (postCode) => post('/sys/post/delete', { postCode })
};
