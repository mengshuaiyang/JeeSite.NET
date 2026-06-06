import { post, get } from './request';
export const userApi = {
    list: (data) => post('/sys/user/list', data),
    get: (userCode) => get('/sys/user/get', { userCode }),
    save: (data) => post('/sys/user/save', data),
    delete: (userCode) => post('/sys/user/delete', { userCode })
};
