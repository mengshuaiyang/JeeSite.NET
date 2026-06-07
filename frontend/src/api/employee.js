import { post, get } from './request';
export const employeeApi = {
    list: (data) => post('/sys/employee/list', data),
    get: (code) => get('/sys/employee/get', { empCode: code }),
    save: (data) => post('/sys/employee/save', data),
    delete: (code) => post('/sys/employee/delete', { empCode: code })
};
