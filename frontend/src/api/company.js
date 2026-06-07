import { post, get } from './request';
export const companyApi = {
    tree: () => get('/sys/company/tree'),
    get: (code) => get('/sys/company/get', { companyCode: code }),
    save: (data) => post('/sys/company/save', data),
    delete: (code) => post('/sys/company/delete', { companyCode: code })
};
