import { post } from './request';
export const authApi = {
    login: (data) => post('/sys/auth/login', data)
};
