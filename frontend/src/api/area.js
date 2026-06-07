import { post, get } from './request';
export const areaApi = {
    tree: () => get('/sys/area/tree'),
    get: (code) => get('/sys/area/get', { areaCode: code }),
    save: (data) => post('/sys/area/save', data),
    delete: (code) => post('/sys/area/delete', { areaCode: code })
};
